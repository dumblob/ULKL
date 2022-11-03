@rem http://steve-jansen.github.io/guides/windows-batch-scripting/index.html

@rem disable the default echoing of every executed command
@echo off

rem adds setlocal, chdir, ... (nonexistent before cca Win2000)
setlocal enableextensions
rem make all variables local solely to this batch
setlocal
rem treat string !abc! as a variable and expand it each time it's
rem accessed (unlike %abc% which gets expanded only once while parsing
rem a block in parenthesis)
setlocal enabledelayedexpansion

rem FIXME c:\users\jpa\desktop\win\gerdltgt\make_pkg.cmd

rem https://stackoverflow.com/a/26079981
rem prevent interpretation for the first time
goto :endtrim
rem :trim <ret_var_label> <unquoted_variable0> <unquoted_variable1> ...
:trim
  setlocal
  if "%~1"=="" set emsg=%%1 != ""
  if not "%emsg%"=="" (
    echo ERR failed assert in %0 %emsg% >&2
    if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
    exit 1
  )

  set x=%*
  for /f "tokens=1*" %%a in ("!x!") do endlocal & set %~1=%%b
  exit /b
:endtrim

rem double quotes are not allowed in paths => we can safely remove them
set ccline=%CMDCMDLINE:"=%
set cspec=%COMSPEC:"=%
call :trim ccline %ccline%

set msklc=C:\Program Files (x86)\Microsoft Keyboard Layout Creator 1.4

rem argument 0 (full path with file name of this script)
set layoutpath=%~dp0
rem remove trailing backslash (otherwise the "basename" trick doesn't work)
set layoutpath=%layoutpath:~0,-1%
rem basename
for %%_ in ("%layoutpath%") do set kbdname=%%~n_

if not exist "%msklc%" (
  echo ERR Dir "%msklc%" not found. Is MSKLC installed? >&2
  rem wait for user input
  rem (can't use %CMDCMDLINE%, because it doesn't work when double clicked)
  if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
  rem /b exits the batch, not the cmd.exe process
  exit /b 1
)

for %%f in (
    "%layoutpath%\%kbdname%.c"
    "%layoutpath%\%kbdname%.def"
    "%layoutpath%\%kbdname%.h"
    "%layoutpath%\%kbdname%.klc"
    "%layoutpath%\%kbdname%.rc") do (
  if not exist %%f (
    echo ERR %%f >&2
    echo ERR ^^^^^^File not found (the file name without >&2
    echo ERR extension must be the same as its parent directory^). >&2
    if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
    exit /b 1
  )
)

rem switch all processing of octets to UTF-8 (useful e.g. for "for /f ...")
rem note: wrongly looking characters on stdout are just a font issue
rem note: switching to non-ANSI codepage draws find, more, ... unusable
rem chcp 65001

set productname=
for /f "usebackq tokens=1,2*" %%a in ("%layoutpath%\%kbdname%.klc") do (
  if "%%a"=="KBD" (
    if "%%b"=="%kbdname%" (
      set productname=%%~c
      goto :endfor00
    )
  )
)
:endfor00
if "%productname%"=="" (
  echo ERR Could not parse keyboard description from "%kbdname%.klc" >&2
  echo ERR Try converting it to ANSI 850 or UTF-16 or UTF-8 ( >&2
  echo ERR disregarding whether with BOM or not^). >&2
  if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
  exit /b 1
)

rem is_installed <ret_var_label> <ret2_var_label> <productname>
goto :endis_installed
:is_installed
  setlocal
  if "%~1"=="" set emsg=%%1 != ""
  if "%~2"=="" set emsg=%%2 != ""
  if "%~3"=="" set emsg=%%3 != ""
  if not "%~4"=="" set emsg=number_of_arguments = 3
  if not "%emsg%"=="" (
    echo ERR failed assert in %0 (%emsg%^) >&2
    if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
    exit 1
  )

  set found=0
  set guid32=

  rem list all installed apps: wmic product get name
  for /f "tokens=1" %%a in (
      'wmic product where name^="%~3" get IdentifyingNumber 2^> NUL') do (
    if "!found!" equ "1" (
      set guid32=%%a
      goto :endfor01
    )
    if "%%a"=="IdentifyingNumber" (
      rem at least one such app is installed
      set found=1
    )
  )
  :endfor01

  rem can't use %errorlevel% because it gets inconveniently overwritten
  endlocal & set "%~1=%found%" & set "%~2=%guid32%"
  exit /b
:endis_installed

echo INFO Checking if installed: "%productname%"

call :is_installed found guid32 "%productname%"
echo FIXME DEBUGguid32=%guid32%
if /i "%found%" neq "0" (
  rem the kbd uninstaller sometimes leaves shareddlls entries in registry
  rem the kbd uninstaller leaves some dll files even if SharedDlls is 0

rem CreateObject^( "Shell.Application" ^).ShellExecute ^
rem     "wmic", "product where IdentifyingNumber=""%guid32%"" call uninstall", "", "runas", 1 :^
  echo on error resume next :^
CreateObject^( "Wscript.Shell" ^).Run ^
    "wmic product where IdentifyingNumber=""%guid32%"" call uninstall", 1, true :^
const hklm = ^&H80000002 :^
set reg = GetObject^( ^
    "winmgmts:{impersonationLevel=impersonate}!\\.\root\default:StdRegProv" ^) :^
set fs = createobject^( "Scripting.FileSystemObject" ^) :^
for each s in array^( ^
    "C:\Windows\System32\%kbdname%.dll", ^
    "C:\Windows\SysWOW64\%kbdname%.dll" ^) :^
  reg.DeleteValue hklm, "SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDlls\", s :^
  fs.DeleteFile s, true :^
next :^
fs.DeleteFile "%temp%\marker.txt", true > "%temp%\elevated.vbs"

  rem FIXME how will this work if the current account will not
  rem   have admin rights? will there be any issues with password?
  rem https://stackoverflow.com/a/23825726
  echo on error resume next :^
set fs = createObject^( "Scripting.FileSystemObject" ^) :^
fs.createTextFile^( "%temp%\marker.txt" ^) :^
createObject^( "Shell.Application" ^).ShellExecute ^
    "cscript", "%temp%\elevated.vbs", "", "runas", 1 :^
x = 600 :^
do while fs.fileExists^( "%temp%\marker.txt" ^) :^
  if x ^< 0 then : exit do : end if :^
  wscript.sleep 100 :^
  x = x -1 :^
loop > "%temp%\elevate.vbs"

  echo INFO Uninstalling "%productname%"

  "%temp%\elevate.vbs"
  rem del /f /q "%temp%\elevate.vbs" > NUL
  rem del /f /q "%temp%\elevated.vbs" > NUL

  rem we can't read stdout nor any return values from elevated processes through pipe
  rem (we could read a file, but checking again is the safest possible method)
  call :is_installed found _ "%productname%"
  if /i "%found%" neq "0" (
    echo ERR Can't uninstall the kbd layout. >&2
    echo ERR Please do it manually and re-run this script. >&2
    if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)
    exit 1
  )
)
:endif00

if exist "%userprofile%\documents\%kbdname%" (
  rem removes all files recursively (does leave all directories in place)
  del /f /s /q "%userprofile%\documents\%kbdname%" > NUL
  rem removes all empty directories recursively
  rmdir /s /q "%userprofile%\documents\%kbdname%" > NUL
)

echo INFO >&2
echo INFO Instructions for the "Keyboard Layout Creator": >&2
echo INFO 1. click "Project" in menu -^> "Build DLL and Setup Package" >&2
echo INFO 2. click "No" to disable viewing of "KeyboardVerify.log" >&2
echo INFO 3. click "No" to disable opening of the Installer package directory >&2
echo INFO 4. close the window of the "Keyboard Layout Creator" >&2
echo INFO >&2

rem MSKLC will correctly generate all the install and setup files
rem "pipe to more" trick to wait for closing
"%msklc%\MSKLC.exe" "%layoutpath%\%kbdname%.klc" | more

if exist "%userprofile%\Documents\KeyboardVerify.log" (
  del /f /q "%userprofile%\Documents\KeyboardVerify.log" > NUL
)
rem FIXME
pause
exit /b 1

rem recompile just the DLLs (the rest is prepared from MSKLC)
rem x86, wow64 (32bit on 64bit system), x86_64, ia64 (x86_64 Itanium)
for %%p in (
    "%msklc%\bin\i386"
    "%msklc%\bin\i386"
    "%msklc%\bin\i386\amd64"
    "%msklc%\bin\i386\ia64") do (
  if %%p=="%msklc%\bin\i368" (
    echo howk11
    rem the second iteration is the same compiler, but a wow64 binary
    if "!outdir!"=="i386" (
      set defwow64=-DBUILD_WOW6432
      set outdir=wow64
    ) else (
      set outdir=i386
    )
  )
  if %%p=="%msklc%\bin\i368\amd64" (
    echo howk 1100
    set outdir=amd64
  )
  if %%p=="%msklc%\bin\i368\ia64"  set outdir=ia64

  echo INFO Compiling layout "!outdir!\%kbdname%.dll" >&2

  echo howk22

  rem FIXME call this once for all architectures?
  rem %%p\rc.exe -r -i%msklc%\inc -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l 49 -Fo%TEMP%/my_layout.res %layoutpath%\%kbdname%.rc
  "%%p\rc.exe" -r "-i%msklc%\inc" -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l 49 "%layoutpath%\%kbdname%.rc"

  rem %%p\cl.exe -nologo -I%msklc%\inc -DNOGDICAPMASKS -DNOWINMESSAGES -DNOWINSTYLES -DNOSYSMETRICS -DNOMENUS -DNOICONS -DNOSYSCOMMANDS -DNORASTEROPS -DNOSHOWWINDOW -DOEMRESOURCE -DNOATOM -DNOCLIPBOARD -DNOCOLOR -DNOCTLMGR -DNODRAWTEXT -DNOGDI -DNOKERNEL -DNONLS -DNOMB -DNOMEMMGR -DNOMETAFILE -DNOMINMAX -DNOMSG -DNOOPENFILE -DNOSCROLL -DNOSERVICE -DNOSOUND -DNOTEXTMETRIC -DNOWINOFFSETS -DNOWH -DNOCOMM -DNOKANJI -DNOHELP -DNOPROFILER -DNODEFERWINDOWPOS -DNOMCX -DWIN32_LEAN_AND_MEAN -DRoster -DSTD_CALL -D_WIN32_WINNT=x5 %defwow64% /DWINVER=x5 -D_WIN32_IE=x5 /MD /c /Zp8 /Gy /W3 /WX /Gz /Gm- /EHs-c- /GR- /GF -Z7 /Oxs %TEMP%/my_layout.obj "%layoutpath%\%kbdname%.c"

  rem %%p\link.exe -nologo %s %s -SECTION:INIT,d -OPT:REF -OPT:ICF -IGNORE:439,478 -noentry -dll -libpath:%s -subsystem:native,5. -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 -osversion:4. -version:4. /release -def:%kbdname%.def %TEMP%/my_layout.res %TEMP%/my_layout.obj-merge:.edata=.data -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text -out %kbdname%_arch00.dll
  rem %%p\link.exe -nologo %s %s -SECTION:INIT,d -OPT:REF -OPT:ICF -IGNORE:439,478 -noentry -dll -libpath:%s -subsystem:native,5. -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 -osversion:4. -version:4. /release -def:%kbdname%.def %TEMP%/my_layout.res %TEMP%/my_layout.obj-merge:.edata=.data -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text
)

echo INFO Success! Find the kbd layout in: >&2
echo INFO %userprofile%\Documents\%kbdname% >&2
rem FIXME the condition is wrong - bad variables?
if not "%ccline%"=="%cspec%" set /p _=(Press ENTER to exit^)

rem vim: set wrap:

#!/bin/sh

# FIXME ULKL gerd
#   Ctrl + s writes `=` in Excel instead of saving the file
#   Shift + Del doesn't work in Windows (file) Explorer
#   Ctrl + f doesn't work in nvim and in Chromium it triggers the built-in search instead of the "page down cVim function"
#   Ctrl + Backspace sometimes doesn't delete word backwards (e.g. in notepad)
#   Ctrl + at sign sometimes doesn't work (e.g. in nvim; but in notepad it works)
#   is this because in gerdLTGT.klc is the third level in many cases disabled and if not, it doesn't produce the desired VK_xxx key?
#   some apps seem to handle combinations themself and some rely on the resulting "character"/"abbreviation" provided by the operating system
# FIXME test
#   Windows remote desktop - all 3 combinations (outside only, inside only, outside inside)
#   Virtualbox - || -

set -e

readonly msklc='C:\Program Files (x86)\Microsoft Keyboard Layout Creator 1.4'
readonly layoutpath="$(cd "$(dirname "$0")"; pwd)"
readonly kbdname="$(basename "$layoutpath")"
# Semver with the second part serving also the purpose of the missing last part
readonly my_ver=0.9

if [ ! -x "$msklc" ]; then
  printf 'ERR %s\n' "$msklc" >&2
  printf 'ERR ^^^Could not open the MSKLC dir. Is MSKLC 1.4 installed?\n' "$msklc" >&2
  exit 1
fi

printf 'INFO Checking for files availability...\n'

for f in \
    "$layoutpath\\${kbdname}.c" \
    "$layoutpath\\${kbdname}.def" \
    "$layoutpath\\${kbdname}.h" \
    "$layoutpath\\${kbdname}.klc" \
    "$layoutpath\\${kbdname}.rc"; do
  if [ ! -r "$f" ]; then
    printf 'ERR %s\n' "$f" >&2
    printf 'ERR ^^^Could not read the file. Make sure the filename without\n' >&2
    printf 'ERR extension matches the name of its parent directory).\n' >&2
    exit 1
  fi
done

readonly productname_quoted="$( \
    grep -E '^[[:blank:]]*KBD' "$layoutpath\\${kbdname}.klc" | \
    sed -r 's|^[[:blank:]]*KBD[[:blank:]]+[^[:blank:]]+[[:blank:]]+||')"
if [ -z "$productname_quoted" ]; then
  printf 'ERR Could not parse keyboard description from %s\n' "${kbdname}.klc" >&2
  exit 1
fi
readonly localeid="$(grep -E '^[[:blank:]]*LOCALEID' "$layoutpath\\${kbdname}.klc" | tr '[:upper:]' '[:lower:]' | sed -r 's|^[[:blank:]]*localeid[[:blank:]]+"0*([0-9]+).*|\1|')"
if [ -z "$localeid" ]; then
  printf 'ERR Could not parse keyboard localeid from %s\n' "${kbdname}.klc" >&2
  exit 1
fi

# (\\CREED\ROOT\CIMV2:Win32_Product.IdentifyingNumber="{ACBB506E-BC38-45FA-BC22-6A6E2E1D9E81}",Name="German Dvorak ltgt (useful for text with lots of <>)",Version="1.0.3.40")->Uninstall() wird ausgeführt
# Methode wurde ausgeführt.
# Ausgabeparameter:
# instance of __PARAMETERS
# {
#         ReturnValue = 1603;
# };

# run_elevated <path_to_executable>
run_elevated()(
  [ $# -eq 1 ] || {
    printf 'ERR run_elevated: one and only one param allowed\n' >&2
    exit 1
  }
  [ -r "$1" ] || {
    printf 'ERR run_elevated: executable not readable\n' >&2
    exit 1
  }
  [ -n "$(printf %s "$1" | grep -E '/')" ] && {
    printf 'ERR run_elevated: forward slash in path not allowed\n' >&2
    exit 1
  }
  # VBS needs double quotes quoted by putting them twice
  readonly x="$(printf %s "$1" | sed -r 's|"|""|g')"
  # FIXME $TMPDIR not taken into account
  readonly t="$(mktemp -p "$TMPDIR" --suffix .vbs run_elevated.XXXXXXXXXXXXXXXX)"
  trap "rm -f '$t'" INT HUP TERM EXIT
  # printf %s 'CreateObject("Shell.Application").ShellExecute "'"$f2"'","","'"$TMP"'","runas",1' > "$f"
  printf %s 'CreateObject("Shell.Application").ShellExecute "'"$x"'","","","runas",1' > "$t"
  # cscript/wscript
  # FIXME
  printf '%s\n' "cscript $t"
  #cscript "$t"
)

printf 'INFO Searching for conflicting keyboard layouts...\n'

# list all installed apps: wmic product get name
# FIXME !
if [ ! -n "$(wmic product where name="$productname_quoted")" ]; then
  # win apps usually do not accept forward slashes
  readonly a="$(mktemp -p "$TMPDIR" --suffix .cmd cmd_app.XXXXXXXXXXXXXXXX | sed -r 's|/|\\|g')"
  readonly a_log="${a}.log"

  # make traps local
  (
    trap "rm -f '$a' '$a_log'" INT HUP TERM EXIT RETURN
    printf %s "wmic product where name=$productname_quoted call uninstall > $a_log" > "$a"
    printf 'INFO Uninstalling conflicting keyboard layouts.\n'
    # FIXME https://msdn.microsoft.com/en-us/library/xsy6k3ys(VS.85).aspx
    run_elevated "$a"

    # due to deferred DB-operations etc.
    printf 'INFO Waiting '
    c=10
    while printf '%ds... ' "$c"; do
      if [ "$(($(grep -E ReturnValue "$a_log" 2>/dev/null | sed -r 's|^[^=]+=||')))" -ne 0 \
          -o "$c" -le 0 ]; then
        rm -f "$a_log"
        printf '\n%s\n' "ERR Couldn't uninstall the kbd layout \"$kbdname\"." >&2
        exit 1
      fi
      # this takes time => no need for sleep 1
      [ -z "$(wmic product where name="$productname_quoted")" ] && break || true
      c=$((c-1))
    done
    printf '\n'
  )

  # FIXME unregister the DLL
  #   what about DLLs in use? restart needed (logout won't help as kbd layouts are also for the login page)?
cat >/dev/null <<\END
on error resume next
CreateObject( "Wscript.Shell" ).Run "wmic product where IdentifyingNumber="""{11D34ED0-8C85-4C7C-BCB8-59884CAC02FC}""" call uninstall", 1, true

const hklm = &H80000002
set reg = GetObject( "winmgmts:{impersonationLevel=impersonate}StdRegProv" )
set fs = createobject( "Scripting.FileSystemObject" )
for each s in array( "C:\Windows\System32\gerdLTGT.dll", "C:\Windows\SysWOW64\gerdLTGT.dll" )
  reg.DeleteValue hklm, "SOFTWARE\Microsoft\Windows\CurrentVersion\SharedDlls\", s
  fs.DeleteFile s, true
  next
fs.DeleteFile "C:\Users\JPa\AppData\Local\Temp\marker.txt", true
END

  # make traps local
  (
    trap "rm -f '$a'" INT HUP TERM EXIT RETURN
    # FIXME wmic does not remove it
  cat > "$a" <<END
del /f /q C:\\Windows\\System32\\${kbdname}.dll
del /f /q C:\\Windows\\SysWOW64\\${kbdname}.dll
END
    run_elevated "$a"
  )
fi

# FIXME automatically in VBS (periodically check for MSKLC to appear and then try to click it)?
# FIXME can we lock the window to just a certain process for input?
printf '%s\n' "WARN Do not touch any input device (mouse, keyboard, ...)" >&2
cat >&2 <<\END
INFO 1. wait for "Keyboard Layout Creator" window to appear
INFO 2. click "Project" in menu -> "Build DLL and Setup Package"
INFO 3. click "No" to disable viewing of "KeyboardVerify.log"
INFO 4. click "No" to disable opening of the Installer package directory
INFO 5. close the window of the "Keyboard Layout Creator"
END

# FIXME why moving this? does it collide with some default MSKLC-stuff?
#   (maybe it's created by MSKLC when building DLLs and shouldn't stay from last time there)
if [ -e "$USERPROFILE\\documents\\$kbdname" ]; then
  mv "$USERPROFILE\\documents\\$kbdname" "$USERPROFILE\\documents\\$kbdname-$(date +%F-%H%M%S%:::z)"
fi

# FIXME open it with some other geometry?
#"$msklc/MSKLC.exe" "%layoutpath%\%kbdname%.klc"

printf '%s\n' "WARN You can freely use input devices (mouse, keyboard, ...) again" >&2

if [ -e "$USERPROFILE\\Documents\\KeyboardVerify.log" ]; then
  rm "$USERPROFILE\\Documents\\KeyboardVerify.log"
fi

cd "$layoutpath"
# can't use $TEMP as it's a virtual mountpoint (win apps see /tmp as C:\tmp)
readonly TEMP_WIN="$(cmd.exe /c "echo %TEMP%")"
idx=0
for p in \
    "$msklc\\bin\\i386" \
    "$msklc\\bin\\i386" \
    "$msklc\\bin\\i386\\amd64" \
    "$msklc\\bin\\i386\\ia64"; do
  # x86, wow64 (32bit on 64bit system), x86_64, ia64 (x86_64 Itanium)
  case "$p" in
    "$msklc\\bin\\i386")
      libdir=i386
      # https://docs.microsoft.com/de-de/cpp/build/reference/linker-options
      subsys_native_ver=4.00
      # 32bit on 64bit systems
      if [ "$idx" -eq 0 ]; then
        idx=$((idx +1))
        defwow64=-DBUILD_WOW6432
        outdir=wow64
      else
        outdir=i386
      fi ;;
    "$msklc\\bin\\i386\\amd64")
      libdir=amd64
      subsys_native_ver=5.02
      outdir=amd64 ;;
    "$msklc\\bin\\i386\\ia64")
      libdir=ia64
      subsys_native_ver=5.02
      outdir=ia64 ;;
  esac

  # FIXME $outdir not used
  # FIXME arguments with slash rewrite to minus

  #%%p\rc.exe -r -i%msklc%\inc -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l 49 -Fo%TEMP%/my_layout.res %layoutpath%\%kbdname%.rc
  #"${p}\\rc.exe" -i "${msklc}\\inc\\" -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l "$localeid" /fo "${TEMP_WIN}\\my_layout.res" "${kbdname}.rc"
  "$p\\rc.exe" -i "$msklc\\inc" -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 -DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l "$localeid" -fo "$TEMP_WIN\\my_layout.res" "${kbdname}.rc"

  printf '\nHOWK00\n\n'

  # FIXME WINVER and WIN32_IE higher than at rc.exe
  "$p\\cl.exe" -nologo "-I$msklc\\inc" -DNOGDICAPMASKS -DNOWINMESSAGES -DNOWINSTYLES -DNOSYSMETRICS -DNOMENUS -DNOICONS -DNOSYSCOMMANDS -DNORASTEROPS -DNOSHOWWINDOW -DOEMRESOURCE -DNOATOM -DNOCLIPBOARD -DNOCOLOR -DNOCTLMGR -DNODRAWTEXT -DNOGDI -DNOKERNEL -DNONLS -DNOMB -DNOMEMMGR -DNOMETAFILE -DNOMINMAX -DNOMSG -DNOOPENFILE -DNOSCROLL -DNOSERVICE -DNOSOUND -DNOTEXTMETRIC -DNOWINOFFSETS -DNOWH -DNOCOMM -DNOKANJI -DNOHELP -DNOPROFILER -DNODEFERWINDOWPOS -DNOMCX -DWIN32_LEAN_AND_MEAN -DRoster -DSTD_CALL -D_WIN32_WINNT=x5 $defwow64 /DWINVER=x5 -D_WIN32_IE=x5 /MD /c /Zp8 /Gy /W3 /WX /Gz /Gm- /EHs-c- /GR- /GF -Z7 /Oxs "$TEMP_WIN\\my_layout.obj" "${kbdname}.c"

  printf '\nHOWK11\n\n'

  #%%p\link.exe -nologo %s %s -SECTION:INIT,d -OPT:REF -OPT:ICF -IGNORE:439,478 -noentry -dll -libpath:%s -subsystem:native,5. -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 -osversion:4. -version:4. /release -def:%kbdname%.def %TEMP%/my_layout.res %TEMP%/my_layout.obj-merge:.edata=.data -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text -out %kbdname%_arch00.dll
  # FIXME add -IGNORE:439,478
  # FIXME what is -osversion:4.
  "$p\\link.exe" -nologo abc00 abc11 -SECTION:INIT,d -OPT:REF -OPT:ICF -noentry -dll "-libpath:$msklc\\lib\\$libdir" "-subsystem:native,$subsys_native_ver" -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 "-version:$my_ver" /release "-def:${kbdname}.def" "$TEMP_WIN\\my_layout.res" "$TEMP_WIN\\my_layout.obj" -merge:.edata=.data -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text

  printf '\nHOWK22\n\n'

  rm "$TEMP_WIN\\my_layout."*
done

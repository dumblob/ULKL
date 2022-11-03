#!/bin/sh

readonly msklc='C:\Program Files (x86)\Microsoft Keyboard Layout Creator 1.4'
readonly layoutpath="$(dirname "$0")"
readonly kbdname="$(basename "$(dirname "$0")")"

[ -x "$msklc" ] || {
  printf 'ERR %s\n' "$msklc" >&2
  printf 'ERR ^^^Could not open the dir. Is MSKLC 1.4 installed?\n' "$msklc" >&2
  exit 1
}
for f in \
    "$layoutpath/${kbdname}.C" \
    "$layoutpath/${kbdname}.DEF" \
    "$layoutpath/${kbdname}.H" \
    "$layoutpath/${kbdname}.klc" \
    "$layoutpath/${kbdname}.RC"; do
  [ -r "$f" ] || {
    printf 'ERR %s\n' "$f" >&2
    printf 'ERR ^^^Could not read the file. Make sure the filename without\n' >&2
    printf 'ERR extension matches the name of its parent directory).\n' >&2
    exit 1
  }
done

readonly productname_quoted="$( \
    grep -E '^[[:blank:]]*KBD' "$layoutpath/${kbdname}.klc" | \
    sed -r 's|^[[:blank:]]*KBD[[:blank:]]+[^[:blank:]]+[[:blank:]]+||')"
[ -z "$productname_quoted" ] && {
  printf 'ERR Could not parse keyboard description from %s\n' "${kbdname}.klc" >&2
  exit 1
}

# (\\CREED\ROOT\CIMV2:Win32_Product.IdentifyingNumber="{ACBB506E-BC38-45FA-BC22-6A6E2E1D9E81}",Name="German Dvorak ltgt (useful for text with lots of <>)",Version="1.0.3.40")->Uninstall() wird ausgeführt
# Methode wurde ausgeführt.
# Ausgabeparameter:
# instance of __PARAMETERS
# {
#         ReturnValue = 1603;
# };

# list all installed apps: wmic product get name
[ -n "$(wmic product where name="$productname_quoted")" ] && {
  readonly f="$TMP/elevate.vbs"
  readonly f2="$TMP/wmic.cmd"
  readonly f2_win='%TMP%\wmic.cmd'
  # VBS needs double quotes quoted by putting them twice
  readonly f2_vbs='%TMP%\wmic.cmd'
  readonly f2log="${f2}.log"
  readonly f2log_win="${f2_win}.log"
  #readonly p="$(printf %s "$productname_quoted" | sed -r 's|"|""|g')"

  #printf '%s\n%s\n%s\n%s\n%s\n' "$f" "$f2" "$f2log" "$productname_quoted" "$p"

  # FIXME https://msdn.microsoft.com/en-us/library/xsy6k3ys(VS.85).aspx

  # printf %s 'CreateObject("Shell.Application").ShellExecute "'"$f2"'","","'"$TMP"'","runas",1' > "$f"
  printf %s 'CreateObject("Shell.Application").ShellExecute "'"$f2_vbs"'","","","runas",1' > "$f"
  printf %s "wmic product where name=$productname_quoted call uninstall > $f2log_win" > "$f2"

  printf 'INFO Uninstalling conflicting keyboard layouts.\n'
  # FIXME cscript wscript
  #wscript "$f"
  #rm -f "$f" "$f2"

  # due to deferred DB-operations etc.
  printf 'INFO Waiting '
  c=10
  while printf '%d... ' "$c"; do
    [ "$(($(grep -E ReturnValue "$f2log" 2>/dev/null | sed -r 's|^[^=]+=||')))" -ne 0 \
        -o "$c" -le 0 ] && {
      rm -f "$f2log"
      printf '\n%s\n' "ERR Couldn't uninstall the kbd layout \"$kbdname\"." >&2
      exit 1
    }
    # this takes time => no need for sleep 1
    [ -z "$(wmic product where name="$productname_quoted")" ] && break
    c=$((c-1))
  done
  echo

  # FIXME wmic does not remove it
  for f in "C:/Windows/System32/$kbdname" "C:/Windows/SysWOW64/$kbdname"; do
    # FIXME
    #[ -e "$f" ] && rm -f "$f"
    [ -e "$f" ] && printf 'DEBUG rm -f %s\n' "$f"
  done
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
}

# FIXME automatically in VBS (periodically check for MSKLC to appear and then try to click it)?
printf '%s\n' "WARN Please do not touch any input device (mouse, keyboard, ...)" >&2
cat >&2 <<\END
INFO 1. click "Project" in menu -> "Build DLL and Setup Package"
INFO 2. click "No" to disable viewing of "KeyboardVerify.log"
INFO 3. click "No" to disable opening of the Installer package directory
INFO 4. close the window of the "Keyboard Layout Creator"
END

# FIXME why removing this?
[ -e "${USERPROFILE}/documents/$kbdname" ] && \
  printf 'DEBUG would remove %s\n' "${USERPROFILE}/documents/$kbdname" >&2
  #rm -r "${USERPROFILE}\\documents\\$kbdname"

exit 1

"$msklc/MSKLC.exe" "%layoutpath%\%kbdname%.klc"

[ -e "$userprofile/Documents/KeyboardVerify.log" ] && \
  printf 'DEBUG would remove %s\n' "$userprofile/Documents/KeyboardVerify.log" >&2
  #rm "$userprofile/Documents/KeyboardVerify.log" ] && \

# x86, wow64 (32bit on 64bit system), x86_64, ia64 (x86_64 Itanium)
idx=0
for p in \
    "$msklc/bin/i386" \
    "$msklc/bin/i386" \
    "$msklc/bin/i386/amd64" \
    "$msklc/bin/i386/ia64"; do {
  case "$p" in
    "$msklc/bin/i368")
      # 32bit for 64bit systems
      if [ "$idx" -eq 0 ]; then
        idx=$((idx +1))
        defwow64=-DBUILD_WOW6432
        outdir=wow64
      else
        outdir=i386
      fi ;;
    "$msklc/bin/i368/amd64") outdir=amd64 ;;
    "$msklc/bin/i368/ia64") outdir=ia64 ;;
  esac

  # FIXME $outdir not used

  #%%p\rc.exe -r -i%msklc%\inc -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l 49 -Fo%TEMP%/my_layout.res %layoutpath%\%kbdname%.rc
  "${p}\\rc.exe" -r "-i${msklc}\\inc" -DSTD_CALL -DCONDITION_HANDLING=1 -DNT_UP=1 -DNT_INST= -DWIN32=1 -D_NT1X_=1 -DWINNT=1 -D_WIN32_WINNT=x5 /DWINVER=x4 -D_WIN32_IE=x4 -DWIN32_LEAN_AND_MEAN=1 -DDEVL=1 -DFPO=1 -DNDEBUG -l 49 "${layoutpath}\\${kbdname}.rc"

  "${p}\\cl.exe" -nologo "-I${msklc}\\inc" -DNOGDICAPMASKS -DNOWINMESSAGES -DNOWINSTYLES -DNOSYSMETRICS -DNOMENUS -DNOICONS -DNOSYSCOMMANDS -DNORASTEROPS -DNOSHOWWINDOW -DOEMRESOURCE -DNOATOM -DNOCLIPBOARD -DNOCOLOR -DNOCTLMGR -DNODRAWTEXT -DNOGDI -DNOKERNEL -DNONLS -DNOMB -DNOMEMMGR -DNOMETAFILE -DNOMINMAX -DNOMSG -DNOOPENFILE -DNOSCROLL -DNOSERVICE -DNOSOUND -DNOTEXTMETRIC -DNOWINOFFSETS -DNOWH -DNOCOMM -DNOKANJI -DNOHELP -DNOPROFILER -DNODEFERWINDOWPOS -DNOMCX -DWIN32_LEAN_AND_MEAN -DRoster -DSTD_CALL -D_WIN32_WINNT=x5 $defwow64 /DWINVER=x5 -D_WIN32_IE=x5 /MD /c /Zp8 /Gy /W3 /WX /Gz /Gm- /EHs-c- /GR- /GF -Z7 /Oxs "${TEMP}\\my_layout.obj" "${layoutpath}\\${kbdname}.c"

  #%%p\link.exe -nologo %s %s -SECTION:INIT,d -OPT:REF -OPT:ICF -IGNORE:439,478 -noentry -dll -libpath:%s -subsystem:native,5. -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 -osversion:4. -version:4. /release -def:%kbdname%.def %TEMP%/my_layout.res %TEMP%/my_layout.obj-merge:.edata=.data -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text -out %kbdname%_arch00.dll
  "${p}\\link.exe" -nologo abc00 abc11 -SECTION:INIT,d -OPT:REF -OPT:ICF -IGNORE:439,478 -noentry -dll -libpath:%s -subsystem:native,5. -merge:.rdata=.text -PDBPATH:NONE -STACK:x4,x1 /opt:nowin98 -osversion:4. -version:4. /release "-def:${kbdname}.def" "${TEMP}\\my_layout.res" "${TEMP}\\my_layout.obj-merge:.edata=.data" -merge:.rdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text-merge:.edata=.data -merge:.srdata=.data -merge:.text=.data -merge:.bss=.data -section:.data,re -MERGE:_PAGE=PAGE -MERGE:_TEXT=.text
}

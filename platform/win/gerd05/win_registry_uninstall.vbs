' https://www.experts-exchange.com/questions/27438954/WMIC-UnInstall-not-working-Correctly.html
' FIXME
'   HKEY_CLASSES_ROOT\Installer\Products
'   HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Uninstall
'   HKLM\              SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall
'   HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall
'   HKEY_CLASSES_ROOT (HKCR)
'   HKEY_CURRENT_USER (HKCU)
'   HKEY_LOCAL_MACHINE (HKLM)
'   HKEY_USERS (HKU)
'   HKEY_CURRENT_CONFIG (HKCC)





"msiexec" & _
  " /x " & GUID & _
  " /quiet" & _
  " /norestart" & _
  " /lv ""%temp%\uninstall00.log"""








''''''''''''''''''''''''''''' a different approach to list ALL apps
strHost = "."
Const HKLM = &H80000002
Set objReg = GetObject("winmgmts://" & strHost & _
    "/root/default:StdRegProv")
Const strBaseKey = _
    "Software\Microsoft\Windows\CurrentVersion\Uninstall\"
objReg.EnumKey HKLM, strBaseKey, arrSubKeys
 
For Each strSubKey In arrSubKeys
    intRet = objReg.GetStringValue(HKLM, strBaseKey & strSubKey, _
        "DisplayName", strValue)
    If intRet <> 0 Then
        intRet = objReg.GetStringValue(HKLM, strBaseKey & strSubKey, _
        "QuietDisplayName", strValue)
    End If
    If (strValue <> "") and (intRet = 0) Then
        WScript.Echo strValue
    End If
Next
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''














'''''''''''''''''''''''''''''''' yet another approach to list ALL apps
Const HKLM = &H80000002
Set objReg = GetObject("winmgmts://" & "." & "/root/default:StdRegProv")
Set objFSO = CreateObject("Scripting.FileSystemObject")

outFile="programms.txt"

Function writeList(strBaseKey, objReg, objFile)
objReg.EnumKey HKLM, strBaseKey, arrSubKeys
    For Each strSubKey In arrSubKeys
        intRet = objReg.GetStringValue(HKLM, strBaseKey & strSubKey, "DisplayName", strValue)
        If intRet <> 0 Then
            intRet = objReg.GetStringValue(HKLM, strBaseKey & strSubKey, "QuietDisplayName", strValue)
        End If
        objReg.GetStringValue HKLM, strBaseKey & strSubKey, "DisplayVersion", version
        objReg.GetStringValue HKLM, strBaseKey & strSubKey, "InstallDate", insDate
        If (strValue <> "") and (intRet = 0) Then
            objFile.Write strValue & "," & version & "," & insDate & vbCrLf
        End If
    Next
End Function

Set objFile = objFSO.CreateTextFile(outFile,True)
writeList "SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\", objReg, objFile
writeList "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\", objReg, objFile
objFile.Close

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

















' NAME: Uninstall.vbs
' VERSION: 1.1
' AUTHOR: Ken Jones
' DATE  : 9/29/2011
' COMMENT: Uninstall applications within "Add and Remove Programs"

Option Explicit
'On Error Resume Next

'Dim FSO
Dim WSH, objReg, arrSubKeys
Dim strComputer, strKeyPath, strSubKey, strRegDisplayNameValue, strRegUninstallString, strGUID, strInstallType, strRegPublisherValue

Set WSH = CreateObject("WScript.Shell")
'Set FSO = CreateObject("Scripting.FileSystemObject")

const HKEY_LOCAL_MACHINE = &H80000002

' Remove all Adobe products: Call Uninstall("Publisher", "Adobe")
' Remove all Acrobat products: Call Uninstall("DisplayName", "Acrobat")
Call Uninstall( "Publisher", "Fujitsu" )
Call Uninstall( "DisplayName", "ScanSnap" )
Call Uninstall( "DisplayName", "FineReader" )
Call Uninstall( "DisplayName", "CardMinder" )

Function Uninstall( searchtype, searchname )
  searchtype = lcase( searchtype )
  searchname = lcase( searchname )

  strComputer = "."
  Set objReg = GetObject( _
    "winmgmts:{impersonationLevel=impersonate}!\\" & _
    strComputer & "\root\default:StdRegProv" )
  strKeyPath = "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
  objReg.EnumKey HKEY_LOCAL_MACHINE, strKeyPath, arrSubKeys

  For Each strSubKey In arrSubKeys
    objReg.GetStringValue HKEY_LOCAL_MACHINE, strKeyPath & "\" & strSubKey, "Publisher", strRegPublisherValue
    objReg.GetStringValue HKEY_LOCAL_MACHINE, strKeyPath & "\" & strSubKey, "DisplayName", strRegDisplayNameValue

    if searchtype = "publisher" then
      if instr( lcase( strregpublishervalue ), searchname ) <> 0 then
        reguninstall()
      end if
    elseif searchtype = "displayname" then
      if instr( lcase( strregdisplaynamevalue ), searchname ) <> 0 then
        reguninstall()
      end if
    end if
  Next
End Function

' Routine for registry uninstall information
sub reguninstall
  strRegUninstallString = WSH.RegRead("HKLM\" & strKeyPath & "\" & strSubKey & "\UninstallString")
  ' Install type is MsiExec
  If InStr (lcase(strRegUninstallString), lcase("MsiExec")) <> 0 Then
    strGUID = Mid( strRegUninstallString, instr(strRegUninstallString, "{"), 38 )
    WSH.Run "msiexec /x " & strGUID & " /quiet /norestart /lv ""%temp%\" & strRegDisplayNameValue & " Uninstall.log""", 0, True

    'strInstallType = "MsiExec"
    'debug_output()

  ' Install type is InstallShield
  ElseIf InStr (lcase(strRegUninstallString), lcase("InstallShield")) <> 0 Then
    WSH.Run strRegUninstallString & " -NoQuestion", 0, True

    'strInstallType = "InstallShield"
    'debug_output()

  ' Install type is unknown
  Else
    strInstallType = "Unknown"
    debug_output()
  End If
End Sub

' Used to show output with timeout
Sub debug_output
  WSH.Popup _
  "Publisher Name: " & strRegPublisherValue & vbCrLf & _
    "Display Name: " & strRegDisplayNameValue & vbCrLf & _
    "Install Type: " & strInstallType & vbCrLf & _
    "SubKey: " & strSubKey & vbCrLf & _
    "Uninstall Key: " & strRegUninstallString, _
  60, _  ' timeout
  "Unknown Installer", _
  48  ' wtf?
end sub

Select all

' vim: set wrap:

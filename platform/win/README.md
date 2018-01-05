## Available solutions

1. Standalone [AutoHotkey](https://github.com/Lexikos/AutoHotkey_L ) - does not require install nor administrator priviliges and is open source

    * https://github.com/rubo77/Portable-Keyboard-Layout (see also the [updated version of the klc2ini.pl](https://github.com/amire80/msklc_reader ) script)

1. [MSKLC]() - does require administrator priviliges to install the resulting compiled DLL layouts and everything is closed source

    * [Virtual-Key Codes](https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx ) (see [Appendix 1](https://reactos.org/wiki/Create_a_keyboard_layout ) for hints which key is which on a HW keyboard)
    * [.klc format](https://pastebin.com/UXc1ub4V ) parser in C# (disassembled from MSKLC)
    * [klc2ini.pl](https://github.com/amire80/msklc_reader ) - parses .klc files and converts them to AutoHotkey .ini format


## Useful links

* https://www.windows-noob.com/forums/topic/6701-custom-keyboard-layout/
    * `[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layouts\a0000420]`
* http://www.kbdedit.com/manual/admin_deploy.html#Language bar (`HKCU\Keyboard Layout\Preload`, `HKCU\Keyboard Layout\Substitutes`)
* Microsoft [TSF](https://msdn.microsoft.com/de-de/library/windows/desktop/ms538086(v=vs.85).aspx ) (Text Services Framework) is used by MS Office and similar, but not by many applications (in 2008 Notepad, etc.)
    * http://archives.miloush.net/michkap/archive/2005/04/16/408853.html
* http://www.sensefulsolutions.com/2010/08/how-to-fix-keyboard-shortcuts-in-klc-eg.html
* http://www.klm32.com/KbdEdit.html
* http://ahkscript.org/

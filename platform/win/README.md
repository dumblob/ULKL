## Available solutions

1. Standalone [AutoHotkey](https://github.com/Lexikos/AutoHotkey_L ) - does not require install nor administrator priviliges and is open source

    * https://github.com/rubo77/Portable-Keyboard-Layout (see also the [updated version of the klc2ini.pl](https://github.com/amire80/msklc_reader ) script)

1. [MSKLC]() - does require administrator priviliges to install the resulting compiled DLL layouts and everything is closed source

    * [Virtual-Key Codes](https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx ) (see [Appendix 1](https://reactos.org/wiki/Create_a_keyboard_layout ) for hints which key is which on a HW keyboard)
    * [.klc format](https://pastebin.com/UXc1ub4V ) parser in C# (disassembled from MSKLC)
    * [klc2ini.pl](https://github.com/amire80/msklc_reader ) - parses .klc files and converts them to AutoHotkey .ini format

## Useful links

* http://www.sensefulsolutions.com/2010/08/how-to-fix-keyboard-shortcuts-in-klc-eg.html
* http://www.klm32.com/KbdEdit.html
* http://ahkscript.org/

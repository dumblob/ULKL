## Open Issues

### LShift + RShift modifier combination doesn't work

1. add CR for 3+ levels

1. maybe enforce LF for 2- levels (for 3+ levels CR is enforced)?

1. re-read https://metacpan.org/pod/UI::KeyboardLayout#Keyboard-input-on-Windows,-Part-I:-what-is-the-kernel-doing? and come up with some hacks?

1. generate all possible `Ctrl + Shift + Alt + AltGr` combination by MSKLC and compare sources to gain more knowledge about `CharModifiers` table

1. reevaluate `CharModifiers` table by taking inspiration from https://github.com/microsoft/Windows-driver-samples/blob/master/input/layout/fe_kbds/jpn/101/kbd101.c

1. dead keys chain
    - `.klc` files support dead keys: http://archives.miloush.net/michkap/archive/2011/04/16/10154700.html
        - the last row in a `.klc` dead keys table shouldn't have any special meaning but is a good practice to allow at least for one escape hatch from a dead keys chain => I'd prefer to specify as many escape hatches as needed to make the dead keys chain feel as "non-dead" as possible
    - note dead keys probably behave differently if triggered by a modifier key => test it!
    - use `Private Use` characters - see http://www.kbdedit.com/manual/ex17_compose_key_chained_dead.html

1. try `LShift + RCtrl` and `RShift + LCtrl` instead of `LShift + RShift`

1. try IME (with invisible/non-existent UI) in addition to `.klc` with a standard Shift state
    - check it works **by default** seamlessly (only turning IME on in system settings is acceptable but it's unacceptable to turn IME on at login screen every time it'll show up)
        - at the login screen
        - for all users
    - sometimes has some issues (e.g. shows a tiny annoying button/icon in just under every input field widget)
    - sometimes does not work on password input fields

## Available general solutions

1. Standalone [AutoHotkey](https://github.com/Lexikos/AutoHotkey_L ) - does not require install nor administrator priviliges and is open source

    * https://github.com/rubo77/Portable-Keyboard-Layout (see also the [updated version of the klc2ini.pl](https://github.com/amire80/msklc_reader ) script)
    * see `SendInput` etc. in https://github.com/MicrosoftDocs/win32/blob/docs/desktop-src/inputdev/about-keyboard-input.md
    * disadvantages
        * not easily made working on login screen (and even then it's error-prone)

1. [MSKLC](https://www.microsoft.com/en-us/download/details.aspx?id=22339 ) - does require administrator priviliges to install the resulting compiled DLL layouts and everything is closed source

    * [Virtual-Key Codes](https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx ) (see [Appendix 1](https://reactos.org/wiki/Create_a_keyboard_layout ) for hints which key is which on a HW keyboard)
    * [.klc format](https://pastebin.com/UXc1ub4V ) parser in C# (hopefully similar to the one in MSKLC)
    * [klc2ini.pl](https://github.com/amire80/msklc_reader ) - parses .klc files and converts them to AutoHotkey .ini format


## Useful links

* *extended key* means it's one from the "middle group" on a full/long keyboard (which has 3 separate blocks: alphanumeric, arrow, and numeric) - see https://docs.microsoft.com/en-us/windows/win32/inputdev/about-keyboard-input
* Michael Kaplan series about Windows internals of keyboard handling https://web.archive.org/web/20101013223255/http://blogs.msdn.com/b/michkap/archive/2006/04/22/581107.aspx
* a non-Microsoft reimplementation of some of the kernel building blocks of keyboard handling on Windows - it **dumps the content of structures** etc.: https://github.com/awakecoding/Win32Keyboard
* `kbd.h` from 1991 (in contrast to the current `kbd.h` from 1995 or newer): https://gist.github.com/ozh/5340054
* thorough explanation of reasoning behind the frozen state of the Windows keyboard handling internals (probably from the author of kbdedit.com ): https://answers.microsoft.com/en-us/insider/forum/insider_wintp-insider_devices-insiderplat_pc/how-to-implement-multiple-deadkey-strokes/4ff38c09-b58c-490a-963e-3cc745dfb396
* https://github.com/MaiaVictor/OSX/tree/master/windows_keyboard (modern AHK/AutoHotKey script initialization with superior speed and less issues)
    * note AHK can then produces `.exe` files (probably self contained) - that's super handy!
* https://www.windows-noob.com/forums/topic/6701-custom-keyboard-layout/
    * `[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layouts\a0000420]`
* http://www.kbdedit.com/manual/admin_deploy.html#Language bar (`HKCU\Keyboard Layout\Preload`, `HKCU\Keyboard Layout\Substitutes`)
* Microsoft [TSF](https://msdn.microsoft.com/de-de/library/windows/desktop/ms538086(v=vs.85).aspx ) (Text Services Framework) is used by MS Office and similar, but not by many applications (in 2008 Notepad, etc.)
    * http://archives.miloush.net/michkap/archive/2005/04/16/408853.html
* Microsoft documentatin how winAPI for keyboard events looks like & behaves: https://github.com/MicrosoftDocs/win32/blob/docs/desktop-src/inputdev/about-keyboard-input.md
* http://www.sensefulsolutions.com/2010/08/how-to-fix-keyboard-shortcuts-in-klc-eg.html
* http://www.klm32.com/KbdEdit.html
* http://ahkscript.org/
* [dverty](https://github.com/chid/dvorak-qwerty/tree/master/dverty ) with simple regedit register files for better locale settings

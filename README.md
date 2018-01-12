# ULKL (Uniform Latin Keyboard Layouts)

Ever experienced any of the following issues?

* A *slow typing* feeling.
* *Tricky-accessible characters* (especially the national ones).
* The need to *learn yet another* cryptic shortcut.
* Pressing *large amount* of keys to write even the most basic characters.
* The feeling *everything is randomly scattered* and destroying the integrity of inputting anything using a keyboard.
* *Hand-pain* (wrist, carpal tunnels etc.).
* *Switching to another nationalized layout* (both in the head and in the hand-memory).
* *Weird, inefficient* layouts on smaller devices (PDAs, handhelds, netbooks, palmtops, etc.).
* Impossibility to type a character into a *password field or a security app* (happens on login screens, [HW password storage devices](https://www.crowdsupply.com/nth-dimension/signet ), HW TPM modules, etc.).
* *Other issues* with keyboard input...

ULKL mitigate these issues by introducing an all-encompassing, fully **intuitive** way of having a separate keyboard layout for **each latin-based alphabet**, while targetting 100% mutual compatibility between them (achieved through high uniformity).

The layout design idea is thus:

> Put the nationalized characters at places, where your fingers expect them to be **without** pressing any modifiers.

(modifiers are in general keys like `Shift` `Ctrl` `Alt` etc.)

It means that typing `č` will use the same finger as typing `c`. The same holds for `Č` and `C` - even here, only one modifier is needed for both cases - the `Shift`. This allows one to very quickly switch between layouts **without the need to learn** anything. Exceptions to this rule, where finger overloading would happen, still guarantee a position of the symbol logical enough to be learned in a second. The following graphics explains it on the `czed` (ltgt variant) layout.

FIXME add a few shiny pictures (animated gifs?) depicting among other things differences between czed and engd to demonstrate compatibility and the easy-to-learn property

Please note, the goal is **not** to have one *ultimate keyboard layout* (disregarding if by combining keys or by switching to ultra-high levels or both), because that leads by definition to awkward writing of many symbols, numeric expressions, digraphs, trigraphs, math, etc.

## Supported features

* the particular national alphabet fully accessible without any modifier (so called **first level layout**)
    * the absence of a modifier prevents majority of issues with password and other trusted inputs
* all non-alpha and non-numeric symbols from the [Dvorak simplified keyboard](http://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard) ```~ ! @ # $ % ^ & * ( ) { } ` [ ] " < > ? + | ' , . / = \ _ - : ;``` (convenient also for IT specialists)
* numbers
* additional frequently used symbols like ```§ @ ° non-breakable-space en-dash```
* nationalized punctuation like `„“` (always only the *writer* variant of the layout)
* **second level layout** accessible through the modifier "left shift" or through the modifier "right shift"
* **third level layout** accessible through the modifier combination "left shift and right shift"<sup>**1**</sup> (i.e. both must be pressed at the same time and either of them then released to press a different key<sup>**2**</sup>)
* **fourth level layout** and any **higher level layout** is guaranteed being exactly the same as the third level layout

<sup>**1**</sup>it's recommended to press first the shift under the hand which in the end is going to press the desired key

<sup>**2**</sup>in x11 there is a bug and the **first** pressed shift (not **either** of the two shifts) must be released to press the desired key

FIXME add FSM depicting the work with Shifts (mainly the third level) and later with other modifiers (probably just the compose key)

## Available layouts

Supported platforms include *X11* (Linux, BSD, etc.), *console* (Linux, BSD), *Mac OS*, and *Windows*. Layouts are to be found in corresponding directories: `platform/x11` `platform/console` `platform/osx` `platform/win`.

The naming convention of layouts follows the [ISO 639-2](http://www.loc.gov/standards/iso639-2/php/English_list.php) language naming standard and adds the suffix `d` as an abbreviation for `Dvorak` (as the layouts are based on the [Dvorak simplified keyboard](http://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard ) layout).

Dvorak layout surprisingly performs only [very few percent](http://mkweb.bcgsc.ca/carpalx/?popular_alternatives) worse than optimal layouts for each latin-based language, but provides by far the best overall score among latin languages while maintaining a relaxed feeling while typing (compared to other layouts). Also considering the rapidly growing amount of human input in English (compared to other languages), Dvorak as a layout primarily focused on English is a sustainable solution for decades. This makes ULKL a perfect fit for teaching [touch typing](https://en.wikipedia.org/wiki/Touch_typing ) at schools.

Example layout names:

* `czed` Czech dvorak
* `gerd` German dvorak
* `engd` English dvorak (yes, this is Dvorak, but with improvements for the 3. level as found in all other `<lang>d` layouts in this repository)
* `find` Finnish dvorak
* `slod` Slovak dvorak

Each layout has 2 variants - *ltgt* and *writer*. The *writer* variant is allowed to provide up to 4 language-specific punctuation characters (e.g. `„“`), which might be useful especially to writers at the expense of slightly less accessible `<` and `>` characters. The *ltgt* variant on the other hand does provide `<` and `>` characters well accessible just like Dvorak does. *ltgt* is the default variant.

## Usage

### Windows

1. Download binary from the `platform/win/` directory of this repository.
1. Double-click the downloaded file and follow instructions (if any).
1. Log out and log in to let the changes take effect.

### X11

* Testing (without installation)

    1. Run

        `setxkbmap -layout czed -variant ltgt -print | xkbcomp "-I$HOME/ULKL/platform/x11" - $DISPLAY`

        (you can safely ignore the warnings `No symbols defined for...` and `Key ... not found in ...`, it's common also for non-ULKL layouts).

* System-wide installation

    1. Put the layout files to `/usr/share/X11/xkb/symbols/` or create appropriate symlinks (e.g. `/usr/share/X11/xkb/symbols/find`).
    1. Run `setxkbmap czed -variant ltgt`.

### Mac OS

1. Move the keyboard layout to `/Library/Keyboard Layouts/`. Keyboard layouts in `~/Library/Keyboard Layouts/` cannot be selected in password dialogs or in the login screen.
2. Restart to apply the changes. Logging out and back in is not enough.
3. Enable the new keyboard layout from System Preferences.

To apply changes to a keyboard layout later, run `sudo touch '/Library/Keyboard Layouts'` and restart.

## TODO

* a tiny "manual" (just a paragraph in README.md?) how to setup the ULKL **including** a standardized cross-platform way of switching between ULKL layouts
* **Semver versioning** of the whole repository (not to each layout file nor to the whole platform)
* organize packages for easy installation (win -> msi/exe; osx -> dmg; x11 -> *distro-specific-e.g.-PKGBUILD*)
* fix various mistakes in the layouts
    * remove all the unnecessary symbols left from the original dvorak layout file in `platform/osx/czed.keylayout`
    * in x11 layouts do not produce any characters when AltGr is pressed together with an arbitrary key
    * in x11 layouts remove the need of cross-Shift press for switching to the 3. level
* **add support for the `Compose`/`Multi_key`/`U+2384 COMPOSITION SYMBOL` key** for many (all?) non-ASCII characters from all supported latin alphabets
    * list of symbols: `/usr/share/X11/locale/*/Compose`, https://www.internationalphoneticassociation.org/sites/default/files/IPA_Doulos_2015.pdf
    * use some existing structured method how to logically compose characters
        * as a tree of shape similarities (if ambiguous, then linquistically or in the worst case historically related)?
            * ogonek
            * dot above a character
            * "roof" above a character
        * should it be related to the dvorak layout?
        * how much and how influential should be the frequency of the particular shapes/characters?
    * which **typographical characters** will be incorporated at first
        * use a general "mode" (sometimes activated by `Ctrl + Shift + u`) for unicode character description input?
        * most/all currencies
            * $ (USD, US Dollar; yes, dollar as well - just to maintain consistency over duplication avoidance)
            * € (EUR, Euro)
            * ¥ U+00A5 (Chinese Yen)
            * (GBP, British Pound)
            * (JPY, Japanese Yen)
            * ₹ (INR, Indian Rupee)
            * ...
        * all characters already present in the **3 levels** of the layout (because someone might prefer the compose method to switching between different layouts)
            * U+2013 EN DASH
            * upper and lower indexes (`^` `_`)
        * everything else
            * per mille (U+2030) character
            * U+002D HYPHEN-MINUS
            * U+2192 RIGHTWARDS ARROW
            * U+21D2 RIGHTWARDS DOUBLE ARROW
            * U+2014 EM DASH
            * U+2012 FIGURE DASH (for ranges? or just for numbers?)
            * U+2010 HYPHEN
            * U+2011 NON-BREAKING HYPHEN
            * U+2015 HORIZONTAL BAR (semantically the same as a FRACTION SLASH)
            * U+2044 FRACTION SLASH (basically the same as a plain slash)
            * ...
* add numerical and "middle" block key definitions
* evaluate the following ideas for changes/additions
    * Shift+Backspace as Del
    * Shift+Enter as non-breakable LF (how much widespread is it?)
    * should **each** key have both Shift and DoubleShift variants?
    * take into consideration, that numbers are often written with
        * `,.` comma and period (must be **independent** from locale)
        * `=` equal
        * `*` asterisk
        * `/` slash
        * `e` or `E` designating `*10^` ("times ten power")
        * ` ` space or non-breakable-space (needed for splitting groups of numbers)
    * add characters for phonetics written in English or make it a separate layout?
    * pressing one Shift, then the other (disregarding in which order) and releasing **either** of the two pressed Shifts will switch temporarily to 3rd level for one non-Shift character
    * pressing one Shift, then the other (disregarding in which order) and releasing **both** two pressed Shifts will switch permanently to 3rd level until any Shift is pressed (without the requirement to release it before proceeding; so this pressed Shift will permanently switch off the 3rd level and will activate 2nd level as usually)
        * this 3rd level could turn on the CapsLock LED (or maybe not to prevent confusion?)
* get certification from [Ceska ergonomicka spolecnost](http://www.vubp.cz/ces/ )
* generate GIF with the 3 levels for each layout
* take a look at the following and maybe get in touch with authors
    * https://github.com/Koodimonni/OnniDvorak
    * http://dump.doomtech.net/NorskDvorakMac.zip
    * https://github.com/mitsuhiko/osx-keyboard-layouts
* support more platforms (Blackberry, Android, ...)
* write a general howto for creation of new language-specific ULKL layouts
    * moment of enlightenment: computer is there to serve us and we are there not to serve computer (we, humans, will **not** learn unnatural movements, because we can easily choose a better option)
    * Mac OS has layouts in XML with DTD -> test the layout against the DTD
* add right and left hand variants for each layout
* email Jaroslav Zaviacic
    * which layout uses Ms. Matouskova?
    * introduce ULKL, ask if they could test it (ULKL is harmonized with the current language corpus analysis etc.)
    * http://www.interinfo.org/products/prijmeni-jmeno-i/
* analyse/... corpuses of other languages/nationalities/alphabets

## Additional information

#### How to overwrite CapsLock by Escape

In *X11* either with `xmodmap -e 'clear Lock' -e 'keycode 0x42 = Escape'` or by putting

~~~~
remove Lock = Caps_Lock
keysym Caps_Lock = Escape
~~~~

to `"$HOME/.xmodmaprc"`.

In *console* e.g. by running echo `'keycode 58 = Escape Escape Escape Escape' | loadkeys -`.

#### Dash, en dash, minus, hyphen, soft hyphen, non-breaking hyphen, ...

Characters *minus* and *en dash* were chosen to be present instead of others based on the following information.

* Soft hyphen (SHY) can't be used, because in Unicode it's invisible.
* The widespread Chicago Manual of Style dictates use of *en dash* instead of *hyphen* for word joining.
* The only allowed character for ranges in typography is *en dash*.
* The ČSN standard allows use of *minus* instead of *dash*. It became common practice already in 80s.
* Manual word splitting with *hyphen* is not and will not be needed (all SW makes it automatically nowadays).

#### Motivation and reasoning for decisions made in ULKL

(The following text is in Czech because of it's history. It'll be translated as soon as someone requests it.)

Volim proto variantu, kdy uzivatel bezne pouziva vice jazykove-specifickych layoutu a mezi nimi se podle potreby prepina. Tim ovsem vznika otazka nauceni se mnoha layoutu. Resenim je zachovat kompatibilitu zakladnich znaku (napr. cele latinky) a vhodne menit pouze zbytek (lze napr. prohazet urovne, cimz se dosahne pozadovane efektivity) podle predem daneho jednoducheho vzoru.

Otazkou je, jake layouty tedy vytvorit? Dospel jsem k nazoru, ze je nejvhodnejsi cilit na dve zakladni skupiny uzivatelu klavesnic.

1) sekretarky, spisovatele, lide pisici casto ceske texty, bezni lide (napr. uzivatele socialnich siti)

2) IT uzivatel (programator, admin apod.)

Prvni skupina vyzaduje caste psani ceskych znaku (at uz malych ci velkych), interpunkcnich znamenek, cislic a nekterych specifickych znaku jako napr. paragraf, stupen, euro, dolar apod. Prave pro tuto skupinu jsem vytvoril nove rozvrzeni vychazejici z DVORAK. Efektivita psani cesky na DVORAK je mnohem vyssi nez na QWERTZ (viz. prilozene materialy a mereni spolu s daty z Ceskeho jazykoveho korpusu) a *nikoliv* jak je napr. uvadeno na webu http://www.uklavesnice.estranky.cz/clanky/klavesnice.html .

Druha skupina je jiz z principu schopna si sama prehodit layout, naucit se neco noveho na pocitaci apod. Tam neni potreba nic menit a vymyslet. Bud si sami vytvori vlastni layout a nebo budou pouzivat ISO dvorak, z nehoz vychazi prave tyto nove narodnostne-specificke varianty Dvorak.

Podle meho nazoru je zbytecne zabyvat se prohazovanim Y/Z (jak se tim zabyva napr. web http://www.ceskaklavesnice.cz/historie), prestoze tato malickost cloveka muze mirne otravovat. Avsak jedna se o tak nepatrny detail, ktery je kazdy kdo pise vsemi deseti schopen (docasne) udrzet v hlave. Pripadne si prepne na druhou variantu (kazdy bezne dostupny OS jiz mnoho let umi bezproblemove prepinat mezi rozlozenimi).

Pri navrhu czd byla rovnou udelana reserse abeced pouzivajicich latinsky zaklad a bylo zjisteno, ze slovenska abeceda obsahuje nejvice pismen a ceska je hned druha v poradi. Spolu s tim byla navrzena jednoducha metoda jak vytvaret narodni klavesnicova rozlozeni pro jazyky pouzivajici jako zaklad latinku. A\ protoze se podarilo uspesne vytvorit slovenskou a ceskou variantu dvorak, koncept zarucuje, ze danou metodou lze v pripade ostatnich latinskych abeced dosahnout nemene kvalitnich vysledku.

## References

[French AZERTY layout is totally unusable in practice](http://arstechnica.com/tech-policy/2016/01/france-says-azerty-keyboards-fail-french-typists/ )

[French BÉPO](https://en.wikipedia.org/wiki/Keyboard_layout#B.C3.89PO ) - Dvorak-based layout

[Swedish Dvorak layout](https://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard#Svorak )

[Ivan Pascal - xkb internals](http://pascal.tsu.ru/en/xkb/internals.html#wrap )

[Creating custom keyboard layouts for X11 using XKB](http://michal.kosmulski.org/computing/articles/custom-keyboard-layouts-xkb.html )

X upstream: xkeyboard-config-2.10.1/rules/compat/layoutsMapping.lst

[huge XCompose configuration](https://github.com/rrthomas/pointless-xcompose )

[Ukelele key layout instructions for Mac OS](http://osxnotes.net/keylayout-files-and-ukelele.html )

[Ukrainian example of an Ukelele layout for Mac OS](https://github.com/palmerc/Ukrainian-Russian/blob/master/Ukrainian )

[Apple documentation about Mac OS layouts](https://developer.apple.com/library/mac/technotes/tn2056/_index.html )

[MessagEase mobile keyboard](https://www.exideas.com/ME )

[Czech National Corpus](http://ucnk.ff.cuni.cz/syn2010.php ) (maintained by FFUK)

[UCWCS X11 layout](http://kam.mff.cuni.cz/~pasky/dev/debian/lenny/ucwcs-xkb/ucwcs-xkb/README.CZ )

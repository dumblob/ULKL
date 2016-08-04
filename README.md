# ULKL (Uniform Latin Keyboard Layouts)

Ever experienced problems like *slow typing* or *hand-pain* (wrist, carpal tunnels etc.) or *switching to another nationalized layout both in the head and the hand-memory* or *tricky-accessible characters* (especially the national ones)?

This project offers you a solution which mitigates among others the above-mentioned issues by introducing an **easy-to-remember** and **easy-to-learn** way of having conceptually uniform keyboard layout for each latin-based language.

The layout design idea is thus:

> put the nationalized characters at places, where your fingers expect them to be **without pressing any modifiers**

Modifiers are keys like `Shift` `Ctrl` `Alt` etc. In other words, typing `č` will use the same finger as typing `c`. The same holds for `Č` and `C` - even here, only one modifier is needed for both cases - the `Shift`. This allows one to very quickly switch between layouts without the need to learn anything. Exceptions to this rule, where finger overloading would happen, still guarantee a position of the symbol enough logical to be learned in a second. The following graphics explains it on the `czed` (ltgt variant) example.

FIXME add here a colorful picture (an animated gif?)

Please note, the goal is **not** to have one *ultimate keyboard layout* (disregarding if by combining keys or by switching to ultra-high levels or both), because that leads by definition to awkward writing of many symbols, numeric expressions, digraphs, trigraphs, math, etc.

## Supported features

* the particular national alphabet
* all non-alpha and non-numeric symbols from the [Dvorak simplified keyboard](http://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard) (useful not only for IT people)
* numbers
* additional frequently used symbols like ```§ @ ° € ` non-breakable-space```
* nationalized punctuation like `„“` (always only the *writer* variant of the layout)

## Available layouts

Supported platforms include *X11* (Linux, BSD, etc.), *console* (Linux, BSD), *MacOSX*, and *Windows*. Layouts are to be found in corresponding directories: `platform/x11` `platform/console` `platform/osx` `platform/win`.

The naming convention of layouts follows the [ISO 639-2](http://www.loc.gov/standards/iso639-2/php/English_list.php) language naming standard and adds the suffix `d` as an abbreviation for `Dvorak` as the layouts are based on the [Dvorak simplified keyboard](http://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard) layout.

Dvorak layout surprisingly performs only [very few percent](http://mkweb.bcgsc.ca/carpalx/?popular_alternatives) worse than optimal layouts for each latin-based language, but provides by far the best overall score among latin languages. Also considering the growing amount of human input in English (compared to other languages), Dvorak as a layout primarily focused on English is a sustainable layout solution for decades. Thus allowing us to teach this layout also at schools.

Example names:
* `czed` Czech dvorak
* `gerd` German dvorak
* `engd` English dvorak (yes, this is Dvorak, but with improvements for the 3. level as found in all other `<lang>d` layouts in this repository)
* `find` Finnish dvorak
* `slod` Slovak dvorak

Each layout has 2 variants - *ltgt* and *writer*. The *writer* variant is allowed to provide about 4 characters (e.g. `„“`), which might be useful especially to writers at the expense of slightly less accessible `<` and `>` characters. The *ltgt* variant on the other hand does provide `<` and `>` characters well accessible just like Dvorak does. *ltgt* is the default variant.

## Usage

### X11

For testing purposes:

`setxkbmap -layout czed -variant ltgt -print | xkbcomp "-I$HOME/ULKL/platform/x11" - $DISPLAY`

(you can safely ignore the warnings *No symbols defined for...* and *Key ... not found in ...*, it's normal also for other layouts)

Or system-wide by putting the layout files to `/usr/share/X11/xkb/symbols/` or creating appropriate symlinks (e.g. `/usr/share/X11/xkb/symbols/find`) and running:

`setxkbmap czed -variant ltgt`

### Mac OS X

Instructions stolen from http://osxnotes.net/keylayout-files-and-ukelele.html .

1. Move the keyboard layout to `/Library/Keyboard Layouts/`. Keyboard layouts in `~/Library/Keyboard Layouts/` cannot be selected in password dialogs or in the login screen.
2. Restart to apply the changes. Logging out and back in is not enough.
3. Enable the new keyboard layout from System Preferences.

To apply changes to a keyboard layout later, run `sudo touch '/Library/Keyboard Layouts'` and restart.

## TODO

* **ADD Semver versioning**
* evaluate following ideas for changes
    * prohodit t\_hacek a n\_hacek
    * cisla se musi psat spolu s
        * `,.` desetinnou carkou + teckou (nesmi to byt zavisle na locale)
        * `=` rovnitkem
        * `*` nasobeni
        * `/` deleni
        * `e` nebo E jako *10^
        * ` ` mezerou! (potrebuji oddelovat skupinky cisel!)
    * CapsLock -> Esc
    * pridat znaky\_pro\_fonetiku\_v\_anglictine?
    * pridat tvrdou mezeru a dalsi vychytavky z me ceske!
    * vyzkouset prohazet cisla jako v puvodnim Dvorak navrhu!
    * Shift+Backspace = Del
    * !!! co takhle aby kazda klavesa mela Shift variantu a DoubleShift variantu?
* fix X11 layouts to not produce any characters when AltGr is pressed together with an arbitrary key
* get certification from [Ceska ergonomicka spolecnost](http://www.vubp.cz/ces/)
* add a gif picture showing differences between czed and engd to demonstrate compatibility and the easy-to-learn property
* take a look at https://github.com/Koodimonni/OnniDvorak and maybe get into contact with the author
* take a look at https://github.com/chid/dvorak-qwerty/tree/master/dverty and finish the Windows version
* add versioning to the whole repository (not to each layout file nor to the whole platform)
* add packages for easy installation (win -> msi; osx -> dmg; x11 -> *distro-specific-e.g.-PKGBUILD*)
* correct mistakes in `platform/osx/czed.keylayout` (remove all the unnecessary symbols left from the original dvorak layout file)
* finally add `engd`
* support more platforms (Blackberry, Android, ...)
* rewrite and structure the "Brief explanation of other reasons behind this idea" part of this readme
* write a general howto for creation of new language-specific ULKL layouts
    * motto: computer is there to serve us and we are there not to serve computer (we, humans, will **not** learn unnatural movements, because we can easily choose a better option)
* email Jaroslav Zaviacic
    * which layout uses Ms. Matouskova
    * introduce ULKL, ask if they could test it, ULKL was created with analyses of the newest language corpus from CUNI in mind etc.
    * http://www.interinfo.org/products/prijmeni-jmeno-i/

## Miscellaneous

#### Overwrite CapsLock by Escape

In *X11* either with `xmodmap -e 'clear Lock' -e 'keycode 0x42 = Escape'` or by putting

~~~~
remove Lock = Caps_Lock
keysym Caps_Lock = Escape
~~~~

to `"$HOME/.xmodmaprc"`.

In *console* e.g. by running echo `'keycode 58 = Escape Escape Escape Escape' | loadkeys -`.

## Motivation and reasoning for decisions made in ULKL

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

[French AZERTY layout is totally unusable in practice](http://arstechnica.com/tech-policy/2016/01/france-says-azerty-keyboards-fail-french-typists/)

[French BÉPO](https://en.wikipedia.org/wiki/Keyboard_layout#B.C3.89PO ) - Dvorak-based layout

[Swedish Dvorak layout](https://en.wikipedia.org/wiki/Dvorak_Simplified_Keyboard#Svorak )

[Ivan Pascal - xkb internals](http://pascal.tsu.ru/en/xkb/internals.html#wrap)

[Creating custom keyboard layouts for X11 using XKB](http://michal.kosmulski.org/computing/articles/custom-keyboard-layouts-xkb.html)

[huge XCompose configuration](https://github.com/rrthomas/pointless-xcompose)

[Ukrainian example of Ukelele layout (Mac OS X)](https://github.com/palmerc/Ukrainian-Russian/blob/master/Ukrainian)

[Apple documentation about Mac OS X layouts](https://developer.apple.com/library/mac/technotes/tn2056/_index.html)

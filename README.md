What?!
======

Ever experienced problems with *slow typing* or *hand-pain* (wrist, carpal tunnels etc.) or *switching to another nationalized layout in your head and hand-memory* or *tricky-accessible characters* (especially the national-specific ones)?

This project offers you a solution which mitigates the above-mentioned issues by introducing a easy-to-remember and easy-to-learn unified approach to creating nationalized but conceptually-uniform keyboard layouts.

Structure of this directory
---------------------------

### X11 layout files

*   `czd` czech dvorak
*   `fid` finnish dvorak
*   `ged` german dvorak, *in progress (not implemented yet)*
*   `...` ... dvorak
*   `dvorakng` dvorak next generation, *in progress (not implemented yet)*

### Unix/Linux/BSD console layout files

*In progress (not implemented yet).* (binary special files?)

### Windows layout files

*In progress (not implemented yet).* (binary special files?)

### MacOSX layout files

*In progress (not implemented yet).* (XML files?)

Usage
-----

### Direct usage

X11

`setxkbmap -print -I/path/to/a/layout/file -layout czd -variant xml | xkbcomp - "$DISPLAY"`

### System-wide usage

X11

Possible after putting the layout files to `/usr/share/X11/xkb/symbols/` or creating appropriate symlinks (e.g. `/usr/share/X11/xkb/symbols/fid`).

`setxkbmap czd -variant xml`

`setxkbmap fid`

### Overwrite CapsLock by Escape

X11

`setxkbmap dvorak; xmodmap -e 'clear Lock' -e 'keycode 0x42 = Escape'`

Motto
-----

The goal is to *not* create an *ultimate keyboard layout* (either by combining keys or by switching to ultra-high levels or both), because that will by definition lead to unpleasant writing of many symbols, digraphs, trigraphs etc.

Elaboration explaining the reasons behind this idea/attitude
------------------------------------------------------------

(The following text is in Czech because of it\`s history. It'll be translated as soon as someone requests it.)

Volim proto variantu, kdy uzivatel bezne pouziva vice jazykove-specifickych layoutu a mezi nimi se podle potreby prepina. Tim ovsem vznika otazka nauceni se mnoha layoutu. Resenim je zachovat kompatibilitu zakladnich znaku (napr. cele latinky) a vhodne menit pouze zbytek (lze napr. prohazet urovne, cimz se dosahne pozadovane efektivity) podle predem daneho jednoducheho vzoru.

Otazkou je, jake layouty tedy vytvorit? Dospel jsem k nazoru, ze je nejvhodnejsi cilit na dve zakladni skupiny uzivatelu klavesnic.

1) sekretarky, spisovatele, lide pisici casto ceske texty, bezni lide (napr. uzivatele socialnich siti)

2) IT uzivatel (programator, admin apod.)

Prvni skupina vyzaduje caste psani ceskych znaku (at uz malych ci velkych), interpunkcnich znamenek, cislic a nekterych specifickych znaku jako napr. paragraf, stupen, euro, dolar apod. Prave pro tuto skupinu jsem vytvoril nove rozvrzeni vychazejici z DVORAK. Efektivita psani cesky na DVORAK je mnohem vyssi nez na QWERTZ (viz. prilozene materialy a mereni spolu s daty z Ceskeho jazykoveho korpusu) a *nikoliv* jak je napr. uvadeno na webu http://www.uklavesnice.estranky.cz/clanky/klavesnice.html .

Druha skupina je jiz z principu schopna si sama prehodit layout, naucit se neco noveho na pocitaci apod. Tam neni potreba nic menit a vymyslet. Bud si sami vytvori vlastni layout a nebo budou pouzivat ISO dvorak, z nehoz vychazi prave tyto nove narodnostne-specificke varianty Dvorak.

Podle meho nazoru je zbytecne zabyvat se prohazovanim Y/Z (jak se tim zabyva napr. web http://www.ceskaklavesnice.cz/historie), prestoze tato malickost cloveka muze mirne otravovat. Avsak jedna se o tak nepatrny detail, ktery je kazdy kdo pise vsemi deseti schopen (docasne) udrzet v hlave. Pripadne si prepne na druhou variantu (kazdy bezne dostupny OS jiz mnoho let umi bezproblemove prepinat mezi rozlozenimi).

Pri navrhu czd byla rovnou udelana reserse abeced pouzivajicich latinsky zaklad a bylo zjisteno, ze slovenska abeceda obsahuje nejvice pismen a ceska je hned druha v poradi. Spolu s tim byla navrzena jednoducha metoda jak vytvaret narodni klavesnicova rozlozeni pro jazyky pouzivajici jako zaklad latinku. A\ protoze se podarilo uspesne vytvorit slovenskou a ceskou variantu dvorak, koncept zarucuje, ze danou metodou lze v pripade ostatnich latinskych abeced dosahnout nemene kvalitnich vysledku.

References
----------

[Ivan Pascal - xkb internals](http://pascal.tsu.ru/en/xkb/internals.html#wrap)
[huge XCompose configuration](https://github.com/rrthomas/pointless-xcompose)

/****************************** Module Header ******************************\
* Module Name: gerdLTGT.H
*
* keyboard layout header
*
* Copyright (c) 1985-2001, Microsoft Corporation
*
* Various defines for use by keyboard input code.
*
* History:
*
* created by KBDTOOL v3.40 Mon May 29 08:05:02 2017
*
\***************************************************************************/

/*
 * kbd type should be controlled by cl command-line argument
 */
#define KBD_TYPE 4

/*
* Include the basis of all keyboard table values
*/
#include "kbd.h"
/***************************************************************************\
* The table below defines the virtual keys for various keyboard types where
* the keyboard differ from the US keyboard.
*
* _EQ() : all keyboard types have the same virtual key for this scancode
* _NE() : different virtual keys for this scancode, depending on kbd type
*
*     +------+ +----------+----------+----------+----------+----------+----------+
*     | Scan | |    kbd   |    kbd   |    kbd   |    kbd   |    kbd   |    kbd   |
*     | code | |   type 1 |   type 2 |   type 3 |   type 4 |   type 5 |   type 6 |
\****+-------+_+----------+----------+----------+----------+----------+----------+*/

#undef  T0C
 #define T0C _EQ(                                      OEM_4                      )
#undef  T0D
 #define T0D _EQ(                                      OEM_6                      )
#undef  T10
 #define T10 _EQ(                                      OEM_7                      )
#undef  T11
 #define T11 _EQ(                                  OEM_COMMA                      )
#undef  T12
 #define T12 _EQ(                                 OEM_PERIOD                      )
#undef  T13
 #define T13 _EQ(                                        'P'                      )
#undef  T14
 #define T14 _EQ(                                        'Y'                      )
#undef  T15
 #define T15 _EQ(                                        'F'                      )
#undef  T16
 #define T16 _EQ(                                        'G'                      )
#undef  T17
 #define T17 _EQ(                                        'C'                      )
#undef  T18
 #define T18 _EQ(                                        'R'                      )
#undef  T19
 #define T19 _EQ(                                        'L'                      )
#undef  T1A
 #define T1A _EQ(                                      OEM_2                      )
#undef  T1B
 #define T1B _EQ(                                   OEM_PLUS                      )
#undef  T1F
 #define T1F _EQ(                                        'O'                      )
#undef  T20
 #define T20 _EQ(                                        'E'                      )
#undef  T21
 #define T21 _EQ(                                        'U'                      )
#undef  T22
 #define T22 _EQ(                                        'I'                      )
#undef  T23
 #define T23 _EQ(                                        'D'                      )
#undef  T24
 #define T24 _EQ(                                        'H'                      )
#undef  T25
 #define T25 _EQ(                                        'T'                      )
#undef  T26
 #define T26 _EQ(                                        'N'                      )
#undef  T27
 #define T27 _EQ(                                        'S'                      )
#undef  T28
 #define T28 _EQ(                                  OEM_MINUS                      )
#undef  T2C
 #define T2C _EQ(                                      OEM_1                      )
#undef  T2D
 #define T2D _EQ(                                        'Q'                      )
#undef  T2E
 #define T2E _EQ(                                        'J'                      )
#undef  T2F
 #define T2F _EQ(                                        'K'                      )
#undef  T30
 #define T30 _EQ(                                        'X'                      )
#undef  T31
 #define T31 _EQ(                                        'B'                      )
#undef  T33
 #define T33 _EQ(                                        'W'                      )
#undef  T34
 #define T34 _EQ(                                        'V'                      )
#undef  T35
 #define T35 _EQ(                                        'Z'                      )


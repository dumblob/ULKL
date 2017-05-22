using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
namespace Microsoft.Globalization.Tools.KeyboardLayoutCreator
{
  internal class KeyboardTextFile
  {
    private string[] KeyWords = new string[]
    {
      "KBD",
        "VERSION",
        "COPYRIGHT",
        "COMPANY",
        "LOCALEID",
        "LOCALENAME",
        "MODIFIERS",
        "SHIFTSTATE",
        "ATTRIBUTES",
        "LAYOUT",
        "DEADKEY",
        "LIGATURE",
        "KEYNAME",
        "KEYNAME_EXT",
        "KEYNAME_DEAD",
        "DESCRIPTIONS",
        "LANGUAGENAMES",
        "ENDKBD"
    };
    private string m_stFileName;
    private string m_stFile = "";
    private string m_stCompany;
    private string m_stCopyright;
    private string m_stLocaleId;
    private string m_stLocaleName;
    private string m_stVersion;
    private string m_stName;
    private string m_stDescription;
    private ArrayList m_alModifiers = new ArrayList();
    private ArrayList m_alKeyName = new ArrayList();
    private ArrayList m_alKeyName_Ext = new ArrayList();
    private ArrayList m_alKeyName_Dead = new ArrayList();
    private ArrayList m_alShiftState;
    private ArrayList m_alLigature;
    private ArrayList m_alAttributes;
    private ArrayList m_alDeadKeys;
    private ArrayList m_alDescriptions;
    private ArrayList m_alLanguageNames;
    private SortedList m_slLayout = new SortedList();
    internal string LOCALEID
    {
      get
      {
        if (this.m_stLocaleId == null || this.m_stLocaleId.Length == 0)
        {
          this.m_stLocaleId = "";
          int length = "LOCALEID".Length;
          int num = this.m_stFile.IndexOf("LOCALEID");
          if (num != -1)
          {
            int num2 = this.m_stFile.IndexOf('\r', num + 1);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              this.m_stLocaleId = text.Trim(new char[]
                  {
                  ' ',
                  '"'
                  });
            }
          }
          this.m_stLocaleId = "00000000" + this.m_stLocaleId;
          this.m_stLocaleId = this.m_stLocaleId.Substring(this.m_stLocaleId.Length - 8);
        }
        return this.m_stLocaleId;
      }
    }
    internal string LOCALENAME
    {
      get
      {
        if (this.m_stLocaleName == null)
        {
          int length = "LOCALENAME".Length;
          int num = this.m_stFile.IndexOf("LOCALENAME");
          if (num != -1)
          {
            int num2 = this.m_stFile.IndexOf('\r', num + 1);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              this.m_stLocaleName = text.Trim(new char[]
                  {
                  ' ',
                  '"'
                  });
            }
          }
          if (this.m_stLocaleName == null)
          {
            this.m_stLocaleName = "";
          }
        }
        return this.m_stLocaleName;
      }
    }
    internal string COPYRIGHT
    {
      get
      {
        if (this.m_stCopyright == null)
        {
          int length = "COPYRIGHT".Length;
          int num = this.m_stFile.IndexOf("COPYRIGHT");
          if (num != -1)
          {
            int num2 = this.m_stFile.IndexOf('\r', num + 1);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              this.m_stCopyright = text.Trim(new char[]
                  {
                  ' ',
                  '"'
                  });
            }
          }
          if (this.m_stCopyright == null)
          {
            this.m_stCopyright = "";
          }
        }
        return this.m_stCopyright;
      }
    }
    internal string COMPANY
    {
      get
      {
        if (this.m_stCompany == null)
        {
          int length = "COMPANY".Length;
          int num = this.m_stFile.IndexOf("COMPANY");
          if (num != -1)
          {
            int num2 = this.m_stFile.IndexOf('\r', num + 1);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              this.m_stCompany = text.Trim(new char[]
                  {
                  ' ',
                  '"'
                  });
            }
          }
          if (this.m_stCompany == null)
          {
            this.m_stCompany = "";
          }
        }
        return this.m_stCompany;
      }
    }
    internal string VERSION
    {
      get
      {
        if (this.m_stVersion == null)
        {
          int length = "VERSION".Length;
          int num = this.m_stFile.IndexOf("VERSION");
          if (num != -1)
          {
            int num2 = this.m_stFile.IndexOf('\r', num + 1);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              this.m_stVersion = text.Trim(new char[]
                  {
                  ' ',
                  '"'
                  });
            }
          }
          if (this.m_stVersion == null)
          {
            this.m_stVersion = "";
          }
        }
        return this.m_stVersion;
      }
    }
    internal string NAME
    {
      get
      {
        if (this.m_stName == null)
        {
          this.FillHeaderVariables();
        }
        return this.m_stName;
      }
    }
    internal string DESCRIPTION
    {
      get
      {
        if (this.m_stDescription == null)
        {
          this.FillHeaderVariables();
        }
        return this.m_stDescription;
      }
    }
    internal ArrayList MODIFIERS
    {
      get
      {
        if (this.m_alModifiers == null || this.m_alModifiers.Count == 0)
        {
          int length = "MODIFIERS".Length;
          int num = this.m_stFile.IndexOf("MODIFIERS");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              do
              {
                int num3 = text.IndexOf(' ');
                if (num3 != -1)
                {
                  this.m_alModifiers.Add(new ValuePair(text.Substring(0, num3 - 1).TrimStart(new char[0]), text.Substring(num3 + 1).Trim(new char[]
                          {
                          ' ',
                          '"'
                          })));
                }
                text = text.Substring(text.IndexOf('\r') + 2);
              }
              while (text.Length > 0);
            }
          }
          int arg_E0_0 = this.m_alModifiers.Count;
        }
        return this.m_alModifiers;
      }
    }
    internal ArrayList SHIFTSTATE
    {
      get
      {
        if (this.m_alShiftState == null || this.m_alShiftState.Count == 0)
        {
          int length = "SHIFTSTATE".Length;
          int num = this.m_stFile.IndexOf("SHIFTSTATE");
          this.m_alShiftState = new ArrayList();
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i] != null && array[i].Length > 0)
                {
                  int num3 = array[i].IndexOf(";");
                  if (num3 != -1 && num3 != 0)
                  {
                    array[i] = array[i].Substring(0, num3 - 1);
                    this.m_alShiftState.Add(Convert.ToByte(array[i], Utilities.s_nfi));
                  }
                  else
                  {
                    this.m_alShiftState.Add(Convert.ToByte(array[i], Utilities.s_nfi));
                  }
                }
              }
              this.m_alShiftState.TrimToSize();
            }
          }
          int arg_145_0 = this.m_alShiftState.Count;
        }
        return this.m_alShiftState;
      }
    }
    internal SortedList LAYOUT
    {
      get
      {
        if (this.m_slLayout == null || this.m_slLayout.Count == 0)
        {
          int length = "LAYOUT".Length;
          int num = this.m_stFile.IndexOf("LAYOUT");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  string[] array2 = array[i].Split(new char[]
                      {
                      ' '
                      });
                  if (array2.Length >= 2)
                  {
                    LayoutRow layoutRow = null;
                    if (array2.Length > 2)
                    {
                      layoutRow = new LayoutRow(array2[1], array2[0]);
                      if (array2[2].Equals("SGCap"))
                      {
                        layoutRow.CapsInfo = 0;
                        layoutRow.SGCap = true;
                      }
                      else
                      {
                        layoutRow.CapsInfo = byte.Parse(array2[2], NumberStyles.AllowHexSpecifier, Utilities.s_nfi);
                      }
                      for (int j = 3; j < array2.Length; j++)
                      {
                        layoutRow.AddCharValue(array2[j]);
                      }
                      if (layoutRow.SGCap)
                      {
                        layoutRow.AddCharValue(65535u);
                        i++;
                        string[] array3 = array[i].TrimStart(new char[]
                            {
                            ' '
                            }).Split(new char[]
                              {
                              ' '
                              });
                        for (int k = 3; k < array3.Length; k++)
                        {
                          if (array3[k].Length > 0)
                          {
                            layoutRow.AddCharValue(array3[k]);
                          }
                        }
                      }
                    }
                    if (layoutRow != null && !this.m_slLayout.ContainsKey(layoutRow.Vk) && array2.Length > 2)
                    {
                      this.m_slLayout.Add(layoutRow.Vk, layoutRow);
                    }
                  }
                }
              }
            }
          }
          if (this.m_slLayout != null)
          {
            int arg_227_0 = this.m_slLayout.Count;
          }
        }
        return this.m_slLayout;
      }
    }
    internal ArrayList LIGATURE
    {
      get
      {
        if (this.m_alLigature == null || this.m_alLigature.Count == 0)
        {
          int length = "LIGATURE".Length;
          int num = this.m_stFile.IndexOf("LIGATURE");
          this.m_alLigature = new ArrayList();
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  string[] array2 = array[i].Split(new char[]
                      {
                      ' '
                      });
                  if (array2.Length >= 4)
                  {
                    byte mod = (byte)this.SHIFTSTATE[(int)byte.Parse(array2[1], CultureInfo.InvariantCulture)];
                    LigatureRow value;
                    if (array2.Length == 4)
                    {
                      value = new LigatureRow(array2[0], mod, new string[]
                          {
                          array2[2],
                          array2[3]
                          });
                    }
                    else
                    {
                      if (array2.Length == 5)
                      {
                        value = new LigatureRow(array2[0], mod, new string[]
                            {
                            array2[2],
                            array2[3],
                            array2[4]
                            });
                      }
                      else
                      {
                        value = new LigatureRow(array2[0], mod, new string[]
                            {
                            array2[2],
                            array2[3],
                            array2[4],
                            array2[5]
                            });
                      }
                    }
                    this.m_alLigature.Add(value);
                  }
                }
              }
              this.m_alLigature.TrimToSize();
            }
          }
          int arg_1D6_0 = this.m_alLigature.Count;
        }
        return this.m_alLigature;
      }
    }
    internal ArrayList ATTRIBUTES
    {
      get
      {
        if (this.m_alAttributes == null || this.m_alAttributes.Count == 0)
        {
          int length = "ATTRIBUTES".Length;
          int num = this.m_stFile.IndexOf("ATTRIBUTES");
          this.m_alAttributes = new ArrayList();
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  this.m_alAttributes.Add(array[i]);
                }
              }
              this.m_alAttributes.TrimToSize();
            }
          }
          int arg_E8_0 = this.m_alAttributes.Count;
        }
        return this.m_alAttributes;
      }
    }
    internal ArrayList KEYNAME
    {
      get
      {
        if (this.m_alKeyName == null)
        {
          this.m_alKeyName = new ArrayList();
        }
        if (this.m_stFile.Length > 0 && this.m_alKeyName.Count == 0)
        {
          int length = "KEYNAME".Length;
          int num = this.m_stFile.IndexOf("KEYNAME");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  int num3 = array[i].IndexOf(' ', 1);
                  if (num3 != -1)
                  {
                    this.m_alKeyName.Add(new ValuePair(array[i].Substring(0, num3), array[i].Substring(num3 + 1).Trim(new char[]
                            {
                            ' '
                            })));
                  }
                }
              }
            }
          }
        }
        if (this.m_alKeyName.Count == 0)
        {
          this.m_alKeyName.Add(new ValuePair("01", "Esc"));
          this.m_alKeyName.Add(new ValuePair("0e", "Backspace"));
          this.m_alKeyName.Add(new ValuePair("0f", "Tab"));
          this.m_alKeyName.Add(new ValuePair("1c", "Enter"));
          this.m_alKeyName.Add(new ValuePair("1d", "Ctrl"));
          this.m_alKeyName.Add(new ValuePair("2a", "Shift"));
          this.m_alKeyName.Add(new ValuePair("36", "\"Right Shift\""));
          this.m_alKeyName.Add(new ValuePair("37", "\"Num *\""));
          this.m_alKeyName.Add(new ValuePair("38", "Alt"));
          this.m_alKeyName.Add(new ValuePair("39", "Space"));
          this.m_alKeyName.Add(new ValuePair("3a", "\"Caps Lock\""));
          this.m_alKeyName.Add(new ValuePair("3b", "F1"));
          this.m_alKeyName.Add(new ValuePair("3c", "F2"));
          this.m_alKeyName.Add(new ValuePair("3d", "F3"));
          this.m_alKeyName.Add(new ValuePair("3e", "F4"));
          this.m_alKeyName.Add(new ValuePair("3f", "F5"));
          this.m_alKeyName.Add(new ValuePair("40", "F6"));
          this.m_alKeyName.Add(new ValuePair("41", "F7"));
          this.m_alKeyName.Add(new ValuePair("42", "F8"));
          this.m_alKeyName.Add(new ValuePair("43", "F9"));
          this.m_alKeyName.Add(new ValuePair("44", "F10"));
          this.m_alKeyName.Add(new ValuePair("45", "Pause"));
          this.m_alKeyName.Add(new ValuePair("46", "\"Scroll Lock\""));
          this.m_alKeyName.Add(new ValuePair("47", "\"Num 7\""));
          this.m_alKeyName.Add(new ValuePair("48", "\"Num 8\""));
          this.m_alKeyName.Add(new ValuePair("49", "\"Num 9\""));
          this.m_alKeyName.Add(new ValuePair("4a", "\"Num -\""));
          this.m_alKeyName.Add(new ValuePair("4b", "\"Num 4\""));
          this.m_alKeyName.Add(new ValuePair("4c", "\"Num 5\""));
          this.m_alKeyName.Add(new ValuePair("4d", "\"Num 6\""));
          this.m_alKeyName.Add(new ValuePair("4e", "\"Num +\""));
          this.m_alKeyName.Add(new ValuePair("4f", "\"Num 1\""));
          this.m_alKeyName.Add(new ValuePair("50", "\"Num 2\""));
          this.m_alKeyName.Add(new ValuePair("51", "\"Num 3\""));
          this.m_alKeyName.Add(new ValuePair("52", "\"Num 0\""));
          this.m_alKeyName.Add(new ValuePair("53", "\"Num Del\""));
          this.m_alKeyName.Add(new ValuePair("54", "\"Sys Req\""));
          this.m_alKeyName.Add(new ValuePair("57", "F11"));
          this.m_alKeyName.Add(new ValuePair("58", "F12"));
          this.m_alKeyName.Add(new ValuePair("7c", "F13"));
          this.m_alKeyName.Add(new ValuePair("7d", "F14"));
          this.m_alKeyName.Add(new ValuePair("7e", "F15"));
          this.m_alKeyName.Add(new ValuePair("7f", "F16"));
          this.m_alKeyName.Add(new ValuePair("80", "F17"));
          this.m_alKeyName.Add(new ValuePair("81", "F18"));
          this.m_alKeyName.Add(new ValuePair("82", "F19"));
          this.m_alKeyName.Add(new ValuePair("83", "F20"));
          this.m_alKeyName.Add(new ValuePair("84", "F21"));
          this.m_alKeyName.Add(new ValuePair("85", "F22"));
          this.m_alKeyName.Add(new ValuePair("86", "F23"));
          this.m_alKeyName.Add(new ValuePair("87", "F24"));
        }
        return this.m_alKeyName;
      }
    }
    internal ArrayList KEYNAME_EXT
    {
      get
      {
        if (this.m_alKeyName_Ext == null)
        {
          this.m_alKeyName_Ext = new ArrayList();
        }
        if (this.m_stFile.Length > 0 && this.m_alKeyName_Ext.Count == 0)
        {
          int length = "KEYNAME_EXT".Length;
          int num = this.m_stFile.IndexOf("KEYNAME_EXT");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  int num3 = array[i].IndexOf(' ', 1);
                  if (num3 != -1)
                  {
                    this.m_alKeyName_Ext.Add(new ValuePair(array[i].Substring(0, num3), array[i].Substring(num3 + 1).Trim(new char[]
                            {
                            ' '
                            })));
                  }
                }
              }
            }
          }
        }
        if (this.m_alKeyName_Ext.Count == 0)
        {
          this.m_alKeyName_Ext.Add(new ValuePair("1c", "\"Num Enter\""));
          this.m_alKeyName_Ext.Add(new ValuePair("1d", "\"Right Ctrl\""));
          this.m_alKeyName_Ext.Add(new ValuePair("35", "\"Num /\""));
          this.m_alKeyName_Ext.Add(new ValuePair("37", "\"Prnt Scrn\""));
          this.m_alKeyName_Ext.Add(new ValuePair("38", "\"Right Alt\""));
          this.m_alKeyName_Ext.Add(new ValuePair("45", "\"Num Lock\""));
          this.m_alKeyName_Ext.Add(new ValuePair("46", "Break"));
          this.m_alKeyName_Ext.Add(new ValuePair("47", "Home"));
          this.m_alKeyName_Ext.Add(new ValuePair("48", "Up"));
          this.m_alKeyName_Ext.Add(new ValuePair("49", "\"Page Up\""));
          this.m_alKeyName_Ext.Add(new ValuePair("4b", "Left"));
          this.m_alKeyName_Ext.Add(new ValuePair("4d", "Right"));
          this.m_alKeyName_Ext.Add(new ValuePair("4f", "End"));
          this.m_alKeyName_Ext.Add(new ValuePair("50", "Down"));
          this.m_alKeyName_Ext.Add(new ValuePair("51", "\"Page Down\""));
          this.m_alKeyName_Ext.Add(new ValuePair("52", "Insert"));
          this.m_alKeyName_Ext.Add(new ValuePair("53", "Delete"));
          this.m_alKeyName_Ext.Add(new ValuePair("54", "<00>"));
          this.m_alKeyName_Ext.Add(new ValuePair("56", "Help"));
          this.m_alKeyName_Ext.Add(new ValuePair("5b", "\"Left Windows\""));
          this.m_alKeyName_Ext.Add(new ValuePair("5c", "\"Right Windows\""));
          this.m_alKeyName_Ext.Add(new ValuePair("5d", "Application"));
        }
        return this.m_alKeyName_Ext;
      }
    }
    internal ArrayList KEYNAME_DEAD
    {
      get
      {
        if (this.m_stFile.Length > 0 && this.m_alKeyName_Dead.Count == 0)
        {
          int length = "KEYNAME_DEAD".Length;
          int num = this.m_stFile.IndexOf("KEYNAME_DEAD");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  int num3 = array[i].IndexOf(' ');
                  if (num3 > 0)
                  {
                    this.m_alKeyName_Dead.Add(new ValuePair(array[i].Substring(0, num3).Trim(), array[i].Substring(num3 + 1).Trim(new char[]
                            {
                            '"'
                            })));
                  }
                }
              }
            }
          }
          if (this.m_alKeyName_Dead.Count == 0)
          {
            int arg_138_0 = this.DEADKEYS.Count;
          }
        }
        return this.m_alKeyName_Dead;
      }
    }
    internal ArrayList DEADKEYS
    {
      get
      {
        if (this.m_alDeadKeys == null || this.m_alDeadKeys.Count == 0)
        {
          int length = "DEADKEY".Length;
          int num = this.m_stFile.IndexOf("DEADKEY");
          this.m_alDeadKeys = new ArrayList();
          while (num != -1)
          {
            int num2 = this.NextKeyword(num + 1);
            string value = this.m_stFile.Substring(num + length, num2 - num - length);
            DeadKeyTable value2 = new DeadKeyTable(value);
            this.m_alDeadKeys.Add(value2);
            int startIndex = num2 - 1;
            num = this.m_stFile.IndexOf("DEADKEY", startIndex);
          }
        }
        int arg_99_0 = this.m_alDeadKeys.Count;
        return this.m_alDeadKeys;
      }
    }
    internal ArrayList DESCRIPTIONS
    {
      get
      {
        if (this.m_alDescriptions == null || this.m_alDescriptions.Count == 0)
        {
          this.m_alDescriptions = new ArrayList();
        }
        if (this.m_stFile.Length > 0 && this.m_alDescriptions.Count == 0)
        {
          int length = "DESCRIPTIONS".Length;
          int num = this.m_stFile.IndexOf("DESCRIPTIONS");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  int num3 = array[i].IndexOf(' ', 1);
                  if (num3 != -1)
                  {
                    this.m_alDescriptions.Add(new ValuePair(array[i].Substring(0, num3), array[i].Substring(num3 + 1).Trim(new char[]
                            {
                            ' '
                            })));
                  }
                }
              }
            }
          }
        }
        int arg_145_0 = this.m_alDescriptions.Count;
        return this.m_alDescriptions;
      }
    }
    internal ArrayList LANGUAGENAMES
    {
      get
      {
        if (this.m_alLanguageNames == null || this.m_alLanguageNames.Count == 0)
        {
          this.m_alLanguageNames = new ArrayList();
        }
        if (this.m_stFile.Length > 0 && this.m_alLanguageNames.Count == 0)
        {
          int length = "LANGUAGENAMES".Length;
          int num = this.m_stFile.IndexOf("LANGUAGENAMES");
          if (num != -1)
          {
            int num2 = this.NextKeyword(num + length);
            if (num2 != -1)
            {
              string text = this.m_stFile.Substring(num + length, num2 - num - length);
              string[] array = text.Split(new char[]
                  {
                  '\r'
                  });
              for (int i = 0; i < array.Length; i++)
              {
                array[i] = array[i].Trim();
                if (array[i].Length > 0 && array[i][0] != ';')
                {
                  int num3 = array[i].IndexOf(' ', 1);
                  if (num3 != -1)
                  {
                    this.m_alLanguageNames.Add(new ValuePair(array[i].Substring(0, num3), array[i].Substring(num3 + 1).Trim(new char[]
                            {
                            ' '
                            })));
                  }
                }
              }
            }
          }
        }
        int arg_145_0 = this.m_alLanguageNames.Count;
        return this.m_alLanguageNames;
      }
    }
    internal KeyboardTextFile(string fileName, string name, string description, CultureInfo culture, string company, string copyright, UIntPtr localeId, VirtualKey[] vks, ArrayList alNames, ArrayList alExtNames, ArrayList alAttributes, ArrayList alDesciptions, ArrayList alLanguageNames)
    {
      int[] array = new int[]
      {
        -1,
          -1,
          -1,
          -1,
          -1,
          -1
      };
      bool[] array2 = new bool[256];
      bool flag = false;
      StringBuilder stringBuilder = null;
      StringBuilder stringBuilder2 = null;
      StringBuilder stringBuilder3 = null;
      StringBuilder stringBuilder4 = null;
      StringBuilder stringBuilder5 = null;
      StringBuilder stringBuilder6 = null;
      SortedList sortedList = new SortedList();
      int num = 0;
      this.m_alAttributes = alAttributes;
      this.m_alKeyName = alNames;
      this.m_alKeyName_Ext = alExtNames;
      this.m_alDescriptions = alDesciptions;
      this.m_alLanguageNames = alLanguageNames;
      if (fileName.Length == 0)
      {
        this.m_stFileName = name + ".txt";
      }
      else
      {
        this.m_stFileName = fileName;
      }
      if (File.Exists(this.m_stFileName))
      {
        File.Delete(this.m_stFileName);
      }
      StreamWriter streamWriter = new StreamWriter(this.m_stFileName, false, Encoding.Unicode);
      streamWriter.Write("KBD\t");
      streamWriter.Write(name);
      streamWriter.Write("\t\"");
      streamWriter.Write(description);
      streamWriter.Write("\"\r\n");
      streamWriter.WriteLine();
      if (copyright.Length > 0)
      {
        streamWriter.Write("COPYRIGHT\t\"");
        streamWriter.Write(copyright);
        streamWriter.Write("\"\r\n");
        streamWriter.WriteLine();
      }
      streamWriter.Write("COMPANY\t\"");
      streamWriter.Write(company);
      streamWriter.Write("\"\r\n");
      streamWriter.WriteLine();
      streamWriter.Write("LOCALENAME\t\"");
      streamWriter.Write(culture.Name);
      streamWriter.Write("\"\r\n");
      streamWriter.WriteLine();
      streamWriter.Write("LOCALEID\t\"");
      streamWriter.Write(localeId.ToUInt32().ToString("x8", Utilities.s_nfi));
      streamWriter.Write("\"\r\n");
      streamWriter.WriteLine();
      streamWriter.WriteLine("VERSION\t1.0\r\n");
      if (this.m_alAttributes != null && this.m_alAttributes.Count > 0)
      {
        streamWriter.WriteLine("ATTRIBUTES");
        for (int i = 0; i < this.m_alAttributes.Count; i++)
        {
          streamWriter.WriteLine((string)this.m_alAttributes[i]);
        }
        streamWriter.WriteLine();
      }
      for (int j = 0; j < vks.Length; j++)
      {
        array2[j] = false;
        for (int k = 0; k < array.Length; k++)
        {
          ShiftState shiftState = vks[j].State(k);
          if (shiftState != null && shiftState.IsDefined)
          {
            if (shiftState.Characters.Length > 1 && shiftState.GetLayoutChar(num) == "%%")
            {
              flag = true;
            }
            array[k] = 0;
            array2[j] = true;
            if (num < k)
            {
              num = k;
            }
          }
        }
        string key = vks[j].ScanCode.ToString("x2", Utilities.s_nfi);
        if (array2[j] && !sortedList.ContainsKey(key))
        {
          sortedList.Add(key, vks[j]);
        }
      }
      streamWriter.WriteLine("SHIFTSTATE\r\n");
      int num2 = 3;
      stringBuilder3 = new StringBuilder();
      stringBuilder4 = new StringBuilder();
      if (-1 != array[0])
      {
        num2++;
        stringBuilder3.Append("\t0");
        stringBuilder4.Append("\t----");
        streamWriter.Write("0\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.WriteLine();
      }
      if (-1 != array[1])
      {
        num2++;
        stringBuilder3.Append("\t1");
        stringBuilder4.Append("\t----");
        streamWriter.Write("1\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.Write(" : Shft");
        streamWriter.WriteLine();
      }
      array[2] = 0;
      if (-1 != array[2])
      {
        num2++;
        stringBuilder3.Append("\t2");
        stringBuilder4.Append("\t----");
        streamWriter.Write("2\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.Write(" :       Ctrl");
        streamWriter.WriteLine();
      }
      if (-1 != array[3])
      {
        num2++;
        stringBuilder3.Append("\t3");
        stringBuilder4.Append("\t----");
        streamWriter.Write("3\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.Write(" : Shft  Ctrl");
        streamWriter.WriteLine();
      }
      if (-1 != array[4])
      {
        num2++;
        stringBuilder3.Append("\t6");
        stringBuilder4.Append("\t----");
        streamWriter.Write("6\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.Write(" :       Ctrl Alt");
        streamWriter.WriteLine();
      }
      if (-1 != array[5])
      {
        num2++;
        stringBuilder3.Append("\t7");
        stringBuilder4.Append("\t----");
        streamWriter.Write("7\t//Column ");
        streamWriter.Write(num2.ToString(Utilities.s_nfi));
        streamWriter.Write(" : Shft  Ctrl Alt");
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine();
      int num3 = 0;
      for (int l = 0; l < array.Length; l++)
      {
        if (-1 != array[l])
        {
          array[l] = num3;
          num3++;
        }
      }
      streamWriter.WriteLine("LAYOUT\t\t;an extra '@' at the end is a dead key\r\n");
      streamWriter.Write("//SC\tVK_\t\tCap");
      streamWriter.Write(stringBuilder3.ToString());
      streamWriter.WriteLine();
      streamWriter.Write("//--\t----\t\t----");
      streamWriter.Write(stringBuilder4.ToString());
      streamWriter.WriteLine();
      streamWriter.WriteLine();
      VirtualKey virtualKey;
      for (int m = 0; m < sortedList.Count; m++)
      {
        virtualKey = (VirtualKey)sortedList.GetByIndex(m);
        if (virtualKey.VK != 110 && array2[(int)virtualKey.VK])
        {
          stringBuilder3 = new StringBuilder();
          stringBuilder3.Append(virtualKey.ScanCode.ToString("x2", Utilities.s_nfi));
          stringBuilder3.Append('\t');
          string text = Utilities.VkStringOfIvk((int)virtualKey.VK);
          stringBuilder3.Append(text);
          stringBuilder3.Append('\t');
          if (text.Length < 6)
          {
            stringBuilder3.Append('\t');
          }
          if (virtualKey.IsSgCaps || virtualKey.IsShiftSgCaps)
          {
            stringBuilder3.Append("SGCap");
            stringBuilder4 = new StringBuilder();
            stringBuilder4.Append("-1\t-1\t0\t");
          }
          else
          {
            stringBuilder3.Append(((virtualKey.CapLok ? 1 : 0) + (virtualKey.AltGrCapLock ? 4 : 0)).ToString(Utilities.s_nfi));
          }
          stringBuilder3.Append('\t');
          stringBuilder = new StringBuilder();
          stringBuilder.Append("// ");
          stringBuilder2 = new StringBuilder();
          if (virtualKey.IsSgCaps || virtualKey.IsShiftSgCaps)
          {
            stringBuilder2.Append("// ");
          }
          for (int n = 0; n < array.Length; n++)
          {
            if (-1 != array[n])
            {
              ShiftState shiftState = virtualKey.State(n);
              stringBuilder3.Append(shiftState.GetLayoutChar(num));
              if (shiftState.IsDeadKey)
              {
                stringBuilder3.Append('@');
                if (stringBuilder6 == null)
                {
                  stringBuilder6 = new StringBuilder();
                }
                stringBuilder6.Append(((ushort)shiftState.Characters[0]).ToString("x4", Utilities.s_nfi));
                stringBuilder6.Append('\t');
                stringBuilder6.Append('"');
                stringBuilder6.Append(shiftState.CharacterName);
                stringBuilder6.Append('"');
                stringBuilder6.Append("\r\n");
                if (stringBuilder5 == null)
                {
                  stringBuilder5 = new StringBuilder();
                }
                stringBuilder5.Append("DEADKEY\t");
                stringBuilder5.Append(((ushort)shiftState.Characters[0]).ToString("x4", Utilities.s_nfi));
                stringBuilder5.Append("\r\n\r\n");
                if (shiftState.DeadKeyPairs != null)
                {
                  foreach (ValuePair valuePair in shiftState.DeadKeyPairs)
                  {
                    ushort value = Convert.ToUInt16(valuePair.Name, Utilities.s_nfi);
                    ushort value2 = Convert.ToUInt16(valuePair.Value, Utilities.s_nfi);
                    stringBuilder5.Append(value.ToString("x4", Utilities.s_nfi));
                    stringBuilder5.Append('\t');
                    stringBuilder5.Append(value2.ToString("x4", Utilities.s_nfi));
                    stringBuilder5.Append('\t');
                    stringBuilder5.Append("// ");
                    stringBuilder5.Append((char)value);
                    stringBuilder5.Append(" -> ");
                    stringBuilder5.Append((char)value2);
                    stringBuilder5.Append("\r\n");
                  }
                  stringBuilder5.Append("\r\n");
                }
              }
              stringBuilder3.Append('\t');
              if (stringBuilder.Length > 3)
              {
                stringBuilder.Append(", ");
              }
              if (shiftState.CharacterName != null && shiftState.CharacterName.Length > 0)
              {
                stringBuilder.Append(shiftState.CharacterName);
              }
              else
              {
                stringBuilder.Append("<none>");
              }
              if ((virtualKey.IsSgCaps && n == 0) || (virtualKey.IsShiftSgCaps && n == 1))
              {
                ShiftState shiftState2 = virtualKey.State((VirtualKey.SSEnum)(n + 8));
                if (shiftState2 == null || shiftState2.Characters.Length == 0)
                {
                  stringBuilder4.Append(shiftState.GetLayoutChar(num));
                }
                else
                {
                  stringBuilder4.Append(shiftState2.GetLayoutChar(num));
                  if (shiftState2.IsDeadKey)
                  {
                    stringBuilder4.Append('@');
                    if (stringBuilder6 == null)
                    {
                      stringBuilder6 = new StringBuilder();
                    }
                    stringBuilder6.Append(((ushort)shiftState2.Characters[0]).ToString("x4", Utilities.s_nfi));
                    stringBuilder6.Append('\t');
                    stringBuilder6.Append('"');
                    stringBuilder6.Append(shiftState2.CharacterName);
                    stringBuilder6.Append('"');
                    stringBuilder6.Append("\r\n");
                    if (stringBuilder5 == null)
                    {
                      stringBuilder5 = new StringBuilder();
                    }
                    stringBuilder5.Append("DEADKEY\t");
                    stringBuilder5.Append(((ushort)shiftState2.Characters[0]).ToString("x4", Utilities.s_nfi));
                    stringBuilder5.Append("\r\n\r\n");
                    if (shiftState2.DeadKeyPairs != null)
                    {
                      foreach (ValuePair valuePair2 in shiftState2.DeadKeyPairs)
                      {
                        ushort value3 = Convert.ToUInt16(valuePair2.Name, Utilities.s_nfi);
                        ushort value4 = Convert.ToUInt16(valuePair2.Value, Utilities.s_nfi);
                        stringBuilder5.Append(value3.ToString("x4", Utilities.s_nfi));
                        stringBuilder5.Append('\t');
                        stringBuilder5.Append(value4.ToString("x4", Utilities.s_nfi));
                        stringBuilder5.Append('\t');
                        stringBuilder5.Append("// ");
                        stringBuilder5.Append((char)value3);
                        stringBuilder5.Append(" -> ");
                        stringBuilder5.Append((char)value4);
                        stringBuilder5.Append("\r\n");
                      }
                      stringBuilder5.Append("\r\n");
                    }
                  }
                  if (virtualKey.IsSgCaps || virtualKey.IsShiftSgCaps)
                  {
                    stringBuilder4.Append('\t');
                    if (stringBuilder2.Length > 3)
                    {
                      stringBuilder2.Append(", ");
                    }
                    if (shiftState2.CharacterName != null && shiftState2.CharacterName.Length > 0)
                    {
                      stringBuilder2.Append(shiftState2.CharacterName);
                    }
                    else
                    {
                      stringBuilder.Append("<none>");
                    }
                  }
                }
              }
            }
          }
          streamWriter.Write(stringBuilder3.ToString());
          streamWriter.Write('\t');
          streamWriter.Write(stringBuilder.ToString());
          streamWriter.WriteLine();
          if (virtualKey.IsSgCaps || virtualKey.IsShiftSgCaps)
          {
            streamWriter.Write(stringBuilder4.ToString());
            streamWriter.Write('\t');
            streamWriter.Write(stringBuilder2.ToString());
            streamWriter.WriteLine();
          }
        }
      }
      virtualKey = vks[110];
      stringBuilder3 = new StringBuilder();
      stringBuilder3.Append(virtualKey.ScanCode.ToString("x2", Utilities.s_nfi));
      stringBuilder3.Append('\t');
      stringBuilder3.Append(Utilities.VkStringOfIvk((int)virtualKey.VK));
      stringBuilder3.Append('\t');
      stringBuilder3.Append(((virtualKey.CapLok ? 1 : 0) + (virtualKey.AltGrCapLock ? 4 : 0)).ToString(Utilities.s_nfi));
      stringBuilder3.Append('\t');
      stringBuilder = new StringBuilder();
      for (int num4 = 0; num4 < array.Length; num4++)
      {
        if (-1 != array[num4])
        {
          ShiftState shiftState = virtualKey.State(num4);
          stringBuilder3.Append(shiftState.GetLayoutChar(num));
          if (shiftState.IsDeadKey)
          {
            stringBuilder3.Append('@');
          }
          stringBuilder3.Append('\t');
          if (stringBuilder.Length > 3)
          {
            stringBuilder.Append(", ");
          }
          if (shiftState.CharacterName.Length > 0)
          {
            stringBuilder.Append(shiftState.CharacterName);
          }
          if (shiftState.IsDeadKey)
          {
            stringBuilder6.Append(((ushort)shiftState.Characters[0]).ToString("x4", Utilities.s_nfi));
            stringBuilder6.Append("\t\"");
            stringBuilder6.Append(shiftState.CharacterName);
            stringBuilder6.Append("\"\r\n");
          }
        }
      }
      streamWriter.Write(stringBuilder3.ToString());
      streamWriter.Write('\t');
      streamWriter.Write("// ");
      streamWriter.Write(stringBuilder.ToString());
      streamWriter.Write("\r\n");
      streamWriter.WriteLine();
      if (flag)
      {
        NlsConvert nlsConvert = new NlsConvert();
        streamWriter.WriteLine("LIGATURE\r\n");
        streamWriter.WriteLine("//VK_\tMod#\tChar0\tChar1\tChar2\tChar3");
        streamWriter.WriteLine("//----\t\t----\t----\t----\t----\t----\r\n");
        for (int num5 = 0; num5 < vks.Length; num5++)
        {
          if (array2[num5])
          {
            for (int num6 = 0; num6 < array.Length; num6++)
            {
              virtualKey = vks[num5];
              ShiftState shiftState = virtualKey.State(num6);
              if (shiftState != null && shiftState.Characters != null && shiftState.Characters.Length > 1)
              {
                string characters = shiftState.Characters;
                if (!shiftState.IsSurrogate)
                {
                  stringBuilder3 = new StringBuilder();
                  stringBuilder3.Append(Utilities.VkStringOfIvk(num5));
                  stringBuilder3.Append("\t\t");
                  stringBuilder3.Append(array[num6]);
                  stringBuilder3.Append('\t');
                  stringBuilder = new StringBuilder();
                  stringBuilder.Append("// ");
                  for (int num7 = 0; num7 < characters.Length; num7++)
                  {
                    stringBuilder3.Append(((ushort)characters[num7]).ToString("x4", Utilities.s_nfi));
                    stringBuilder3.Append('\t');
                    if (nlsConvert.FromCharacter(shiftState.Characters.Substring(num7, 1)))
                    {
                      if (stringBuilder.Length > 3)
                      {
                        stringBuilder.Append(" + ");
                      }
                      if (shiftState.CharacterName.Length > 0)
                      {
                        stringBuilder.Append(nlsConvert.ToCharacterName());
                      }
                    }
                  }
                  streamWriter.Write(stringBuilder3.ToString());
                  streamWriter.Write('\t');
                  streamWriter.Write(stringBuilder.ToString());
                  streamWriter.WriteLine();
                }
              }
            }
          }
        }
      }
      streamWriter.WriteLine();
      if (stringBuilder5 != null)
      {
        streamWriter.WriteLine(stringBuilder5.ToString());
      }
      streamWriter.WriteLine("KEYNAME\r\n");
      foreach (ValuePair valuePair3 in this.KEYNAME)
      {
        streamWriter.Write(valuePair3.Name);
        streamWriter.Write('\t');
        streamWriter.Write(valuePair3.Value);
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine();
      streamWriter.WriteLine("KEYNAME_EXT\r\n");
      foreach (ValuePair valuePair4 in this.KEYNAME_EXT)
      {
        streamWriter.Write(valuePair4.Name);
        streamWriter.Write('\t');
        streamWriter.Write(valuePair4.Value);
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine();
      if (stringBuilder6 != null)
      {
        streamWriter.WriteLine("KEYNAME_DEAD\r\n");
        streamWriter.WriteLine(stringBuilder6.ToString());
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine("DESCRIPTIONS\r\n");
      if (this.DESCRIPTIONS.Count == 0)
      {
        streamWriter.Write("0409");
        streamWriter.Write('\t');
        streamWriter.Write(description);
        streamWriter.WriteLine();
      }
      else
      {
        foreach (ValuePair valuePair5 in this.DESCRIPTIONS)
        {
          streamWriter.Write(valuePair5.Name);
          streamWriter.Write('\t');
          streamWriter.Write(valuePair5.Value);
          streamWriter.WriteLine();
        }
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine("LANGUAGENAMES\r\n");
      if (this.LANGUAGENAMES.Count == 0)
      {
        streamWriter.Write("0409");
        streamWriter.Write('\t');
        streamWriter.Write(culture.EnglishName);
        streamWriter.WriteLine();
      }
      else
      {
        foreach (ValuePair valuePair6 in this.LANGUAGENAMES)
        {
          streamWriter.Write(valuePair6.Name);
          streamWriter.Write('\t');
          streamWriter.Write(valuePair6.Value);
          streamWriter.WriteLine();
        }
        streamWriter.WriteLine();
      }
      streamWriter.WriteLine("ENDKBD");
      streamWriter.Flush();
      streamWriter.Close();
    }
    internal KeyboardTextFile(string FileName)
    {
      this.m_stFileName = FileName;
      StreamReader streamReader = new StreamReader(File.OpenRead(FileName));
      string stIn = streamReader.ReadToEnd();
      streamReader.DiscardBufferedData();
      streamReader.Close();
      string text = this.StripAllComments(stIn).Replace('\n', ' ');
      int length = text.IndexOf("ENDKBD") - 1;
      this.m_stFile = this.RemoveWhiteSpace(text.Substring(0, length));
    }
    internal DeadKeyTable DeadKeyTableOfDeadKey(char dk)
    {
      for (int i = 0; i < this.DEADKEYS.Count; i++)
      {
        DeadKeyTable deadKeyTable = (DeadKeyTable)this.DEADKEYS[i];
        if (deadKeyTable.DK.Equals(dk))
        {
          return deadKeyTable;
        }
      }
      return null;
    }
    private int NextKeyword(int ichStart)
    {
      int num = this.m_stFile.Length;
      for (int i = 0; i < this.KeyWords.Length; i++)
      {
        int num2 = this.m_stFile.IndexOf(this.KeyWords[i], ichStart);
        if (num2 != -1 && num2 < num)
        {
          num = num2;
        }
      }
      return num;
    }
    private string StripAllComments(string stIn)
    {
      string[] array = stIn.Split(new char[]
          {
          '\r'
          });
      for (int i = 0; i < array.Length; i++)
      {
        if (array[i].Length > 0)
        {
          if (array[i][0] == ';')
          {
            array[i] = "";
          }
          else
          {
            int num = array[i].IndexOf("//");
            if (num == 0)
            {
              array[i] = "";
            }
            else
            {
              if (num != -1)
              {
                array[i] = array[i].Substring(0, num - 1);
              }
            }
          }
        }
      }
      return string.Join("\r", array);
    }
    private void FillHeaderVariables()
    {
      int length = "KBD".Length;
      int num = this.m_stFile.IndexOf("KBD");
      if (num != -1)
      {
        int num2 = this.m_stFile.IndexOf('\r', num + 1);
        if (num2 != -1)
        {
          string text = this.m_stFile.Substring(num + length, num2 - num - length);
          text = text.TrimStart(new char[0]);
          int num3 = text.IndexOf(' ');
          if (num3 != -1)
          {
            this.m_stName = text.Substring(0, num3).Trim(new char[]
                {
                ' ',
                '"'
                });
            this.m_stDescription = text.Substring(num3 + 1).Trim(new char[]
                {
                ' ',
                '"'
                });
          }
        }
      }
      if (this.m_stName == null)
      {
        this.m_stName = "";
      }
      if (this.m_stDescription == null)
      {
        this.m_stDescription = "";
      }
    }
    private string RemoveWhiteSpace(string stIn)
    {
      string text = stIn.Replace('\t', ' ');
      int num;
      do
      {
        num = text.IndexOf("  ");
        if (num >= 0)
        {
          text = text.Replace("  ", " ");
        }
      }
      while (num >= 0);
      return text;
    }
  }
}

// vim: set wrap:

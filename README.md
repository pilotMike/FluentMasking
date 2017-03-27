# FluentMasking
A fluent way to create classes to assist with masking and formatting text.  
Precreated classes include SSN, EIN, and Phone.

# Formatting SSN
  
    var ssn = "123456789";
    var result = ssn.Mask(Masks.SSN);
    // "###-##-6789"
    
    // format only, no character masking
    var ssn = "123456789";
    var result = ssn.Mask(Masks.Formats.SSN);
    // "123-45-6789"

# Overloading mask characters
  
    var ssn = "123456789";
    var result = ssn.Mask(Masks.SSN.SetMaskCharacter('*'));
    // "***-**-6789"

# Creating your own classes
  AddCharacterFilter takes a function that removes characters. Enums are provided for numbers and letters.  
  SetMask is a function that takes the string and a character to use for masking.  
  SetFormat is called after SetMask and sets the function to use after characters have been masked or 
  the function and what condition it should be called.  
  
      public class PhoneMask : AbstractMask
      {
          public PhoneMask()
          {
              AddCharacterFilter(Characters.Numbers);
              SetFormat(phone => phone.Length == 7, phone => phone.Insert(3, "-"));
              SetFormat(phone => phone.Length == 10, FormatLongPhone);
          }

          private static string FormatLongPhone(string phone)
          {
              var areaCode = "(" + phone.Substring(0, 3) + ")";
              var rest = phone.Substring(3).Insert(3, "-");
              return areaCode + " " + rest;
          }
      }
  
      public class SsnMask : AbstractMask
      {
          internal SsnMask()
          {
              AddCharacterFilter(Characters.Numbers);
              MaskWhen(s => s.Length == 9); // sets the condition on which to perform masking.

              SetMask((s,c) => s.Substring(s.Length - 5).PadLeft(9, c)); // sets the character mask
              SetFormat(s => s.Insert(5, "-").Insert(2, "-")); // final formatting
          }
      }

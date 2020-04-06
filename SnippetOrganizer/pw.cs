using System;
using System.Linq;
using System.Text;

namespace SnippetOrganizer.Ui
{

   // TODO move to NuGet Package

   public interface IPasswordGenerator
   {
      bool AllowConsecutiveCharacters { get; set; }
      bool AllowRepeatCharacters { get; set; }
      char[] AlphaChars { get; set; }
      char[] Exclusions { get; set; }
      char[] NumericChars { get; set; }
      char[] SpecialChars { get; set; }

      string Generate(int pwdLength);
   }

   public class PasswordGenerator : IPasswordGenerator
   {
      public bool AllowRepeatCharacters { get; set; }
      public bool AllowConsecutiveCharacters { get; set; }
      public char[] AlphaChars { get; set; }
      public char[] NumericChars { get; set; }
      public char[] SpecialChars { get; set; }
      public char[] Exclusions { get; set; }


      private readonly Random _random;
      private const string AlphaSet = "abcdefghijklmnopqrstuvwxyz";
      private const string NumberSet = "1234567890";
      private const string SpecialSet = "!@#$%^&*()_+=`";

      public PasswordGenerator()
      {
         _random = new Random();
         AllowConsecutiveCharacters = true;
         AllowRepeatCharacters = true;

         AlphaChars = AlphaSet.ToCharArray();
         NumericChars = NumberSet.ToCharArray();
         SpecialChars = SpecialSet.ToCharArray();
      }

      public PasswordGenerator(char[] alphaChars, char[] numericChars, char[] specialChars, char[] excludeChars)
      {
         _random = new Random();
         AllowConsecutiveCharacters = false;
         AllowRepeatCharacters = false;

         AlphaChars = alphaChars;
         NumericChars = numericChars;
         SpecialChars = specialChars;
         Exclusions = excludeChars;
      }

      private char GetRandomCharacter(char[] charArray)
      {
         var randomCharPosition = _random.Next(charArray.Length);
         var randomChar = charArray[randomCharPosition];
         return randomChar;
      }

      public string Generate(int pwdLength)
      {
         var pwdBuffer = new StringBuilder();
         var lastCharacter = '\n';

         var values = Enumerable.Range(0, pwdLength).OrderBy(x => _random.Next()).ToArray();
         var upperIndex = values[0];

         var numIndex = -99;
         if (NumericChars.Length > 0)
            numIndex = values[1];

         var specIndex = -98;
         if (NumericChars.Length > 0)
            specIndex = values[2];

         for (var i = 0; i < pwdLength; i++)
         {
            char[] arrayToPullFrom;
            if (i == numIndex)
               arrayToPullFrom = NumericChars;
            else if (i == specIndex)
               arrayToPullFrom = SpecialChars;
            else
               arrayToPullFrom = AlphaChars;

            var nextCharacter = GetRandomCharacter(arrayToPullFrom);

            if (!AllowConsecutiveCharacters)
            {
               while (char.ToUpper(lastCharacter) == char.ToUpper(nextCharacter))
               {
                  nextCharacter = GetRandomCharacter(arrayToPullFrom);
               }
            }

            if (!AllowRepeatCharacters)
            {
               var temp = pwdBuffer.ToString();
               var duplicateIndex = temp.IndexOf(nextCharacter.ToString(), StringComparison.CurrentCultureIgnoreCase);
               while (-1 != duplicateIndex)
               {
                  nextCharacter = GetRandomCharacter(arrayToPullFrom);
                  duplicateIndex = temp.IndexOf(nextCharacter);
               }
            }

            if ((null != Exclusions))
            {
               while (Exclusions.Any(c => char.ToUpper(c) == char.ToUpper(nextCharacter)))
               {
                  nextCharacter = GetRandomCharacter(arrayToPullFrom);
               }
            }

            if (i == upperIndex)
               nextCharacter = char.ToUpper(nextCharacter);

            pwdBuffer.Append(nextCharacter);
            lastCharacter = nextCharacter;
         }

         return pwdBuffer.ToString();
      }


   }

}

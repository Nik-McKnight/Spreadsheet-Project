/// <summary>
/// Author: Nik McKnight
/// Date: 2/16/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
///
/// </summary>

using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    public static class Extensions
    {
        /// <summary>
        /// Used to test the Validate delegate
        /// </summary>
        /// <param name="input">input formula</param>
        /// <returns></returns>
        public static bool Validate(string input)
        {
            bool numberFound = false;
            char tempChar = input[0];
            if (!Char.IsLetter(tempChar) & tempChar != '_')
            {
                return false;
            }

            else
            {
                for (int i = 1; i < input.Length; i++)
                {
                    tempChar = input[i];
                    if (!Char.IsDigit(tempChar) & !Char.IsLetter(tempChar) & tempChar != '_')
                    {
                        return false;
                    }

                    if (Char.IsDigit(tempChar))
                    {
                        numberFound = true;
                    }

                    else if ((Char.IsLetter(tempChar) | tempChar == '_') & numberFound == true)
                    {
                        return false;
                    }
                }
            }

            if (!numberFound)
            {
                //throw new FormulaError(invalidVariableMessage);
                return false;
            }
            else return true;

        }

        /// <summary>
        /// Used to test the Validate delegate
        /// </summary>
        /// <param name="input">input formula</param>
        /// <returns></returns>
        public static bool CheckVariableName(string input)
        {
            bool numberFound = false;
            if (input == null | input == "") {
                throw new Exception();
            }
            char tempChar = input[0];
            if (!Char.IsLetter(tempChar))
            {
                return false;
            }

            else
            {
                for (int i = 1; i < input.Length; i++)
                {
                    tempChar = input[i];
                    if (!Char.IsDigit(tempChar) & !Char.IsLetter(tempChar))
                    {
                        return false;
                    }

                    if (Char.IsDigit(tempChar))
                    {
                        numberFound = true;
                    }

                    else if ((Char.IsLetter(tempChar)) & numberFound == true)
                    {
                        return false;
                    }
                }
            }

            if (!numberFound)
            {
                //throw new FormulaError(invalidVariableMessage);
                return false;
            }
            else return true;

        }

        /// <summary>
        /// A Normalize method to use in testing
        /// </summary>
        /// <param name="input"></param>
        public static string Normalize(string input)
        {
            string temp = "";
            foreach (char c in input)
            {
                temp += c.ToString().ToUpper();
            }
            return temp;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        public static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    
}
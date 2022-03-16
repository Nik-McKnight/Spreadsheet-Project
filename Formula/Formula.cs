// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

/// <summary>
/// Author: Nik McKnight
/// Date: 2/11/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents
///
/// 
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        //Delegates
        private Func<string, double> Lookup;
        private Func<string, string> Normalize;
        private Func<string, bool> Validate;


        
        //List<string> normalFormula = new List<string>();
        private string inputString;
        private double value;
        private string[] operators = { "+", "-", "*", "/"};
        private string[] parentheses = {"(", ")" };
        private IEnumerable<String> tokens;
        private int hashCode;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            Normalize = s => s;
            Validate = s => true;
            ParseInput(formula);

            try
            {
                inputString = formula;
                tokens = GetTokens(inputString);
                ParseInput(inputString);
                hashCode = GetHashCode();
                //value = (double)this.Evaluate(s => 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            Normalize = normalize;
            Validate = isValid;
            ParseInput(formula);

            try
            {
                inputString = normalize(formula);
                Validate(formula);
                tokens = GetTokens(inputString);
                ParseInput(inputString);
                hashCode = GetHashCode();
                //value = (double)this.Evaluate(Lookup);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            string[] substrings = Regex.Split(inputString, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)|( )");
            List<string> trimmedStrings = TrimSubstrings(substrings);
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            bool isInt = false;
            double tempNum = 0;
            HashSet<string> cellNamesFound = new HashSet<string>();
            List<Tuple<string, double>> cellValues = new List<Tuple<string, double>>();
            Lookup = lookup;

            string EmptyValueStackMessage = "Value stack is empty. Cannot perform operation.";
            string DivideByZeroMessage = "Cannot divide by zero.";
            string TooFewValuesInStackMessage = "Too few values in value stack. Cannot perform operation";
            string NoLeftParenthesisMessage = "Operator stack does not contain expected '('. Remove a ').";
            string tooManyValuesMessage = "Too many values in value stack and not enough operators.";
            string notExactlyOneOperatorMessage = "There is not exactly one operator in the stack when there are multiple values.";
            string notExactlyTwoValuesMessage = "There are not exactly two values in the stack when there is one remaining operator.";
            string invalidVariableMessage = "Invalid variable name detected. Enter a valid variable.";

          
            foreach (string substring in trimmedStrings)
            {
                //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
                isInt = double.TryParse(substring, out tempNum);

                // integer
                if (isInt == true)
                {
                    if (operatorStack.Count > 0)
                    {

                        if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                        {
                            if (valueStack.Count == 0)
                            {
                                return new FormulaError(EmptyValueStackMessage);
                            }

                            else if (tempNum == 0 & operatorStack.Peek() == "/")
                            {
                                return new FormulaError(DivideByZeroMessage);
                            }

                            else
                            {
                                tempNum = Operate(tempNum, valueStack.Pop(), operatorStack.Pop());
                            }
                        }
                    }
                    valueStack.Push(tempNum);
                }

                // variable/Spreadsheet Cell
                else if (IsCellName(substring))
                {
                    if (!Validate(substring))
                    {
                        throw new FormulaFormatException("Invalid variable name detected.");
                    }
                    //If variable has already been used, that value has been stored in a tuple and the value will be looked up there.
                    if (cellNamesFound.Contains(Normalize(substring)))
                    {
                        foreach (Tuple<string, double> tempTuple in cellValues)
                        {
                            if (tempTuple.Item1 == substring)
                            {
                                tempNum = tempTuple.Item2;
                                break;
                            }
                        }
                    }
                    //Looks up value with Lookup if that variable name has not been used yet.
                    else
                    {
                        string tempString = Normalize(substring);
                        try
                        {
                            tempNum = Lookup(tempString);
                        }
                        catch (Exception ex)
                        {
                            return new FormulaError("Unknown variable");
                        }
                        cellNamesFound.Add(substring);
                        cellValues.Add(new Tuple<string, double>(substring, tempNum));
                    }

                    if (operatorStack.Count > 0)
                    {
                        if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                        {
                            if (valueStack.Count == 0)
                            {
                                return new FormulaError(EmptyValueStackMessage);
                            }

                            else if (tempNum == 0 & operatorStack.Peek() == "/")
                            {
                                return new FormulaError(DivideByZeroMessage);
                            }

                            else
                            {
                                tempNum = Operate(tempNum, valueStack.Pop(), operatorStack.Pop());
                            }
                        }
                    }


                    valueStack.Push(tempNum);
                }

                // + or -
                else if (substring == "+" | substring == "-")
                {
                    if (valueStack.Count == 0)
                    {
                        return new FormulaError(EmptyValueStackMessage);
                    }
                    if (operatorStack.Count() > 0)
                    {
                        if (operatorStack.Peek() == "+" | operatorStack.Peek() == "-")
                        {
                            if (valueStack.Count() < 2)
                            {
                                return new FormulaError(TooFewValuesInStackMessage);
                            }

                            else
                            {
                                tempNum = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                                valueStack.Push(tempNum);
                            }
                        }
                    }
                    operatorStack.Push(substring);
                }

                // *, /, or (
                else if (substring == "*" | substring == "/" | substring == "(")
                {
                    operatorStack.Push(substring);
                }

                // )
                else if (substring == ")")
                {
                    if (operatorStack.Peek() == "+" | operatorStack.Peek() == "-")
                    {
                        if (valueStack.Count() < 2)
                        {
                            return new FormulaError(TooFewValuesInStackMessage);
                        }

                        else
                        {
                            tempNum = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                            valueStack.Push(tempNum);
                        }
                    }

                    if (operatorStack.Count() == 0)
                    {
                        return new FormulaError(NoLeftParenthesisMessage);
                    }

                    if (operatorStack.Peek() != "(")
                    {
                        return new FormulaError(NoLeftParenthesisMessage);
                    }

                    operatorStack.Pop();

                    if (operatorStack.Count > 0)
                    {
                        if (valueStack.Count() < 2 & operatorStack.Peek() != "(")
                        {
                            return new FormulaError(TooFewValuesInStackMessage);
                        }

                        else
                        {
                            if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                            {
                                if (valueStack.Count == 0)
                                {
                                    return new FormulaError(EmptyValueStackMessage);
                                }

                                else if (tempNum == 0 & operatorStack.Peek() == "/")
                                {
                                    return new FormulaError(DivideByZeroMessage);
                                }

                                else
                                {
                                    tempNum = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                                    valueStack.Push(tempNum);
                                }
                            }
                        }
                    }
                }

                else
                {
                    return new FormulaError(invalidVariableMessage);
                }
            }
            // end processing tokens

            if (operatorStack.Count == 0)
            {
                if (valueStack.Count() > 1)
                {
                    return new FormulaError(tooManyValuesMessage);
                }

                else if (valueStack.Count == 0)
                {
                    return tempNum;
                }

                else
                {
                    return valueStack.Pop();
                }
            }

            else
            {
                if (operatorStack.Count() != 1)
                {
                    return new FormulaError(notExactlyOneOperatorMessage);
                }

                else if (valueStack.Count() != 2)
                {
                    return new FormulaError(notExactlyTwoValuesMessage);
                }

                else
                {
                    return Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                }
            }
            return null;
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> tempSet = new HashSet<string>();
            foreach(var token in tokens)
            {
                if (operators.Contains(token) | parentheses.Contains(token) |Double.TryParse((string)token, out double temp)) 
                {
                    continue;
                }
                else if (Validate(token))
                {
                    tempSet.Add(Normalize(token));
                }
            }
            return tempSet;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string result = "";
            foreach(char c in inputString)
            {
                if (c != ' ')
                {
                    result += c;
                }
            }
            return result;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            try
            {
                IEnumerable<String> outsideTokens = new List<String>();
                outsideTokens = GetTokens(obj.ToString());

                string sourceToken;
                string outsideToken;
                double sourceValue;
                double outsideValue;

                if (GetHashCode() != obj.GetHashCode() | outsideTokens == null | (tokens.Count() != outsideTokens.Count()))
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < tokens.Count(); i++)
                    {
                        sourceToken = tokens.ElementAt(i);
                        outsideToken = outsideTokens.ElementAt(i);
                        if (operators.Contains(sourceToken) | parentheses.Contains(sourceToken))
                        {
                            if (!(sourceToken.Equals(outsideToken)))
                            {
                                return false;
                            }
                        }
                        else if (IsCellName(sourceToken))
                        {
                            if (sourceToken != outsideToken)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Double.TryParse(sourceToken, out sourceValue);
                            Double.TryParse(outsideToken, out outsideValue);
                            if (sourceValue.ToString() != outsideValue.ToString())
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 1;

            foreach (string token in tokens)
            {
                //Adds value * 11111 to hashcode if the token is a number
                if (Double.TryParse(token, out double value))
                {
                    hash += (int)value++ * 11111;
                }

                //Adds the ascii code of each character in the token otherwise
                else
                {
                    foreach (char c in token)
                    {
                        hash += (int)c * 11111;
                    }
                }
            }
            return hash;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
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


        //convert to void/throw errors
        /// <summary>
        /// Checks formula for syntax errors
        /// </summary>
        /// <returns></returns>
        private void ParseInput(string inputString)
        {
            IEnumerable<String> tokens;
            int leftParCount;
            int rightParCount;
            leftParCount = 0;
            rightParCount = 0;
            tokens = GetTokens(inputString);
            string lastToken;
            string token;

            

            // One Token Rule
            if (tokens == null | tokens.Count() == 0)
            {
                throw new FormulaFormatException("Violated One Token Rule. Enter a formula that is not null.");
            }

            lastToken = tokens.ElementAt(0);

            // Starting Token Rule
            if (!(Double.TryParse(lastToken, out double temp1) | (IsCellName(lastToken)) | lastToken == "(" | IsCellName(lastToken))) {
                throw new FormulaFormatException("Violated the Starting Token Rule. Enter a formula that begins with a number, variable, or '('.");
            }

            // Right Parentheses Rule
            if (lastToken == "(")
            {
                leftParCount++;
            }
            if (lastToken == ")")
            {
                rightParCount++;
            }
            if (rightParCount > leftParCount)
            {
                throw new FormulaFormatException("Violated the Right Parentheses Rule. Too many ')' detected.");
            }

            for (int i = 1; i < tokens.Count(); i++) 
            {
                token = tokens.ElementAt(i);
                lastToken = tokens.ElementAt(i - 1);
                // Invalid Formula
                if (!Validate(token) & IsCellName(token))
                {
                    throw new FormulaFormatException("Formula is invalid.");
                }

                // Specific Token Rule
                if (!(operators.Contains(token) | parentheses.Contains(token) | IsCellName(token) | Double.TryParse(token, out double val))) {
                    throw new FormulaFormatException("Violated the Specific Token Rule. Formula has an invalid character.");
                }

                // Check for 2 operators in a row
                if (operators.Contains(token) & operators.Contains(lastToken)) {
                    throw new FormulaFormatException("Two operators in a row detected.");
                }

                // Right Parentheses Rule
                if (token == "(")
                {
                    leftParCount++;
                }
                if (token == ")")
                {
                    rightParCount++;
                }
                if (rightParCount > leftParCount)
                {
                    throw new FormulaFormatException("Violated the Right Parentheses Rule. Too many ')' detected.");
                }

                //Parenthesis/Operator Following Rule
                if (lastToken == "(")
                {
                    if (!(Double.TryParse(token, out double temp3) | IsCellName(token) | token == "(")) {
                        throw new FormulaFormatException("Violated the Parenthesis/Operator Following Rule. An operator or '(' must be followed by a number, variable, or '('.");
                    }
                }

                //Extra Following Rule
                if (Double.TryParse(lastToken, out double temp4) | IsCellName(lastToken) | lastToken == ")")
                {
                    if (!(operators.Contains(token) | token == ")"))
                    {
                        throw new FormulaFormatException("Violated the Extra Following rule. A number, variable, or ')' must be followed by an operator or ')'.");
                    }
                }

                lastToken = token;
            }

            // Ending Token Rule
            if (!(Double.TryParse(lastToken, out double temp2) | IsCellName(lastToken) | lastToken == ")"))
            {
                throw new FormulaFormatException("Violated the Ending Token Rule. Enter a formula that ends with a number, variable, or ')'.");
            }

            // Balanced Parentheses Rule
            if (leftParCount != rightParCount)
            {
                throw new FormulaFormatException("Violated the Balanced Parentheses Rule. Total number of '(' must equal total number of ')' in the formula.");
            }
        }

        //Removes empty strings and whitespace
        private static List<String> TrimSubstrings(string[] inputStrings)
        {
            List<string> outputStrings = new List<string>();
            foreach (string tempstring in inputStrings)
            {
                if (tempstring.Trim().Length < 1)
                {
                    continue;
                }
                else
                {
                    outputStrings.Add(tempstring.Trim());
                }
            }
            return outputStrings;
        }

        //Performs all operations
        private static double Operate(double secondValue, double firstValue, string operation)
        {
            switch (operation)
            {
                case "+":
                    return firstValue + secondValue;
                    break;

                case "-":
                    return firstValue - secondValue;
                    break;

                case "*":
                    return firstValue * secondValue;
                    break;

                case "/":
                    return firstValue / secondValue;
                    break;

                default:
                    return 0;
                    break;
            }
        }


        
        //Checks input string to determine if it is a valid cell name
        private static Boolean IsCellName(string input)
        {
            bool numberFound = false;
            char tempChar = input[0];
            if (!(Char.IsLetter(tempChar) | tempChar.Equals('_')))
            {
                return false;
            }

            else
            {
                for (int i = 1; i < input.Length; i++)
                {
                    tempChar = input[i];
                    if (!(Char.IsDigit(tempChar) | Char.IsLetter(tempChar) | tempChar.Equals('_')))
                    {
                        return false;
                    }

                    if (Char.IsDigit(tempChar))
                    {
                        numberFound = true;
                    }

                    else if ((Char.IsLetter(tempChar) | tempChar.Equals('_')) & numberFound == true)
                    {
                        return false;
                    }
                }
            }
            return true;
            /*if (!numberFound)
            {
                //throw new FormulaError(invalidVariableMessage);
                return false;
            }
            else return true;*/
        }
        

    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }


}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>

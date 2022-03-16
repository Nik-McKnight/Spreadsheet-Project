using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Author: Nik McKnight
    /// Date: 1/17/22
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
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            List<string> trimmedStrings = TrimSubstrings(substrings);
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();
            bool isInt = false;
            int tempInt = 0;
            List<string> cellNamesFound = new List<string>();
            List<Tuple<string,int>> cellValues = new List<Tuple<string,int>>();

            string EmptyValueStackMessage = "Value stack is empty. Cannot perform operation.";
            string DivideByZeroMessage = "Cannot divide by zero.";
            string TooFewValuesInStackMessage = "Too few values in value stack. Cannot perform operation";
            string NoLeftParenthesisMessage = "Operator stack does not contain expected '('";

            if (trimmedStrings.Count == 0)
            {
                throw new ArgumentException("No input.");
            }

            /* Algorithm
             * 
             * start processing tokens
             * 
             * if t = int
             *    if * or / at top of oper stack
             *       perform operation
             *    else
             *       add to value stack
             *       
             * if t = var
             *    look up value
             *    treat like int
             *    
             * if t is + or -
             *    if + or - at top of operator stack
             *       pop val stack twice
             *       pop op stack once
             *       perform operation
             *       add new value to val stack
             *    add t to operator stack
             *    
             * if t is * or /
             *    push t onto the operator stack
             * 
             * if t is (
             *    push t onto the operator stack
             *    
             * if t is )
             *    if + or - at top of operator stack
             *       pop val stack twice
             *       pop op stack once
             *       perform operation
             *       add new value to val stack
             *       
             *    pop ( from oper stack
             *    
             *    if * or / at top of oper stack
             *       pop val stack twice
             *       pop op stack once
             *       perform operation
             *       add new value to val stack
             *       
             * end processing tokens
             * 
             * if oper stack empty
             *    val stack should only have one number
             *    pop and return final value
             *    
             * else should be one operator in stack and two values in stack
             *    perform operation and return final value
             * 
             */

            //start processing tokens


            foreach (string substring in trimmedStrings)
            {
                //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
                isInt = int.TryParse(substring, out tempInt);

                // integer
                if (isInt == true)
                {
                    if (operatorStack.Count > 0)
                    {

                        if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                        {
                            if (valueStack.Count == 0)
                            {
                                throw new ArgumentException(EmptyValueStackMessage);
                            }

                            else if (tempInt == 0 & operatorStack.Peek() == "/")
                            {
                                throw new ArgumentException(DivideByZeroMessage);
                            }

                            else
                            {
                                tempInt = Operate(tempInt, valueStack.Pop(), operatorStack.Pop());
                            }
                        }
                    }
                    valueStack.Push(tempInt);
                }

                // variable/Spreadsheet Cell
                else if (IsCellName(substring))
                {
                    //If variable has already been used, that value has been stored in a tuple and the value will be looked up there.
                    if (cellNamesFound.Contains(substring))
                    {
                        foreach (Tuple<string, int> tempTuple in cellValues)
                        {
                            if (tempTuple.Item1 == substring)
                            {
                                tempInt = tempTuple.Item2;
                                break;
                            }
                        }
                    }
                    //Looks up value with Lookup if that variable name has not been used yet.
                    else                   
                    {
                        tempInt = variableEvaluator(substring);
                        cellNamesFound.Add(substring);
                        cellValues.Add(new Tuple<string, int>(substring, tempInt));
                    }

                    if (operatorStack.Count > 0)
                    {
                        if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                        {
                            if (valueStack.Count == 0)
                            {
                                throw new ArgumentException(EmptyValueStackMessage);
                            }

                            else if (tempInt == 0 & operatorStack.Peek() == "/")
                            {
                                throw new ArgumentException(DivideByZeroMessage);
                            }

                            else
                            {
                                tempInt = Operate(tempInt, valueStack.Pop(), operatorStack.Pop());
                            }
                        }
                    }


                    valueStack.Push(tempInt);
                }

                // + or -
                else if (substring == "+" | substring == "-")
                {
                    if (valueStack.Count == 0)
                    {
                        throw new ArgumentException(EmptyValueStackMessage);
                    }
                    if (operatorStack.Count() > 0)
                    {
                        if (operatorStack.Peek() == "+" | operatorStack.Peek() == "-")
                        {
                            if (valueStack.Count() < 2)
                            {
                                throw new ArgumentException(TooFewValuesInStackMessage);
                            }

                            else
                            {
                                tempInt = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                                valueStack.Push(tempInt);
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
                            throw new ArgumentException(TooFewValuesInStackMessage);
                        }

                        else
                        {
                            tempInt = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                            valueStack.Push(tempInt);
                        }
                    }

                    if (operatorStack.Count() == 0)
                    {
                        throw new ArgumentException(NoLeftParenthesisMessage);
                    }

                    if (operatorStack.Peek() != "(")
                    {
                        throw new ArgumentException(NoLeftParenthesisMessage);
                    }

                    operatorStack.Pop();

                    if (operatorStack.Count > 0)
                    {
                        if (valueStack.Count() < 2 & operatorStack.Peek() != "(")
                        {
                            throw new ArgumentException(TooFewValuesInStackMessage);
                        }

                        else
                        {
                            if (operatorStack.Peek() == "*" | operatorStack.Peek() == "/")
                            {
                                if (valueStack.Count == 0)
                                {
                                    throw new ArgumentException(EmptyValueStackMessage);
                                }

                                else if (tempInt == 0 & operatorStack.Peek() == "/")
                                {
                                    throw new ArgumentException(DivideByZeroMessage);
                                }

                                else
                                {
                                    tempInt = Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                                    valueStack.Push(tempInt);
                                }
                            }
                        }
                    }
                }
            }
            // end processing tokens

            if (operatorStack.Count == 0)
            {
                if (valueStack.Count() > 1)
                {
                    throw new ArgumentException("Too many values in value stack.");
                }

                else if (valueStack.Count == 0)
                {
                    return tempInt;
                }

                else {
                    return valueStack.Pop();
                }
            }

            else
            {
                if (operatorStack.Count() != 1)
                {
                    throw new ArgumentException("There is not exactly one operator in the stack.");
                }

                else if (valueStack.Count() != 2) {
                    throw new ArgumentException("There are not exactly two values in the stack.");
                }

                else {
                    return Operate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
                }
            }
        }

        //Removes empty strings and whitespace
        public static List<String> TrimSubstrings(string[] inputStrings)
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
        public static int Operate(int secondValue, int firstValue, string operation)
        {
            switch(operation)
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
        public static Boolean IsCellName(string input)
        {
            bool numberFound = false;
            char tempChar;
            if (!Char.IsLetter(input[0]))
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

                    else if (Char.IsLetter(tempChar) & numberFound == true)
                    {
                        return false;
                    }
                }
            }

            if (!numberFound) { 
                throw new ArgumentException("Invalid variable");
                return false; }
            else return true;
        }
    }
}
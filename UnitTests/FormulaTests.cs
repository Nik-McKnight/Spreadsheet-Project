/// <summary>
/// Author: Nik McKnight
/// Date: 2/4/22
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections;
using System.Collections.Generic;


namespace FormulaTest
{
    [TestClass]
    public class FormulaTest
    {
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEqualsFunction()
        {
            Formula f = new Formula("a1 + 1 - 1.5", Normalize, Validate);
            Assert.AreEqual(true, (f.Equals(new Formula("a1 + 1 - 1.5", Normalize, Validate))));
            Assert.AreEqual(false, (f.Equals(new Formula("a1 + 1 + 1.5", Normalize, Validate))));
            Assert.AreEqual(false, (f.Equals(new Formula("a1 + 1 - 1.5"))));
            Assert.AreEqual(true, (f.Equals(new Formula("A1 + 1 - 1.5", Normalize, Validate))));
            Assert.AreEqual(true, (f.Equals(new Formula("A1 + 1 - 1.5000", Normalize, Validate))));
            f = new Formula("1e4", Normalize, Validate);
            Assert.AreEqual(true, (f.Equals(new Formula("10000", Normalize, Validate))));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEqualsOperator()
        {
            Formula f = new Formula("a1 + 1 - 1.5", Normalize, Validate);
            Assert.AreEqual(true, (f == new Formula("a1 + 1 - 1.5", Normalize, Validate)));
            Assert.AreEqual(false, (f == new Formula("a1 + 1 + 1.5", Normalize, Validate)));
            Assert.AreEqual(false, (f == new Formula("a1 + 1 - 1.5")));
            Assert.AreEqual(true, (f == new Formula("A1 + 1 - 1.5", Normalize, Validate)));
            Assert.AreEqual(true, (f == new Formula("A1 + 1 - 1.5000", Normalize, Validate)));
            f = new Formula("1e4", Normalize, Validate);
            Assert.AreEqual(true, (f == new Formula("10000", Normalize, Validate)));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNotEqualsOperator()
        {
            Formula f = new Formula("a1 + 1 - 1.5", Normalize, Validate);
            Assert.AreEqual(false, (f != new Formula("a1 + 1 - 1.5", Normalize, Validate)));
            Assert.AreEqual(true, (f != new Formula("a1 + 1 + 1.5", Normalize, Validate)));
            Assert.AreEqual(true, (f != new Formula("a1 + 1 - 1.5")));
            Assert.AreEqual(false, (f != new Formula("A1 + 1 - 1.5", Normalize, Validate)));
            Assert.AreEqual(false, (f != new Formula("A1 + 1 - 1.5000", Normalize, Validate)));
            f = new Formula("1e4", Normalize, Validate);
            Assert.AreEqual(false, (f != new Formula("10000", Normalize, Validate)));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestGetHashCode()
        {
            Formula f = new Formula("a1 + 1 - 1.5", Normalize, Validate);
            Assert.AreEqual(f.GetHashCode(), f.GetHashCode());
            Assert.AreEqual(f.GetHashCode(), (new Formula("a1 + 1 - 1.5", Normalize, Validate)).GetHashCode());
            Assert.AreNotEqual(f.GetHashCode(), (new Formula("f5 - a1 + 1 - 1", Normalize, Validate)).GetHashCode());
        }
        /*
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOneTokenRule()
        {
            Formula f = new Formula("");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestStartingTokenRule()
        {
            Formula f = new Formula("+1");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEndingTokenRule()
        {
            Formula f = new Formula("1+");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSpecificTokenRule()
        {
            Formula f = new Formula("1+%");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestRightParenthesesRule()
        {
            Formula f = new Formula("(1+1))");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestParenthesisOperatorFollowingRule()
        {
            Formula f = new Formula("()");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraFollowingRule()
        {
            Formula f = new Formula("(1+1)(");
        }
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestBalancedParenthesesRule()
        {
            Formula f = new Formula("((1+1)");
        }
        */

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("FormulaError Tests")]
        public void TestDivideByZero()
        {
            Formula f = new Formula("1/0", Normalize, Validate);
            FormulaError e = (FormulaError)f.Evaluate(Lookup);
            Assert.AreEqual("Cannot divide by zero.", e.Reason);
        }



        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestSingleNumber()
        {
            Formula f = new Formula("1", Normalize, Validate);
            Assert.AreEqual(1.0, f.Evaluate(Lookup));
            f = new Formula("1.5", Normalize, Validate);
            Assert.AreEqual(1.5, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestSingleVariable()
        {
            Formula f = new Formula("C3", Normalize, Validate);
            Assert.AreEqual(3.0, f.Evaluate(Lookup));
            f = new Formula("G7", Normalize, Validate);
            Assert.AreEqual(6.5, f.Evaluate(Lookup));
            f = new Formula("a1", Normalize, Validate);
            Assert.AreEqual(1.0, f.Evaluate(Lookup));
            f = new Formula("X5", Normalize, Validate);
            Assert.AreEqual(-1.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestAddition()
        {
            Formula f = new Formula("5+3", Normalize, Validate);
            Assert.AreEqual(8.0, f.Evaluate(Lookup));
            f = new Formula("4.0+5.5", Normalize, Validate);
            Assert.AreEqual(9.5, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestSubtraction()
        {
            Formula f = new Formula("18-10", Normalize, Validate);
            Assert.AreEqual(8.0, f.Evaluate(Lookup));
            f = new Formula("18.0-9.6", Normalize, Validate);
            Assert.AreEqual(8.4, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestMultiplication()
        {
            Formula f = new Formula("2*4", Normalize, Validate);
            Assert.AreEqual(8.0, f.Evaluate(Lookup));
            f = new Formula("2.5*4", Normalize, Validate);
            Assert.AreEqual(10.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestDivision()
        {
            Formula f = new Formula("16/2", Normalize, Validate);
            Assert.AreEqual(8.0, f.Evaluate(Lookup));
            f = new Formula("9/2.0", Normalize, Validate);
            Assert.AreEqual(4.5, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestArithmeticWithVariable()
        {
            Formula f = new Formula("2+D4", Normalize, Validate);
            Assert.AreEqual(6.0, f.Evaluate(Lookup));
            f = new Formula("C3+C3", Normalize, Validate);
            Assert.AreEqual(6.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestLeftToRight()
        {
            Formula f = new Formula("2*6+3", Normalize, Validate);
            Assert.AreEqual(15.0, f.Evaluate(Lookup));
            f = new Formula("2*6.0+3", Normalize, Validate);
            Assert.AreEqual(15.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestOrderOperations()
        {
            Formula f = new Formula("2+6*3", Normalize, Validate);
            Assert.AreEqual(20.0, f.Evaluate(Lookup));
            f = new Formula("2+6.0*3", Normalize, Validate);
            Assert.AreEqual(20.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestParenthesesTimes()
        {
            Formula f = new Formula("(2+6)*3", Normalize, Validate);
            Assert.AreEqual(24.0, f.Evaluate(Lookup));
            f = new Formula("(2+6.0)*3", Normalize, Validate);
            Assert.AreEqual(24.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestTimesParentheses()
        {
            Formula f = new Formula("2*(3+5)", Normalize, Validate);
            Assert.AreEqual(16.0, f.Evaluate(Lookup));
            f = new Formula("2*(3.0+5)", Normalize, Validate);
            Assert.AreEqual(16.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestPlusParentheses()
        {
            Formula f = new Formula("2+(3+5)", Normalize, Validate);
            Assert.AreEqual(10.0, f.Evaluate(Lookup));
            f = new Formula("2+(3.0+5)", Normalize, Validate);
            Assert.AreEqual(10.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestPlusComplex()
        {
            Formula f = new Formula("2+(3+5*9)", Normalize, Validate);
            Assert.AreEqual(50.0, f.Evaluate(Lookup));
            f = new Formula("2+(3.0+5*9)", Normalize, Validate);
            Assert.AreEqual(50.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestOperatorAfterParens()
        {
            Formula f = new Formula("(1*1)-2/2", Normalize, Validate);
            Assert.AreEqual(0.0, f.Evaluate(Lookup));
            f = new Formula("(1*1.0)-2/2", Normalize, Validate);
            Assert.AreEqual(0.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestComplexTimesParentheses()
        {
            Formula f = new Formula("2+3*(3+5)", Normalize, Validate);
            Assert.AreEqual(26.0, f.Evaluate(Lookup));
            f = new Formula("2+3.0*(3+5)", Normalize, Validate);
            Assert.AreEqual(26.0, f.Evaluate(Lookup));
        }

        /// <summary>
        /// Self-explanatory
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [TestCategory("Evaluation Tests")]
        public void TestComplexAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2", Normalize, Validate);
            Assert.AreEqual(194.0, f.Evaluate(Lookup));
            f = new Formula("2+3*5+(3.0+4*8)*5+2", Normalize, Validate);
            Assert.AreEqual(194.0, f.Evaluate(Lookup));
        }


        /// <summary>
        /// Tests this test class's Normalize method
        /// </summary>
        [TestMethod]
        [TestCategory("Delegate Functions")]
        public void TestNormalize()
        {
            string test = "a + b + C";
            Assert.AreEqual("A + B + C", Normalize(test));
        }

        /// <summary>
        /// Tests this test class's Validate method
        /// </summary>
        [TestMethod]
        [TestCategory("Delegate Functions")]
        public void TestValidate()
        {
            Assert.AreEqual(true, Validate("a1"));
            Assert.AreEqual(false, Validate("aa"));
        }

        /// <summary>
        /// Tests this test class's Lookup method
        /// </summary>
        [TestMethod]
        [TestCategory("Delegate Functions")]
        public void TestLookup()
        {
            Assert.AreEqual(1, Lookup("A1"));
            Assert.AreEqual(2, Lookup("B2"));
            Assert.AreEqual(2.0, Lookup(Normalize("b2")));
            Assert.AreEqual(-1, Lookup("Z1"));
        }

        /// <summary>
        /// Used to test the Normalize delegate
        /// </summary>
        /// <param name="input">input formula</param>
        /// <returns></returns>
        private static string Normalize(string input)
        {
            string result = "";
            foreach (char c in input)
            {
                result += c.ToString().ToUpper();
            }
            return result;
        }

        /// <summary>
        /// Used to test the Validate delegate
        /// </summary>
        /// <param name="input">input formula</param>
        /// <returns></returns>
        private static bool Validate(string input)
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

            if (!numberFound)
            {
                //throw new FormulaError(invalidVariableMessage);
                return false;
            }
            else return true;

        }

        /// <summary>
        /// Used to test the Lookup delegate
        /// </summary>
        /// <param name="input">input variable</param>
        /// <returns></returns>
        private static double Lookup(string variable)
        {
            Hashtable LookupTable = new Hashtable();
            LookupTable.Add("A1", 1.0);
            LookupTable.Add("B2", 2.0);
            LookupTable.Add("C3", 3.0);
            LookupTable.Add("D4", 4.0);
            LookupTable.Add("E5", 5.0);
            LookupTable.Add("F6", 6.0);
            LookupTable.Add("G7", 6.5);

            if (LookupTable.Contains(variable))
            {
                return (double)LookupTable[variable];
            }

            else return -1;
        }
    }


}
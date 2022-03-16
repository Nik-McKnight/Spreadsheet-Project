// See https://aka.ms/new-console-template for more information




/*You should write at least one test for each operator,
 * one for parentheses, one for order of operation, 
 * and then as many more as you can think of. 
 */

using FormulaEvaluator;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using SS;

string testString;


/*testTrimSubstrings();
testAdd();
testSubtract();
testMultiply();
testDivide();
testParentheses();
testOrderOfOperations();
testVariable();
testNoInput();
testErrors();*/

//Console.WriteLine(Evaluator.Evaluate("", null));

TestSaveFile();

void TestSaveFile()
{
    AbstractSpreadsheet ss = new Spreadsheet("C:\\Users\\troja\\source\\repos\\CS3500\\Spreadsheet\\Test_The_Evaluator_Console_App", s => true, s => s, "");
    for (int i = 0; i < 100; i++)
    {
        ss.SetContentsOfCell("A" + i, i.ToString());
    }
    ss.Save("save.txt");
    //Assert.IsTrue(File.Exists("save.txt"));
}

void testTrimSubstrings() {

    testString = "(2 + 35 ) * A7";
    List<string> expected = new List<string> { "(", "2", "+", "35", ")", "*", "A7" };
    string[] temp = Regex.Split(testString, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
    List<string> actual = Evaluator.TrimSubstrings(temp);

    if (actual.SequenceEqual(expected)) {
        Console.WriteLine("TrimSubstrings works :)");
    }

    else
    {
        Console.WriteLine("TrimSubstrings doesn't work :(");
    }
}

void testAdd()
{
    if (Evaluator.Evaluate("5+3+2", null) == 10) Console.WriteLine("Add works :)");
    else Console.WriteLine("Add doesn't work :(");
}

void testSubtract()
{
    if (Evaluator.Evaluate("5-3-2", null) == 0) Console.WriteLine("Subtract works :)");
    else Console.WriteLine("Subtract doesn't work :(");
}

void testMultiply()
{
    if (Evaluator.Evaluate("5*3*2", null) == 30) Console.WriteLine("Multiply works :)");
    else Console.WriteLine("Multiply doesn't work :(");
}

void testDivide()
{
    if (Evaluator.Evaluate("30/3/2", null) == 5) Console.WriteLine("Divide works :)");
    else Console.WriteLine("Divide doesn't work :(");
}

void testParentheses()
{
    if (Evaluator.Evaluate("5*(3+2)", null) == 25) Console.WriteLine("Parentheses work :)");
    else Console.WriteLine("Parenthes don't work :(");
}

void testOrderOfOperations()
{
    if (Evaluator.Evaluate("(2-1)+(U2/(2+3))*2", OtherLookup) == 5) Console.WriteLine("Order of Operations works :)");
    else Console.WriteLine("Order of Operations doesn't work :(");
}

void testVariable()
{
    bool allTestsPass = false;
    if (Evaluator.Evaluate("A1 * 5", SimpleLookup) == 50) allTestsPass = true;
    if (Evaluator.Evaluate("A1 * 5", OtherLookup) == 25) allTestsPass = true;
    if (Evaluator.Evaluate("A1 * 5", OtherLookup) == 50) allTestsPass = true;
    if (Evaluator.Evaluate("A1 * 5", OtherLookup) == 100) allTestsPass = true;
    if (Evaluator.Evaluate("A1 * 5", OtherLookup) == -5) allTestsPass = true;

    if (allTestsPass==true) Console.WriteLine("Variable Works :)");
    else Console.WriteLine("Variable doesn't work :(");
}

void testNoInput()
{
    if (Evaluator.Evaluate("", null) == 0) Console.WriteLine("No input works :)");
    else Console.WriteLine("No input doesn't work :(");
}

int SimpleLookup(string v)
{
    return 10;
}

int OtherLookup(string v)
{
    switch (v)
    {
        case "A1":
            return 5;
            break;

        case "U2":
            return 10;
            break;

        case "V8":
            return 20;
            break;

        default:
            return 0;
            break;
    }
}

void testErrors()
{
    try
    {
        Evaluator.Evaluate("+",null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught empty value stack");
    }

    try
    {
        Evaluator.Evaluate("1/0", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught divide by zero");
    }

    try
    {
        Evaluator.Evaluate("G5", OtherLookup);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught variable with no value");
    }

    try
    {
        Evaluator.Evaluate("-1", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught fewer than two values in value stack");
    }

    try
    {
        Evaluator.Evaluate("5+5)", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught missing '('");
    }

    try
    {
        Evaluator.Evaluate("+ *", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught more than one operator on stack during final step");
    }

    try
    {
        Evaluator.Evaluate("1 1", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught fewer than one operator on stack during final step");
    }

    try
    {
        Evaluator.Evaluate("1 +", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught too few values on stack during final step");
    }

    try
    {
        Evaluator.Evaluate("1 1 + 1", null);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Caught too many values on stack during final step");
    }
}
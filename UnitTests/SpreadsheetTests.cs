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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SpreadsheetTest
{
    [TestClass]
    public class SpreadsheetTest
    {
        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestSpreadsheetConstructor()
        {
            Spreadsheet test = new Spreadsheet();
            Assert.IsTrue(test != null);
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestGetCellContentsEmpty()
        {
            Spreadsheet test = new Spreadsheet();
            Assert.AreEqual("", test.GetCellContents("A1"));
            HashSet<string> testCells = (HashSet<string>)test.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, actual: testCells.Count);
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestGetCellContentsDouble()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "1.0");
            Assert.AreEqual(1.0, test.GetCellContents("A1"));
            test.SetContentsOfCell("A1", "1");
            Assert.AreEqual(1.0, test.GetCellContents("A1"));
            test.SetContentsOfCell("A1", "1e-3");
            Assert.AreEqual(.001, test.GetCellContents("A1"));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestGetCellContentsString()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "B2 + C3");
            Assert.AreEqual("B2 + C3", test.GetCellContents("A1"));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestGetCellContentsFormula()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "=B2 + C3");
            Formula f1 = (Formula)test.GetCellContents("A1");
            Assert.IsTrue(f1.Equals(new Formula("B2+C3")));
            Assert.AreEqual("B2+C3", test.GetCellContents("A1").ToString());
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestGetNamesOfAllNonemptyCells()
        {
            Spreadsheet test = new Spreadsheet();
            HashSet<string> expected = new HashSet<string>();
            HashSet<string> actual = new HashSet<string>();
            actual = (HashSet<string>)test.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(expected.SetEquals(actual));
            test.SetContentsOfCell("A1", "1.0");
            expected.Add("A1");
            actual = (HashSet<string>)test.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(expected.SetEquals(actual));
            test.SetContentsOfCell("B2", "1.0");
            expected.Add("B2");
            actual = (HashSet<string>)test.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(expected.SetEquals(actual));
            test.SetContentsOfCell("C3", "=1.0 + 1");
            expected.Add("C3");
            actual = (HashSet<string>)test.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(expected.SetEquals(actual));

        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestChangeCellContents()
        {
            List<string> expected = new List<string>();
            List<string> actual = new List<string>();
            expected.Add("A1");
            Spreadsheet test = new Spreadsheet();
            actual = (List<string>)test.SetContentsOfCell("A1", "1.0");
            CollectionAssert.AreEqual(expected, actual);
            actual = (List<string>)test.SetContentsOfCell("A1", "test");
            CollectionAssert.AreEqual(expected, actual);
            actual = (List<string>)test.SetContentsOfCell("A1", "=1+1");
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestSetContentsOfCellDouble()
        {
            List<string> expected = new List<string>();
            List<string> actual = new List<string>();
            expected.Add("A1");
            Spreadsheet test = new Spreadsheet();
            actual = (List<string>)test.SetContentsOfCell("A1", "1.0");
            //https://stackoverflow.com/questions/11055632/how-to-compare-lists-in-unit-testing
            CollectionAssert.AreEqual(expected, actual);

            // Make sure the type of the contents of the cell is correct
            Type actualType;
            actualType = test.GetCellContents("A1").GetType();
            Assert.IsTrue(actualType == typeof(double));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestSetContentsOfCellString()
        {
            List<string> expected = new List<string>();
            List<string> actual = new List<string>();
            expected.Add("A1");
            Spreadsheet test = new Spreadsheet();
            actual = (List<string>)test.SetContentsOfCell("A1", "B2");
            CollectionAssert.AreEqual(expected, actual);

            // Make sure the type of the contents of the cell is correct
            Type actualType;
            actualType = test.GetCellContents("A1").GetType();
            Assert.IsTrue(actualType == typeof(string));

        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Method Tests")]
        public void TestSetContentsOfCellFormula()
        {
            List<string> expected = new List<string>();
            List<string> actual = new List<string>();
            expected.Add("A1");
            Spreadsheet test = new Spreadsheet();
            actual = (List<string>)test.SetContentsOfCell("A1", "=B2+ C3");
            CollectionAssert.AreEqual(expected, actual);
            actual = (List<string>)test.SetContentsOfCell("B2", "=C3");
            expected.Insert(0, "B2");
            CollectionAssert.AreEqual(expected, actual);
            actual = (List<string>)test.SetContentsOfCell("C3", "=D4");
            expected.Insert(0, "C3");
            CollectionAssert.AreEqual(expected, actual);

            // Make sure the type of the contents of the cell is correct
            Type actualType;
            actualType = test.GetCellContents("A1").GetType();
            Assert.IsTrue(actualType == typeof(Formula));


        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.InvalidNameException))]
        public void TestSetContentsOfCellBadCellNameDouble()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A5A", "1.0");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.InvalidNameException))]
        public void TestSetContentsOfCellBadCellNameString()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A5A", "1.0");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.InvalidNameException))]
        public void TestSetContentsOfCellBadCellNameFormula()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A5A", "=1.0+1");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCellNullString()
        {
            Spreadsheet test = new Spreadsheet();
            string s = null;
            test.SetContentsOfCell("A5", s);
        }


        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.CircularException))]
        public void TestCheckForCircularDependency()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "A1");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.CircularException))]
        public void TestCheckForCircularDependencyAgain()
        {
            Spreadsheet test = new Spreadsheet();
            test.SetContentsOfCell("A1", "B2");
            test.SetContentsOfCell("B2", "C3");
            test.SetContentsOfCell("C3", "A1");
        }

        /// <summary>
        /// Builds a spreadsheet with 10 used cells, saves it, and loads it.
        /// </summary>
        [TestMethod]
        public void TestSaveAndLoadFile()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "");
            for (int i = 1; i <= 10; i++)
            {
                ss.SetContentsOfCell("A" + i, i.ToString());
            }
            ss.Save("save.txt");
            Assert.IsTrue(File.Exists("save.txt"));

            AbstractSpreadsheet ss2 = new Spreadsheet();
            Assert.AreEqual(0, ss2.GetNamesOfAllNonemptyCells().Count());
            ss2 = new Spreadsheet("save.txt", s => true, s => s, "");
            Assert.AreEqual(10, ss2.GetNamesOfAllNonemptyCells().Count());

            for (int i = 1; i <= 10; i++)
            {
                Assert.AreEqual(ss.GetCellContents("A" + i), ss2.GetCellContents("A" + i));
            }
            Assert.AreEqual(ss.Version, ss2.Version);
        }

        /// <summary>
        /// Builds a spreadsheet with 67600 used cells, saves it, and loads it.
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void TestStressSaveAndLoadFile()
        {
            String[] letters = {"a", "B" ,"C", "D", "E", "F", "G", "H",
                "I", "J", "K", "L", "m", "N", "O", "p", "Q", "R", "S",
                "T", "U", "v", "V", "W", "X", "Y", "Z"};
            HashSet<string> set = new HashSet<string>();
            foreach (string letter1 in letters)
            {
                foreach (string letter2 in letters)
                {
                    set.Add(letter1 + letter2);
                }
            }
            AbstractSpreadsheet ss = new Spreadsheet(Extensions.Validate, Extensions.Normalize, "");
            foreach (string column in set)
            {
                for (int row = 1; row <= 100; row++)
                {
                    ss.SetContentsOfCell(column + row, row.ToString());
                }
            }
            Assert.IsTrue(File.Exists("save.txt"));

            ss.Save("save.txt");
            Assert.IsTrue(File.Exists("save.txt"));

            AbstractSpreadsheet ss2 = new Spreadsheet();
            Assert.AreEqual(0, ss2.GetNamesOfAllNonemptyCells().Count());
            ss2 = new Spreadsheet("save.txt", s => true, s => s, "");
            Assert.AreEqual(67600, ss2.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.SpreadsheetReadWriteException))]
        public void TestSaveWithBadFileName()
        {
            AbstractSpreadsheet ss = new Spreadsheet(s => true, s => s, "");
            ss.Save("/save.txt");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.SpreadsheetReadWriteException))]
        public void TestLoadWithBadFileName()
        {
            AbstractSpreadsheet ss = new Spreadsheet("/save.txt", s => true, s => s, "");
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [TestCategory("Exceptions Tests")]
        [ExpectedException(typeof(SS.SpreadsheetReadWriteException))]
        public void TestLoadWithWrongVersion()
        {
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "100");

        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        public void TestGetCellValueDouble()
        {
            Spreadsheet ss = new Spreadsheet(Extensions.Validate, Extensions.Normalize, "");
            ss.SetContentsOfCell("A1", "1.0");
            Assert.AreEqual(1.0, ss.GetCellValue("a1"));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        public void TestGetCellValueString()
        {
            Spreadsheet ss = new Spreadsheet(Extensions.Validate, Extensions.Normalize, "");
            ss.SetContentsOfCell("A1", "A4+b7");
            Assert.AreEqual("A4+b7", ss.GetCellValue("a1"));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        public void TestGetCellValueFormula()
        {
            Spreadsheet ss = new Spreadsheet(Extensions.Validate, Extensions.Normalize, "");
            ss.SetContentsOfCell("A1", "=A4+b7");
            Assert.IsInstanceOfType(ss.GetCellValue("A1"), typeof(FormulaError));
            ss.SetContentsOfCell("A4", "1.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A1"), typeof(FormulaError));
            ss.SetContentsOfCell("B7", "6.5");
            Assert.AreEqual(7.5, ss.GetCellValue("a1"));
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        public void TestChanged()
        {
            Spreadsheet ss = new Spreadsheet("save.txt", Extensions.Validate, Extensions.Normalize, "");
            Assert.IsFalse(ss.Changed);
            ss.SetContentsOfCell("A1", "=A4+b7");
            Assert.IsTrue(ss.Changed);
            ss.Save("save.txt");
            Assert.IsFalse(ss.Changed);

        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testGetCellContentsBlankName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents(null);
        }

        /// <summary>
        /// Self-Explanatory
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void testGetCellContentsEmptyName()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.GetCellContents("");
        }
    }
}
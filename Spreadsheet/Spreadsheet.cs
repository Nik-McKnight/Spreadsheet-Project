/// <summary>
/// Author: Nik McKnight
/// Date: 2/16/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code, not counting the starter code, from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>

using SpreadsheetUtilities;
using System.Collections;
using System.Xml;

namespace SS
{
    /// <summary>
    /// A class that represents a spreadsheet.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // A Hashtable that contains the contents for all populated cells
        private Hashtable cells;
        private Cell tempCell;
        private DependencyGraph dependencies;
        private string? filePath;
        private XmlWriter writer;

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }


        /// <summary>
        /// Constructs an empty spreadsheet
        /// </summary>
        /// 
        public Spreadsheet() : this(Extensions.CheckVariableName, s => s, "default")
        {
        }

        /// <summary>
        /// Constructs a spreadsheet
        /// </summary>
        /// <param name="isValid"> defines what valid variables look like for the application</param>
        /// <param name="normalize">defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="version"> defines the version of the spreadsheet (should it be saved)</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            this.cells = new Hashtable();
            this.dependencies = new DependencyGraph();
            this.IsValid = isValid;
            Changed =false;
        }

        /// <summary>
        /// Constructs a spreadsheet
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isValid"> defines what valid variables look like for the application</param>
        /// <param name="normalize">defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="version"> defines the version of the spreadsheet (should it be saved)</param>
        /// <exception cref="SpreadsheetReadWriteException">Thrown when there is an error with a file save or load</exception>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : this(isValid,normalize,version)
        {
            this.filePath = filePath;
            if (Version != GetSavedVersion(filePath))
            {
                throw new SpreadsheetReadWriteException("File version does not match.");
            }
            Load(filePath);

        }

        /// <summary>
        /// Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// <returns>The contents of the cell if they are valid.</returns>
        /// <exception cref="InvalidNameException">Thrown if the name is invalid: blank/empty/""</exception>
        public override object GetCellContents(string name)
        {
            try
            {
                name = Normalize(name);
                if (!IsValid(name))
                {
                    throw new InvalidNameException();
                }
                if (cells.ContainsKey(name))
                {
                    this.tempCell = (Cell)cells[name];
                    return tempCell.GetContents();
                }
                return "";
            }
            catch (Exception e)
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            //return (IEnumerable<string>)cells.Keys;
            //throw new NotImplementedException();
            HashSet<string> names = new HashSet<string>();
            foreach (DictionaryEntry cell in cells)
            {
                names.Add((string)cell.Key);
            }
            return (IEnumerable<string>)names;
        }

        /// <summary>
        /// Sets cell contents to a double
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="number">A double to set to content</param>
        /// <returns>A list of all cells that need to be recalculated</returns>
        /// <exception cref="InvalidNameException">Invalid name detected</exception>
        protected override IList<string> SetCellContents(string name, double number)
        {
            IList<string> output = new List<string>();
            name = Normalize(name);
            if (!IsValid(name) | name == null)
            {
                throw new InvalidNameException();
            }

            this.tempCell = new Cell(name, number);

            if (cells.ContainsKey(name))
            {
                //changes stored cell
                cells[name] = tempCell;
            }
            else
            {
                cells.Add(name, tempCell);
            }
            updateDependencies(name);

            foreach (string tempName in GetCellsToRecalculate(name))
            {
                output.Add(tempName);
            }
            Changed =true;
            return output;
        }


        /// <summary>
        /// Sets cell contents to a string
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="number">A string to set to content</param>
        /// <returns>A list of all cells that need to be recalculated</returns>
        /// <exception cref="ArgumentNullException">No argument given</exception>
        /// <exception cref="InvalidNameException">Invalid name detected</exception>
        protected override IList<string> SetCellContents(string name, string text)
        {
            IList<string> output = new List<string>();
            name = Normalize(name);
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsValid(name) | name == null)
            {
                throw new InvalidNameException();
            }
            this.tempCell = new Cell(name, text);

            if (cells.ContainsKey(name))
            {
                //changes stored cell
                cells[name] = tempCell;
            }
            else
            {
                cells.Add(name, tempCell);
            }
            updateDependencies(name);
            foreach (string tempName in GetCellsToRecalculate(name))
            {
                output.Add(tempName);
            }
            /*if (text == "")
            {
                cells.Remove(name);
            }*/
            Changed =true;
            return output;
        }

        /// <summary>
        /// Sets cell contents to a formula
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="number">A formula to set to content</param>
        /// <returns>A list of all cells that need to be recalculated</returns>
        /// <exception cref="ArgumentNullException">No argument given</exception>
        /// <exception cref="InvalidNameException">Invalid name detected</exception>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            name = Normalize(name);
            object oldContents = GetCellContents(name);
            if (!IsValid(name) | name == null)
            {
                throw new InvalidNameException();
            }
            try
            {
                IList<string> output = new List<string>();
                this.tempCell = new Cell(name, formula);

                if (cells.ContainsKey(name))
                {
                    //changes stored cell
                    cells[name] = tempCell;
                }
                else
                {
                    cells.Add(name, tempCell);
                }
                updateDependencies(name);
                foreach (string tempName in GetCellsToRecalculate(name))
                {
                    output.Add(tempName);
                }

                Changed =true;
                return output;
            }
            catch (Exception ex)
            {
                this.tempCell = new Cell(name, oldContents);
                cells[name] = tempCell;
                updateDependencies(name);
                if (oldContents == "")
                {
                    cells.Remove(name);
                }

                if (ex is CircularException)
                {
                    throw new CircularException();
                }
                else
                {
                    throw new ArgumentNullException("Formula is null");
                }
            }
        
        }

        /// <summary>
        /// Updates the dependency graph for a given cell
        /// 
        /// Only replaces the dependees of the cell
        /// </summary>
        /// <param name="name">The name of the cell whose dependencies must be updated</param>
        private void updateDependencies(string name)
        {
            name = Normalize(name);
            Cell tempCell = (Cell)cells[name];
            dependencies.ReplaceDependees(name, tempCell.GetDependeesFromFormula());
        }

        /// <summary>
        /// Returns the dependents of a given cell
        /// </summary>
        /// <param name="name">The name of the cell whose dependents need to be looked up</param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return dependencies.GetDependents(Normalize(name));
        }

        /// <summary>
        /// Parses the input and calls the appropriate SetCellContents()
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="InvalidNameException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);
            Formula tempFormula;
            object tempContents = content;
            IList<string> result = new List<string>();
            if (name == null | !IsValid(name))
            {
                throw new InvalidNameException();
            }

            if (content == null)
            {
                throw new ArgumentNullException();
            }

            if (content == "")
            {
                result = SetCellContents(name, "");
            }

            else if (content[0] == '=') {
                tempFormula = new Formula(Normalize(content[1..]), Normalize, IsValid);
                result = SetCellContents(name, tempFormula);
                tempContents = tempFormula;
            }

            else if (Double.TryParse(content, out double number))
            {
                result = SetCellContents(name, number);
                tempContents= number;
            }

            else
            {
                result = SetCellContents(name, content);
            }

            updateCellValues(result);
            //SetCellValue(name, tempContents);
            if (content == "")
            {
                cells.Remove(name);
            }
            return result;
        }

        private void updateCellValues(IList<string> changed)
        {
            Cell tempCell;
            object tempContents;
            foreach (string name in changed)
            {
                try
                {
                    tempCell = (Cell)cells[name];
                    tempContents = GetCellContents(name);
                    SetCellValue(name, tempContents);
                }
                catch (Exception e)
                {
                    throw new FormulaFormatException("");
                }
            }
        }
        /// <summary>
        ///   Look up the version information in the given file. If there are any problems opening, reading, 
        ///   or closing the file, the method should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// 
        /// <remarks>
        ///   In an ideal world, this method would be marked static as it does not rely on an existing SpreadSheet
        ///   object to work; indeed it should simply open a file, lookup the version, and return it.  Because
        ///   C# does not support this syntax, we abused the system and simply create a "regular" method to
        ///   be implemented by the base class.
        /// </remarks>
        /// 
        /// <exception cref="SpreadsheetReadWriteException"> 
        ///   Thrown if any problem occurs while reading the file or looking up the version information.
        /// </exception>
        /// 
        /// <param name="filename"> The name of the file (including path, if necessary)</param>
        /// <returns>Returns the version information of the spreadsheet saved in the named file.</returns>
        public override string GetSavedVersion(string filename)
        {
            string tempVersion = "";
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement("spreadsheet"))
                        {
                            //Sets version of spreadsheet that is in the file
                            tempVersion = reader.GetAttribute("version");
                            return tempVersion;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException("File version does not match.");
            }
            return tempVersion;
        }

        /// <summary>
        /// Saves spreadsheet to file.
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="SpreadsheetReadWriteException">Thrown on bad file name</exception>
        public override void Save(string filename)
        {
            //type of cell contents
            Type type;
            string contents;
            try
            {
                //https://csharp.net-tutorials.com/xml/writing-xml-with-the-xmlwriter-class/
                using (writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    //Saves Version
                    writer.WriteAttributeString("version", Version);
                    //Starts saving cells
                    writer.WriteStartElement("cells");
                    //Saves all non-empty cells
                    foreach (string name in GetNamesOfAllNonemptyCells())
                    {
                        tempCell = (Cell)cells[name];
                        type = tempCell.GetContents().GetType();
                        //Starts cell
                        writer.WriteStartElement("cell");
                        //Saves name
                        writer.WriteElementString("name", tempCell.GetName());

                        if (type == typeof(Formula))
                        {
                            contents = "=" + tempCell.GetContents().ToString();
                        }

                        else
                        {
                            contents = tempCell.GetContents().ToString();
                        }
                        //saves content with appropriate type
                        writer.WriteElementString("contents", contents);
                        //Ends cell
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                    Changed =false;
                }
            }
            catch (Exception ex)
            {
                throw new SpreadsheetReadWriteException("Error saving file");
            }
        }

        /// <summary>
        /// Loads a file name called in the 4-argument constructor
        /// </summary>
        /// <param name="filename">Filename from 4-argument constructor</param>
        /// <exception cref="SpreadsheetReadWriteException">Thrown on bad file name</exception>
        private void Load(string filename)
        {
            string tempName = null;
            string tempContents = null;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        
                        if (reader.IsStartElement("spreadsheet"))
                        {
                            //Sets version of spreadsheet that is in the file
                            Version = reader.GetAttribute("version");
                        }
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "name":
                                    reader.Read();
                                    tempName = reader.Value;
                                    break;

                                case "contents":
                                    reader.Read();
                                    tempContents = reader.Value;
                                    SetContentsOfCell(tempName, tempContents);
                                    break;
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.ToString());
            }
            Changed =false;
        }

        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell that we want the value of (will be normalized)</param>
        /// 
        /// <returns>
        ///   Returns the value (as opposed to the contents) of the named cell.  The return
        ///   value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </returns>
        public override object GetCellValue(string name)
        {
            Cell tempCell;
            name = Normalize(name);
            if (name == null | !IsValid(name))
            {
                throw new InvalidNameException();
            }
            if (!cells.ContainsKey(name))
            {
                return "";
            }
            tempCell = (Cell)cells[name];
            
            return tempCell.GetValue();
        }

        private void SetCellValue(string name, object contents)
        {
            Cell tempCell = (Cell)cells[name];
            object tempVal;
            Type type = contents.GetType();
            if (type == typeof(string) | type == typeof(double))
            {
                tempVal = contents;
            }
            else
            {
                tempVal = ((Formula)contents).Evaluate(Lookup);
                if (tempVal == null)
                {
                    tempVal = new FormulaError();
                }
            }
            tempCell.SetValue(tempVal);
            cells[name] = tempCell;
        }

        private double Lookup(string name)
        {
            Cell tempCell;
            object tempVal;
            try
            {
                tempCell = (Cell)cells[name];
                tempVal = tempCell.GetValue();
            }
            catch 
            {
                tempVal = null;
            }
            return (double)tempVal;
        }
    }

    /// <summary>
    /// A struct that represents cells in the spreadsheet
    /// </summary>
    public struct Cell
    {
        private string name;
        private object contents;
        private object value;
        private HashSet<string> dependees;
        private HashSet<string> dependents;
        private Type type;

        /// <summary>
        /// Constructor for a cell
        /// 
        /// Works with doubles, strings, and formulas
        /// </summary>
        /// <param name="name">Cell name</param>
        /// <param name="contents">Cell contents</param>
        public Cell(string name, object contents)
        {
            this.name = name;
            this.contents = contents;
            this.type = contents.GetType();
            this.dependees = new HashSet<string>();
            this.dependents = new HashSet<string>();
            this.value = "";
            /*if (type == typeof(Formula))
            {
                this.value = ((Formula)contents).Evaluate(s => 1);
                this.dependees = GetDependeesFromFormula();
            }
            else if (type == typeof(string) | type == typeof(double))
            {
                this.value = contents;
            }
            else
            {

                this.value = "";
            }*/
        }

        /// <summary>
        /// self-explanatory
        /// </summary>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// self-explanatory
        /// </summary>
        public object GetContents()
        {
            return contents;
        }

        /// <summary>
        /// Sets contents of cell
        /// 
        /// Works for double, string, and formula
        /// </summary>
        public void SetContents(object contents)
        {
            this.contents = contents;
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <returns>Data type of cell's contents</returns>
        public Type GetContentsType()
        {
            return type;
        }

        public object GetValue()
        {
            return value;
        }

        public void SetValue(object value)
        {
            this.value = value;
        }
        /// <summary>
        /// Scans the formula to find any cells that this cell depends on.
        /// </summary>
        /// <returns>A set of names of dependees for this cell.</returns>
        public HashSet<string> GetDependeesFromFormula()
        {
            HashSet<string> dependees = new HashSet<string>();
            IEnumerable<string> tokens;
            tokens = Extensions.GetTokens(contents.ToString());
            foreach (string token in tokens)
            {
                if (Extensions.CheckVariableName(token))
                {
                    dependees.Add(token);
                }
            }
            return dependees;
        }
    }
}
/// <summary>
/// Author: Nik McKnight
/// Date: 3/4/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>

using SpreadsheetGrid_Core;
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;

namespace GUI
{
    public partial class SpreadsheetGUI : Form
    {
        private Spreadsheet ss = new Spreadsheet();
        private string cellName = "A1";
        private object cellContents;
        private object cellValue;
        private static int numCols = 26;
        private static int numRows = 99;
        private string fileName;


        /// <summary>
        /// Constructor
        /// </summary>
        public SpreadsheetGUI()
        {
            ss = new Spreadsheet(Extensions.Validate, Extensions.Normalize, "six");
            InitializeComponent();
            fileName = null;
            spreadsheetGridWidget1.SetSelection(0, 0, false);
            updateTextBoxes();
            ContentTextBox.Select();
            this.spreadsheetGridWidget1.SelectionChanged += doit;
        }

        /// <summary>
        /// Event Handler for changing which cell is selected.
        /// </summary>
        /// <param name="sender"></param>
        private void doit(SpreadsheetGridWidget sender)
        {
            sender.GetSelection(out int col, out int row);
            cellName = GetCellName(col, row);
            updateTextBoxes();
            ContentTextBox.Focus();
            Debug.WriteLine($"clicked on {row}, {col}");
            Debug.WriteLine($"clicked on {cellName}");
            Debug.WriteLine(ContentTextBox.Text);
        }

        /// <summary>
        /// Returns the name of the selected cell
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetCellName(int col, int row)
        {
            string output = "";
            output += (char)(col + 65);
            output += row + 1;
            return Extensions.Normalize(output);
        }

        /// <summary>
        /// Returns the column of a given cell name
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private int GetCol(string cellName)
        {
            return (int)(Extensions.Normalize(cellName)[0] - 65);
        }

        /// <summary>
        /// Returns the row of a given cell name
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private int GetRow(string cellName)
        {
            string result;
            result = cellName[1..];
            int.TryParse(result, out int row);
            try
            {
                return row - 1;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Used to update all of the text boxes above the spreadsheet
        /// </summary>
        private void updateTextBoxes()
        {
            cellContents = ss.GetCellContents(cellName);
            cellValue = ss.GetCellValue(cellName);
            CellTextBox.Text = cellName;
            if (cellContents.GetType() == typeof(Formula))
            {
                cellContents = "=" + cellContents;
            }
            ContentTextBox.Text = cellContents.ToString();
            ValueTextBox.Text = cellValue.ToString();

            FileNameTextBox.Text = Path.GetFileName(fileName);
        }

        /// <summary>
        /// Event handler for specific keys
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int col;
            int row;
            col = GetCol(cellName);
            row = GetRow(cellName);

            switch (keyData)
            {
                // Allows data to be entered by hitting enter
                case Keys.Enter:
                    Enter();
                    Debug.WriteLine("enter");
                    break;

                // Allows changing cells with the arrow keys
                case Keys.Left:
                    if (col > 0)
                    {
                        col--;
                        cellName = GetCellName(col, row);
                        updateTextBoxes();
                    }
                    Debug.WriteLine("left");
                    break;

                // Allows changing cells with the arrow keys
                case Keys.Right:
                    if (col < numCols - 1)
                    {
                        col++;
                        cellName = GetCellName(col, row);
                        updateTextBoxes();
                    }
                    Debug.WriteLine("right");
                    break;

                // Allows changing cells with the arrow keys
                case Keys.Up:
                    if (row > 0)
                    {
                        row--;
                        cellName = GetCellName(col, row);
                        updateTextBoxes();
                    }
                    Debug.WriteLine("up");
                    break;

                // Allows changing cells with the arrow keys
                case Keys.Down:
                    if (row < numRows - 1)
                    {
                        row++;
                        cellName = GetCellName(col, row);
                        updateTextBoxes();
                    }
                    Debug.WriteLine("down");
                    break;
                
                // Use tab to switch between entering a cell name and entering content for the selected cell
                case Keys.Tab:
                    if (ContentTextBox.Focused)
                    {
                        CellTextBox.Focus();
                    }
                    else
                    {
                        ContentTextBox.Focus();
                    }
                    break;
                    Debug.WriteLine("tab");

      
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            spreadsheetGridWidget1.SetSelection(col, row, false);
            return true;
        }

        /// <summary>
        /// Calls Enter() when clicking the Enter button on the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterButton_Click(object sender, EventArgs e)
        {
            Enter();
        }

        /// <summary>
        /// Enters data and updates spreadsheet after hitting/clicking Enter
        /// </summary>
        private void Enter()
        {
            IList<string> cells;
            string cellBox = Extensions.Normalize(CellTextBox.Text);
            string contentBox = ContentTextBox.Text;
            try
            {
                // Changes selection if a different cell name has been entered
                if (cellBox != cellName)
                {
                    spreadsheetGridWidget1.SetSelection(GetCol(cellBox), GetRow(cellBox), false);
                    cellName = cellBox;
                }

                // Changes content if different content has been entered
                if (contentBox != cellContents.ToString())
                {
                    if (String.IsNullOrEmpty(ContentTextBox.Text))
                    {
                        cells = ss.SetContentsOfCell(cellName, "");
                    }
                    else
                    {
                        cells = ss.SetContentsOfCell(cellName, ContentTextBox.Text);
                    }
                    ValueTextBox.Text = ss.GetCellValue(cellName).ToString();
                    updateCellsOnGUI(cells);
                }
                updateTextBoxes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetType().ToString());
            }
        }

        /// <summary>
        /// Updates the given cells with the proper values on the GUI
        /// </summary>
        /// <param name="input">Cells whose values need to be recalculated and updated on the GUI</param>
        private void updateCellsOnGUI(object input)
        {
            IList<string> cells = (IList<string>)input;
            foreach (string cell in cells)
            {
                spreadsheetGridWidget1.SetValue(GetCol(cell), GetRow(cell), ss.GetCellValue(cell).ToString());
            }
        }

        /// <summary>
        /// Used for saving the current file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (fileName == null)
            {
                SaveAs();
            }
            else
            {
                Save();
            }
        }

        /// <summary>
        /// Used for saving the current file with a different name.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        /// <summary>
        /// Saves the current file.
        /// </summary>
        private void Save()
        {
            try
            {
                if (fileName != "")
                {
                    if (!(fileName.Contains(".sprd")))
                    {
                        fileName += ".sprd";
                    }
                    ss.Save(fileName);
                    Debug.WriteLine($"Saved {fileName}");
                    updateTextBoxes();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Saves the current file with a different name.
        /// </summary>
        private void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Spreadsheet File|*.sprd";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileName = sfd.FileName;
                Save();

            }
        }

        /// <summary>
        /// Loads a file
        /// </summary>
        private void Load()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Spreadsheet File|*.sprd";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                clearSheet();
                try
                {
                    if (fileName != "")
                    {
                        ss = new Spreadsheet(fileName, Extensions.Validate, Extensions.Normalize, "six");
                        Debug.WriteLine($"Loaded {fileName}");
                        foreach (string cell in ss.GetNamesOfAllNonemptyCells())
                        {
                            spreadsheetGridWidget1.SetValue(GetCol(cell), GetRow(cell), ss.GetCellValue(cell).ToString());
                        }
                        updateTextBoxes();
                    }

                }
                catch
                {

                }
            }

        }

        /// <summary>
        /// Calls Load()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_Click(object sender, EventArgs e)
        {
            Load();
        }

        /// <summary>
        /// Sets every cell to be empty on the GUI
        /// </summary>
        private void clearSheet()
        {
            for (int i = 0; i < numCols; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    spreadsheetGridWidget1.SetValue(i, j, "");
                }
            }
        }

        /// <summary>
        /// Informs user if cell has not been saved when trying to close the spreadsheet. Closes immediately if the sheet has not been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //https://stackoverflow.com/questions/47788288/how-to-cancel-closing-winform
            var result = DialogResult.Yes;
            if (ss.Changed)
            {
                result = MessageBox.Show("Close spreadsheet anyway?", "Unsaved changes detected.", MessageBoxButtons.YesNo);
            }
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

    }
}
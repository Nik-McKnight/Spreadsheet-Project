/// <summary>
/// Author: Nik McKnight
/// Date: 3/4/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code, not counting any starter or autofill code, from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// </summary>

namespace GUI
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.spreadsheetGridWidget1 = new SpreadsheetGrid_Core.SpreadsheetGridWidget();
            this.EnterButton = new System.Windows.Forms.Button();
            this.ContentTextBox = new System.Windows.Forms.TextBox();
            this.ValueTextBox = new System.Windows.Forms.TextBox();
            this.ContentLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.CellLabel = new System.Windows.Forms.Label();
            this.CellTextBox = new System.Windows.Forms.TextBox();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // spreadsheetGridWidget1
            // 
            this.spreadsheetGridWidget1.AutoScroll = true;
            this.spreadsheetGridWidget1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.spreadsheetGridWidget1.Location = new System.Drawing.Point(99, 106);
            this.spreadsheetGridWidget1.Name = "spreadsheetGridWidget1";
            this.spreadsheetGridWidget1.Size = new System.Drawing.Size(2156, 786);
            this.spreadsheetGridWidget1.TabIndex = 0;
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(92, 77);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(75, 23);
            this.EnterButton.TabIndex = 3;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // ContentTextBox
            // 
            this.ContentTextBox.Location = new System.Drawing.Point(279, 77);
            this.ContentTextBox.Name = "ContentTextBox";
            this.ContentTextBox.Size = new System.Drawing.Size(100, 23);
            this.ContentTextBox.TabIndex = 4;
            this.ContentTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ValueTextBox
            // 
            this.ValueTextBox.Location = new System.Drawing.Point(385, 77);
            this.ValueTextBox.Name = "ValueTextBox";
            this.ValueTextBox.ReadOnly = true;
            this.ValueTextBox.Size = new System.Drawing.Size(100, 23);
            this.ValueTextBox.TabIndex = 5;
            this.ValueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ContentLabel
            // 
            this.ContentLabel.AutoSize = true;
            this.ContentLabel.Location = new System.Drawing.Point(303, 59);
            this.ContentLabel.Name = "ContentLabel";
            this.ContentLabel.Size = new System.Drawing.Size(50, 15);
            this.ContentLabel.TabIndex = 6;
            this.ContentLabel.Text = "Content";
            this.ContentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(416, 59);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(35, 15);
            this.ValueLabel.TabIndex = 7;
            this.ValueLabel.Text = "Value";
            this.ValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CellLabel
            // 
            this.CellLabel.AutoSize = true;
            this.CellLabel.Location = new System.Drawing.Point(209, 59);
            this.CellLabel.Name = "CellLabel";
            this.CellLabel.Size = new System.Drawing.Size(27, 15);
            this.CellLabel.TabIndex = 9;
            this.CellLabel.Text = "Cell";
            this.CellLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CellTextBox
            // 
            this.CellTextBox.Location = new System.Drawing.Point(173, 77);
            this.CellTextBox.Name = "CellTextBox";
            this.CellTextBox.Size = new System.Drawing.Size(100, 23);
            this.CellTextBox.TabIndex = 8;
            this.CellTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Location = new System.Drawing.Point(92, 23);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveAsButton.TabIndex = 10;
            this.SaveAsButton.Text = "Save As";
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(356, 5);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(60, 15);
            this.FileNameLabel.TabIndex = 12;
            this.FileNameLabel.Text = "File Name";
            this.FileNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Enabled = false;
            this.FileNameTextBox.Location = new System.Drawing.Point(335, 23);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(100, 23);
            this.FileNameTextBox.TabIndex = 11;
            this.FileNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(254, 23);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 13;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(173, 23);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 14;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(441, 22);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(75, 23);
            this.NewButton.TabIndex = 15;
            this.NewButton.Text = "New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // SpreadsheetGUI
            // 
            this.AcceptButton = this.EnterButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 1116);
            this.Controls.Add(this.NewButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.FileNameLabel);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.SaveAsButton);
            this.Controls.Add(this.CellLabel);
            this.Controls.Add(this.CellTextBox);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.ContentLabel);
            this.Controls.Add(this.ValueTextBox);
            this.Controls.Add(this.ContentTextBox);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.spreadsheetGridWidget1);
            this.Name = "SpreadsheetGUI";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetGUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HelpProvider helpProvider1;
        private Button EnterButton;
        private TextBox ContentTextBox;
        private TextBox ValueTextBox;
        private Label ContentLabel;
        private Label ValueLabel;
        private Label CellLabel;
        private TextBox CellTextBox;
        private Button SaveAsButton;
        private Label FileNameLabel;
        private TextBox FileNameTextBox;
        private Button LoadButton;
        protected SpreadsheetGrid_Core.SpreadsheetGridWidget spreadsheetGridWidget1;
        private Button SaveButton;
        private Button NewButton;
    }
}
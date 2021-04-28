
namespace MapEditor
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxTileSelector = new System.Windows.Forms.GroupBox();
            this.buttonColor6 = new System.Windows.Forms.Button();
            this.buttonColor5 = new System.Windows.Forms.Button();
            this.buttonColor4 = new System.Windows.Forms.Button();
            this.buttonColor3 = new System.Windows.Forms.Button();
            this.buttonColor2 = new System.Windows.Forms.Button();
            this.buttonColor1 = new System.Windows.Forms.Button();
            this.groupBoxCurrentTile = new System.Windows.Forms.GroupBox();
            this.buttonCurrentTile = new System.Windows.Forms.Button();
            this.buttonSaveFile = new System.Windows.Forms.Button();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.groupBoxMap = new System.Windows.Forms.GroupBox();
            this.groupBoxTileSelector.SuspendLayout();
            this.groupBoxCurrentTile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTileSelector
            // 
            this.groupBoxTileSelector.Controls.Add(this.buttonColor6);
            this.groupBoxTileSelector.Controls.Add(this.buttonColor5);
            this.groupBoxTileSelector.Controls.Add(this.buttonColor4);
            this.groupBoxTileSelector.Controls.Add(this.buttonColor3);
            this.groupBoxTileSelector.Controls.Add(this.buttonColor2);
            this.groupBoxTileSelector.Controls.Add(this.buttonColor1);
            this.groupBoxTileSelector.Location = new System.Drawing.Point(13, 13);
            this.groupBoxTileSelector.Name = "groupBoxTileSelector";
            this.groupBoxTileSelector.Size = new System.Drawing.Size(100, 160);
            this.groupBoxTileSelector.TabIndex = 0;
            this.groupBoxTileSelector.TabStop = false;
            this.groupBoxTileSelector.Text = "Tile Selector";
            // 
            // buttonColor6
            // 
            this.buttonColor6.BackColor = System.Drawing.Color.Black;
            this.buttonColor6.Location = new System.Drawing.Point(53, 112);
            this.buttonColor6.Name = "buttonColor6";
            this.buttonColor6.Size = new System.Drawing.Size(40, 40);
            this.buttonColor6.TabIndex = 5;
            this.buttonColor6.UseVisualStyleBackColor = false;
            this.buttonColor6.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // buttonColor5
            // 
            this.buttonColor5.BackColor = System.Drawing.Color.LightBlue;
            this.buttonColor5.Location = new System.Drawing.Point(7, 112);
            this.buttonColor5.Name = "buttonColor5";
            this.buttonColor5.Size = new System.Drawing.Size(40, 40);
            this.buttonColor5.TabIndex = 4;
            this.buttonColor5.UseVisualStyleBackColor = false;
            this.buttonColor5.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // buttonColor4
            // 
            this.buttonColor4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonColor4.Location = new System.Drawing.Point(53, 66);
            this.buttonColor4.Name = "buttonColor4";
            this.buttonColor4.Size = new System.Drawing.Size(40, 40);
            this.buttonColor4.TabIndex = 3;
            this.buttonColor4.UseVisualStyleBackColor = false;
            this.buttonColor4.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // buttonColor3
            // 
            this.buttonColor3.BackColor = System.Drawing.Color.Peru;
            this.buttonColor3.Location = new System.Drawing.Point(7, 66);
            this.buttonColor3.Name = "buttonColor3";
            this.buttonColor3.Size = new System.Drawing.Size(40, 40);
            this.buttonColor3.TabIndex = 2;
            this.buttonColor3.UseVisualStyleBackColor = false;
            this.buttonColor3.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // buttonColor2
            // 
            this.buttonColor2.BackColor = System.Drawing.Color.Silver;
            this.buttonColor2.Location = new System.Drawing.Point(53, 20);
            this.buttonColor2.Name = "buttonColor2";
            this.buttonColor2.Size = new System.Drawing.Size(40, 40);
            this.buttonColor2.TabIndex = 1;
            this.buttonColor2.UseVisualStyleBackColor = false;
            this.buttonColor2.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // buttonColor1
            // 
            this.buttonColor1.BackColor = System.Drawing.Color.Green;
            this.buttonColor1.Location = new System.Drawing.Point(7, 20);
            this.buttonColor1.Name = "buttonColor1";
            this.buttonColor1.Size = new System.Drawing.Size(40, 40);
            this.buttonColor1.TabIndex = 0;
            this.buttonColor1.UseVisualStyleBackColor = false;
            this.buttonColor1.Click += new System.EventHandler(this.buttonColorChange_Click);
            // 
            // groupBoxCurrentTile
            // 
            this.groupBoxCurrentTile.Controls.Add(this.buttonCurrentTile);
            this.groupBoxCurrentTile.Location = new System.Drawing.Point(13, 180);
            this.groupBoxCurrentTile.Name = "groupBoxCurrentTile";
            this.groupBoxCurrentTile.Size = new System.Drawing.Size(100, 100);
            this.groupBoxCurrentTile.TabIndex = 1;
            this.groupBoxCurrentTile.TabStop = false;
            this.groupBoxCurrentTile.Text = "Current Tile";
            // 
            // buttonCurrentTile
            // 
            this.buttonCurrentTile.BackColor = System.Drawing.Color.Green;
            this.buttonCurrentTile.Enabled = false;
            this.buttonCurrentTile.Location = new System.Drawing.Point(19, 22);
            this.buttonCurrentTile.Name = "buttonCurrentTile";
            this.buttonCurrentTile.Size = new System.Drawing.Size(60, 60);
            this.buttonCurrentTile.TabIndex = 0;
            this.buttonCurrentTile.UseVisualStyleBackColor = false;
            // 
            // buttonSaveFile
            // 
            this.buttonSaveFile.Location = new System.Drawing.Point(25, 286);
            this.buttonSaveFile.Name = "buttonSaveFile";
            this.buttonSaveFile.Size = new System.Drawing.Size(75, 70);
            this.buttonSaveFile.TabIndex = 2;
            this.buttonSaveFile.Text = "Save File";
            this.buttonSaveFile.UseVisualStyleBackColor = true;
            this.buttonSaveFile.Click += new System.EventHandler(this.buttonSaveFile_Click);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(25, 372);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 70);
            this.buttonLoadFile.TabIndex = 3;
            this.buttonLoadFile.Text = "Load File";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // groupBoxMap
            // 
            this.groupBoxMap.Location = new System.Drawing.Point(119, 19);
            this.groupBoxMap.Name = "groupBoxMap";
            this.groupBoxMap.Size = new System.Drawing.Size(600, 600);
            this.groupBoxMap.TabIndex = 4;
            this.groupBoxMap.TabStop = false;
            this.groupBoxMap.Text = "Map";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 624);
            this.Controls.Add(this.groupBoxMap);
            this.Controls.Add(this.buttonLoadFile);
            this.Controls.Add(this.buttonSaveFile);
            this.Controls.Add(this.groupBoxCurrentTile);
            this.Controls.Add(this.groupBoxTileSelector);
            this.Name = "EditorForm";
            this.Text = "Level Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.groupBoxTileSelector.ResumeLayout(false);
            this.groupBoxCurrentTile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTileSelector;
        private System.Windows.Forms.Button buttonColor6;
        private System.Windows.Forms.Button buttonColor5;
        private System.Windows.Forms.Button buttonColor4;
        private System.Windows.Forms.Button buttonColor3;
        private System.Windows.Forms.Button buttonColor2;
        private System.Windows.Forms.Button buttonColor1;
        private System.Windows.Forms.GroupBox groupBoxCurrentTile;
        private System.Windows.Forms.Button buttonCurrentTile;
        private System.Windows.Forms.Button buttonSaveFile;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.GroupBox groupBoxMap;
    }
}
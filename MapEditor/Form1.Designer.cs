
namespace MapEditor
{
    partial class Form1
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
            this.buttonLoadMap = new System.Windows.Forms.Button();
            this.groupBoxCreateMap = new System.Windows.Forms.GroupBox();
            this.buttonCreateMap = new System.Windows.Forms.Button();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.groupBoxCreateMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadMap
            // 
            this.buttonLoadMap.Location = new System.Drawing.Point(12, 12);
            this.buttonLoadMap.Name = "buttonLoadMap";
            this.buttonLoadMap.Size = new System.Drawing.Size(260, 71);
            this.buttonLoadMap.TabIndex = 0;
            this.buttonLoadMap.Text = "Load Map";
            this.buttonLoadMap.UseVisualStyleBackColor = true;
            this.buttonLoadMap.Click += new System.EventHandler(this.buttonLoadMap_Click);
            // 
            // groupBoxCreateMap
            // 
            this.groupBoxCreateMap.Controls.Add(this.buttonCreateMap);
            this.groupBoxCreateMap.Controls.Add(this.textBoxHeight);
            this.groupBoxCreateMap.Controls.Add(this.textBoxWidth);
            this.groupBoxCreateMap.Controls.Add(this.labelHeight);
            this.groupBoxCreateMap.Controls.Add(this.labelWidth);
            this.groupBoxCreateMap.Location = new System.Drawing.Point(12, 89);
            this.groupBoxCreateMap.Name = "groupBoxCreateMap";
            this.groupBoxCreateMap.Size = new System.Drawing.Size(260, 260);
            this.groupBoxCreateMap.TabIndex = 1;
            this.groupBoxCreateMap.TabStop = false;
            this.groupBoxCreateMap.Text = "Create New Map";
            // 
            // buttonCreateMap
            // 
            this.buttonCreateMap.Location = new System.Drawing.Point(21, 155);
            this.buttonCreateMap.Name = "buttonCreateMap";
            this.buttonCreateMap.Size = new System.Drawing.Size(218, 77);
            this.buttonCreateMap.TabIndex = 4;
            this.buttonCreateMap.Text = "Create Map";
            this.buttonCreateMap.UseVisualStyleBackColor = true;
            this.buttonCreateMap.Click += new System.EventHandler(this.buttonCreateMap_Click);
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(137, 109);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(102, 20);
            this.textBoxHeight.TabIndex = 3;
            this.textBoxHeight.Text = "20";
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(137, 47);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(102, 20);
            this.textBoxWidth.TabIndex = 2;
            this.textBoxWidth.Text = "20";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(18, 112);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(76, 13);
            this.labelHeight.TabIndex = 1;
            this.labelHeight.Text = "Height (in tiles)";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(21, 50);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(73, 13);
            this.labelWidth.TabIndex = 0;
            this.labelWidth.Text = "Width (in tiles)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 361);
            this.Controls.Add(this.groupBoxCreateMap);
            this.Controls.Add(this.buttonLoadMap);
            this.Name = "Form1";
            this.Text = "Level Editor";
            this.groupBoxCreateMap.ResumeLayout(false);
            this.groupBoxCreateMap.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadMap;
        private System.Windows.Forms.GroupBox groupBoxCreateMap;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Button buttonCreateMap;
        private System.Windows.Forms.TextBox textBoxHeight;
    }
}


namespace TestWindowsFormsApplication
{
    partial class TestForm
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
            this.bitmapPanel = new System.Windows.Forms.Panel();
            this.buttonGetPath = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bitmapPanel
            // 
            this.bitmapPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitmapPanel.Location = new System.Drawing.Point(12, 12);
            this.bitmapPanel.Name = "bitmapPanel";
            this.bitmapPanel.Size = new System.Drawing.Size(200, 100);
            this.bitmapPanel.TabIndex = 0;
            this.bitmapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bitmapPanel_Paint);
            this.bitmapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bitmapPanel_MouseUp);
            // 
            // buttonGetPath
            // 
            this.buttonGetPath.Location = new System.Drawing.Point(897, 12);
            this.buttonGetPath.Name = "buttonGetPath";
            this.buttonGetPath.Size = new System.Drawing.Size(75, 23);
            this.buttonGetPath.TabIndex = 1;
            this.buttonGetPath.Text = "Get Path";
            this.buttonGetPath.UseVisualStyleBackColor = true;
            this.buttonGetPath.Click += new System.EventHandler(this.buttonGetPath_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(872, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(897, 67);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonGetPath);
            this.Controls.Add(this.bitmapPanel);
            this.Name = "TestForm";
            this.Text = "Test App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel bitmapPanel;
        private System.Windows.Forms.Button buttonGetPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonReset;
    }
}


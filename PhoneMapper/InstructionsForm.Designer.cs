
namespace PhoneMapper
{
    partial class InstructionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstructionsForm));
            this.labelInstructions = new System.Windows.Forms.Label();
            this.textBoxSampleLogFile = new System.Windows.Forms.TextBox();
            this.labelInstructions2 = new System.Windows.Forms.Label();
            this.textBoxMacroDroidWritetoFileText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelInstructions
            // 
            this.labelInstructions.AutoSize = true;
            this.labelInstructions.Location = new System.Drawing.Point(12, 9);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(338, 52);
            this.labelInstructions.TabIndex = 0;
            this.labelInstructions.Text = resources.GetString("labelInstructions.Text");
            // 
            // textBoxSampleLogFile
            // 
            this.textBoxSampleLogFile.Location = new System.Drawing.Point(12, 64);
            this.textBoxSampleLogFile.Multiline = true;
            this.textBoxSampleLogFile.Name = "textBoxSampleLogFile";
            this.textBoxSampleLogFile.Size = new System.Drawing.Size(338, 190);
            this.textBoxSampleLogFile.TabIndex = 1;
            this.textBoxSampleLogFile.Text = resources.GetString("textBoxSampleLogFile.Text");
            // 
            // labelInstructions2
            // 
            this.labelInstructions2.AutoSize = true;
            this.labelInstructions2.Location = new System.Drawing.Point(8, 257);
            this.labelInstructions2.Name = "labelInstructions2";
            this.labelInstructions2.Size = new System.Drawing.Size(322, 26);
            this.labelInstructions2.TabIndex = 2;
            this.labelInstructions2.Text = "The above format can be achieved using the following MacroDroid\r\n v5.8.15 \'Write " +
    "to file\' action text:";
            // 
            // textBoxMacroDroidWritetoFileText
            // 
            this.textBoxMacroDroidWritetoFileText.Location = new System.Drawing.Point(11, 286);
            this.textBoxMacroDroidWritetoFileText.Multiline = true;
            this.textBoxMacroDroidWritetoFileText.Name = "textBoxMacroDroidWritetoFileText";
            this.textBoxMacroDroidWritetoFileText.Size = new System.Drawing.Size(337, 63);
            this.textBoxMacroDroidWritetoFileText.TabIndex = 3;
            this.textBoxMacroDroidWritetoFileText.Text = resources.GetString("textBoxMacroDroidWritetoFileText.Text");
            // 
            // InstructionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 354);
            this.Controls.Add(this.textBoxMacroDroidWritetoFileText);
            this.Controls.Add(this.labelInstructions2);
            this.Controls.Add(this.textBoxSampleLogFile);
            this.Controls.Add(this.labelInstructions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstructionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instructions";
            this.Load += new System.EventHandler(this.InstructionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInstructions;
        private System.Windows.Forms.TextBox textBoxSampleLogFile;
        private System.Windows.Forms.Label labelInstructions2;
        private System.Windows.Forms.TextBox textBoxMacroDroidWritetoFileText;
    }
}
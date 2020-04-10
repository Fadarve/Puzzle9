namespace puzzle99
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
            
            this.CmbTamanyos = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // CmbTamanyos
            // 
            this.CmbTamanyos.FormattingEnabled = true;
            this.CmbTamanyos.Location = new System.Drawing.Point(866, 41);
            this.CmbTamanyos.Name = "CmbTamanyos";
            this.CmbTamanyos.Size = new System.Drawing.Size(121, 24);
            this.CmbTamanyos.TabIndex = 10;
            this.CmbTamanyos.SelectedIndexChanged += new System.EventHandler(this.CmbTamanyos_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 750);
            this.Controls.Add(this.CmbTamanyos);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CmbTamanyos;
    }
}


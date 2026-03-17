namespace HxCore.Win.PDFXE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axPXV_Control1 = new AxPDFXEdit.AxPXV_Control();
            ((System.ComponentModel.ISupportInitialize)(this.axPXV_Control1)).BeginInit();
            this.SuspendLayout();
            // 
            // axPXV_Control1
            // 
            this.axPXV_Control1.Enabled = true;
            this.axPXV_Control1.Location = new System.Drawing.Point(391, 100);
            this.axPXV_Control1.Name = "axPXV_Control1";
            this.axPXV_Control1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPXV_Control1.OcxState")));
            this.axPXV_Control1.Size = new System.Drawing.Size(192, 192);
            this.axPXV_Control1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.axPXV_Control1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axPXV_Control1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxPDFXEdit.AxPXV_Control axPXV_Control1;
    }
}
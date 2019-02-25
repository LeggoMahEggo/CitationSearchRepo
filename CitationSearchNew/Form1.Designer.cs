namespace CitationSearchNew
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.lstMishna = new System.Windows.Forms.ListBox();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnSaveConsecutiveQuotes = new System.Windows.Forms.Button();
            this.lstQuoteNumberList = new System.Windows.Forms.ListBox();
            this.btnShowAllQuoteNumbers = new System.Windows.Forms.Button();
            this.txtMishna = new System.Windows.Forms.TextBox();
            this.lstMishnaQuotes = new System.Windows.Forms.ListBox();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(13, 13);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(123, 23);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load Masechet";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lstMishna
            // 
            this.lstMishna.FormattingEnabled = true;
            this.lstMishna.ItemHeight = 16;
            this.lstMishna.Location = new System.Drawing.Point(3, 3);
            this.lstMishna.Name = "lstMishna";
            this.lstMishna.Size = new System.Drawing.Size(120, 84);
            this.lstMishna.TabIndex = 1;
            this.lstMishna.SelectedIndexChanged += new System.EventHandler(this.lstMishna_SelectedIndexChanged);
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnSaveConsecutiveQuotes);
            this.pnlControls.Controls.Add(this.lstQuoteNumberList);
            this.pnlControls.Controls.Add(this.btnShowAllQuoteNumbers);
            this.pnlControls.Controls.Add(this.txtMishna);
            this.pnlControls.Controls.Add(this.lstMishnaQuotes);
            this.pnlControls.Controls.Add(this.lstMishna);
            this.pnlControls.Location = new System.Drawing.Point(13, 42);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(758, 438);
            this.pnlControls.TabIndex = 2;
            this.pnlControls.Visible = false;
            // 
            // btnSaveConsecutiveQuotes
            // 
            this.btnSaveConsecutiveQuotes.Location = new System.Drawing.Point(3, 344);
            this.btnSaveConsecutiveQuotes.Name = "btnSaveConsecutiveQuotes";
            this.btnSaveConsecutiveQuotes.Size = new System.Drawing.Size(179, 60);
            this.btnSaveConsecutiveQuotes.TabIndex = 9;
            this.btnSaveConsecutiveQuotes.Text = "Save CSV for Consecutive Quote Occurence in Masechet";
            this.btnSaveConsecutiveQuotes.UseVisualStyleBackColor = true;
            this.btnSaveConsecutiveQuotes.Click += new System.EventHandler(this.btnSaveConsecutiveQuotes_Click);
            // 
            // lstQuoteNumberList
            // 
            this.lstQuoteNumberList.FormattingEnabled = true;
            this.lstQuoteNumberList.ItemHeight = 16;
            this.lstQuoteNumberList.Location = new System.Drawing.Point(188, 238);
            this.lstQuoteNumberList.Name = "lstQuoteNumberList";
            this.lstQuoteNumberList.Size = new System.Drawing.Size(285, 84);
            this.lstQuoteNumberList.TabIndex = 8;
            // 
            // btnShowAllQuoteNumbers
            // 
            this.btnShowAllQuoteNumbers.Enabled = false;
            this.btnShowAllQuoteNumbers.Location = new System.Drawing.Point(3, 238);
            this.btnShowAllQuoteNumbers.Name = "btnShowAllQuoteNumbers";
            this.btnShowAllQuoteNumbers.Size = new System.Drawing.Size(179, 60);
            this.btnShowAllQuoteNumbers.TabIndex = 7;
            this.btnShowAllQuoteNumbers.Text = "All Quote Occurences from Mishna (in order)";
            this.btnShowAllQuoteNumbers.UseVisualStyleBackColor = true;
            // 
            // txtMishna
            // 
            this.txtMishna.Location = new System.Drawing.Point(244, 3);
            this.txtMishna.Multiline = true;
            this.txtMishna.Name = "txtMishna";
            this.txtMishna.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtMishna.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMishna.Size = new System.Drawing.Size(237, 91);
            this.txtMishna.TabIndex = 4;
            // 
            // lstMishnaQuotes
            // 
            this.lstMishnaQuotes.FormattingEnabled = true;
            this.lstMishnaQuotes.HorizontalScrollbar = true;
            this.lstMishnaQuotes.ItemHeight = 16;
            this.lstMishnaQuotes.Location = new System.Drawing.Point(3, 123);
            this.lstMishnaQuotes.Name = "lstMishnaQuotes";
            this.lstMishnaQuotes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lstMishnaQuotes.Size = new System.Drawing.Size(566, 84);
            this.lstMishnaQuotes.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 492);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.btnLoad);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ListBox lstMishna;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.ListBox lstMishnaQuotes;
        private System.Windows.Forms.TextBox txtMishna;
        private System.Windows.Forms.ListBox lstQuoteNumberList;
        private System.Windows.Forms.Button btnSaveConsecutiveQuotes;
        private System.Windows.Forms.Button btnShowAllQuoteNumbers;
    }
}


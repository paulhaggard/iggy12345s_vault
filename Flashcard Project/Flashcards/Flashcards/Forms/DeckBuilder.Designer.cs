namespace Flashcards.Forms
{
    partial class DeckBuilder
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
            this.cardList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.addWritten = new System.Windows.Forms.Button();
            this.loadChkBx = new System.Windows.Forms.CheckBox();
            this.okayBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cardList
            // 
            this.cardList.FormattingEnabled = true;
            this.cardList.Location = new System.Drawing.Point(12, 29);
            this.cardList.Name = "cardList";
            this.cardList.Size = new System.Drawing.Size(155, 290);
            this.cardList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Card List";
            // 
            // addWritten
            // 
            this.addWritten.Location = new System.Drawing.Point(173, 29);
            this.addWritten.Name = "addWritten";
            this.addWritten.Size = new System.Drawing.Size(113, 23);
            this.addWritten.TabIndex = 2;
            this.addWritten.Text = "Add Card";
            this.addWritten.UseVisualStyleBackColor = true;
            this.addWritten.Click += new System.EventHandler(this.addWritten_Click);
            // 
            // loadChkBx
            // 
            this.loadChkBx.AutoSize = true;
            this.loadChkBx.Checked = true;
            this.loadChkBx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loadChkBx.Location = new System.Drawing.Point(172, 272);
            this.loadChkBx.Name = "loadChkBx";
            this.loadChkBx.Size = new System.Drawing.Size(88, 17);
            this.loadChkBx.TabIndex = 5;
            this.loadChkBx.Text = "Load to Main";
            this.loadChkBx.UseVisualStyleBackColor = true;
            // 
            // okayBtn
            // 
            this.okayBtn.Location = new System.Drawing.Point(253, 295);
            this.okayBtn.Name = "okayBtn";
            this.okayBtn.Size = new System.Drawing.Size(75, 23);
            this.okayBtn.TabIndex = 6;
            this.okayBtn.Text = "Accept";
            this.okayBtn.UseVisualStyleBackColor = true;
            this.okayBtn.Click += new System.EventHandler(this.okayBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(172, 295);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 7;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(173, 107);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 20);
            this.textBox1.TabIndex = 8;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Name";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Remove Card";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DeckBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 330);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okayBtn);
            this.Controls.Add(this.loadChkBx);
            this.Controls.Add(this.addWritten);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cardList);
            this.Name = "DeckBuilder";
            this.Text = "DeckBuilder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox cardList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addWritten;
        private System.Windows.Forms.CheckBox loadChkBx;
        private System.Windows.Forms.Button okayBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}
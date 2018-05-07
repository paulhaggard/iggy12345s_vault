namespace Flashcards.Forms
{
    partial class WrittenResponseForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.questionText = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.whoCares = new System.Windows.Forms.Label();
            this.hintList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addHintBtn = new System.Windows.Forms.Button();
            this.removeHintBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Question";
            // 
            // questionText
            // 
            this.questionText.Location = new System.Drawing.Point(15, 25);
            this.questionText.Multiline = true;
            this.questionText.Name = "questionText";
            this.questionText.Size = new System.Drawing.Size(408, 152);
            this.questionText.TabIndex = 1;
            this.questionText.TextChanged += new System.EventHandler(this.questionText_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 196);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(408, 94);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // whoCares
            // 
            this.whoCares.AutoSize = true;
            this.whoCares.Location = new System.Drawing.Point(12, 180);
            this.whoCares.Name = "whoCares";
            this.whoCares.Size = new System.Drawing.Size(42, 13);
            this.whoCares.TabIndex = 3;
            this.whoCares.Text = "Answer";
            // 
            // hintList
            // 
            this.hintList.FormattingEnabled = true;
            this.hintList.Location = new System.Drawing.Point(429, 25);
            this.hintList.Name = "hintList";
            this.hintList.Size = new System.Drawing.Size(120, 264);
            this.hintList.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(429, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Hints";
            // 
            // addHintBtn
            // 
            this.addHintBtn.Location = new System.Drawing.Point(555, 25);
            this.addHintBtn.Name = "addHintBtn";
            this.addHintBtn.Size = new System.Drawing.Size(75, 23);
            this.addHintBtn.TabIndex = 6;
            this.addHintBtn.Text = "New";
            this.addHintBtn.UseVisualStyleBackColor = true;
            this.addHintBtn.Click += new System.EventHandler(this.addHintBtn_Click);
            // 
            // removeHintBtn
            // 
            this.removeHintBtn.Location = new System.Drawing.Point(555, 55);
            this.removeHintBtn.Name = "removeHintBtn";
            this.removeHintBtn.Size = new System.Drawing.Size(75, 23);
            this.removeHintBtn.TabIndex = 7;
            this.removeHintBtn.Text = "Remove";
            this.removeHintBtn.UseVisualStyleBackColor = true;
            this.removeHintBtn.Click += new System.EventHandler(this.removeHintBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(15, 296);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // acceptBtn
            // 
            this.acceptBtn.Location = new System.Drawing.Point(96, 296);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 9;
            this.acceptBtn.Text = "Accept";
            this.acceptBtn.UseVisualStyleBackColor = true;
            this.acceptBtn.Click += new System.EventHandler(this.acceptBtn_Click);
            // 
            // WrittenResponseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 333);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.removeHintBtn);
            this.Controls.Add(this.addHintBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hintList);
            this.Controls.Add(this.whoCares);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.questionText);
            this.Controls.Add(this.label1);
            this.Name = "WrittenResponseForm";
            this.Text = "WrittenResponse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox questionText;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label whoCares;
        private System.Windows.Forms.ListBox hintList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addHintBtn;
        private System.Windows.Forms.Button removeHintBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button acceptBtn;
    }
}
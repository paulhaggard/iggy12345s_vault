namespace Flashcards.Forms
{
    partial class MultipleGuessForm
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
            this.questionText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.answerList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.acceptBtn = new System.Windows.Forms.Button();
            this.addAnswerBtn = new System.Windows.Forms.Button();
            this.removeAnswerBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // questionText
            // 
            this.questionText.Location = new System.Drawing.Point(15, 25);
            this.questionText.Multiline = true;
            this.questionText.Name = "questionText";
            this.questionText.Size = new System.Drawing.Size(300, 145);
            this.questionText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Question";
            // 
            // answerList
            // 
            this.answerList.FormattingEnabled = true;
            this.answerList.Location = new System.Drawing.Point(321, 25);
            this.answerList.Name = "answerList";
            this.answerList.Size = new System.Drawing.Size(120, 147);
            this.answerList.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Answers";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(15, 176);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // acceptBtn
            // 
            this.acceptBtn.Location = new System.Drawing.Point(96, 176);
            this.acceptBtn.Name = "acceptBtn";
            this.acceptBtn.Size = new System.Drawing.Size(75, 23);
            this.acceptBtn.TabIndex = 5;
            this.acceptBtn.Text = "Accept";
            this.acceptBtn.UseVisualStyleBackColor = true;
            // 
            // addAnswerBtn
            // 
            this.addAnswerBtn.Location = new System.Drawing.Point(447, 25);
            this.addAnswerBtn.Name = "addAnswerBtn";
            this.addAnswerBtn.Size = new System.Drawing.Size(75, 23);
            this.addAnswerBtn.TabIndex = 6;
            this.addAnswerBtn.Text = "Add";
            this.addAnswerBtn.UseVisualStyleBackColor = true;
            this.addAnswerBtn.Click += new System.EventHandler(this.addAnswerBtn_Click);
            // 
            // removeAnswerBtn
            // 
            this.removeAnswerBtn.Location = new System.Drawing.Point(448, 55);
            this.removeAnswerBtn.Name = "removeAnswerBtn";
            this.removeAnswerBtn.Size = new System.Drawing.Size(75, 23);
            this.removeAnswerBtn.TabIndex = 7;
            this.removeAnswerBtn.Text = "Remove";
            this.removeAnswerBtn.UseVisualStyleBackColor = true;
            // 
            // MultipleGuessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 205);
            this.Controls.Add(this.removeAnswerBtn);
            this.Controls.Add(this.addAnswerBtn);
            this.Controls.Add(this.acceptBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.answerList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.questionText);
            this.Name = "MultipleGuessForm";
            this.Text = "MultipleGuessForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox questionText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox answerList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button acceptBtn;
        private System.Windows.Forms.Button addAnswerBtn;
        private System.Windows.Forms.Button removeAnswerBtn;
    }
}
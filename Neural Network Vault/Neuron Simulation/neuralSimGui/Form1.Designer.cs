namespace Neuron_Simulation
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
            this.LayoutBox = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.InputLayerWeights = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HiddenLayerAWeights = new System.Windows.Forms.PictureBox();
            this.HiddenLayerBWeights = new System.Windows.Forms.PictureBox();
            this.Activations = new System.Windows.Forms.Label();
            this.InputLayerActivations = new System.Windows.Forms.PictureBox();
            this.HiddenLayerAActivations = new System.Windows.Forms.PictureBox();
            this.HiddenLayerBActivations = new System.Windows.Forms.PictureBox();
            this.OutputLayerActivations = new System.Windows.Forms.PictureBox();
            this.TestNet = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.numSampCtrl = new System.Windows.Forms.NumericUpDown();
            this.numItrCtrl = new System.Windows.Forms.NumericUpDown();
            this.numSampLbl = new System.Windows.Forms.Label();
            this.numItrLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LayoutBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputLayerWeights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerAWeights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerBWeights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputLayerActivations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerAActivations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerBActivations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputLayerActivations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numItrCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // LayoutBox
            // 
            this.LayoutBox.Location = new System.Drawing.Point(12, 12);
            this.LayoutBox.Name = "LayoutBox";
            this.LayoutBox.Size = new System.Drawing.Size(723, 401);
            this.LayoutBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.LayoutBox.TabIndex = 0;
            this.LayoutBox.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(107, 419);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(352, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 419);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Training Progress:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(465, 419);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Train";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InputLayerWeights
            // 
            this.InputLayerWeights.Location = new System.Drawing.Point(12, 473);
            this.InputLayerWeights.Name = "InputLayerWeights";
            this.InputLayerWeights.Size = new System.Drawing.Size(115, 101);
            this.InputLayerWeights.TabIndex = 5;
            this.InputLayerWeights.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Layer Weights:";
            // 
            // HiddenLayerAWeights
            // 
            this.HiddenLayerAWeights.Location = new System.Drawing.Point(133, 473);
            this.HiddenLayerAWeights.Name = "HiddenLayerAWeights";
            this.HiddenLayerAWeights.Size = new System.Drawing.Size(115, 101);
            this.HiddenLayerAWeights.TabIndex = 5;
            this.HiddenLayerAWeights.TabStop = false;
            // 
            // HiddenLayerBWeights
            // 
            this.HiddenLayerBWeights.Location = new System.Drawing.Point(254, 473);
            this.HiddenLayerBWeights.Name = "HiddenLayerBWeights";
            this.HiddenLayerBWeights.Size = new System.Drawing.Size(115, 101);
            this.HiddenLayerBWeights.TabIndex = 5;
            this.HiddenLayerBWeights.TabStop = false;
            // 
            // Activations
            // 
            this.Activations.AutoSize = true;
            this.Activations.Location = new System.Drawing.Point(742, 13);
            this.Activations.Name = "Activations";
            this.Activations.Size = new System.Drawing.Size(62, 13);
            this.Activations.TabIndex = 7;
            this.Activations.Text = "Activations:";
            // 
            // InputLayerActivations
            // 
            this.InputLayerActivations.Location = new System.Drawing.Point(745, 29);
            this.InputLayerActivations.Name = "InputLayerActivations";
            this.InputLayerActivations.Size = new System.Drawing.Size(115, 101);
            this.InputLayerActivations.TabIndex = 5;
            this.InputLayerActivations.TabStop = false;
            // 
            // HiddenLayerAActivations
            // 
            this.HiddenLayerAActivations.Location = new System.Drawing.Point(745, 136);
            this.HiddenLayerAActivations.Name = "HiddenLayerAActivations";
            this.HiddenLayerAActivations.Size = new System.Drawing.Size(115, 101);
            this.HiddenLayerAActivations.TabIndex = 5;
            this.HiddenLayerAActivations.TabStop = false;
            // 
            // HiddenLayerBActivations
            // 
            this.HiddenLayerBActivations.Location = new System.Drawing.Point(745, 243);
            this.HiddenLayerBActivations.Name = "HiddenLayerBActivations";
            this.HiddenLayerBActivations.Size = new System.Drawing.Size(115, 101);
            this.HiddenLayerBActivations.TabIndex = 5;
            this.HiddenLayerBActivations.TabStop = false;
            // 
            // OutputLayerActivations
            // 
            this.OutputLayerActivations.Location = new System.Drawing.Point(745, 350);
            this.OutputLayerActivations.Name = "OutputLayerActivations";
            this.OutputLayerActivations.Size = new System.Drawing.Size(115, 101);
            this.OutputLayerActivations.TabIndex = 5;
            this.OutputLayerActivations.TabStop = false;
            // 
            // TestNet
            // 
            this.TestNet.Location = new System.Drawing.Point(736, 568);
            this.TestNet.Name = "TestNet";
            this.TestNet.Size = new System.Drawing.Size(101, 23);
            this.TestNet.TabIndex = 8;
            this.TestNet.Text = "Test Network";
            this.TestNet.UseVisualStyleBackColor = true;
            this.TestNet.Click += new System.EventHandler(this.TestNet_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(843, 568);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 9;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 445);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Current Error:";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(180, 445);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.ErrorLabel.TabIndex = 11;
            // 
            // numSampCtrl
            // 
            this.numSampCtrl.Location = new System.Drawing.Point(375, 487);
            this.numSampCtrl.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSampCtrl.Name = "numSampCtrl";
            this.numSampCtrl.Size = new System.Drawing.Size(120, 20);
            this.numSampCtrl.TabIndex = 12;
            // 
            // numItrCtrl
            // 
            this.numItrCtrl.Location = new System.Drawing.Point(375, 526);
            this.numItrCtrl.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numItrCtrl.Name = "numItrCtrl";
            this.numItrCtrl.Size = new System.Drawing.Size(118, 20);
            this.numItrCtrl.TabIndex = 13;
            // 
            // numSampLbl
            // 
            this.numSampLbl.AutoSize = true;
            this.numSampLbl.Location = new System.Drawing.Point(375, 471);
            this.numSampLbl.Name = "numSampLbl";
            this.numSampLbl.Size = new System.Drawing.Size(68, 13);
            this.numSampLbl.TabIndex = 14;
            this.numSampLbl.Text = "num samples";
            // 
            // numItrLbl
            // 
            this.numItrLbl.AutoSize = true;
            this.numItrLbl.Location = new System.Drawing.Point(375, 510);
            this.numItrLbl.Name = "numItrLbl";
            this.numItrLbl.Size = new System.Drawing.Size(72, 13);
            this.numItrLbl.TabIndex = 15;
            this.numItrLbl.Text = "num iterations";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 616);
            this.Controls.Add(this.numItrLbl);
            this.Controls.Add(this.numSampLbl);
            this.Controls.Add(this.numItrCtrl);
            this.Controls.Add(this.numSampCtrl);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.TestNet);
            this.Controls.Add(this.Activations);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.HiddenLayerBWeights);
            this.Controls.Add(this.OutputLayerActivations);
            this.Controls.Add(this.HiddenLayerBActivations);
            this.Controls.Add(this.HiddenLayerAActivations);
            this.Controls.Add(this.HiddenLayerAWeights);
            this.Controls.Add(this.InputLayerActivations);
            this.Controls.Add(this.InputLayerWeights);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LayoutBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.LayoutBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputLayerWeights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerAWeights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerBWeights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputLayerActivations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerAActivations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HiddenLayerBActivations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputLayerActivations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSampCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numItrCtrl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.PictureBox LayoutBox;
        private System.Windows.Forms.PictureBox InputLayerWeights;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox HiddenLayerAWeights;
        private System.Windows.Forms.PictureBox HiddenLayerBWeights;
        private System.Windows.Forms.Label Activations;
        private System.Windows.Forms.PictureBox InputLayerActivations;
        private System.Windows.Forms.PictureBox HiddenLayerAActivations;
        private System.Windows.Forms.PictureBox HiddenLayerBActivations;
        private System.Windows.Forms.PictureBox OutputLayerActivations;
        private System.Windows.Forms.Button TestNet;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.NumericUpDown numSampCtrl;
        private System.Windows.Forms.NumericUpDown numItrCtrl;
        private System.Windows.Forms.Label numSampLbl;
        private System.Windows.Forms.Label numItrLbl;
    }
}


namespace directedStudies
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
            this.btnStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelExtract = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn = new System.Windows.Forms.Button();
            this.tbAnnotations = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseOutputExtract = new System.Windows.Forms.Button();
            this.tbOutputExtract = new System.Windows.Forms.TextBox();
            this.labelSelectImagesTrain = new System.Windows.Forms.Label();
            this.btnBrowseImagesExtract = new System.Windows.Forms.Button();
            this.tbImagesExtract = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowseImagesTest = new System.Windows.Forms.Button();
            this.tbImagesTest = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseOutputTest = new System.Windows.Forms.Button();
            this.tbOutputTest = new System.Windows.Forms.TextBox();
            this.labelModel = new System.Windows.Forms.Label();
            this.btnBrowseModel = new System.Windows.Forms.Button();
            this.tbModelTest = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowseNegTrain = new System.Windows.Forms.Button();
            this.tbNegTrain = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowsePosTrain = new System.Windows.Forms.Button();
            this.tbPosTrain = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.tbModelOutput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(154, 179);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 28);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(376, 547);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(162, 114);
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(274, 466);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(320, 17);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "Press the Start button to start extracting channels";
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(271, 147);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 16;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnTest
            // 
            this.btnTest.AllowDrop = true;
            this.btnTest.Location = new System.Drawing.Point(271, 147);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelExtract);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn);
            this.panel1.Controls.Add(this.tbAnnotations);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBrowseOutputExtract);
            this.panel1.Controls.Add(this.tbOutputExtract);
            this.panel1.Controls.Add(this.labelSelectImagesTrain);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnBrowseImagesExtract);
            this.panel1.Controls.Add(this.tbImagesExtract);
            this.panel1.Location = new System.Drawing.Point(833, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(24, 69);
            this.panel1.TabIndex = 18;
            // 
            // labelExtract
            // 
            this.labelExtract.AutoSize = true;
            this.labelExtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExtract.Location = new System.Drawing.Point(174, 6);
            this.labelExtract.Name = "labelExtract";
            this.labelExtract.Size = new System.Drawing.Size(67, 24);
            this.labelExtract.TabIndex = 25;
            this.labelExtract.Text = "Extract";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "Select annotations folder";
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(14, 50);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(75, 23);
            this.btn.TabIndex = 23;
            this.btn.Text = "Browse";
            this.btn.UseVisualStyleBackColor = true;
            // 
            // tbAnnotations
            // 
            this.tbAnnotations.Location = new System.Drawing.Point(95, 50);
            this.tbAnnotations.Name = "tbAnnotations";
            this.tbAnnotations.Size = new System.Drawing.Size(510, 22);
            this.tbAnnotations.TabIndex = 22;
            this.tbAnnotations.Text = "D:\\uOttawa\\Winter 2018\\INRIAPerson\\INRIAPerson\\ann";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "Select output folder";
            // 
            // btnBrowseOutputExtract
            // 
            this.btnBrowseOutputExtract.Location = new System.Drawing.Point(14, 151);
            this.btnBrowseOutputExtract.Name = "btnBrowseOutputExtract";
            this.btnBrowseOutputExtract.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutputExtract.TabIndex = 20;
            this.btnBrowseOutputExtract.Text = "Browse";
            this.btnBrowseOutputExtract.UseVisualStyleBackColor = true;
            // 
            // tbOutputExtract
            // 
            this.tbOutputExtract.Location = new System.Drawing.Point(95, 151);
            this.tbOutputExtract.Name = "tbOutputExtract";
            this.tbOutputExtract.Size = new System.Drawing.Size(510, 22);
            this.tbOutputExtract.TabIndex = 19;
            this.tbOutputExtract.Text = "C:\\Users\\welah\\Desktop\\testing";
            // 
            // labelSelectImagesTrain
            // 
            this.labelSelectImagesTrain.AutoSize = true;
            this.labelSelectImagesTrain.Location = new System.Drawing.Point(11, 75);
            this.labelSelectImagesTrain.Name = "labelSelectImagesTrain";
            this.labelSelectImagesTrain.Size = new System.Drawing.Size(136, 17);
            this.labelSelectImagesTrain.TabIndex = 18;
            this.labelSelectImagesTrain.Text = "Select images folder";
            // 
            // btnBrowseImagesExtract
            // 
            this.btnBrowseImagesExtract.Location = new System.Drawing.Point(14, 95);
            this.btnBrowseImagesExtract.Name = "btnBrowseImagesExtract";
            this.btnBrowseImagesExtract.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImagesExtract.TabIndex = 17;
            this.btnBrowseImagesExtract.Text = "Browse";
            this.btnBrowseImagesExtract.UseVisualStyleBackColor = true;
            // 
            // tbImagesExtract
            // 
            this.tbImagesExtract.Location = new System.Drawing.Point(95, 95);
            this.tbImagesExtract.Name = "tbImagesExtract";
            this.tbImagesExtract.Size = new System.Drawing.Size(510, 22);
            this.tbImagesExtract.TabIndex = 16;
            this.tbImagesExtract.Text = "D:\\uOttawa\\Winter 2018\\INRIAPerson\\INRIAPerson\\Train\\pos";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.btnBrowseImagesTest);
            this.panel2.Controls.Add(this.tbImagesTest);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.btnBrowseOutputTest);
            this.panel2.Controls.Add(this.tbOutputTest);
            this.panel2.Controls.Add(this.btnTest);
            this.panel2.Controls.Add(this.labelModel);
            this.panel2.Controls.Add(this.btnBrowseModel);
            this.panel2.Controls.Add(this.tbModelTest);
            this.panel2.Location = new System.Drawing.Point(78, 228);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(735, 187);
            this.panel2.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Select images folder";
            // 
            // btnBrowseImagesTest
            // 
            this.btnBrowseImagesTest.Location = new System.Drawing.Point(14, 70);
            this.btnBrowseImagesTest.Name = "btnBrowseImagesTest";
            this.btnBrowseImagesTest.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseImagesTest.TabIndex = 20;
            this.btnBrowseImagesTest.Text = "Browse";
            this.btnBrowseImagesTest.UseVisualStyleBackColor = true;
            // 
            // tbImagesTest
            // 
            this.tbImagesTest.Location = new System.Drawing.Point(95, 70);
            this.tbImagesTest.Name = "tbImagesTest";
            this.tbImagesTest.Size = new System.Drawing.Size(431, 22);
            this.tbImagesTest.TabIndex = 19;
            this.tbImagesTest.Text = "D:\\uOttawa\\Winter 2018\\INRIAPerson\\INRIAPerson\\Test\\pos";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "Output";
            // 
            // btnBrowseOutputTest
            // 
            this.btnBrowseOutputTest.Location = new System.Drawing.Point(14, 119);
            this.btnBrowseOutputTest.Name = "btnBrowseOutputTest";
            this.btnBrowseOutputTest.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutputTest.TabIndex = 10;
            this.btnBrowseOutputTest.Text = "Browse";
            this.btnBrowseOutputTest.UseVisualStyleBackColor = true;
            // 
            // tbOutputTest
            // 
            this.tbOutputTest.Location = new System.Drawing.Point(95, 119);
            this.tbOutputTest.Name = "tbOutputTest";
            this.tbOutputTest.Size = new System.Drawing.Size(431, 22);
            this.tbOutputTest.TabIndex = 9;
            this.tbOutputTest.Text = "C:\\Users\\welah\\Desktop\\testing";
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(10, 4);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(89, 17);
            this.labelModel.TabIndex = 8;
            this.labelModel.Text = "Select model";
            // 
            // btnBrowseModel
            // 
            this.btnBrowseModel.Location = new System.Drawing.Point(14, 24);
            this.btnBrowseModel.Name = "btnBrowseModel";
            this.btnBrowseModel.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseModel.TabIndex = 7;
            this.btnBrowseModel.Text = "Browse";
            this.btnBrowseModel.UseVisualStyleBackColor = true;
            // 
            // tbModelTest
            // 
            this.tbModelTest.Location = new System.Drawing.Point(95, 24);
            this.tbModelTest.Name = "tbModelTest";
            this.tbModelTest.Size = new System.Drawing.Size(431, 22);
            this.tbModelTest.TabIndex = 6;
            this.tbModelTest.Text = "C:\\Users\\welah\\Desktop\\testing\\tree.xml";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.btnBrowseNegTrain);
            this.panel3.Controls.Add(this.tbNegTrain);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.btnBrowsePosTrain);
            this.panel3.Controls.Add(this.btnTrain);
            this.panel3.Controls.Add(this.tbPosTrain);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.tbModelOutput);
            this.panel3.Location = new System.Drawing.Point(78, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(735, 187);
            this.panel3.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 17);
            this.label5.TabIndex = 23;
            this.label5.Text = "Neg";
            // 
            // btnBrowseNegTrain
            // 
            this.btnBrowseNegTrain.Location = new System.Drawing.Point(12, 69);
            this.btnBrowseNegTrain.Name = "btnBrowseNegTrain";
            this.btnBrowseNegTrain.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseNegTrain.TabIndex = 22;
            this.btnBrowseNegTrain.Text = "Browse";
            this.btnBrowseNegTrain.UseVisualStyleBackColor = true;
            // 
            // tbNegTrain
            // 
            this.tbNegTrain.Location = new System.Drawing.Point(93, 69);
            this.tbNegTrain.Name = "tbNegTrain";
            this.tbNegTrain.Size = new System.Drawing.Size(514, 22);
            this.tbNegTrain.TabIndex = 21;
            this.tbNegTrain.Text = "D:\\uOttawa\\Winter 2018\\INRIAPerson\\INRIAPerson\\Train\\neg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "Pos";
            // 
            // btnBrowsePosTrain
            // 
            this.btnBrowsePosTrain.Location = new System.Drawing.Point(12, 24);
            this.btnBrowsePosTrain.Name = "btnBrowsePosTrain";
            this.btnBrowsePosTrain.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePosTrain.TabIndex = 19;
            this.btnBrowsePosTrain.Text = "Browse";
            this.btnBrowsePosTrain.UseVisualStyleBackColor = true;
            // 
            // tbPosTrain
            // 
            this.tbPosTrain.Location = new System.Drawing.Point(93, 24);
            this.tbPosTrain.Name = "tbPosTrain";
            this.tbPosTrain.Size = new System.Drawing.Size(514, 22);
            this.tbPosTrain.TabIndex = 18;
            this.tbPosTrain.Text = "D:\\uOttawa\\Winter 2018\\old\\INRIAPerson\\INRIAPerson\\train_64x128_H96\\pos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Model Output";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 119);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // tbModelOutput
            // 
            this.tbModelOutput.Location = new System.Drawing.Point(93, 119);
            this.tbModelOutput.Name = "tbModelOutput";
            this.tbModelOutput.Size = new System.Drawing.Size(514, 22);
            this.tbModelOutput.TabIndex = 6;
            this.tbModelOutput.Text = "C:\\Users\\welah\\Desktop\\testing";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 668);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseOutputExtract;
        private System.Windows.Forms.TextBox tbOutputExtract;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnBrowseOutputTest;
        private System.Windows.Forms.TextBox tbOutputTest;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Button btnBrowseModel;
        private System.Windows.Forms.TextBox tbModelTest;
        private System.Windows.Forms.Label labelSelectImagesTrain;
        private System.Windows.Forms.Button btnBrowseImagesExtract;
        private System.Windows.Forms.TextBox tbImagesExtract;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.TextBox tbAnnotations;
        private System.Windows.Forms.Label labelExtract;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowseNegTrain;
        private System.Windows.Forms.TextBox tbNegTrain;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnBrowsePosTrain;
        private System.Windows.Forms.TextBox tbPosTrain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbModelOutput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBrowseImagesTest;
        private System.Windows.Forms.TextBox tbImagesTest;
        private System.Windows.Forms.Label label6;
    }
}


using Emgu.CV;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using System.Xml;
using Emgu.CV.ML;
using Emgu.CV.Dnn;
using Emgu.CV.ML.MlEnum;

namespace directedStudies
{
    public partial class Form1 : Form
    {
        //List<string> list_file_names = new List<string>();
        coreFunctions coreFunctions = new coreFunctions();
        functionsGets functionsGets = new functionsGets();
        public Form1()
        {
            InitializeComponent();
        }
        private void btnBrowseAnnotations_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbAnnotations.Text = fbd.SelectedPath;
                }
            }
        }
        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbOutputExtract.Text = fbd.SelectedPath;
                }
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            pictureBox.Image = Properties.Resources.Animation;
            backgroundWorker1.RunWorkerAsync();
        }
        
        

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is String)
            {
                this.progressLabel.Text = (String)e.UserState;  //update progressLabel
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.pictureBox.Image = null;
            if (e.Error != null)    //if an error occured
            {
                this.progressLabel.Text = "Operation failed: " + e.Error.Message;   //report error
                this.pictureBox.Image = Properties.Resources.Error; //show error icon
            }
            else    //if no error occured
            {
                this.progressLabel.Text = "Operation completed successfuly";
                this.pictureBox.Image = Properties.Resources.Information;   //show information icon
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.backgroundWorker1.ReportProgress(0, string.Format("Processing..."));  //show that we are still alive
            coreFunctions.startExtraction(tbImagesExtract.Text, tbOutputExtract.Text, tbAnnotations.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox.Image = Properties.Resources.ready1;
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            {/*
                Matrix<float> samp = new Matrix<float>(2834 + 2 * 1218, 5120, 1);
                Matrix<float> labels = new Matrix<float>(2834 + 2 * 1218, 1, 1);
                int mat_row_index = 0;
                List<Image<Bgr, byte>> list_train_pos = new List<Image<Bgr, byte>>();
                List<Image<Bgr, byte>> list_train_neg = new List<Image<Bgr, byte>>();

                //INITIAL TRAINING OF POSITIVE WINDOWS

                string[] pos_train_images_paths = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\train_64x128_H96\pos");
                foreach (var pos_train_image_path in pos_train_images_paths)
                {
                    Image<Bgr, byte> pos_train_image = new Image<Bgr, byte>(pos_train_image_path);
                    pos_train_image = pos_train_image.SmoothGaussian(3);
                    Rectangle roi;
                    if (pos_train_image.Width == 70)
                        roi = new Rectangle(6, 6, 64, 128);
                    else
                        roi = new Rectangle(32, 32, 64, 128);
                    pos_train_image.ROI = roi;
                    list_train_pos.Add(pos_train_image.Copy());
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos_train_image.Copy());
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    coreFunctions.AddACFToMat(samp, s, mat_row_index);
                    labels.Data[mat_row_index, 0] = 1;
                    mat_row_index++;
                }
                Console.WriteLine("Added Pos");

                //INITIAL TRAINING OF NEGATIVE WINDOWS

                string[] neg_train_image_paths = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Train\neg");
                foreach (var neg_train_image_path in neg_train_image_paths)
                {
                    Image<Bgr, byte> neg_train_image = new Image<Bgr, byte>(neg_train_image_path);
                    neg_train_image = neg_train_image.SmoothGaussian(3);
                    List<Image<Bgr, byte>> rois = functionsGets.GetRandomNegRectangles(neg_train_image.Copy(), 2);
                    foreach (var roi in rois)
                    {
                        list_train_neg.Add(roi.Copy());
                        List<Image<Gray, UInt16>> channels = functionsGets.GetACF(roi.Copy());
                        Matrix<float> s = functionsGets.getMatFromImageList(channels);
                        coreFunctions.AddACFToMat(samp, s, mat_row_index);
                        labels.Data[mat_row_index, 0] = 2;
                        mat_row_index++;
                    }
                }
                Console.WriteLine("Added Neg");

                TrainData td = new TrainData(samp, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, labels);

                Console.WriteLine("Training forest");

                RTrees forest = new RTrees();
                forest.MaxDepth = 2;
                forest.Use1SERule = true;
                forest.Train(td);

                Console.WriteLine("Getting HN");

                List<Image<Bgr, byte>> list_negs_retrain = new List<Image<Bgr, byte>>();
                foreach (var neg in list_train_neg)
                {
                    list_negs_retrain.Add(neg);
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg.Copy());
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = forest.Predict(s);
                    if (Math.Round(x) == 1)
                    {
                        list_negs_retrain.Add(neg);
                    }
                }

                Console.WriteLine("Getting HP");

                List<Image<Bgr, byte>> list_pos_retrain = new List<Image<Bgr, byte>>();
                foreach (var pos in list_train_pos)
                {
                    list_pos_retrain.Add(pos);
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos.Copy());
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = forest.Predict(s);
                    if (Math.Round(x) == 2)
                    {
                        list_pos_retrain.Add(pos);
                    }
                }

                Console.WriteLine("Equalizing");
                int diff = Math.Abs(list_train_pos.Count - list_negs_retrain.Count);
                for (int i = 0; i < diff; i++)
                {
                    Random rand = new Random();
                    if (list_pos_retrain.Count > list_negs_retrain.Count)
                    {
                        int index = rand.Next(0, neg_train_image_paths.Count());
                        Image<Bgr, byte> neg = new Image<Bgr, byte>(neg_train_image_paths[index]);
                        List<Image<Bgr, byte>> neg_to_be_added = functionsGets.GetRandomNegRectangles(neg.Copy(), 1);
                        list_negs_retrain.Add(neg_to_be_added[0].Copy());
                    }
                    else
                    {
                        int index = rand.Next(0, list_train_pos.Count - 1);
                        list_pos_retrain.Add(list_train_pos[index].Copy());
                    }
                }

                samp = new Matrix<float>(list_pos_retrain.Count + list_negs_retrain.Count, 5120, 1);
                labels = new Matrix<float>(list_pos_retrain.Count + list_negs_retrain.Count, 1, 1);
                mat_row_index = 0;

                Console.WriteLine("Readding pos");
                foreach (var pos in list_pos_retrain)
                {
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos.Copy());
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    coreFunctions.AddACFToMat(samp, s, mat_row_index);
                    labels.Data[mat_row_index, 0] = 1;
                    mat_row_index++;
                }
                Console.WriteLine("Readding neg");
                foreach (var neg in list_negs_retrain)
                {
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg.Copy());
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    coreFunctions.AddACFToMat(samp, s, mat_row_index);
                    labels.Data[mat_row_index, 0] = 2;
                    mat_row_index++;
                }
                Console.WriteLine("Retraining");
                TrainData tdf = new TrainData(samp, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, labels);
                RTrees forestf = new RTrees();
                forestf.MaxDepth = 2;
                forestf.Use1SERule = true;
                forestf.Train(tdf);

                FileStorage fs = new FileStorage(@"C:\Users\welah\Desktop\testing\" + "forest.xml", FileStorage.Mode.Write);
                forestf.Write(fs);
                fs.ReleaseAndGetString();


                Console.WriteLine("Predicting pos");
                int total_pos = 0;
                int total_neg = 0;
                int ann = 0;
                List<float> xspos = new List<float>();
                List<float> xsneg = new List<float>();
                //string[] image_folders_pos = Directory.GetDirectories(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Test\pos");
                string[] images_paths_pos = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\test_64x128_H96\pos");
                foreach (var image_path in images_paths_pos)
                {
                    Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                    img_original = img_original.SmoothGaussian(3);
                    Rectangle roi;
                    if (img_original.Width == 70)
                        roi = new Rectangle(6, 6, 64, 128);
                    else
                        roi = new Rectangle(32, 32, 64, 128);
                    img_original.ROI = new Rectangle(6, 6, 64, 128);

                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(img_original);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = forestf.Predict(s);
                    xspos.Add(x);
                    if (Math.Round(x) == 1)
                        total_pos++;
                }
                Console.WriteLine("Predicting neg");
                string[] images_paths_neg = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Test\neg");
                foreach (var image_path in images_paths_neg)
                {
                    Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                    img_original = img_original.SmoothGaussian(3);
                    List<Image<Bgr, byte>> list_negs = functionsGets.GetRandomNegRectangles(img_original, 2);
                    foreach (var neg in list_negs)
                    {
                        List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg);
                        Matrix<float> s = functionsGets.getMatFromImageList(channels);
                        float x = forestf.Predict(s);
                        xsneg.Add(x);
                        if (Math.Round(x) == 2)
                            total_neg++;
                    }
                }
                int posbel = 0;
                int negab = 0;
                foreach (var item in xspos)
                    if (item <= 1.3)
                        posbel++;
                foreach (var item in xsneg)
                    if (item > 1.3)
                        negab++;
                Console.WriteLine("Av pos: {0} - posbel: {1} - posab: {2}", xspos.Average().ToString(), posbel, xspos.Count() - posbel);
                Console.WriteLine("Av neg: {0} - negab: {1} - negbe: {2}", xsneg.Average().ToString(), negab, xsneg.Count() - negab);

                float avpos = xspos.Average();
                float maxpos = xspos.Max();
                float minpos = xspos.Min();
                float avneg = xsneg.Average();
                float maxneg = xsneg.Max();
                float minneg = xsneg.Min();

                TextWriter tw = new StreamWriter("xpos.txt");

                foreach (var x in xspos)
                    tw.WriteLine(x.ToString());

                tw.Close();

                TextWriter tw1 = new StreamWriter("xneg.txt");

                foreach (var x in xsneg)
                    tw1.WriteLine(x.ToString());

                tw1.Close();
            */}
            int nnegs = 5;
            Matrix<float> samp = new Matrix<float>(2* nnegs * 1218, 5120, 1);
            Matrix<float> labels = new Matrix<float>(2* + nnegs * 1218, 1, 1);
            int mat_row_index = 0;

            //string image_folders_pos_folder = tbImagesTrain.Text;
            //string[] image_folders_pos = Directory.GetDirectories(tbPosTrain.Text);
            List<Image<Bgr, byte>> list_poss = new List<Image<Bgr, byte>>();
            string[] image_files_pos = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\train_64x128_H96\pos");
            foreach (var image_path in image_files_pos)
            {

                //string[] channels = Directory.GetFiles(annotation_folder_pos);
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                img_original = img_original.SmoothGaussian(3);
                Image<Bgr, byte> ped = img_original.Copy();
                if (ped.Width == 70)
                    ped.ROI = new Rectangle(3, 0, 64, 128);
                else
                    ped.ROI = new Rectangle(16, 16, 64, 128);
                list_poss.Add(ped);
                if (ped.Width == 64 && ped.Height == 128)
                {
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(ped);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    //Matrix<int> l = getLabelPos();
                    coreFunctions.AddACFToMat(samp, s, mat_row_index);
                    labels.Data[mat_row_index, 0] = 1;
                    mat_row_index++;
                }
            }

            List<Image<Bgr, byte>> list_negs = new List<Image<Bgr, byte>>();
            string[] image_files_neg = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Train\neg");
            foreach (var image_path in image_files_neg)
            {
                //string[] channels = Directory.GetFiles(annotation_folder_pos);
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                img_original = img_original.SmoothGaussian(3);
                //List<Rectangle> list_rects = coreFunctions.getBoundingBoxes(image_path, @"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Train\annotations");
                List<Image<Bgr, byte>> list_neg_windows = functionsGets.GetRandomNegRectangles(img_original, nnegs);
                foreach (var img in list_neg_windows)
                {
                    //Image<Bgr, byte> temp_neg = img_original.Copy();
                    //temp_neg.ROI = rect;
                    list_negs.Add(img);
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(img);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    //Matrix<float> l = getLabelNeg();
                    coreFunctions.AddACFToMat(samp, s, mat_row_index);
                    labels.Data[mat_row_index, 0] = 2;
                    mat_row_index++;
                    //samp.Mat.PushBack(s.Mat);
                    //labels.Mat.PushBack(l.Mat);
                }
            }
            int difference = Math.Abs(list_poss.Count() - list_negs.Count());
            for (int i = 0; i < difference; i++)
            {
                Random random = new Random();
                int index = random.Next(0, list_poss.Count() - 1);
                Image<Bgr, byte> pos = list_poss[index];
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                //Matrix<int> l = getLabelPos();
                coreFunctions.AddACFToMat(samp, s, mat_row_index);
                labels.Data[mat_row_index, 0] = 1;
                mat_row_index++;
            }
            List<Image<Bgr, byte>> list_negs_final = new List<Image<Bgr, byte>>();
            List<Image<Bgr, byte>> list_poss_final = new List<Image<Bgr, byte>>();
            Console.WriteLine(samp.Size);
            Console.WriteLine("GOING ONCE!");
            TrainData td = new TrainData(samp, DataLayoutType.RowSample, labels);
            RTrees tree = new RTrees();
            //tree.TermCriteria = new MCvTermCriteria(50);
            //tree.TruncatePrunedTree = true;
            //tree.MaxDepth = 2;
            tree.Use1SERule = true;
            tree.TermCriteria = new MCvTermCriteria(0.75);
            //tree.TermCriteria = new MCvTermCriteria(200);
            tree.Train(td);
            Console.WriteLine("SOLD!");

            foreach (var pos in list_poss)
            {
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                float x = tree.Predict(s);
                if (Math.Round(x) == 2)
                    list_poss_final.Add(pos);
            }
            foreach (var pos in list_poss)
                list_poss_final.Add(pos);

            foreach (var neg in list_negs)
            {
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                float x = tree.Predict(s);
                if (Math.Round(x) == 1)
                    list_negs_final.Add(neg);
            }
            foreach (var neg in list_negs)
                list_negs_final.Add(neg);

            //List<Image<Bgr, byte>> finalest_pos = list_poss_final;
            //List<Image<Bgr, byte>> finalest_neg = list_negs_final;
            List<Image<Bgr, byte>> finalest_pos = new List<Image<Bgr, byte>>();
            List<Image<Bgr, byte>> finalest_neg = new List<Image<Bgr, byte>>();
            if (list_poss_final.Count() > list_negs_final.Count())
            {
                int posc = list_poss_final.Count();
                int negc = list_negs_final.Count();
                int diff = posc - negc;
                for (int i = 0; i < diff; i++)
                {
                    Random random = new Random();
                    int index = random.Next(0, list_negs_final.Count() - 1);
                    finalest_neg.Add(list_negs_final[index]);
                }
                foreach (var neg in list_negs_final)
                    finalest_neg.Add(neg);
                finalest_pos = list_poss_final;
            }
            else if (list_negs_final.Count() > list_poss_final.Count())
            {
                int posc = list_poss_final.Count();
                int negc = list_negs_final.Count();
                int diff = negc - posc;
                for (int i = 0; i < diff; i++)
                {
                    Random random = new Random();
                    int index = random.Next(0, list_poss_final.Count() - 1);
                    finalest_pos.Add(list_poss_final[index]);
                }
                foreach (var pos in list_poss_final)
                    finalest_pos.Add(pos);
                finalest_neg = list_negs_final;
            }
            samp = new Matrix<float>(finalest_pos.Count() + finalest_neg.Count(), 5120, 1);
            labels = new Matrix<float>(finalest_pos.Count() + finalest_neg.Count(), 1, 1);
            mat_row_index = 0;
            foreach (var pos in finalest_pos)
            {
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(pos);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                //Matrix<int> l = getLabelPos();
                coreFunctions.AddACFToMat(samp, s, mat_row_index);
                labels.Data[mat_row_index, 0] = 1;
                mat_row_index++;
            }
            foreach (var neg in finalest_neg)
            {
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                //Matrix<int> l = getLabelPos();
                coreFunctions.AddACFToMat(samp, s, mat_row_index);
                labels.Data[mat_row_index, 0] = 2;
                mat_row_index++;
            }
            Console.WriteLine("GOING TWICE!");
            td = new TrainData(samp, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, labels);
            tree = new RTrees();
            //tree.TermCriteria = new MCvTermCriteria(50);
            //tree.TruncatePrunedTree = true;
            //tree.MaxDepth = 2;
            tree.Use1SERule = true;
            tree.TermCriteria = new MCvTermCriteria(0.75);
            //tree.TermCriteria = new MCvTermCriteria(200);
            tree.Train(td);
            Console.WriteLine("SOLD!");


            int total_pos = 0;
            int total_neg = 0;
            int ann = 0;
            List<float> xspos = new List<float>();
            List<float> xsneg = new List<float>();
            //string[] image_folders_pos = Directory.GetDirectories(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Test\pos");
            string[] images_paths_pos = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\test_64x128_H96\pos");
            foreach (var image_path in images_paths_pos)
            {
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                img_original = img_original.SmoothGaussian(3);
                //List<Rectangle> list_bounding_boxes = coreFunctions.getBoundingBoxes(image_path, tbAnnotations.Text);
                Image<Bgr, byte> ped = img_original;
                ped.ROI = new Rectangle(3, 0, 64, 128);

                if (ped.Width == 64 && ped.Height == 128)
                {
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(ped);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = tree.Predict(s);
                    xspos.Add(x);
                    if (Math.Round(x) == 1)
                        total_pos++;
                }
            }
            string[] images_paths_neg = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Test\neg");
            foreach (var image_path in images_paths_neg)
            {
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                img_original = img_original.SmoothGaussian(3);
                //List<Rectangle> list_bounding_boxes = coreFunctions.getBoundingBoxes(image_path, tbAnnotations.Text);
                List<Image<Bgr, byte>> list_neg_windows = functionsGets.GetRandomNegRectangles(img_original, nnegs);
                foreach (var img in list_neg_windows)
                {
                    //Image<Bgr, byte> neg = img_original;
                    //neg.ROI = rectangle;
                    //if (neg.Width > 64 || neg.Height > 128)
                    //{
                    //    neg = neg.Resize(64, 128, Inter.Linear);
                    //}
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(img);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = tree.Predict(s);
                    xsneg.Add(x);
                    if (Math.Round(x) == 2)
                        total_neg++;
                }
            }
            int posbel = 0;
            int negab = 0;
            foreach (var item in xspos)
                if (item < 1.5)
                    posbel++;
            foreach (var item in xsneg)
                if (item > 1.5)
                    negab++;
            Console.WriteLine("Av pos: {0} - posbel: {1} - posab: {2}", xspos.Average().ToString(), posbel, xspos.Count() - posbel);
            Console.WriteLine("Av neg: {0} - negab: {1} - negbe: {2}", xsneg.Average().ToString(), negab, xsneg.Count() - negab);
            FileStorage fs = new FileStorage(tbOutputExtract.Text + @"\forest.xml", FileStorage.Mode.Write);
            tree.Write(fs);
            fs.ReleaseAndGetString();
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            Emgu.CV.ML.RTrees forest = new Emgu.CV.ML.RTrees();
            FileStorage fsr = new FileStorage(@"C:\Users\welah\Desktop\testing\forest.xml", FileStorage.Mode.Read);
            forest.Read(fsr.GetRoot());
            int total_pos = 0;
            int total_neg = 0;
            int ann = 0;

            /*string[] images_paths_pos = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\test_64x128_H96\pos");
            List<float> xspos = new List<float>();
            foreach (var image_path in images_paths_pos)
            {
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                Image<Bgr, byte> ped = img_original;
                ped.ROI = new Rectangle(6,6,64,128);
                List<Image<Gray, UInt16>> channels = functionsGets.GetACF(ped);
                Matrix<float> s = functionsGets.getMatFromImageList(channels);
                float x = Emgu.CV.ML.StatModelExtensions.Predict(tree, s);
                xspos.Add(x);
                Console.WriteLine(x.ToString());
                if (Math.Round(x) == 1)
                    total_pos++;
            }
            string[] images_paths_neg = Directory.GetFiles(@"D:\uOttawa\Winter 2018\INRIAPerson\INRIAPerson\Test\neg");
            List<float> xsneg = new List<float>();
            foreach (var image_path in images_paths_neg)
            {
                Image<Bgr, byte> img_original = new Image<Bgr, byte>(image_path);
                List<Rectangle> list_bounding_boxes = coreFunctions.getBoundingBoxes(image_path, tbAnnotations.Text);
                foreach (var rectangle in list_bounding_boxes)
                {
                    Image<Bgr, byte> neg = img_original;
                    neg.ROI = rectangle;
                    if (neg.Width > 64 || neg.Height > 128)
                    {
                        neg = neg.Resize(64, 128, Inter.Linear);
                    }
                    List<Image<Gray, UInt16>> channels = functionsGets.GetACF(neg);
                    Matrix<float> s = functionsGets.getMatFromImageList(channels);
                    float x = Emgu.CV.ML.StatModelExtensions.Predict(tree, s);
                    xsneg.Add(x);
                    if (Math.Round(x) == 2)
                        total_neg++;
                }
            }
            float avpos = xspos.Average();
            float maxpos = xspos.Max();
            float minpos = xspos.Min();
            float avneg = xsneg.Average();
            float maxneg = xsneg.Max();
            float minneg = xsneg.Min();

            TextWriter tw = new StreamWriter("xpos.txt");

            foreach (var x in xspos)
                tw.WriteLine(x.ToString());

            tw.Close();

            TextWriter tw1 = new StreamWriter("xneg.txt");

            foreach (var x in xsneg)
                tw1.WriteLine(x.ToString());

            tw1.Close();*/

            string[] image_paths = Directory.GetFiles(tbImagesTest.Text);

            foreach (var image_path in image_paths)
            {
                List<Rectangle> list_rectangles_detected = new List<Rectangle>();
                Image<Bgr, byte> image_original = new Image<Bgr, byte>(image_path);
                //image_original = image_original.SmoothGaussian(3);
                Image<Bgr, byte> image_temp = image_original.Copy();
                //List<Image<Gray, UInt16>> list_channels_original = functionsGets.GetACF(image_temp);
                float factor = 1;
                int total_windows = 0;
                while(true)
                {
                    if (factor * image_original.Width > 64 && factor * image_original.Height > 128)
                    {
                        image_temp = image_original.Copy().Resize(factor, Inter.Linear);
                        //List<Image<Gray, UInt16>> list_channels_original = functionsGets.GetACF(image_temp);
                        //image_temp.Save(tbOutputTest.Text + @"\" + Path.GetFileNameWithoutExtension(image_path) + " " + factor.ToString() + ".jpg");
                        for (int m = 0; m < image_temp.Height - 128; m += 8)
                        {
                            for (int n = 0; n < image_temp.Width - 64; n += 8)
                            {
                                total_windows++;
                                List<Image<Gray, UInt16>> list_channels_window = new List<Image<Gray, UInt16>>();
                                Rectangle rect = new Rectangle(n, m, 64, 128);
                                Image<Bgr, byte> win = image_temp.Copy();
                                win.ROI = rect;
                                win = win.SmoothGaussian(3);
                                list_channels_window = functionsGets.GetACF(win);
                                int counter = 0;
                                //image_temp.Draw(rect, new Bgr(Color.Blue));
                                /*for (int i = 0; i < list_channels_original.Count(); i++)
                                {
                                    Image<Gray, UInt16> channel = list_channels_original[i];
                                    channel.ROI = rect;
                                    list_channels_window.Add(channel);
                                }*/
                                Matrix<float> s = functionsGets.getMatFromImageList(list_channels_window);
                                float x = forest.Predict(s);
                                double rounded = Math.Round(x, 2);
                                if (rounded <= 1.05)
                                {
                                    list_rectangles_detected.Add(functionsGets.ScaleRectangleToOriginalImage(rect, factor));
                                    //image_temp.Draw(rect, new Bgr(Color.Red));
                                    total_pos++;
                                }
                                else//if (Math.Round(x) == 2)
                                {
                                    //list_rectangles_detected.Add(functionsGets.ScaleRectangleToOriginalImage(rect, factor));
                                    total_neg++;
                                }
                            }
                        }
                        //image_temp.Save(tbOutputTest.Text + @"\" + Path.GetFileNameWithoutExtension(image_path) + " " + factor.ToString() + ".png");
                        //image_temp = image_original.Resize(factor, Inter.Linear);
                        //image_temp.Save(tbOutputTest.Text + @"\" + Path.GetFileNameWithoutExtension(image_path) + " " + (factor - 0.2).ToString() + ".jpg");
                        //image_temp = image_original.Resize(factor, Inter.Linear);
                        //image_temp = image_temp.Resize(factor, Inter.Linear);
                        //image_temp.Save(tbOutputTest.Text + @"\" + Path.GetFileNameWithoutExtension(image_path) + " " + (factor - 0.2).ToString() + ".jpg");
                    }
                    else
                        break;
                    factor = factor - (float)(0.2);
                }
                Emgu.CV.Util.VectorOfRect vec = new VectorOfRect();
                vec.Push(list_rectangles_detected.ToArray());
                CvInvoke.GroupRectangles(vec, 1, 0.2);
                vec = functionsGets.RemoveContainedRects(vec);
                for (int i = 0; i < vec.Size; i++)
                    image_original.Draw(vec[i], new Bgr(Color.Red));
                image_original.Save(tbOutputTest.Text + @"\" + Path.GetFileName(image_path));
        }
        }
    }
}

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directedStudies
{
    class functionsGets
    {
        public List<Image<Gray, UInt16>> GetACF(Image<Bgr, byte> image)
        {
            List<Image<Gray, UInt16>> list_channels = new List<Image<Gray, UInt16>>();

            Image<Luv, byte> luv;   //will contain luv image to extract LUV channels
            luv = image.Convert<Luv, byte>();    //convert from bgr to luv
            VectorOfUMat channels = new VectorOfUMat(); //contains luv channels
            CvInvoke.Split(luv, channels);  //split them
            Image<Gray, UInt16> image_channel_L = channels[0].ToImage<Gray, UInt16>(); //L channel
            image_channel_L = image_channel_L.SmoothGaussian(3);
            list_channels.Add(image_channel_L);
            Image<Gray, UInt16> image_channel_U = channels[1].ToImage<Gray, UInt16>(); //U channel
            image_channel_U = image_channel_U.SmoothGaussian(3);
            list_channels.Add(image_channel_U);
            Image<Gray, UInt16> image_channel_V = channels[2].ToImage<Gray, UInt16>();  //V channel
            image_channel_V = image_channel_V.SmoothGaussian(3);
            list_channels.Add(image_channel_V);

            Mat gray = new Mat();   //gray version of the original image
            Mat grad = new Mat();   //will contain the gradient magnitude
            Mat grad_x = new Mat(); //sobel x
            Mat grad_y = new Mat(); //sobel y
            Mat abs_grad_x = new Mat(); //abs
            Mat abs_grad_y = new Mat();
            Mat angles = new Mat(); //matrix will contain the angle of every edge in grad magnitude channel

            CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray); //get gray image from bgr

            //channels defined below will contain the edges in different angles
            Image<Gray, UInt16> C1 = new Image<Gray, UInt16>(image.Cols, image.Rows);
            Image<Gray, UInt16> C2 = new Image<Gray, UInt16>(image.Cols, image.Rows);
            Image<Gray, UInt16> C3 = new Image<Gray, UInt16>(image.Cols, image.Rows);
            Image<Gray, UInt16> C4 = new Image<Gray, UInt16>(image.Cols, image.Rows);
            Image<Gray, UInt16> C5 = new Image<Gray, UInt16>(image.Cols, image.Rows);
            Image<Gray, UInt16> C6 = new Image<Gray, UInt16>(image.Cols, image.Rows);


            //apply sobel
            CvInvoke.Sobel(gray, grad_x, Emgu.CV.CvEnum.DepthType.Cv32F, 1, 0, 3);
            CvInvoke.ConvertScaleAbs(grad_x, abs_grad_x, 1, 0);
            CvInvoke.Sobel(gray, grad_y, Emgu.CV.CvEnum.DepthType.Cv32F, 0, 1, 3);
            CvInvoke.ConvertScaleAbs(grad_y, abs_grad_y, 1, 0);
            CvInvoke.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, grad);
            Image<Gray, UInt16> img_gradient = grad.ToImage<Gray, UInt16>();  //will store gradient magnitude as an image
            //CvInvoke.Imwrite("0.jpg", C1);
            list_channels.Add(img_gradient);

            Emgu.CV.Cuda.CudaInvoke.Phase(grad_x, grad_y, angles, true);    //get angles
            Image<Gray, UInt16> img_angles = angles.ToImage<Gray, UInt16>();    //stores the angles as a gray image

            //loop through angles
            for (int i = 0; i < img_angles.Height; i++)
            {
                for (int j = 0; j < img_angles.Width; j++)
                {
                    double current_angle = img_angles.Data[i, j, 0];    //current angle value in degrees
                    if (current_angle > 180)    //if greater than 180
                    {
                        img_angles.Data[i, j, 0] = (UInt16)(img_angles.Data[i, j, 0] - 180);    //fix it
                    }
                    current_angle = img_angles.Data[i, j, 0];   //update current value

                    //according to the value of the angle, add it to the corresponding channel
                    if (current_angle >= 0 && current_angle <= 30)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C1);
                    else if (current_angle > 30 && current_angle <= 60)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C2);
                    else if (current_angle > 60 && current_angle <= 90)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C3);
                    else if (current_angle > 90 && current_angle <= 120)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C4);
                    else if (current_angle > 120 && current_angle <= 150)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C5);
                    else if (current_angle > 150 && current_angle <= 180)
                        addEdgeToChannel(i, j, img_gradient.Data[i, j, 0], C6);

                }
            }
            //smooth channels
            C1 = C1.SmoothGaussian(3);
            C2 = C2.SmoothGaussian(3);
            C3 = C3.SmoothGaussian(3);
            C4 = C4.SmoothGaussian(3);
            C5 = C5.SmoothGaussian(3);
            C6 = C6.SmoothGaussian(3);
            list_channels.Add(C1);
            list_channels.Add(C2);
            list_channels.Add(C3);
            list_channels.Add(C4);
            list_channels.Add(C5);
            list_channels.Add(C6);
            return list_channels;
        }
        public Matrix<float> getMatFromImageList(List<Image<Gray, UInt16>> channels)
        {

            Matrix<float> return_mat = new Matrix<float>(1, 5120);
            int mat_index = 0;
            for (int index = 0; index < channels.Count(); index++)
            {
                Image<Gray, UInt16> image = channels[index];
                float sum = 0;
                for (int i = 0; i < 64; i += 4)
                {
                    for (int j = 0; j < 128; j += 4)
                    {
                        for (int ii = 0; ii < 4; ii++)
                        {
                            for (int jj = 0; jj < 4; jj++)
                            {
                                try
                                {
                                    sum = (float)(sum + image.Data[j + jj, i + ii, 0]);
                                }
                                catch (Exception e)
                                {
                                    break;
                                }
                            }
                        }
                        //arr[mat_index] = sum;
                        return_mat.Data[0, mat_index] = sum;
                        sum = 0;
                        mat_index++;
                    }
                }
            }
            return return_mat;
        }
        public Matrix<float> getMatFromImagePaths(string[] channels)
        {

            Matrix<float> return_mat = new Matrix<float>(1, 5120);
            int mat_index = 0;
            for (int index = 0; index < channels.Count(); index++)
            {
                Image<Gray, UInt16> image = new Image<Gray, UInt16>(channels[index]);
                float sum = 0;
                for (int i = 0; i < 64; i += 4)
                {
                    for (int j = 0; j < 128; j += 4)
                    {
                        for (int ii = 0; ii < 4; ii++)
                        {
                            for (int jj = 0; jj < 4; jj++)
                            {
                                sum = (float)(sum + image.Data[j + jj, i + ii, 0]);
                            }
                        }
                        //arr[mat_index] = sum;
                        return_mat.Data[0, mat_index] = sum;
                        sum = 0;
                        mat_index++;
                    }
                }
            }
            return return_mat;
        }
        public static Image<Gray, double> normalize(Image<Gray, double> image) //function normalizes an image
        {
            List<double> list_raw_data = new List<double>();
            foreach (var value in image.Data)
                list_raw_data.Add(value);

            Image<Gray, double> normalized_image = image;

            double min = Math.Floor(list_raw_data.Min());
            double max = Math.Floor(list_raw_data.Max());
            double diff = Math.Abs(max - min);
            int counter = 0;
            for (int i = 0; i < normalized_image.Height; i++)
            {
                for (int j = 0; j < normalized_image.Width; j++)
                {
                    normalized_image.Data[i, j, 0] = Convert.ToByte(((list_raw_data[counter]) - min) * 255 / diff);
                    counter++;
                }
            }
            return normalized_image;
        }
        public void addEdgeToChannel(int x, int y, float value, Image<Gray, UInt16> channel)
        {
            channel.Data[x, y, 0] = (UInt16)value;
        }
        public VectorOfRect RemoveContainedRects(VectorOfRect rects)
        {
            VectorOfRect final_rects = new VectorOfRect();
            List<Rectangle> rects_list = new List<Rectangle>();
            for (int i = 0; i < rects.Size; i++)
            {
                bool contained = false;
                for (int j = 0; j < rects.Size; j++)
                {
                    if (i != j)
                    {
                        if (rects[j].Contains(rects[i]))
                            contained = true;
                    }
                }
                if (contained == false)
                    rects_list.Add(rects[i]);
                //if (!
            }
            final_rects.Push(rects_list.ToArray());
            return final_rects;
        }
        public Rectangle ScaleRectangleToOriginalImage(Rectangle rectangle, double factor)
        {
            Rectangle rect_final = new Rectangle();
            rect_final.X = (int)(rectangle.X / factor);
            rect_final.Y = (int)(rectangle.Y / factor);
            rect_final.Width = (int)(rectangle.Width / factor);
            rect_final.Height = (int)(rectangle.Height / factor);
            return rect_final;
        }
        public List<Image<Bgr, byte>> GetRandomNegRectangles(Image<Bgr, byte> image, int number)
        {
            Image<Bgr, byte> original_image = image.Copy();
            List<Image<Bgr, byte>> list_rois = new List<Image<Bgr, byte>>();
            int count = 0;
            while (list_rois.Count != number)
            {
                Random random = new Random();
                int rand = random.Next(1, 6);
                double factor = (double)1 / (double)rand;
                if (original_image.Width * factor > 64 && original_image.Height * factor > 128)
                {
                    Image<Bgr, byte> temp_image = original_image.Copy().Resize(factor, Emgu.CV.CvEnum.Inter.Linear);
                    int starting_x = random.Next(0, temp_image.Width - 65);
                    int starting_y = random.Next(0, temp_image.Height - 129);
                    Rectangle rectangle = new Rectangle(starting_x, starting_y, 64, 128);
                    temp_image.ROI = rectangle;
                    temp_image.Save(count.ToString() + ".jpg");
                    list_rois.Add(temp_image);
                   count++;
                }
            }
            return list_rois;
        }
    }
}

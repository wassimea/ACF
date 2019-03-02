using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directedStudies
{
    class coreFunctions
    {
        public List<Rectangle> getBoundingBoxes(string image_path, string annotations_folder)
        {
            List <Rectangle> list_rectangles = new List<Rectangle>();
            string image_name_without_extension = Path.GetFileNameWithoutExtension(image_path);
            string annotations_file = @annotations_folder + @"\" + image_name_without_extension + ".txt";
            if (File.Exists(annotations_file))
            {
                string line = "";
                System.IO.StreamReader file = new System.IO.StreamReader(annotations_file);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Bounding box for object"))
                    {
                        string[] parts_on_colon = line.Split(':');
                        string[] parts_starting_point_ending_point = parts_on_colon[1].Split('-');
                        string starting_point = parts_starting_point_ending_point[0].Split('(', ')')[1];
                        string ending_point = parts_starting_point_ending_point[1].Split('(', ')')[1];
                        string[] parts_starting_point = starting_point.Split(',');
                        string[] parts_ending_point = ending_point.Split(',');
                        int starting_x = Int32.Parse(parts_starting_point[0]);
                        int starting_y = Int32.Parse(parts_starting_point[1]);
                        int ending_x = Int32.Parse(parts_ending_point[0]);
                        int ending_y = Int32.Parse(parts_ending_point[1]);
                        int width = ending_x - starting_x;
                        int height = ending_y - starting_y;

                        if (width < 64)
                        {
                            int difference = Math.Abs(64 - (ending_x - starting_x));
                            int value_to_padd = difference / 2;
                            starting_x = starting_x - value_to_padd;
                            ending_x = ending_x + value_to_padd;
                            if (ending_x - starting_x < 64)
                                starting_x = starting_x - 1;
                        }
                        if (height < 128)
                        {
                            int difference = Math.Abs(128 - (ending_y - starting_y));
                            int value_to_padd = difference / 2;
                            starting_y = starting_y - value_to_padd;
                            ending_y = ending_y + value_to_padd;
                            if (ending_y - starting_y < 128)
                                starting_y = starting_y - 1;
                        }
                        if (width > 64)
                        {
                            for (int i = 1; i < 1000; i++)
                            {
                                int candidate_width = i * 64;
                                if (candidate_width > width)
                                {
                                    int width_difference = candidate_width - width;
                                    int value_to_padd = width_difference / 2;
                                    starting_x = starting_x - value_to_padd;
                                    ending_x = ending_x + value_to_padd;
                                    break;
                                }
                            }
                        }
                        if (height > 128)
                        {
                            for (int i = 1; i < 1000; i++)
                            {
                                int candidate_height = i * 128;
                                if (candidate_height > height)
                                {
                                    int height_difference = candidate_height - height;
                                    int value_to_padd = height_difference / 2;
                                    starting_y = starting_y - value_to_padd;
                                    ending_y = ending_y + value_to_padd;
                                    break;
                                }
                            }
                        }

                        Rectangle rectangle = new Rectangle(starting_x, starting_y, ending_x - starting_x, ending_y - starting_y);
                        double ar = (double)rectangle.Height / (double)rectangle.Width;
                        if (ar != 2)
                            rectangle = fixRectangleAspectRatio(rectangle);

                        list_rectangles.Add(rectangle);
                    }
                }
            }
            else
            {
                Image<Bgr, byte> image = new Image<Bgr, byte>(image_path);
                int width = image.Width;
                int height = image.Height;
                int starting_x;
                int starting_y;
                for (int l = 0; l < 3; l++)
                {
                    Random random = new Random();
                    starting_x = random.Next(0, width - 65);
                    starting_y = random.Next(0, height - 129);
                    Rectangle rectangle = new Rectangle(starting_x, starting_y, 64, 128);
                    list_rectangles.Add(rectangle);
                }
            }
            return list_rectangles;
        }
        Rectangle fixRectangleAspectRatio(Rectangle rectangle)
        {
            Rectangle rectFixed = rectangle;
            int sx = rectangle.X;
            int sy = rectangle.Y;
            int ex = rectangle.X + rectangle.Width;
            int ey = rectangle.Y + rectangle.Height;
            int width = rectangle.Width;
            int height = rectangle.Height;
            if (rectangle.Width > rectangle.Height)
            {
                for (int i = 1; i < 1000; i++)
                {
                    int nheight = 128 * i;
                    if (nheight / width == 2)
                    {
                        int val_to_pad = (nheight - height) / 2;
                        sy -= val_to_pad;
                        ey += val_to_pad;
                        rectFixed.Y = sy;
                        rectFixed.Height = nheight;
                        break;
                    }
                }
            }
            else if ((double)rectangle.Height / (double)rectangle.Width != 2)
            {
                for (int i = 1; i < 1000; i++)
                {
                    int nwidth = 64 * i;
                    if ((double)height / (double)nwidth == 2)
                    {
                        int val_to_pad = (nwidth - width) / 2;
                        sx -= val_to_pad;
                        sx += val_to_pad;
                        rectFixed.X = sx;
                        rectFixed.Width = nwidth;
                        break;
                    }
                }
            }
            if (rectFixed.Height == 0)
                Console.WriteLine("le");
            return rectFixed;
        }
        public void addEdgeToChannel(int x, int y, float value, Image<Gray, UInt16> channel)
        {
            channel.Data[x, y, 0] = (UInt16)value;
        }
        public Image<Gray, UInt16> normalize(Image<Gray, UInt16> image)
        {
            Image<Gray, UInt16> normalized_image = image;
             byte[] array = image.Bytes;
            byte[] normalized = array;
            int min = array.Min();
            int max = array.Max();
            int diff = max - min;
            for (int i = 0; i < array.Length; i++)
            {
                normalized[i] = Convert.ToByte(((array[i] - min) * 255 / diff));
            }
            normalized_image.Bytes = normalized;
            return normalized_image;
        }

        public void startExtraction(string images_folder,string output_folder, string annotations_folfder)
        {
            string destination_folder = images_folder;
            string[] images = Directory.GetFiles(output_folder);
            foreach (var image in images)  //for every image
            {
                string image_name = Path.GetFileNameWithoutExtension(image);
                Image<Bgr, byte> img_original = CvInvoke.Imread(image).ToImage<Bgr, byte>(); //bgr image
                Directory.CreateDirectory(@destination_folder + @"/Bounding Boxes/" + image_name);
                List<Rectangle> list_bounding_boxes = getBoundingBoxes(image, annotations_folfder);
                for (int i = 0; i < list_bounding_boxes.Count; i++)
                {
                    Image<Bgr, byte> image_bounding_box = img_original;
                    image_bounding_box.ROI = list_bounding_boxes[i];
                    image_bounding_box = image_bounding_box.Resize(64, 128, Emgu.CV.CvEnum.Inter.Linear);
                    generateChannels(image_bounding_box, image_name, @destination_folder + @"/Bounding Boxes/" + image_name + @"/" + i.ToString() + @"/");
                    //CvInvoke.Imwrite(@destination_folder + @"/Bounding Boxes/" + image_name + @"/" + i.ToString() + ".jpg", image_bounding_box);
                }
            }
        }
        public void generateChannels(Image<Bgr, byte> img_original, string image_name, string destination_folder)
        {
            Directory.CreateDirectory(destination_folder);
            //string destination_folder = tbDestination.Text;
            img_original = img_original.SmoothGaussian(3);  //smooth gaussian
            Image<Luv, byte> luv;   //will contain luv image to extract LUV channels
            luv = img_original.Convert<Luv, byte>();    //convert from bgr to luv
            VectorOfUMat channels = new VectorOfUMat(); //contains luv channels
            CvInvoke.Split(luv, channels);  //split them
            Image<Gray, double> image_channel_L = channels[0].ToImage<Gray, double>(); //L channel
            image_channel_L = image_channel_L.SmoothGaussian(3);
            Image<Gray, double> image_channel_U = channels[1].ToImage<Gray, double>(); //U channel
            image_channel_U = image_channel_U.SmoothGaussian(3);
            Image<Gray, double> image_channel_V = channels[2].ToImage<Gray, double>();  //V channel
            image_channel_V = image_channel_V.SmoothGaussian(3);
            CvInvoke.Imwrite(@destination_folder + "__L.jpg", image_channel_L);
            CvInvoke.Imwrite(@destination_folder + "__U.jpg", image_channel_U);
            CvInvoke.Imwrite(@destination_folder + "__V.jpg", image_channel_V);

            Mat gray = new Mat();   //gray version of the original image
            Mat grad = new Mat();   //will contain the gradient magnitude
            Mat grad_x = new Mat(); //sobel x
            Mat grad_y = new Mat(); //sobel y
            Mat abs_grad_x = new Mat(); //abs
            Mat abs_grad_y = new Mat();
            Mat angles = new Mat(); //matrix will contain the angle of every edge in grad magnitude channel

            CvInvoke.CvtColor(img_original, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray); //get gray image from bgr

            //channels defined below will contain the edges in different angles
            Image<Gray, UInt16> C1 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);
            Image<Gray, UInt16> C2 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);
            Image<Gray, UInt16> C3 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);
            Image<Gray, UInt16> C4 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);
            Image<Gray, UInt16> C5 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);
            Image<Gray, UInt16> C6 = new Image<Gray, UInt16>(img_original.Cols, img_original.Rows);


            //apply sobel
            CvInvoke.Sobel(gray, grad_x, Emgu.CV.CvEnum.DepthType.Cv32F, 1, 0, 3);
            CvInvoke.ConvertScaleAbs(grad_x, abs_grad_x, 1, 0);
            CvInvoke.Sobel(gray, grad_y, Emgu.CV.CvEnum.DepthType.Cv32F, 0, 1, 3);
            CvInvoke.ConvertScaleAbs(grad_y, abs_grad_y, 1, 0);
            CvInvoke.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, grad);
            Image<Gray, UInt16> img_gradient = grad.ToImage<Gray, UInt16>();  //will store gradient magnitude as an image
            img_gradient = normalize(img_gradient);
            CvInvoke.Imwrite(@destination_folder + "__G.jpg", img_gradient);

            Emgu.CV.Cuda.CudaInvoke.Phase(grad_x, grad_y, angles, true);    //get angles
            Image<Gray, double> img_angles = angles.ToImage<Gray, double>();    //stores the angles as a gray image

            //loop through angles
            for (int i = 0; i < img_angles.Height; i++)
            {
                for (int j = 0; j < img_angles.Width; j++)
                {
                    double current_angle = img_angles.Data[i, j, 0];    //current angle value in degrees
                    if (current_angle > 180)    //if greater than 180
                    {
                        img_angles.Data[i, j, 0] = (double)(img_angles.Data[i, j, 0] - 180);    //fix it
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
            CvInvoke.Imwrite(@destination_folder + "__C1.jpg", C1);
            CvInvoke.Imwrite(@destination_folder + "__C2.jpg", C2);
            CvInvoke.Imwrite(@destination_folder + "__C3.jpg", C3);
            CvInvoke.Imwrite(@destination_folder + "__C4.jpg", C4);
            CvInvoke.Imwrite(@destination_folder + "__C5.jpg", C5);
            CvInvoke.Imwrite(@destination_folder + "__C6.jpg", C6);
        }
        public Matrix<float> AddACFToMat(Matrix<float> mat, Matrix<float> acf, int row_index)
        {
            for (int i = 0; i < 5120; i++)
            {
                mat.Data[row_index, i] = acf[0, i];
            }
            return mat;
        }
    }
}

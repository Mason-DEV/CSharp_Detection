using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Vision;
using AForge.Vision.Motion;



namespace Motion_Detection
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private FilterInfoCollection captureDevices;
        private VideoCaptureDevice camera;
        MotionDetector detector = new MotionDetector(
            new TwoFramesDifferenceDetector(),
            new MotionAreaHighlighting());


        Boolean video = false;
       

        private void Form1_Load(object sender, EventArgs e)
        {
            captureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in captureDevices)
            {
                comboBoxSourceSelector.Items.Add(Device.Name);
              
            }
            comboBoxSourceSelector.SelectedItem = 0;
            camera = new VideoCaptureDevice();
            
            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            object selectedItem = comboBoxSourceSelector.SelectedItem;
   
            if (selectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("You don't have a Video Device Selected" , "Oops");

            }


            else if (video == true)
            {

                camera.NewFrame += new NewFrameEventHandler(videoSource_StopFrame);
                camera.Stop();
                camera = new VideoCaptureDevice(captureDevices[comboBoxSourceSelector.SelectedIndex].MonikerString);
                camera.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                camera.Start();
            }
            
            else
            {


                camera = new VideoCaptureDevice(captureDevices[comboBoxSourceSelector.SelectedIndex].MonikerString);
                camera.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                camera.NewFrame += new NewFrameEventHandler(videoSource_MotionFrame);
                camera.Start();
                video = true;
            }



        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            video = false;
            camera.NewFrame += new NewFrameEventHandler(videoSource_StopFrame);
            camera.Stop();
            
        }

        private void videoSource_StopFrame(object sender, NewFrameEventArgs eventArgs)
        {

            camera.Stop();
            pictureBox1.Image = null;
            pictureBox2.Image = null;

        }
        private void videoSource_MotionFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

            if (detector.ProcessFrame(frame) > 0.02)
            {
                // ring alarm or do somethng else
            }

            pictureBox2.Image = frame;
        }

    }
}

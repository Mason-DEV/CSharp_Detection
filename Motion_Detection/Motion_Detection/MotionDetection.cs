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



namespace Motion_Detection
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private VideoCaptureDevice videoSource;
        private FilterInfoCollection captureDevice;
        Boolean video = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            captureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in captureDevice)
            {
                comboBoxSourceSelector.Items.Add(Device.Name);
            }
            comboBoxSourceSelector.SelectedItem = 0;
            videoSource = new VideoCaptureDevice();
            
            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (video == true)
            {

                videoSource.NewFrame += new NewFrameEventHandler(videoSource_StopFrame);
                videoSource.Stop();
                videoSource = new VideoCaptureDevice(captureDevice[comboBoxSourceSelector.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();                
            }
            else
            {


                videoSource = new VideoCaptureDevice(captureDevice[comboBoxSourceSelector.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
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
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_StopFrame);
            videoSource.Stop();
        }

        private void videoSource_StopFrame(object sender, NewFrameEventArgs eventArgs)
        {

         pictureBox1.Image = null;
         
        }


    }
}

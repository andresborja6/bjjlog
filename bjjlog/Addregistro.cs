﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace bjjlog
{
    public partial class Addregistro : Form
    {
        private FilterInfoCollection videoDevices; // Colección de dispositivos de video
        private VideoCaptureDevice videoSource; // Dispositivo de captura de video
        private Bitmap capturedImage;
        private bool isVideoPlaying = true;
        public string connectionString { get; set; }
        public Addregistro()
        {
            InitializeComponent();
        }

        private void Addregistro_Load(object sender, EventArgs e)
        {

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in videoDevices)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();

        }
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isVideoPlaying)
            {

                using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    int nuevoAncho = 384;
                    int nuevoAlto = 213;
                    Bitmap resizedBitmap = new Bitmap(bitmap, nuevoAncho, nuevoAlto);
                    pictureBox1.Image = resizedBitmap;
                }
            }
        }

        private void Addregistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }

            if (capturedImage != null)
            {
                capturedImage.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                // Capturar el marco actual de la cámara
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                // Asignar el marco capturado a la variable capturedImage
                capturedImage = currentFrame;

                // Detener la reproducción del video
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                isVideoPlaying = false;

                // Mostrar la imagen capturada en el PictureBox
                pictureBox1.Image = capturedImage;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
        }
    }
}
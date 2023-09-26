using System;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace EvalOfCompressAlg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd1 = new OpenFileDialog();
            if(ofd1.ShowDialog() == DialogResult.OK )
            {
                try
                {
                    pictureBox1.Image= new Bitmap(ofd1.FileName) ;
                    pictureBox1.Refresh();
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd2 = new OpenFileDialog();
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox2.Image = new Bitmap(ofd2.FileName);
                    pictureBox2.Refresh();

                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bm1 = new Bitmap(pictureBox1.Image), bm2 = new Bitmap(pictureBox2.Image);
           
            float MSE=0;
            for(int j=0; j<=bm1.Height - 1; j++) { 
                for (int i=0; i<=bm1.Width- 1; i++)
                {
                    
                    
                    MSE += (float)(Math.Pow(bm1.GetPixel(i, j).R - bm2.GetPixel(i, j).R, 2) +
                        Math.Pow(bm1.GetPixel(i, j).G - bm2.GetPixel(i, j).G, 2) +
                        Math.Pow(bm1.GetPixel(i, j).B - bm2.GetPixel(i, j).B, 2))/ 3;
                    Console.WriteLine(bm1.GetPixel(i, j).R);
                }
            }
            label2.Text = "MSE: " + MSE / (bm1.Height * bm1.Width);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bm1 = new Bitmap(pictureBox1.Image), bm2 = new Bitmap(pictureBox2.Image);
            
            float UQI = 0, avX=0, avY=0, sx=0,sy=0,sxy=0;
            for (int j = 0; j <= bm1.Height - 1; j++)
            {
                for (int i = 0; i <= bm1.Width - 1; i++)
                {
                    avX += (bm1.GetPixel(i, j).R + bm1.GetPixel(i, j).G + bm1.GetPixel(i, j).B) / 3;
                    avY += (bm2.GetPixel(i, j).R + bm2.GetPixel(i, j).G + bm2.GetPixel(i, j).B) / 3;
                }
            }
            avX /= (bm1.Width * bm1.Height);
            avY /= (bm2.Width * bm2.Height);

            for (int j = 0; j <= bm1.Height - 1; j++)
            {
                for (int i = 0; i <= bm1.Width - 1; i++)
                {
                    sx += (float)Math.Pow((bm1.GetPixel(i, j).R + bm1.GetPixel(i, j).G + bm1.GetPixel(i, j).B) / 3 - avX, 2);
                    sy += (float)Math.Pow((bm2.GetPixel(i, j).R + bm2.GetPixel(i, j).G + bm2.GetPixel(i, j).B) / 3 - avY, 2);
                    sxy+= ((bm1.GetPixel(i, j).R + bm1.GetPixel(i, j).G + bm1.GetPixel(i, j).B) / 3 - avX)
                        *((bm2.GetPixel(i, j).R + bm2.GetPixel(i, j).G + bm2.GetPixel(i, j).B) / 3 - avY);
                }
            }
            sx /= (bm1.Width * bm1.Height);
            sy /= (bm1.Height * bm1.Width);
            sxy /= (bm1.Width * bm1.Height);
            UQI=(4*sxy*avX*avY)/((sx+sy)*(avX*avX+avY*avY));
            label1.Text = "UQI: " + UQI;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap bm1 = new Bitmap(pictureBox1.Image), bm2 = new Bitmap(pictureBox2.Image);

            int k=5, m=5;
            float UQIi = 0, avX, avY, sx, sy, sxy;
            for (int j = 0; j <= bm1.Height - k; j++)
            {
                for (int i = 0; i <= bm1.Width - m; i++)
                {
                    avX = 0; avY = 0; sx = 0; sy = 0; sxy = 0;
                    for (int jj = j; jj <= k - 1; jj++)
                    {
                        for (int ii = i; ii <= m - 1; ii++)
                        {
                            avX += (bm1.GetPixel(ii, jj).R + bm1.GetPixel(ii, jj).G + bm1.GetPixel(ii, jj).B) / 3;
                            avY += (bm2.GetPixel(ii, jj).R + bm2.GetPixel(ii, jj).G + bm2.GetPixel(ii, jj).B) / 3;
                        }
                    }
                    avX /= (k * m);
                    avY /= (k * m);

                    for (int jj = j; jj <= k - 1; jj++)
                    {
                        for (int ii = i; ii <= m - 1; ii++)
                        {
                            sx += (float)Math.Pow((bm1.GetPixel(ii, jj).R + bm1.GetPixel(ii, jj).G + bm1.GetPixel(ii, jj).B) / 3 - avX, 2);
                            sy += (float)Math.Pow((bm2.GetPixel(ii, jj).R + bm2.GetPixel(ii, jj).G + bm2.GetPixel(ii, jj).B) / 3 - avY, 2);
                            sxy += ((bm1.GetPixel(ii, jj).R + bm1.GetPixel(ii, jj).G + bm1.GetPixel(ii, jj).B) / 3 - avX)
                                * ((bm2.GetPixel(ii, jj).R + bm2.GetPixel(ii, jj).G + bm2.GetPixel(ii, jj).B) / 3 - avY);
                        }
                    }
                    sx /= (k*m);
                    sy /= (k*m);
                    sxy /= (k*m);
                    UQIi += (4 * sxy * avX * avY) / ((sx + sy) * (avX * avX + avY * avY));

                }
            }
            label3.Text = "UQI: " + UQIi / ((bm1.Height-k+1) * (bm1.Width-m+1));
        }
    }
}
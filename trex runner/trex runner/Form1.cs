using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trex_runner
{
    // Trex için kullanacağım değerlerin tanımlanması
    public partial class Form1 : Form
    {
        bool jumping = false;
        int  jumpSpeed;
        int  force = 12;
        int  score = 0;
        int  obstacleSpeed = 10;
        Random rand = new Random();
        int position;
        bool isGameOver = false;


        public Form1()
        {
            InitializeComponent();


            GameReset();
        }

        //Zıplama hızını if koşulu içerisinde döndürerek text'e Score sonucunu yazdırma
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed;

            txtscore.Text = "Score: " + score;
            
            if(jumping== true && force < 0)
            {
                jumping = false;
            }
            if(jumping== true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if (trex.Top > 266 && jumping == false)
            {
                force = 12;
                trex.Top = 267;
                jumpSpeed = 0;
            }

            foreach (Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {

                      x.Left = this.ClientSize.Width + rand.Next(200, 500) +  (x.Width*15);
                        score++;                   
                    }


                    // engele takıldığında tekrar başlamak için ekrana tekrar deneyin çıktısını verme
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gametimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        txtscore.Text += "Oyuna yeniden başlamak için r'ye basın";
                        isGameOver = true;
                    }
                }
            } 

            if (score > 5)
            {
                obstacleSpeed = 15;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }


            if(e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();
            }
        }
        // oyunu sıfırlamak için tanımlamaların değerlerini girilmesi
        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtscore.Text = "Score" + score;
            trex.Image = Properties.Resources.running;
            isGameOver = false;
            trex.Top = 267;

            // foreach döngüsü içerisinde Design bölümündeki Trex görselinin konumuna göre oyun görünümün boyutunu ayarlama 

            foreach (Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "obstacle")
                {
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);

                    x.Left = position;
                }
            }

            gametimer.Start();

        }
    }
}

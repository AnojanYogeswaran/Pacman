using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace PacmanProject
{
    public class Joueur : Entite
    {


        private int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public Joueur(int speed, PictureBox picture) : base(picture, speed)
        {
            this.score = 0;
        }
        public override void move()//bouge le joueur et tourne l'image selon la direction
        {

            if (this.moveRight)
            {
                this.picture.Left += this.speed;
                this.picture.Image = Properties.Resources.EQ1QTR;
                
            }
            if (this.moveLeft)
            {
                this.picture.Left -= this.speed;
                this.picture.Image = Properties.Resources.EQ1QTL;
            }
            if (this.moveUp)
            {
                this.picture.Top -= this.speed;
                this.picture.Image = Properties.Resources.EQ1QT;
            }
            if (this.moveDown)
            {
                this.picture.Top += this.speed;
                this.picture.Image = Properties.Resources.EQ1QTD;
            }
        }
        
    }
    }

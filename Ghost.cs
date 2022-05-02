using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacmanProject
{
    public class Ghost : Entite
    {
        
        public Ghost(PictureBox picture, int speed) : base(picture, speed)
        {


        }
        public override void move()//Chaque fantôme a des mouvements prédéfinis en choisissant
                                   //une combinaison moveLeft||moveRight && moveUp||moveDown
                                   //Tant qu'on dépasse pas l'écran on rebondit sur les blocs extérieurs
        {
            if (this.moveLeft == true)//Si il va à gauche
            {
                this.moveRight = false;
                if (this.picture.Location.X > 50)
                {
                    this.picture.Left -= this.speed;

                }
                else
                {
                    this.moveRight = true;
                }
            }
            if (this.moveRight == true)
            {
                this.moveLeft = false;
                if (this.picture.Location.X < 1100)
                {
                    this.picture.Left += this.speed;

                }
                else
                {
                    this.moveLeft = true;
                }
            }
            if (this.moveUp == true)
            {
                this.moveDown = false;
                if (this.picture.Location.Y > 80)
                {
                    this.picture.Top -= this.speed;

                }
                else
                {
                    this.moveDown = true;
                }
            }
            if (this.moveDown == true)
            {
                this.moveUp = false;
                if (this.picture.Location.Y < 550)
                {
                    this.picture.Top += this.speed;

                }
                else
                {
                    this.moveUp = true;
                }
            }

        }

    }
}

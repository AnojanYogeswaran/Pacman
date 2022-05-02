using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacmanProject
{
    public abstract class Entite
    {
        protected PictureBox picture;
        public PictureBox Picture
        {
            get { return picture; }
            set { picture = value; }
        }
        protected int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public bool moveLeft, moveRight, moveUp, moveDown;
        public bool blockLeft, blockRight, blockUp, blockDown;

        public Entite(PictureBox picture, int speed)
        {
            this.picture = picture;
            this.speed = speed;
            this.moveLeft = this.moveDown = this.moveUp = this.moveRight = false;
            this.blockLeft = this.blockDown = this.blockUp = this.blockRight = false;
        }
        public abstract void move();


    }
}

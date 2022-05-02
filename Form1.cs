using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Media;

namespace PacmanProject
{
    public partial class Form1 : Form
    {   
        private Joueur joueur;
        private Game game;
        private Ghost ghostRed;
        private Ghost ghostPink;
        private Ghost ghostYellow;
        int seconde, minute;
        private bool chompIsPlaying=false;//Pour savoir si le bruit chomp est deja actif si c'est le cas on attend
                                          //qu'il se finisse pour en relancer un autre
        private List<Joueur> joueurs;
        private List<Ghost> ghosts;

        public Form1()
        {
            InitializeComponent();
            GameSetUp();                      
        }

        private void chompSound()
        {
            if (!chompIsPlaying && !game.GamePause)//Si la game est pas en pause et que le chomp se joue pas deja
                                                   //On lance le timer et on joue le son chomp des qu'on entend un mouvement
            {
                chompIsPlaying = true;
                chomp.Start();
                chomp.Tick += chomp_Tick;
                play("chomp.wav");               
            }            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)//Fonction qui prend la touche qu'on rentre et permet de bouger si on est pas bloqué
        {
            if (e.KeyCode == Keys.Right && !joueur.blockRight)
            {
                joueur.moveLeft = joueur.moveUp = joueur.moveDown = false;
                joueur.blockLeft = joueur.blockUp = joueur.blockDown = false;
                joueur.moveRight = true;
                chompSound();
            }

            if (e.KeyCode == Keys.Left && !joueur.blockLeft)
            {
                joueur.moveRight = joueur.moveUp = joueur.moveDown = false;
                joueur.blockRight = joueur.blockUp = joueur.blockDown = false;
                joueur.moveLeft = true;
                chompSound();
            }

            if (e.KeyCode == Keys.Up && !joueur.blockUp)
            {
                joueur.moveLeft = joueur.moveRight = joueur.moveDown = false;
                joueur.blockLeft = joueur.blockRight = joueur.blockDown = false;
                joueur.moveUp = true;
                chompSound();
            }

            if (e.KeyCode == Keys.Down && !joueur.blockDown)
            {
                joueur.moveLeft = joueur.moveUp = joueur.moveRight = false;
                joueur.blockLeft = joueur.blockUp = joueur.blockRight = false;
                joueur.moveDown = true;
                chompSound();
            }

            if (e.KeyCode == Keys.Space)// Pause en appuyant sur Espace, on met bien les timers en pause aussi
            {
                if (game.GamePause == true)
                {
                    gameTimer.Start();
                    chrono.Start();
                    game.GamePause = false;
                    pause.Visible = false;
                }
                else
                {
                    gameTimer.Stop();
                    chrono.Stop();
                    game.GamePause = true;
                    pause.Visible = true;
                }
            }

            if (e.KeyCode == Keys.R)//On restart en appuyant sur R
            {
                Application.Restart();
            }
        }

        void Form1_KeyUp(object sender, KeyEventArgs e)//Fonction quand on relâche la touche pour éviter d'aller en diagonale
        {
            if (e.KeyCode == Keys.Right)
            {
                joueur.moveRight = false;
                
            }

            if (e.KeyCode == Keys.Left)
            {
                joueur.moveLeft = false;
                
            }

            if (e.KeyCode == Keys.Up)
            {
                joueur.moveUp = false;
                
            }

            if (e.KeyCode == Keys.Down)
            {
                joueur.moveDown = false;
                
            }
        }

        private int calculScoreTotal()//Compte le nombre de coin en parcourant la fenêtre
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "coin")
                    {
                        game.ScoreMax++;
                    }
                }
            }
            return game.ScoreMax;
        }

        private void GameLoop(object sender, EventArgs e)//La boucle principale du jeu qui va faire bouger les entités, vérifier les collisions
                                                         //,ramasser les pièces. Vérifier si la partie est gagné/perdu ...
        {   
            game.allMove();//Fait bouger les ghosts et les joueurs
            if (joueur.Score == 35)//Si on a toute les pièces c'est win (score changeable, toute les pièces = game.ScoreMax)
            {
                GameIsWin();
            }

            foreach (Control x in this.Controls)//Parcourt ce qu'il y'a dans la fenêtre et regarde si c'est un coin,wall ou ghost
            {   
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "coin" && x.Visible == true)// Si c'est un coin on mange et on gagne un points
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))
                        {
                            joueur.Score++;
                            txtScore.Text = "Score : " + joueur.Score;
                            x.Visible = false;
                        }
                    }
                    
                    if ((string)x.Tag == "wall")//Si c'est un wall on block le coté où on move (mal codé on peut glitché les murs en fait)
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds) && joueur.moveLeft)
                        {
                            joueur.blockLeft = true;
                            joueur.moveLeft = false;
                        }
                        
                        if (pacman.Bounds.IntersectsWith(x.Bounds) && joueur.moveRight)
                        {
                            joueur.blockRight = true;
                            joueur.moveRight = false;
                        }
                        if (pacman.Bounds.IntersectsWith(x.Bounds) && joueur.moveUp)
                        {
                            joueur.blockUp = true;
                            joueur.moveUp = false;
                        }

                        if (pacman.Bounds.IntersectsWith(x.Bounds) && joueur.moveDown)
                        {
                            joueur.blockDown = true;
                            joueur.moveDown = false;
                        }
                    }

                    if ((string)x.Tag == "ghost")
                    {
                        if (pacman.Bounds.IntersectsWith(x.Bounds))//Si on rencontre un fantôme c'est lose
                        {
                            GameLose();
                        }
                    }
                }
            }           
        }

        private void play(string file)//Fonction pour jouer les musiques
        {
            SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Himezaki\Desktop\Pacman-master\PacmanProject\Musique\" + file);
            //changez le chemin où vous avez les musiques
            simpleSound.Play();
        }
        
        private void GameSetUp()//On set up les paramètres du jeu ici et on le lance
        {
            seconde = 0;
            minute = 0;
            ghostRed = new Ghost(redGhost, 6);
            ghostYellow = new Ghost(yellowGhost, 6);
            ghostPink = new Ghost(pinkGhost, 6);
            joueur = new Joueur(7, pacman);
            ghosts = new List<Ghost>() { ghostYellow, ghostPink, ghostRed };
            joueurs = new List<Joueur> { joueur };
            game = new Game(joueurs, ghosts);
            ghostRed.moveLeft = true;//le fantôme rouge commencera par aller en haut à gauche
            ghostRed.moveUp = true;
            ghostYellow.moveRight = true;//le fantôme jaune commencera par aller en haut à droite
            ghostYellow.moveUp = true;
            ghostPink.moveLeft = true;//le fantôme rose commencera par aller en bas à gauche
            ghostPink.moveDown = true;
            timerMusique.Start();//timerMusique sert à commencer à la fin de la musique du début
            timerMusique.Tick += timerMusique_Tick;
            KeyDown += new KeyEventHandler(Form1_KeyDown);//On ajoute les evènements à KeyDown et à KeyUp
            KeyUp += new KeyEventHandler(Form1_KeyUp);
            game.ScoreMax = calculScoreTotal();

            play("beginning.wav");//Musique du début
            if (game.GamePause != true)
            {
                gameTimer.Tick += GameLoop;//On passe la fonction GameLoop à chaque tick du gameTimer
                gameTimer.Start();
                chrono.Tick += timer1_Tick;
                chrono.Start();
            }           
        }
        private void GameLose()
        {
            gameTimer.Stop();
            play("death.wav");//Musique de mort
            chrono.Stop();
            MessageBox.Show("Oh non ! Vous avez perdu !", "Défaite");
            if (MessageBox.Show("Voulez vous recommencer ?","Play Again", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
            }

            else 
                Application.Exit();                 
        }

        private void GameIsWin()
        {
            gameTimer.Stop();
            play("win.wav"); //Musique de victoire
            chrono.Stop();//On stop le chrono et le timer du jeu
            if (seconde < 10)//Petite condition pour que les secondes à chiffre s'affiche bien avec un 0 avant
            {
                seconde--;
                MessageBox.Show("Vous avez gagné ! Votre temps est " +minute+":0" +seconde,"Victoire");
            }

            else if (seconde < 60)
            {
                seconde--;
                MessageBox.Show("Vous avez gagné ! Votre temps est " + minute + ":" + seconde);
            }
            
            if (MessageBox.Show("Voulez vous recommencer ?", "Play Again", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Restart();
            }

            else
                Application.Exit();
        }

        private void timerMusique_Tick(object sender, EventArgs e)
        {
            timerMusique.Stop();
            gameTimer.Tick += GameLoop;//On passe la fonction GameLoop à chaque tick du gameTimer
            gameTimer.Start();
            chrono.Tick += timer1_Tick;
            chrono.Start();
            Ready.Visible = false;
            game.GamePause = false;          
        }

        private void timer1_Tick(object sender, EventArgs e)//Timer qui sert à chronométrer le temps qu'on met à finir la map
        {           
            if (seconde < 10)
            {
                timer.Text =" Time : " +minute + ":0" + seconde;
                seconde++;
            }

            else if(seconde < 60)
            {
                timer.Text = " Time : " +minute + ":" + seconde;
                seconde++;               
            }

            else
            {
                seconde = 0;
                minute++;
                timer.Text = " Time : " + minute + ":0" + seconde;               
            }  
        }
        private void chomp_Tick(object sender, EventArgs e)//Pour que le bruit chomp ne se repète pas à chaque move
        {    
            chomp.Stop();
            chompIsPlaying = false;
        }
    }
}
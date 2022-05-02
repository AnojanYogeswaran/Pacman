using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanProject
{
        public class Game //Ca servira pour les améliorations multijoueurs ...
        {
            private List<Joueur> joueurs;
            private List<Ghost> ghosts;
            private bool gamePause;
            private int scoreMax;
            public bool GamePause { get { return gamePause; } set { gamePause = value; } }
            public int ScoreMax { get { return scoreMax; } set { scoreMax = value; } }


            public Game(List<Joueur> joueurs, List<Ghost> ghosts)
            {
                this.joueurs = joueurs;
                this.ghosts = ghosts;
                gamePause = true;
                scoreMax = 0;
            }

            public void allMove()
            {
                foreach (var joueur in joueurs)
                {
                    joueur.move();
                }
                foreach (var ghost in ghosts)
                {
                    ghost.move();
                }
            }
           

    }
}


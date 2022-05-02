				-----         A FAIRE         		  -----
Pour que tout fonctionne bien, il faut copier le path du dossier Musique qui est dans le dossier du projet. 
Et remplacer avec à la ligne 209 de Form1.cs dans le new SoundPlayer() en rajoutant un "\" à la fin de Musique.
Passez par l'executable (Pacman avec l'icône) pour lancer le jeu c'est plus fluide.

				-----         Changer la difficulté       -----

Changer les valeurs numeriques dans les new Ghost, new Joueur, dans GameSetUp() après la ligne 218 (SPEED de l'entite)
Si c'est trop dur changer le score ligne 151 (Remplacer game.ScoreMax par le score voulu).




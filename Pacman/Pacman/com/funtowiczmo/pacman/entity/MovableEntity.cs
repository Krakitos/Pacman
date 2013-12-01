using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity
{
    /// <summary>
    /// Classe permettant à une entité de faire une déplacement via une interpolation linéaire
    /// </summary>
    public abstract class MovableEntity : AbstractEntity
    {

        //Vitesse de l'entité (exprimé en case / seconde)
        protected float velocity;

        //Informations sur le point à l'écran
        private Vector2 startingPosition;
        private Vector2 endingPosition;
        private Vector2 positionDiff;
        private TimeSpan startingTime; 

        //Indique si la totalité du mouvement à été effectuée
        private bool isMovementEnded;

        public MovableEntity()
            : base()
        {
            isMovementEnded = true;
            velocity = 1;
        }

        public Vector2 UpdatePosition(GameTime time, EntityDirectionEnum direction)
        {
            float deltaT = (float)time.TotalGameTime.Subtract(startingTime).Milliseconds / 1000;

            //Si le temps est écoulé, pas besoin de faire les autres calculs inutiles
            if (deltaT >= 1)
            {
                isMovementEnded = true;
                return endingPosition;
            }

            //Sinon on doit faire le calcul
            Vector2 pos = new Vector2(startingPosition.X, startingPosition.Y);

            switch (direction)
            {
                case EntityDirectionEnum.RIGH :
                case EntityDirectionEnum.LEFT:
                    pos.X += velocity * positionDiff.X * deltaT;
                    break;
                    
                case EntityDirectionEnum.TOP :
                case EntityDirectionEnum.BOTTOM:
                    pos.Y += velocity * deltaT * positionDiff.Y;
                    break;
            }

            return pos;
        }

        /// <summary>
        /// Renvoie un booleen indiquant si le dernier mouvement entamé est terminé (true) ou toujours en cours (false)
        /// </summary>
        public bool IsMovementEnded
        {
            get { return isMovementEnded; }
        }

        /// <summary>
        /// Permet de récupérer ou définir la vitesse de l'objet
        /// </summary>
        public float Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        /// <summary>
        /// Initialize un nouveau mouvement
        /// </summary>
        /// <param name="time">Objet permettant d'accéder à la référence temporelle du jeu en cours</param>
        /// <param name="startPos">Position de départ (à l'écran) de l'entité</param>
        /// <param name="endPos">Position de fin (à l'écran) de l'entité</param>
        public void StartMovement(GameTime time, Vector2 startPos, Vector2 endPos){
            isMovementEnded = false;
            startingPosition = startPos;
            endingPosition = endPos;
            positionDiff = new Vector2(endingPosition.X - startingPosition.X, endingPosition.Y - startingPosition.X);
            startingTime = time.TotalGameTime;
        }

        public void AbortMovement()
        {
            isMovementEnded = true;
        }
    }
}

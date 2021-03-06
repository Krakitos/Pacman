﻿using Microsoft.Xna.Framework;
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

        //Direction de l'entité
        protected EntityDirectionEnum direction;

        //Informations sur le point à l'écran
        private Vector2 startingPosition;
        private Vector2 endingPosition;
        private Vector2 positionDiff;
        private double duration;
        private TimeSpan startingTime; 

        //Indique si la totalité du mouvement à été effectuée
        private bool isMovementEnded;

        public MovableEntity()
            : base()
        {
            isMovementEnded = true;
            velocity = 1;
        }

        /// <summary>
        /// Initialize un nouveau mouvement
        /// </summary>
        /// <param name="time">Objet permettant d'accéder à la référence temporelle du jeu en cours</param>
        /// <param name="startPos">Position de départ (à l'écran) de l'entité</param>
        /// <param name="endPos">Position de fin (à l'écran) de l'entité</param>
        public void StartMovement(GameTime time, Vector2 startPos, Vector2 endPos, double duration)
        {
            this.duration = duration;
            startingPosition = startPos;
            endingPosition = endPos;

            positionDiff = new Vector2(endingPosition.X - startingPosition.X, endingPosition.Y - startingPosition.Y);

            isMovementEnded = positionDiff.X == 0 && positionDiff.Y == 0;
            startingTime = new TimeSpan(time.TotalGameTime.Ticks);
        }

        public Vector2 UpdatePosition(GameTime time, EntityDirectionEnum direction)
        {
            if (IsMovementEnded) return endingPosition;

            TimeSpan deltaT = time.TotalGameTime.Subtract(startingTime);

            //Si le temps est écoulé, pas besoin de faire les autres calculs inutiles
            if (deltaT.TotalMilliseconds >= duration)
            {
                isMovementEnded = true;
                return endingPosition;
            }

            //Sinon on doit faire le calcul
            Vector2 pos = new Vector2(startingPosition.X, startingPosition.Y);

            switch (direction)
            {
                case EntityDirectionEnum.RIGHT :
                case EntityDirectionEnum.LEFT:
                    pos.X += velocity * positionDiff.X * (float)(deltaT.TotalMilliseconds/duration);
                    break;
                    
                case EntityDirectionEnum.TOP :
                case EntityDirectionEnum.BOTTOM:
                    pos.Y += velocity * positionDiff.Y * (float)(deltaT.TotalMilliseconds / duration);
                    break;
            }

            return pos;
        }

        /// <summary>
        /// Défini la direction dans laquelle se dirige Pacman. Cette propriété permet, entre autres, de définir le skin utilisé pour représenté Pacman suivant
        /// sa direction
        /// </summary>
        public EntityDirectionEnum Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                OnDirectionChanged();
            }
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
                OnPositionChange(value);
                base.Position = value;
            }
        }

        public void AbortMovement()
        {
            isMovementEnded = true;
        }


        protected virtual void OnDirectionChanged()
        {

        }

        protected virtual void OnPositionChange(Vector2 newPos)
        {
            Vector2 diff = new Vector2(Position.X - newPos.X, Position.Y - newPos.Y);
            if (diff.X == 1 && direction != EntityDirectionEnum.LEFT)
            {
                Direction = EntityDirectionEnum.LEFT;
            }
            else if (diff.X == -1 && direction != EntityDirectionEnum.RIGHT)
            {
                Direction = EntityDirectionEnum.RIGHT;
            }
            else if (diff.Y == 1 && direction != EntityDirectionEnum.TOP)
            {
                Direction = EntityDirectionEnum.TOP;
            }
            else if (diff.Y == -1 && direction != EntityDirectionEnum.BOTTOM)
            {
                Direction = EntityDirectionEnum.BOTTOM;

            }
        }
    }
}

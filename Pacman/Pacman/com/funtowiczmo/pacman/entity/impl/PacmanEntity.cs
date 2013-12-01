using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity.signal;
using Pacman.com.funtowiczmo.pacman.utils;
using System;
using System.Collections.Generic;
namespace Pacman.com.funtowiczmo.pacman.entity.impl
{
	public class PacmanEntity : MovableEntity {

        private int points;
        private bool isGodMode;
        private EntityDirectionEnum direction;
        private Dictionary<EntityDirectionEnum, string[]> assets;

        /// <summary>
        /// Construit une nouvelle entité de Pacman. Celle-ci sera orientée vers la droite par défaut. Voir PacmanEntity(EntityDirectionEnum) pour
        /// choisir l'orientation de Pacman.
        /// </summary>
        public PacmanEntity():base()
        {
            //On met Pacman par défaut orienté vers la droite
            direction = EntityDirectionEnum.RIGH;

            //Liste des assets en fonction des directions prises par Pacman
            assets = new Dictionary<EntityDirectionEnum, string[]>();
            assets.Add(EntityDirectionEnum.TOP, new string[] { EntitySkinEnum.PACMAN_HAUT_1, EntitySkinEnum.PACMAN_HAUT_2 });
            assets.Add(EntityDirectionEnum.BOTTOM, new string[] { EntitySkinEnum.PACMAN_BAS_1, EntitySkinEnum.PACMAN_BAS_2 });
            assets.Add(EntityDirectionEnum.LEFT, new string[] { EntitySkinEnum.PACMAN_GAUCHE_1, EntitySkinEnum.PACMAN_GAUCHE_2 });
            assets.Add(EntityDirectionEnum.RIGH, new string[] { EntitySkinEnum.PACMAN_DROITE_1, EntitySkinEnum.PACMAN_DROITE_2 });

            //Par défaut Pacman est vulnérable
            isGodMode = false;

            //On démarre avec 0 points
            points = 0;

            UpdateSkin();
        }

        /// <summary>
        /// Construit une nouvelle entité de Pacman. Celle-ci sera orientée en fonction du paramètre direction.
        /// </summary>
        /// <param name="direction"></param>
        public PacmanEntity(EntityDirectionEnum direction):base()
        {
            //On met Pacman par défaut orienté vers la droite
            this.direction = direction;

            //Liste des assets en fonction des directions prises par Pacman
            assets = new Dictionary<EntityDirectionEnum, string[]>();
            assets.Add(EntityDirectionEnum.TOP, new string[] { EntitySkinEnum.PACMAN_HAUT_1, EntitySkinEnum.PACMAN_HAUT_2 });
            assets.Add(EntityDirectionEnum.BOTTOM, new string[] { EntitySkinEnum.PACMAN_BAS_1, EntitySkinEnum.PACMAN_BAS_2 });
            assets.Add(EntityDirectionEnum.LEFT, new string[] { EntitySkinEnum.PACMAN_GAUCHE_1, EntitySkinEnum.PACMAN_GAUCHE_2 });
            assets.Add(EntityDirectionEnum.RIGH, new string[] { EntitySkinEnum.PACMAN_DROITE_1, EntitySkinEnum.PACMAN_DROITE_2 });

            //Par défaut Pacman est vulnérable
            isGodMode = false;

            //On démarre avec 0 points
            points = 0;

            UpdateSkin();
        }

        /// <summary>
        /// Ajoute p point au total de points possédé par Pacman
        /// </summary>
        /// <param name="p"></param>
        public void AddPoints(int p)
        {
            points += p;
        }

        /// <summary>
        /// Renvoie le nombre de points actuel
        /// </summary>
        public int Points
        {
            get { return points; }
        }

        /// <summary>
        /// Défini le comportement de Pacman (vulnérable ou non)
        /// </summary>
        public bool IsGodMode
        {
            get { return isGodMode; }
            set { 
                isGodMode = value; 

                //On informe les observateurs que le statut de Pacman a changé
                NotifyAll(new PacmanGodModeSignal(this, isGodMode)); 
            }
        }

        /// <summary>
        /// Défini la direction dans laquelle se dirige Pacman. Cette propriété permet, entre autres, de définir le skin utilisé pour représenté Pacman suivant
        /// sa direction
        /// </summary>
        public EntityDirectionEnum Direction
        {
            get { return direction; }
            set { 
                if(direction != value){
                    direction = value;
                    UpdateSkin();
                }
            }
        }

        public Vector2 GetNextMove()
        {
            Vector2 next = new Vector2(position.X, position.Y);
            switch (direction)
            {
                case EntityDirectionEnum.BOTTOM:
                    {
                        next.Y++;
                        break;
                    }
                case EntityDirectionEnum.TOP:
                    {
                        next.Y--;
                        break;
                    }
                case EntityDirectionEnum.LEFT:
                    {
                        next.X--;
                        break;
                    }
                case EntityDirectionEnum.RIGH:
                    {
                        next.X++;
                        break;
                    }
            }

            return next;
        }

        public override string[] GetDefaultSkins()
        {
            return assets[direction];
        }

        private void UpdateSkin()
        {
            NotifyAll(new EntitySkinUpdated(this, assets[direction]));
        }
    }

}

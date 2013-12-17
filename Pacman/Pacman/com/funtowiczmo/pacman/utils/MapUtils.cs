using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.utils
{
    public class MapUtils
    {

        public static EntityDirectionEnum getOppositeDirection(EntityDirectionEnum direction)
        {
            if(direction == EntityDirectionEnum.LEFT){
                return EntityDirectionEnum.RIGHT;
            }else if(direction == EntityDirectionEnum.RIGHT){
                return EntityDirectionEnum.LEFT;
            }else if(direction == EntityDirectionEnum.BOTTOM){
                return EntityDirectionEnum.TOP;
            }else{                
                return EntityDirectionEnum.BOTTOM;
            }
        }
        public static Vector2 GetNextPointWithDirection(Vector2 pos, EntityDirectionEnum direction)
        {
            Vector2 next = new Vector2(pos.X, pos.Y);
            switch (direction)
            {
                case EntityDirectionEnum.BOTTOM:
                    {
                        ++next.Y;
                        break;
                    }
                case EntityDirectionEnum.TOP:
                    {
                        --next.Y;
                        break;
                    }
                case EntityDirectionEnum.LEFT:
                    {
                        --next.X;
                        break;
                    }
                case EntityDirectionEnum.RIGHT:
                    {
                        ++next.X;
                        break;
                    }
            }

            return next;
        }

        public static Vector2 GoAwayFrom(MovableEntity target, MovableEntity monster, map.Map map)
        {
            EntityDirectionEnum nextDir;

            //On regarde si les deux vont dans la bonne direction, si oui il faut changer la direction de la cible, pour ne pas aller vers le "monstre"
            if (target.Direction == monster.Direction)
            {
                //On récupère les directions disponibles, et on en tire une au sort
                List<EntityDirectionEnum> directions = map.GetAvailableDirections(target.Position);
                nextDir = directions.ElementAt(new Random().Next(0, directions.Count));
            }
            else //Sinon let's go on
            {
                nextDir = target.Direction;
            }

            return GetNextPointWithDirection(target.Position, nextDir);
        }
    }
}

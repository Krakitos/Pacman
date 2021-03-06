﻿using Microsoft.Xna.Framework;
using Pacman.com.funtowiczmo.pacman.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.entity.ghost
{
    /// <summary>
    /// Implémentation d'un algorithme de Pathfinding à l'aide de l'algorithme A*, plus rapide que Djikstra, mais moins précis dans certains cas (pas dans le notre)
    /// Cette implémentation utilise des structure Point pour leurs membres stockés sous forme d'entier (int) qui permettent d'effectuer des calculs plus rapide que sur 
    /// des flottants.
    /// </summary>
    public class Pathfinder
    {
        private List<SearchNode> closedList;
        private List<SearchNode> openedList;
        private Dictionary<Point, Point> paths;

        private Map map;

        public Pathfinder(Map map)
        {
            this.map = map;

            openedList = new List<SearchNode>();
            closedList = new List<SearchNode>();
            paths = new Dictionary<Point, Point>();
        }

        /// <summary>
        /// Renvoie le chemin vers le point spécifié
        /// </summary>
        /// <param name="from">Point de départ</param>
        /// <param name="to">Point d'arriver</param>
        /// <returns>Renvoie le prochain point</returns>
        public void GetPath(Point from, Point to)
        {
            bool end = false;

            //On prend les points de départ et d'arrivée
            Point pFrom = new Point((int)from.X, (int)from.Y);
            Point pTo = new Point((int)to.X, (int)to.Y);

            while (!end)
            {

                SearchNode newOpenListNode;

                //On essaye de trouver le meilleur point pour le prochain mouvement
                bool foundNewNode = SelectNodeToVisit(out newOpenListNode);
                if (foundNewNode) //Si on l'a trouvé
                {
                    Point currentPos = newOpenListNode.position;
                    foreach (Point point in GetNeighbors(currentPos))
                    {
                        SearchNode neighbor = new SearchNode(point, ComputeDistance(currentPos, pTo), newOpenListNode.distanceTraveled + 1);
                        if (!InList(openedList, point) && !InList(closedList, point))
                        {
                            openedList.Add(neighbor);
                            paths[point] = newOpenListNode.position;
                        }
                    }

                    if (currentPos == pTo)
                    {
                        end = true;
                    }

                    openedList.Remove(newOpenListNode);
                    closedList.Add(newOpenListNode);
                }
                else
                {
                    end = true;
                    throw new SystemException("No path found");
                }
            }
        }


        /// <summary>
        /// Génère le chemin calculé.
        /// </summary>
        /// <returns>Renvoie le chemin calculé</returns>
        public LinkedList<Point> FinalPath(Point to)
        {
            LinkedList<Point> path = new LinkedList<Point>();
            Point curPrev = to;
            path.AddFirst(curPrev);
            while (paths.ContainsKey(curPrev))
            {
                curPrev = paths[curPrev];
                path.AddFirst(curPrev);
            }
            return path;
        }

        public Vector2 GetNextPointTo(Vector2 from, Vector2 to)
        {
            //On réinitialise les listes
            closedList.Clear();
            openedList.Clear();
            paths.Clear();

            Point pFrom = new Point((int)from.X, (int)from.Y);
            Point pTo = new Point((int)to.X, (int)to.Y);

            //On ajoute le point de départ
            openedList.Add(new SearchNode(pFrom, ComputeDistance(pFrom, pTo), 0));

            //On cherche notre chemin
            try
            {
                GetPath(pFrom, pTo);
            }
            catch (Exception)
            {
                return from;
            }

            //On récupère le chemin généré
            LinkedList<Point> path = FinalPath(pTo);
            path.RemoveFirst();

            if (path.First != null)
            {
                Point next = path.First.Value;
                return new Vector2(next.X, next.Y);
            }

            return from;
        }

        /// <summary>
        /// Cherche le prochain point à visiter dans la liste ouverte
        /// </summary>
        /// <param name="result">Le point qui devra être visité</param>
        /// <returns>Renvoie true si un point à été trouvé, false sinon</returns>
        private bool SelectNodeToVisit(out SearchNode result)
        {
            result = new SearchNode();
            bool success = false;
            float smallestDistance = float.PositiveInfinity;
            float currentDistance = 0f;
            if (openedList.Count > 0)
            {
                //Pour tous les noeuds de la liste ouverte
                foreach (SearchNode node in openedList)
                {
                    //On récupère la distance actuelle
                    currentDistance = node.distanceTraveled + node.distanceToGoal;

                    if (currentDistance <= smallestDistance)
                    {
                        //Si la distance est inférieure, on prend !
                        if (currentDistance < smallestDistance)
                        {
                            success = true;
                            result = node;
                            smallestDistance = currentDistance;
                        }
                        //Sinon on regarde si la distance point à point est égale, mais que la distance totale pour arriver à ce point est inférieure
                        else if (currentDistance == smallestDistance && node.distanceTraveled > result.distanceTraveled)                                                                                     
                        {
                            success = true;
                            result = node;
                            smallestDistance = currentDistance;
                        }
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Détermine si un point est dans la liste passée en paramètre 
        /// </summary>
        /// <param name="list">la liste dans laquelle on doit chercher</param>
        /// <param name="point">Le point à trouver</param>
        /// <returns>True si le point est trouvé, false sinon</returns>
        private bool InList(List<SearchNode> list, Point point)
        {
            foreach (SearchNode node in list)
            {
                if (node.position == point)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Renvoie les voisins disponibles successivement (yield return)
        /// </summary>
        /// <param name="currentPos">Position à partir de laquelle on recherche les voisins</param>
        /// <returns>Une succession de voisins potentiellement intéressant</returns>
        private IEnumerable<Point> GetNeighbors(Point currentPos)
        {
            int x = currentPos.X;
            int y = currentPos.Y;

            if (map.IsNextMoveAuthorized(x + 1, y)) yield return new Point(x + 1, y);
            if (map.IsNextMoveAuthorized(x - 1, y)) yield return new Point(x - 1, y);
            if (map.IsNextMoveAuthorized(x, y + 1)) yield return new Point(x, y + 1);
            if (map.IsNextMoveAuthorized(x, y - 1)) yield return new Point(x, y - 1);

        }

        /// <summary>
        /// Calcule la distance de Manhattan
        /// </summary>
        /// <param name="from">Point d'origine</param>
        /// <param name="to">Point d'arrivé</param>
        /// <returns>Distance entre les deux points</returns>
        private int ComputeDistance(Point from, Point to)
        {
            return (to.X - from.X) + (to.Y - from.Y);
        }


        private struct SearchNode
        {
            public Point position;
            public int distanceToGoal;
            public int distanceTraveled;

            public SearchNode(Point mapPosition, int distanceToGoal, int distanceTraveled)
            {
                this.position = mapPosition;
                this.distanceToGoal = distanceToGoal;
                this.distanceTraveled = distanceTraveled;
            }
        }
    }
}
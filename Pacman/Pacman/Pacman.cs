using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Pacman.com.funtowiczmo.pacman;
using Pacman.com.funtowiczmo.pacman.entity;
using Pacman.com.funtowiczmo.pacman.sound;
using Pacman.com.funtowiczmo.pacman.map;
using Pacman.com.funtowiczmo.pacman.utils;
using Pacman.com.funtowiczmo.pacman.view;
using Pacman.com.funtowiczmo.pacman.errors;
using Pacman.com.funtowiczmo.pacman.entity.impl;
using Pacman.com.funtowiczmo.pacman.view.impl;
using Pacman.com.funtowiczmo.pacman.utils.signal;
using Pacman.com.funtowiczmo.pacman.map.signal;
using Pacman.com.funtowiczmo.pacman.entity.ghost;

namespace Pacman
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class Pacman : Microsoft.Xna.Framework.Game, IObserver<Signal>
    {

        //Constantes
        public const int GOD_MODE_TIME = 5000; //ms

        //Variables de contexte
        bool firstFrame = true;

        //Map
        Map map;
        MapView mapView;

        //Pacman
        PacmanEntity pacman;
        List<EntityView> entitiesView;
        int godModeElapsedTime;

        //Internal var
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Pacman()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 600;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Permet au jeu d’effectuer l’initialisation nécessaire pour l’exécution.
        /// Il peut faire appel aux services et charger tout contenu
        /// non graphique. Calling base.Initialize énumère les composants
        /// et les initialise.
        /// </summary>
        protected override void Initialize()
        {
            pacman = new PacmanEntity();
            pacman.Subscribe(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est appelé une fois par partie. Emplacement de chargement
        /// de tout votre contenu.
        /// </summary>
        protected override void LoadContent()
        {
            // Créer un SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: utilisez this.Content pour charger votre contenu de jeu ici

            //Initialisation du manager des textures
            AssetsManager instance = AssetsManager.GetInstance();

            //Chargement des textures et ajout au manager
            instance.AddTexture(EntitySkinEnum.PACMAN_BAS_1, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_BAS_1));
            instance.AddTexture(EntitySkinEnum.PACMAN_BAS_2, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_BAS_2));
            instance.AddTexture(EntitySkinEnum.PACMAN_DROITE_1, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_DROITE_1));
            instance.AddTexture(EntitySkinEnum.PACMAN_DROITE_2, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_DROITE_2));
            instance.AddTexture(EntitySkinEnum.PACMAN_GAUCHE_1, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_GAUCHE_1));
            instance.AddTexture(EntitySkinEnum.PACMAN_GAUCHE_2, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_GAUCHE_2));
            instance.AddTexture(EntitySkinEnum.PACMAN_HAUT_1, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_HAUT_1));
            instance.AddTexture(EntitySkinEnum.PACMAN_HAUT_2, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_HAUT_2));
            instance.AddTexture(EntitySkinEnum.PACMAN_MORT_1, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_MORT_1));
            instance.AddTexture(EntitySkinEnum.PACMAN_MORT_2, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_MORT_2));
            instance.AddTexture(EntitySkinEnum.PACMAN_MORT_3, Content.Load<Texture2D>(EntitySkinEnum.PACMAN_MORT_3));
            instance.AddTexture(EntitySkinEnum.GROS_BEAN, Content.Load<Texture2D>(EntitySkinEnum.GROS_BEAN));
            instance.AddTexture(EntitySkinEnum.BEAN, Content.Load<Texture2D>(EntitySkinEnum.BEAN));
            instance.AddTexture(EntitySkinEnum.FANTOME_VERT, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_VERT));
            instance.AddTexture(EntitySkinEnum.FANTOME_ROUGE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ROUGE));
            instance.AddTexture(EntitySkinEnum.FANTOME_ROSE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ROSE));
            instance.AddTexture(EntitySkinEnum.FANTOME_PEUR, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_PEUR));
            instance.AddTexture(EntitySkinEnum.FANTOME_ORANGE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ORANGE));
            instance.AddTexture(EntitySkinEnum.FANTOME_BLEU, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_BLEU));
            instance.AddTexture(EntitySkinEnum.MUR, Content.Load<Texture2D>(EntitySkinEnum.MUR));
            instance.AddTexture(EntitySkinEnum.ROUTE, Content.Load<Texture2D>(EntitySkinEnum.ROUTE));

            //Chargement des sons et ajout au manager 
            instance.AddSound(SoundEnum.INVINCIBLE, Content.Load<SoundEffect>(SoundEnum.INVINCIBLE));
            instance.AddSound(SoundEnum.MONSTER_EATEN, Content.Load<SoundEffect>(SoundEnum.MONSTER_EATEN));
            instance.AddSound(SoundEnum.PACMAN_EATEN, Content.Load<SoundEffect>(SoundEnum.PACMAN_EATEN));
            instance.AddSound(SoundEnum.PELLET_EAT_1, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_1));
            instance.AddSound(SoundEnum.PELLET_EAT_2, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_2));
            instance.AddSound(SoundEnum.SIREN, Content.Load<SoundEffect>(SoundEnum.SIREN));

            //On charge la map
            LoadMap();

            //On initialize les vues des entités (Pacman, fantomes)
            InitEntitiesViews();
        }

        private void LoadMap()
        {
            try
            {
                map = new Map("map1.txt");
                map.Subscribe(this);
                map.Load();
            }
            catch (MapNotFoundException mnfe)
            {
                System.Console.Error.WriteLine(mnfe.Message);
            }
            catch(MalformedMapException mme)
            {
                System.Console.Error.WriteLine(mme.Message);
            }

            MapAdapter adapter = new MapAdapter(map);
            mapView = new MapView();
            adapter.DrawMap(mapView);
        }

        /// <summary>
        /// UnloadContent est appelé une fois par partie. Emplacement de déchargement
        /// de tout votre contenu.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Déchargez le contenu non ContentManager ici
            Content.Unload();
        }

        /// <summary>
        /// Permet au jeu d’exécuter la logique de mise à jour du monde,
        /// de vérifier les collisions, de gérer les entrées et de lire l’audio.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Update(GameTime gameTime)
        {
            // Permet au jeu de se fermer
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            // TODO: Ajoutez votre logique de mise à jour ici
            // Mise à jour des skin des entityview en fonction du temps écoulé
            foreach (EntityView ev in entitiesView)
            {
                ev.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            //On regarde ce qui pourrait éventuellement se trouver à la position actuelle de Pacman
            map.CheckPosition(pacman.Position);

            //Check si le pacman est toujours invulnérable
            if (pacman.IsGodMode)
            {
                godModeElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (godModeElapsedTime >= GOD_MODE_TIME) //Sinon on regarde si la différence entre l'heure de démarrage
                {                                                         //et la durée du GodMode est > 0, si oui, alors on doit lui retirer
                    Console.WriteLine("Ending God Mode");
                    pacman.IsGodMode = false;
                }
            }


            //Gestion des events clavier
            HandleKeyboardInput();

            base.Update(gameTime);
        }

        /// <summary>
        /// Appelé quand le jeu doit se dessiner.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Ajoutez votre code de dessin ici

            //Début de la mise à jour de l'affichage
            spriteBatch.Begin();

            //Mise à jour de la map si necessaire
            mapView.UpdateMap(spriteBatch);

            if (firstFrame)
            {
                firstFrame = false;
                pacman.SetInitialPosition(map.GetRandomInitialPacmanPosition());
            }

            //Mise à jour des entités visuelles
            foreach (EntityView ev in entitiesView)
            {

                if (ev.GetType() == typeof(PacmanView))
                {
                   //Si le dernier mouvement est terminé
                   if (pacman.IsMovementEnded)
                    {
                       //On demande à calculer le prochain mouvement que Pacman effectura
                        Vector2 next = pacman.GetNextMove();

                        //On demande à la map de vérifier que ce mouvement est possible
                        if (map.IsNextMoveAuthorized(next)) 
                        {
                            //Initialisation du mouvement, on récupère les positions sur l'écran via la vue de la map qui s'occupe de la conversion
                            pacman.StartMovement(gameTime, mapView.ConvertPointToScreenPoint(pacman.Position), mapView.ConvertPointToScreenPoint(next), 150);
                            
                            //Définition du point d'arriver dans le reférentiel de la map
                            pacman.Position = next;

                            //Récupération du prochain point calculé
                            Vector2 destPoint = pacman.UpdatePosition(gameTime, pacman.Direction);

                            //Affichage
                            ev.DrawFrame(spriteBatch, destPoint);
                        }
                        else
                        {
                            ev.DrawFrame(spriteBatch, mapView.ConvertPointToScreenPoint(pacman.Position));
                        }
                    }
                   else //Sinon on met à jour le mouvement actuel
                   {
                       //Récupération du prochain point calculé
                       Vector2 destPoint = pacman.UpdatePosition(gameTime, pacman.Direction);

                       //Affichage
                       ev.DrawFrame(spriteBatch, destPoint);
                   }
                }
                else
                {
                    //TODO : Gérer les fantomes
                    ev.DrawFrame(spriteBatch, mapView.ConvertPointToScreenPoint(1, 2));
                }
            }

            //Fin de la mise a jour
            spriteBatch.End();
            
            base.Draw(gameTime);
        }


        private void InitEntitiesViews()
        {
            entitiesView = new List<EntityView>();

            //Vue pour Pacman
            EntityView pv = new PacmanView(pacman);
            entitiesView.Add(pv);

            EntityView ghost = new EntityView(new GhostEntity(EntitySkinEnum.FANTOME_ROUGE, new RandomMovementPolicy()));
            entitiesView.Add(ghost);
        }

        private void HandleKeyboardInput()
        {
            KeyboardState state = Keyboard.GetState();

            //On limite à un seul changement de direction par appel à Update().
            if (!state.IsKeyDown(Keys.Up))
            {
                if (!state.IsKeyDown(Keys.Down))
                {
                    if (!state.IsKeyDown(Keys.Left))
                    {
                        if (state.IsKeyDown(Keys.Right))
                        {
                            //Handle Right key down
                            if (pacman.Direction != EntityDirectionEnum.RIGH)
                            {
                                pacman.Direction = EntityDirectionEnum.RIGH;
                                pacman.AbortMovement();
                            }
                        }
                    }
                    else
                    {
                        //Handle Left key down
                        if (pacman.Direction != EntityDirectionEnum.LEFT)
                        {
                            pacman.Direction = EntityDirectionEnum.LEFT;
                            pacman.AbortMovement();
                        }
                    }
                }
                else
                {
                    //Handle Down key down
                    if (pacman.Direction != EntityDirectionEnum.BOTTOM)
                    {
                        pacman.Direction = EntityDirectionEnum.BOTTOM;
                    }
                }
            }
            else
            {
                //Handle Up key down
                if (pacman.Direction != EntityDirectionEnum.TOP)
                {
                    pacman.Direction = EntityDirectionEnum.TOP;
                    pacman.AbortMovement();
                }
            }
        }

        public void OnCompleted()
        {
            //Supprimer tous les écouteurs
        }

        public void OnError(Exception error)
        {
            Console.Error.WriteLine(error);
        }

        public void OnNext(Signal value)
        {
            Type type = value.GetType();

            if (type == typeof(MapBeanEatenSignal))
            {
                MapBeanEatenSignal mbes = (MapBeanEatenSignal)value;
                if (mbes.IsBigBean)
                {
                    pacman.IsGodMode = true;
                    godModeElapsedTime = 0;
                    Console.WriteLine("Entering God Mode");
                }
                else
                {
                    pacman.AddPoints(100);
                }
            }
            else if (type == typeof(MapAllBeansEatenSignal))
            {
                Console.WriteLine("WIN");
            }
        }
    }
}

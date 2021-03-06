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
        public const int MOVEMENTS_DURATION = 300; //ms

        //Variables de contexte
        GameState currentState = GameState.WAITING_INTERACTION;

        //Map
        Pathfinder pathfinder;
        Map map;
        MapView mapView;

        //Pacman
        PacmanEntity pacman;
        List<EntityView> entitiesView;
        int godModeElapsedTime;

        //Différentes vues
        InformationsView informationsView;
        WinView winView;
        LooseView looseView;

        //Internal var
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Pacman()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 700;

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
            instance.AddTexture(EntitySkinEnum.FANTOME_ROUGE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ROUGE));
            instance.AddTexture(EntitySkinEnum.FANTOME_ROSE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ROSE));
            instance.AddTexture(EntitySkinEnum.FANTOME_PEUR, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_PEUR));
            instance.AddTexture(EntitySkinEnum.FANTOME_ORANGE, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_ORANGE));
            instance.AddTexture(EntitySkinEnum.FANTOME_BLEU, Content.Load<Texture2D>(EntitySkinEnum.FANTOME_BLEU));
            instance.AddTexture(EntitySkinEnum.MUR, Content.Load<Texture2D>(EntitySkinEnum.MUR));
            instance.AddTexture(EntitySkinEnum.ROUTE, Content.Load<Texture2D>(EntitySkinEnum.ROUTE));
            instance.AddTexture(EntitySkinEnum.HEART, Content.Load<Texture2D>(EntitySkinEnum.HEART));

            //Chargement des sons et ajout au manager et vérification de la présence d'un composant audio disponible
            try
            {
                instance.AddSound(SoundEnum.INVINCIBLE, Content.Load<SoundEffect>(SoundEnum.INVINCIBLE));
                instance.AddSound(SoundEnum.MONSTER_EATEN, Content.Load<SoundEffect>(SoundEnum.MONSTER_EATEN));
                instance.AddSound(SoundEnum.PACMAN_EATEN, Content.Load<SoundEffect>(SoundEnum.PACMAN_EATEN));
                instance.AddSound(SoundEnum.PELLET_EAT_1, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_1));
                instance.AddSound(SoundEnum.PELLET_EAT_2, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_2));
                instance.AddSound(SoundEnum.SIREN, Content.Load<SoundEffect>(SoundEnum.SIREN));
            }
            catch (NoAudioHardwareException)
            {
                SoundManager.GetInstance().Available = false;
            }

            //Chargement des polices
            instance.AddFont("default", Content.Load<SpriteFont>("default"));

            //On charge la map
            LoadMap();

            //Initialisation du pathfinder
            pathfinder = new Pathfinder(map);

            //Initialise les vues
            InitViews();

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

            //Check si le pacman est toujours invulnérable
            if (pacman.IsGodMode)
            {
                godModeElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (godModeElapsedTime >= GOD_MODE_TIME) //Sinon on regarde si la différence entre l'heure de démarrage
                {                                                         //et la durée du GodMode est > 0, si oui, alors on doit lui retirer
                    pacman.IsGodMode = false;
                    NotifyGodModeChange();
                }
            }


            //Gestion des events clavier
            HandleInput();


            //Mise a jour des entites (Après un éventuel changement de direction via les Input)
            if (currentState == GameState.PLAYING) {
                foreach (EntityView ev in entitiesView)
                {
                    MovableEntity mv = (MovableEntity)ev.RelatedEntity;

                    //Si le mouvement est terminé
                    if (mv.IsMovementEnded)
                    {
                        Vector2 next;

                        //On récupère le prochain mouvement pour l'entité
                        if (ev.RelatedEntity is PacmanEntity)
                        {
                            //On regarde ce qui pourrait éventuellement se trouver à la position actuelle de Pacman
                            map.CheckPosition(pacman.Position);

                            next = pacman.GetNextMove();
                        }
                        else
                        {
                            GhostEntity ge = (GhostEntity)ev.RelatedEntity;

                            if (ge.Position == pacman.Position)
                            {
                                //Si Pacman est invulnérable et qu'il mange un fantome, on lui donne des points !
                                if (pacman.IsGodMode)
                                {
                                    pacman.AddPoints(100);
                                }
                                else
                                {
                                    //On retire une vie à Pacman
                                    pacman.RemoveALife();

                                    //S'il n'en a plus, on affiche game over
                                    if (pacman.RemainingLife == 0)
                                    {
                                        currentState = GameState.LOOSE;
                                    }
                                    else //Sinon on reset
                                    {
                                        currentState = GameState.WAITING_INTERACTION;
                                        InitEntitiesPosition();
                                    }
                                }
                            }

                            Vector2 goal = ge.ComputeNextMove(pacman, map);
                            next = pathfinder.GetNextPointTo(ge.Position, goal);
                        }

                        //On demande à la map de vérifier que ce mouvement est possible
                        if (map.IsNextMoveAuthorized(next))
                        {
                            //Initialisation du mouvement, on récupère les positions sur l'écran via la vue de la map qui s'occupe de la conversion
                            mv.StartMovement(gameTime, mapView.ConvertPointToScreenPoint(mv.Position), mapView.ConvertPointToScreenPoint(next), MOVEMENTS_DURATION);

                            //Définition du point d'arriver dans le reférentiel de la map
                            mv.Position = next;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Appelé quand le jeu doit se dessiner.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Ajoutez votre code de dessin ici

            //Début de la mise à jour de l'affichage
            spriteBatch.Begin();

            if (currentState == GameState.PLAYING || currentState == GameState.WAITING_INTERACTION)
            {
                //Mise à jour de la map si necessaire
                mapView.Draw(spriteBatch, 600, 600);

                //On affiche les informations
                Rectangle informationsPos = new Rectangle(mapView.Width, 0, GraphicsDevice.Viewport.Width - mapView.Width, GraphicsDevice.Viewport.Height);
                informationsView.Draw(spriteBatch, pacman, informationsPos);

                //Mise à jour des entités visuelles
                foreach (EntityView ev in entitiesView)
                {
                    if (ev.RelatedEntity is MovableEntity)
                    {
                        MovableEntity me = (MovableEntity)ev.RelatedEntity;

                        //Récupération du prochain point calculé si la partie est lancé, sinon l'affichage est statique
                        Vector2 destPoint = currentState == GameState.PLAYING ? me.UpdatePosition(gameTime, me.Direction) : me.Position;

                        //Affichage
                        ev.DrawFrame(spriteBatch, destPoint);
                    }
                }
            }else if(currentState == GameState.WON){ //Fenêtre gagné
                winView.Draw(spriteBatch);
            }
            else if (currentState == GameState.LOOSE) //Fenêtre perdu
            {
                looseView.Draw(spriteBatch);
            }

            //Fin de la mise a jour
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void InitViews()
        {
            SpriteFont font = AssetsManager.GetInstance().GetFont("default");

            //On charge l'affichage des informations
            informationsView = new InformationsView(GraphicsDevice, font);

            //On charge la vue gagnante
            winView = new WinView(font);

            //On charge la vue perdante
            looseView = new LooseView(font);
        }


        private void InitEntitiesViews()
        {
            entitiesView = new List<EntityView>();

            //Vue pour Pacman
            entitiesView.Add(new PacmanView(pacman));

            //Vues pour les fantomes
            entitiesView.Add(new EntityView(new GhostEntity(EntitySkinEnum.FANTOME_ROUGE, new RedMovementPolicy())));

            entitiesView.Add(new EntityView(new GhostEntity(EntitySkinEnum.FANTOME_BLEU, new BlueMovementPolicy())));

            entitiesView.Add(new EntityView(new GhostEntity(EntitySkinEnum.FANTOME_ROSE, new PinkMovementPolicy())));

            entitiesView.Add(new EntityView(new GhostEntity(EntitySkinEnum.FANTOME_ORANGE, new OrangeMovementPolicy())));
        }

        private void InitEntitiesPosition()
        {
            foreach (EntityView ev in entitiesView)
            {
                if (ev.RelatedEntity is MovableEntity)
                {
                    ((MovableEntity)ev.RelatedEntity).AbortMovement();
                }

                if (ev is PacmanView) pacman.SetInitialPosition(map.GetRandomInitialPacmanPosition());
                else ev.RelatedEntity.Position = map.GetRandomInitialGhostPosition();
            }
        }

        private void HandleInput()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            //Si on était dans l'attente d'une interaction de l'user, on l'a reçu :)

            //On regarde si la manette est connectée
            bool result = false;

            if (gamePadState.IsConnected)
            {
                result = HandleGamePadInput(gamePadState);
                
            }
            else //Sinon on utilise le clavier
            {
                result = HandleKeyboardInput();
            }   
         
            if(result && currentState == GameState.WAITING_INTERACTION){
                currentState = GameState.PLAYING;
            }
        }

        private bool HandleGamePadInput(GamePadState state)
        {
            Vector2 leftThumbSticks = state.ThumbSticks.Left;

            if (leftThumbSticks.X < 0.75)
            {
                if (leftThumbSticks.X > -0.75)
                {
                    if (leftThumbSticks.Y < 0.75)
                    {
                        if (leftThumbSticks.Y < -0.75)
                        {
                            //Handle Down key down
                            if (pacman.Direction != EntityDirectionEnum.BOTTOM)
                            {
                                pacman.Direction = EntityDirectionEnum.BOTTOM;
                                pacman.AbortMovement();                                
                            }

                            return true;
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

                        return true;
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

                    return true;
                }
            }
            else
            {
                //Handle Right key down
                if (pacman.Direction != EntityDirectionEnum.RIGHT)
                {
                    pacman.Direction = EntityDirectionEnum.RIGHT;
                    pacman.AbortMovement();
                }

                return true;
            }

            return false;
        }

        private bool HandleKeyboardInput()
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
                            if (pacman.Direction != EntityDirectionEnum.RIGHT)
                            {
                                pacman.Direction = EntityDirectionEnum.RIGHT;
                                pacman.AbortMovement();
                            }

                            return true;
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

                        return true;
                    }
                }
                else
                {
                    //Handle Down key down
                    if (pacman.Direction != EntityDirectionEnum.BOTTOM)
                    {
                        pacman.Direction = EntityDirectionEnum.BOTTOM;
                        pacman.AbortMovement();
                    }

                    return true;
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

                return true;
            }

            return false;
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
                    if (!pacman.IsGodMode)
                    {
                        pacman.IsGodMode = true;
                        pacman.AddPoints(20);
                        NotifyGodModeChange();
                    }
                    godModeElapsedTime = 0;
                }
                else
                {
                    pacman.AddPoints(10);
                }
            }
            else if (type == typeof(MapAllBeansEatenSignal))
            {
                currentState = GameState.WON;
            }
        }

        private void NotifyGodModeChange()
        {
            foreach (EntityView ev in entitiesView)
            {
                if (ev.RelatedEntity.GetType() == typeof(GhostEntity))
                {
                    GhostEntity g = (GhostEntity)ev.RelatedEntity;
                    g.AffraidMode = !g.AffraidMode;
                }
            }
        }

        private enum GameState
        {
            MENU, PLAYING, WON, LOOSE, WAITING_INTERACTION
        }
    }
}

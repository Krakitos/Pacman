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

namespace Pacman
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class Pacman : Microsoft.Xna.Framework.Game
    {

        //Constante
        public const int GOD_MODE_TIME = 100; //ms



        //Map
        Map map;
        MapView mapView;

        //Pacman
        PacmanEntity pacman;
        GameTime godModeStartingTime;

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
            // TODO: Ajouter votre logique d’initialisation ici
            pacman = new PacmanEntity();

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
            AssetsManager instance = AssetsManager.GetInstance();

            Console.WriteLine("Loading Texture and Sounds");

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

            instance.AddSound(SoundEnum.INVINCIBLE, Content.Load<SoundEffect>(SoundEnum.INVINCIBLE));
            instance.AddSound(SoundEnum.MONSTER_EATEN, Content.Load<SoundEffect>(SoundEnum.MONSTER_EATEN));
            instance.AddSound(SoundEnum.PACMAN_EATEN, Content.Load<SoundEffect>(SoundEnum.PACMAN_EATEN));
            instance.AddSound(SoundEnum.PELLET_EAT_1, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_1));
            instance.AddSound(SoundEnum.PELLET_EAT_2, Content.Load<SoundEffect>(SoundEnum.PELLET_EAT_2));
            instance.AddSound(SoundEnum.SIREN, Content.Load<SoundEffect>(SoundEnum.SIREN));

            LoadMap();
        }

        private void LoadMap()
        {
            try
            {
                map = new Map("map1.txt");
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
            mapView = new MapView(GraphicsDevice);
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

            //Check si le pacman est toujours invulnérable
            if (pacman.IsGodMode)
            {
                if (gameTime.ElapsedGameTime.Subtract(godModeStartingTime.ElapsedGameTime).Milliseconds >= GOD_MODE_TIME)
                {
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
            //Mise à jour de la map
            mapView.Begin();
            if (mapView.NeedRefresh) mapView.UpdateMap(); 
            mapView.End();

            //Mise à jour de PacMan
            base.Draw(gameTime);
        }

        private void HandleKeyboardInput()
        {
            KeyboardState state = Keyboard.GetState();

            //We only check for one direction change at frame
            if (!state.IsKeyDown(Keys.Up))
            {
                if (!state.IsKeyDown(Keys.Down))
                {
                    if (!state.IsKeyDown(Keys.Left))
                    {
                        if (state.IsKeyDown(Keys.Right))
                        {
                            //Handle Right key down
                            Console.WriteLine("Right pressed");
                        }
                    }
                    else
                    {
                        //Handle Left key down
                        Console.WriteLine("Left pressed");
                    }
                }
                else
                {
                    //Handle Down key down
                    Console.WriteLine("Down pressed");
                }
            }
            else
            {
                //Handle Up key down
                Console.WriteLine("Up pressed");
            }
        }
    }
}

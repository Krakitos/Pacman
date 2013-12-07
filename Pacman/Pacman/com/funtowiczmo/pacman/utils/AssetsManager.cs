using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman.com.funtowiczmo.pacman.utils
{
    public class AssetsManager
    {
        private static AssetsManager instance = new AssetsManager();
        
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SoundEffect> sounds;
        private Dictionary<string, SpriteFont> fonts;

        private AssetsManager()
        {
            textures = new Dictionary<string, Texture2D>();
            sounds = new Dictionary<string, SoundEffect>();
            fonts = new Dictionary<string, SpriteFont>();
        }

        public void AddTexture(string name, Texture2D texture)
        {
            textures[name] = texture;
        }

        public Texture2D GetTexture(string name)
        {
            return textures[name];
        }

        public void AddSound(string name, SoundEffect sound){
            sounds[name] = sound;
        }

        public SoundEffect GetSound(string name)
        {
            return sounds[name];
        }

        public void AddFont(string name, SpriteFont font)
        {
            fonts.Add(name, font);
        }

        public SpriteFont GetFont(string name)
        {
            return fonts[name];
        }

        public static AssetsManager GetInstance()
        {
            return instance;
        }
    }
}

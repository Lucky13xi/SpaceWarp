using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace xnaExtras
{
    public class SpriteWithAnimations : Sprite
    {
        public AnimationList spriteAnimations = new AnimationList();

        // animation version
        public SpriteWithAnimations(Texture2D textureImage, GraphicsDevice Device, Vector2 position, Vector2 origin, Vector2 velocity,
            float scale, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, Device, position, velocity, scale, collisionCueName, scoreValue)
        {   }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteAnimations.Draw(spriteBatch, position, Scale, spriteAnimations.SpriteEffect);
            }
        }

        public override void Update(GameTime gameTime)
        {           
            AnimationList.animationList[spriteAnimations.CurrentAnimation].Update(gameTime);
            base.Update(gameTime);
            //base.Clamp();
        }
    }
}

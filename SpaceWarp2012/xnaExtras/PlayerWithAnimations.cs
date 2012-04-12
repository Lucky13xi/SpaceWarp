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
    public class PlayerWithAnimations : SpriteWithAnimations
    {
        public PlayerWithAnimations(Texture2D textureImage, GraphicsDevice Device, Vector2 position, Vector2 origin,
            Vector2 velocity, float scale, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, Device, position, origin, velocity, scale, collisionCueName, scoreValue)
        { }

        public override void Update(GameTime gameTime)
        {
            position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Keep the sprite onscreen using clamp
            position.X = MathHelper.Clamp(position.X, 0, WindowWidth - TextureImage.Width * Scale);
            position.Y = MathHelper.Clamp(position.Y, 0, WindowHeight - TextureImage.Height * Scale);

            base.Update(gameTime);
        }
    }
}

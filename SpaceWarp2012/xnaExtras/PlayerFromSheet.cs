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
    public class PlayerFromSheet : SpriteFromSheet
    {
        public PlayerFromSheet(Texture2D textureImage, GraphicsDevice Device, Vector2 position,
            Vector2 velocity, Point frameSize, Point currentFrame, Point sheetSize, float scale, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, Device, position, velocity, frameSize, currentFrame, sheetSize, scale, 
            collisionCueName, scoreValue)
        { }
        //do just one row of the spritesheet
        public override void Update(GameTime gameTime, int row)
        {
            position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity *= 0.95f;

            base.Update(gameTime, row);

            Clamp();
        }

        //loops through entire spritesheet, called by autosprites in dodgeblade so they
        //can use polymorphism and check against player position
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //keep the sprite onscreen
            Clamp();
        }

        public override void Clamp()
        {
            //keep the sprite onscreen, this version uses spritesheet frame to do it
            position.X = MathHelper.Clamp(position.X, 0, WindowWidth - frameSize.X * Scale);
            position.Y = MathHelper.Clamp(position.Y, 0, WindowHeight - frameSize.Y * Scale);
        }
    }
}

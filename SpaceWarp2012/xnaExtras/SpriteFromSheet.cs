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
    //ADDED: new class to support spritesheets
    public class SpriteFromSheet : Sprite
    {
        //ADDED: following properties for sprite sheet support
        protected Point frameSize;      // Size of frames in sprite sheet
        public Point FrameSize
        {
            get { return frameSize; }
            set { frameSize = value; }
        }
        protected Point currentFrame;   // Index of current frame in sprite sheet
        public Point CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }
        protected Point sheetSize;      // Number of columns and rows in the sprite sheet
        public Point SheetSize
        {
            get { return sheetSize; }
            set { sheetSize = value; }
        }

        //for power ups/downs, keep original scale and velocity so we can go back
        //to these values after power up is done
        protected float OriginalScale { get; set; }
        public Vector2 OriginalVelocity { get; set; }

        // This method is called when the Player is created - sprite sheet version

        public SpriteFromSheet(Texture2D textureImage, GraphicsDevice Device, Vector2 position, Vector2 velocity, Point frameSize,
            Point currentFrame, Point sheetSize, float scale, SoundEffect collisionCueName, int scoreValue)
            : base(textureImage, Device, position, velocity, scale, collisionCueName, scoreValue)
        {
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.scale = scale;

            OriginalScale = 1;
        }
         
        public void ResetScale()
        {
            Scale = OriginalScale;
        }
        public void ResetSpeed()
        {
            velocity = OriginalVelocity;
        }
        public void ModifyScale(float modifier)
        {
            Scale *= modifier;
        }
        public void ModifySpeed(float modifier)
        {
            velocity *= modifier;
        }

        //loops through entire spritesheet, called by autosprites in dodgeblade so they
        //can use polymorphism and check against player position
        public virtual void Update(GameTime gameTime, Vector2 player)
        {
            ++currentFrame.X;
            if (currentFrame.X >= sheetSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= sheetSize.Y)
                {
                    currentFrame.Y = 0;
                }
            }
        }
        //loops through entire spritesheet
        public override void Update(GameTime gameTime)
        {
            ++currentFrame.X;
            if (currentFrame.X >= sheetSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= sheetSize.Y)
                {
                    currentFrame.Y = 0;
                }
            }
        }
        //loops through one row of spritesheet (first row is row 0)
        public virtual void Update(GameTime gameTime, int row)
        {
            currentFrame.Y = row;

            if (currentFrame.X > sheetSize.X)
            {
                currentFrame.X = 0;
            }
            else
            {
                currentFrame.X++;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage,
                             position,
                             new Rectangle(currentFrame.X * frameSize.X,
                                           currentFrame.Y * frameSize.Y,
                                           frameSize.X,
                                           frameSize.Y),
                             Color.White,
                             0,
                             Vector2.Zero,
                             Scale,
                             SpriteEffects.None,
                             0);
        }

        //rectangle occupied by texture
        public override Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, FrameSize.X, FrameSize.Y);
            }
        }

        public bool IsOutOfBounds()
        {
            if (position.X < -frameSize.X ||
                position.X > WindowWidth ||
                position.Y < -frameSize.Y ||
                position.Y > WindowHeight)
            {
                return true;
            }
            return false;
        }
    }
}

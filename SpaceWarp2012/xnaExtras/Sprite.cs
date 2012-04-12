using System;
using System.Collections.Generic;
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
    public class Sprite
    {
        // This variable will hold our position - make it a property so game class
        //can use it to change position when mouse moved
        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //make this protected so derived classes can use it
        protected Texture2D TextureImage { get; set; }

        // For window bounds
        protected int WindowWidth { get; set; }
        protected int WindowHeight { get; set; }

        //made this vector instead of float so it can apply to both x and y
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        protected float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        //sound to be made when sprite collides with something
        public SoundEffect CollisionCueName { get; private set; }

        public int ScoreValue { get; protected set; } // Value to add to player score when this sprite is hit
        public bool Active { get; set; }

        // base version
        public Sprite(Texture2D textureImage, GraphicsDevice Device, Vector2 position, Vector2 velocity, float scale,
            SoundEffect collisionCueName, int scoreValue)
        {
            // The position that is passed in is now set to the position above
            this.position = position;
            TextureImage = textureImage;
            this.velocity = velocity;
            this.scale = scale;

            WindowHeight = Device.Viewport.Height;
            WindowWidth = Device.Viewport.Width;
 
            CollisionCueName = collisionCueName;

            ScoreValue = scoreValue;

            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            //move the sprite
            position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Clamp()
        {
            //keep the sprite onscreen 
            position.X = MathHelper.Clamp(position.X, 0, WindowWidth - (TextureImage.Width * Scale));
            position.Y = MathHelper.Clamp(position.Y, 0, WindowHeight - (TextureImage.Height * Scale));
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
            spriteBatch.Draw(TextureImage,
                 position,
                 null,
                 Color.White,
                 0,
                 Vector2.Zero,
                 Scale,
                 SpriteEffects.None,
                 0);
                }
        }
        //rectangle occupied by texture
        public virtual Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)position.X , (int)position.Y, Convert.ToInt32(TextureImage.Width * Scale), Convert.ToInt32(TextureImage.Height * Scale));
            }
        }
        //is there a collision with another sprite?
        public bool CollisionSprite(Sprite sprite)
        {
            return CollisionRect.Intersects(sprite.CollisionRect);
        }
        //is there a collision with the mouse?
        public bool CollisionMouse(Point mousePoint)
        {
            return CollisionRect.Contains(mousePoint);
        }

        // These match up with the Arrow keys
        public void Up()
        {
            velocity.Y -= 20.0f;
        }

        public void Down()
        {
            velocity.Y += 20.0f;
        }

        public virtual void Right()
        {
            velocity.X += 20.0f;
        }

        public virtual void Left()
        {
            velocity.X -= 20.0f;
        }

        public virtual void Idle()
        {
            velocity.X = 0;
            velocity.Y = 0;
        }

    }
}

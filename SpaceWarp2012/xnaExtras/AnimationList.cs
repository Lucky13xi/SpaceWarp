using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace xnaExtras
{
    public class AnimationList 
    {
        //dictionary of animations rather than list - with a dictionary, they can have names
        public static Dictionary<string, Animation> animationList = new Dictionary<string, Animation>();
        //allows us to flip the image for left/right or up/down
        public SpriteEffects SpriteEffect { get; set; }
        //allows image to be drawn with x and y as the center instead of the top left
        public Vector2 SpriteOrigin { get; set; }
        //which animation is currently playing
        public string CurrentAnimation { get; set; }

        public AnimationList()
        {        }

           // This method adds an animation to the dictionary
        public void AddAnimation(String animationName, Animation animation)
        {
            animationList.Add(animationName, animation);
        }

        public void SetMoveLeft()
        {
            //assume images in dictionary are facing right, 
            //this flips it so it's facing left
            SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        public void SetMoveRight()
        {
            //images already facing right, so no flip needed
            SpriteEffect = SpriteEffects.None;
        }

        public void Draw(SpriteBatch batch, Vector2 position, float scale, SpriteEffects spriteEffect)
        {
            //if at end of animation, start at the beginning again
            if (animationList[CurrentAnimation].cellList.Count == 0 || animationList[CurrentAnimation].CurrentCell < 0 ||
                animationList[CurrentAnimation].cellList.Count <= animationList[CurrentAnimation].CurrentCell) return;

            //draw using position and scale, along with sprite effect and sprite origin
            batch.Draw(animationList[CurrentAnimation].cellList[animationList[CurrentAnimation].CurrentCell].Cell, position, null, Color.White, 0.0f, SpriteOrigin,
                new Vector2(scale, scale), spriteEffect, 0.0f);
        }
    }
}

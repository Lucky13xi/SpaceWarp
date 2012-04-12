using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace xnaExtras
{
    public class BackgroundLayer
    {
        //each layer is declared as an object of this class
        public Texture2D picture;
        public Vector2 position = Vector2.Zero;
        public Vector2 offset = Vector2.Zero;
        public float depth = 0.0f;
        public float moveRate = 0.0f;
        public Vector2 pictureSize = Vector2.Zero;
        public Color color = Color.White;
    }

    public class MultiBackground
    {
        //these booleans control all movement
        private bool moving = false;
        private bool moveLeftRight = true;

        private Vector2 windowSize;
        //each layer gets added to this list
        private List<BackgroundLayer> layerList = new List<BackgroundLayer>();

        //draw method creates its own batch
        private SpriteBatch batch;

        public MultiBackground(GraphicsDeviceManager graphics)
        {
            windowSize.X = graphics.PreferredBackBufferWidth;
            windowSize.Y = graphics.PreferredBackBufferHeight;
            batch = new SpriteBatch(graphics.GraphicsDevice);
        }

        //creates a layer from the data passed in and adds it to the list
        public void AddLayer(Texture2D picture, float depth, float moveRate)
        {
            BackgroundLayer layer = new BackgroundLayer();
            layer.picture = picture;
            //depth of 0 is closest, depth of 1 is furthest away
            layer.depth = depth;
            //if moveRate is positive, it will move right or down
            //if its negative, it will move left or up
            layer.moveRate = moveRate;
            layer.pictureSize.X = picture.Width;
            layer.pictureSize.Y = picture.Height;

            layerList.Add(layer);   
        }

        //used to sort layers in Draw method
        public int CompareDepth(BackgroundLayer layer1, BackgroundLayer layer2)
        {
            if (layer1.depth < layer2.depth)
                return 1;
            if (layer1.depth > layer2.depth)
                return -1;
            if (layer1.depth == layer2.depth)
                return 0;
            return 0;
        }

        //similar to update 
        //but used to move one time based on a rate, 
        //rather than update, which is based on gametime
        public void Move(float rate)
        {
            float moveRate = rate / 60.0f;
            //loop through all layers in list
            foreach (BackgroundLayer layer in layerList)
            {
                //calculate distance to move 
                float moveDistance = layer.moveRate * moveRate;
                //only do something if moving is false
                //(update method works if moving is true)
                if (!moving)
                {
                    //if moving left to right, change X
                    if (moveLeftRight)
                    {
                        layer.position.X += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.X = layer.position.X % layer.pictureSize.X;
                    }
                    //if moving up and down, change Y
                    else
                    {
                        layer.position.Y += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.Y = layer.position.Y % layer.pictureSize.Y;
                    }
                }
            }
        }

        //update should be called from the game1.cs update method
        public void Update(GameTime gameTime)
        {
            //loop through layers in list
            foreach (BackgroundLayer layer in layerList)
            {
                //this method will execute 60 times/second so divide by 60
                //to calculate how much to move each time
                float moveDistance = layer.moveRate / 60.0f;
                //do something only if moving is true
                if (moving)
                {
                    //if moving left to right, change X
                    if (moveLeftRight)
                    {
                        layer.position.X += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.X = layer.position.X % layer.pictureSize.X;
                    }
                    //if moving up and down, change Y
                    else
                    {
                        layer.position.Y += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.Y = layer.position.Y % layer.pictureSize.Y;
                    }
                }
            }
        }

        public void Draw()
        {
            //sort layers by depth
            layerList.Sort( CompareDepth );
            //draw uses its own batch
            batch.Begin();
            //loop through layers
            for (int i = 0; i < layerList.Count; i++)
            {
                //moe up or down
                if (!moveLeftRight)
                {
                    //draw1
                    if (layerList[i].position.Y < windowSize.Y)
                    {
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y), layerList[i].color);
                    }
                    //draw2
                    //use if statement so code works for both left to right 
                    //and right to left scrolling
                    if (layerList[i].position.Y > 0.0f)
                        //used when scrolling from left to right          
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y - layerList[i].pictureSize.Y), layerList[i].color);
                    else
                        batch.Draw(layerList[i].picture, new Vector2(0.0f, layerList[i].position.Y + layerList[i].pictureSize.Y), layerList[i].color);
                }
                else
                {
                    //draw1
                    if (layerList[i].position.X < windowSize.X)
                    {
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X, 0.0f), layerList[i].color);
                    }
                    //draw2
                    if (layerList[i].position.X > 0.0f)
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X - layerList[i].pictureSize.X, 0.0f), layerList[i].color);
                    else
                        batch.Draw(layerList[i].picture, new Vector2(layerList[i].position.X + layerList[i].pictureSize.X, 0.0f), layerList[i].color);
                }
            }       
            batch.End();
        }

        //methods to set the booleans to true and false
        public void SetMoveUpDown()
        {
            moveLeftRight = false;
        }

        public void SetMoveLeftRight()
        {
            moveLeftRight = true;
        }

        public void Stop()
        {
            moving = false;
        }

        public void StartMoving()
        {
            moving = true;
        }

        public void SetLayerPosition(int layerNumber, Vector2 offset)
        {
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            //change offset (start position) of a layer
            layerList[layerNumber].offset = offset;
            layerList[layerNumber].position += offset;
        }

        public void SetLayerAlpha(int layerNumber, float percent)
        {
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            float alpha = (percent / 100.0f);

            layerList[layerNumber].color = new Color(new Vector4(0.0f, 0.0f, 0.0f, alpha));
        }
    }
}

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
    public struct AnimationCell
    {
        //one cell (texture) in an animation
        public Texture2D Cell { get; set; }
    }

    public class Animation
    {
        //which cell is currently being displayed
        public int CurrentCell { get; set; }

        //booleans to control state of the animation
        private bool Looping { get; set; }
        private bool Stopped { get; set; }
        private bool Playing { get; set; }
        // Time we need to goto next frame
        private float TimeShift { get; set; }
        // Time since last shift
        private float TotalTime { get; set; }
        //which cell are we starting and ending with
        private int Start { get; set; }
        private int End { get; set; }
        //list of all cells 
        public List<AnimationCell> cellList;

        public Animation()
        {
            cellList = new List<AnimationCell>();
        }

        public void AddCell( Texture2D cellPicture )
        {
            //creates a new cell from the texture and
            //adds it to the list of cells
            AnimationCell cell = new AnimationCell();
            cell.Cell = cellPicture;

            cellList.Add(cell);
        }

        //loop through all cells in animation list over and over
        public void LoopAll(float seconds)
        {            
            if (Playing) return;
            
            Stopped = false;
            if (Looping) return;

            Looping = true;
            Start = 0;
            End = cellList.Count - 1;

            CurrentCell = Start;
            TimeShift = seconds / (float)cellList.Count; 
        }
        //loop through a group of cells in list over and over
        public void Loop(int start, int end, float seconds )
        {
            if (Playing) return;

            Stopped = false;
            if (Looping) return;

            Looping = true;
            this.Start = start;
            this.End = end;

            CurrentCell = start;
            float difference = (float)end - (float)start;

            TimeShift = seconds / difference;
        }
        //stop the animation
        public void Stop()
        {
            if (Playing) return;

            Stopped = true;
            Looping = false;
            TotalTime = 0.0f;
            TimeShift = 0.0f;
        }
        //display one cell (frame) only
        public void GotoFrame(int number)
        {
            if (Playing) return;

            if (number < 0 || number >= cellList.Count) return;
            CurrentCell = number;
        }
        //play all cells in animation list once
        public void PlayAll(float seconds)
        {
            if (Playing) return;
            GotoFrame(0);
            Stopped = false;
            Looping = false;
            Playing = true;
            Start = 0;
            End = cellList.Count - 1;

            TimeShift = seconds / (float)cellList.Count; 
        }
        //play a group of cells in list once
        public void Play(int start, int end, float seconds)
        {
            if (Playing) return;
            GotoFrame(start);
            Stopped = false;
            Looping = false;
            Playing = true;
            this.Start = start;
            this.End = end;

            float difference = (float)end - (float)start;

            TimeShift = seconds / difference;
        }
        //update - go to next cell in list
        public void Update(GameTime gameTime)
        {
            if (Stopped) return;

            TotalTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (TotalTime > TimeShift)
            {
                TotalTime = 0.0f;

                CurrentCell++;

                if (Looping)
                {
                    if (CurrentCell > End) CurrentCell = Start;
                }
                if (CurrentCell > End)
                {
                    CurrentCell = End;
                    Playing = false;
                    Stopped = true;
                }
            }
        }
    }
}

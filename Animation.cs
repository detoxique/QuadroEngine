using SFML.Graphics;
using SFML.System;
using QuadroEngine;

namespace QuadroEngine
{
    public class Animation : GameObject, Drawable
    {
        public IntRect FrameSize;
        public IntRect Frame;
        public Texture SpriteSheet;
        public Sprite result = new Sprite();
        public int Step = 0;

        public bool IsPlaying = true, Loop = false;

        public int CurrentFrame = 0;

        public int Flip, FramesCount;

        public Clock Timer = new Clock();

        public float Speed;

        public Animation()
        {

        }

        public Animation(Texture SpriteSheet, IntRect TextureRect, float Speed, int FramesCount, bool Loop)
        {
            this.SpriteSheet = SpriteSheet;
            FrameSize = TextureRect;
            this.Speed = Speed;
            this.FramesCount = FramesCount;
            this.Loop = Loop;
            result.Texture = SpriteSheet;
        }

        /// <summary>
        /// Updating an animation
        /// </summary>
        public override void Update(float deltatime, Entity camera)
        {
            if (IsPlaying)
            {
                if (Timer.ElapsedTime.AsMilliseconds() >= Speed)
                {
                    CurrentFrame++;
                    Timer.Restart();
                }

                if (CurrentFrame > FramesCount && !Loop)
                {
                    IsPlaying = false;
                }

                if (Loop && CurrentFrame > FramesCount)
                {
                    CurrentFrame = 0;
                }
            }
            if (camera != null)
                result.Position = new Vector2f(WorldPosition.X - camera.Position.X, WorldPosition.Y - camera.Position.Y);
            Frame = new IntRect(CurrentFrame * (FrameSize.Width + Step), 0, FrameSize.Width, FrameSize.Height);
        }

        /// <summary>
        /// Makes the animation play
        /// </summary>
        public void Play()
        {
            IsPlaying = true;
        }

        /// <summary>
        /// Makes the animation pause
        /// </summary>
        public void Pause()
        {
            IsPlaying = false;
        }

        /// <summary>
        /// Next animation frame
        /// </summary>
        public void NextFrame()
        {
            if (CurrentFrame < FramesCount)
                CurrentFrame++;
            else
                CurrentFrame = 0;
        }

        /// <summary>
        /// Previous animation frame
        /// </summary>
        public void PreviousFrame()
        {
            if (CurrentFrame - 1 > 0)
                CurrentFrame--;
            else
                CurrentFrame = FramesCount;
        }

        /// <summary>
        /// Returns current sprite
        /// </summary>
        /// <returns>Current sprite</returns>
        public Sprite GetSprite()
        {
            result.TextureRect = Frame;
            return result;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            result.TextureRect = Frame;
            target.Draw(result, states);
        }
    }
}

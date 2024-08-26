using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_1
{
    internal class Player : Sprite
    {
        private AnimationState currentAnimationState;
        private int idleFrameCount;
        private int movingFrameCount;
        private int currentIdleFrame;
        private int currentMovingFrame;
        private int idleFrameUpdateInterval = 15; // Adjust this value for idle animation speed
        private int movingFrameUpdateInterval = 5; // Adjust this value for moving animation speed
        private int frameUpdateCounter = 0;
        private Texture2D idleTexture;
        private Texture2D runningTexture;

        public Player(Texture2D idleTexture, Texture2D runningTexture, Vector2 position) : base(idleTexture, position)
        {
            this.idleTexture = idleTexture;
            this.runningTexture = runningTexture;
            idleFrameCount = 5; // Set the number of frames for idle animation
            movingFrameCount = 8; // Set the number of frames for moving animation
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, 1000, 400);
            }
        }



        public void Update(GameTime gameTime, Vector2 playerPosition, bool isMoving)
        {
            this.position = playerPosition;

            // Update the current animation state
            currentAnimationState = isMoving ? AnimationState.Moving : AnimationState.Idle;

            // Calculate the current frame
            FrameCalculation();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Direction playerDirection)
        {
            Texture2D currentTexture = currentAnimationState == AnimationState.Idle ? idleTexture : runningTexture;
            int currentFrame = currentAnimationState == AnimationState.Idle ? currentIdleFrame : currentMovingFrame;

            // Determine sprite effects based on movement direction
            SpriteEffects spriteEffects = playerDirection == Direction.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // Scale factor
            float scale = 3.5f;

            spriteBatch.Draw(
                currentTexture,
                new Vector2(position.X - cameraPosition.X, position.Y - cameraPosition.Y), // Apply camera offset
                new Rectangle(31 + (currentFrame * 93), 71, 30, 40), // Replace with your actual frame size
                Color.White,
                0f, // Rotation angle (0 for no rotation)
                new Vector2(15, 20), // Origin (center of the sprite)
                scale, // Scale
                spriteEffects, // Sprite effects (flipping)
                0f); // Layer depth
        }

        /// <summary>
        /// Calculate the current frame for animations.
        /// </summary>
        private void FrameCalculation()
        {
            frameUpdateCounter++;

            int currentFrameUpdateInterval = currentAnimationState == AnimationState.Moving
                ? movingFrameUpdateInterval
                : idleFrameUpdateInterval;

            if (frameUpdateCounter > currentFrameUpdateInterval)
            {
                frameUpdateCounter = 0;

                // Update the current frame based on the animation state
                if (currentAnimationState == AnimationState.Moving)
                {
                    currentMovingFrame++;
                    if (currentMovingFrame >= movingFrameCount)
                    {
                        currentMovingFrame = 0;
                    }
                }
                else if (currentAnimationState == AnimationState.Idle)
                {
                    currentIdleFrame++;
                    if (currentIdleFrame >= idleFrameCount)
                    {
                        currentIdleFrame = 0;
                    }
                }
            }
        }
    }
}

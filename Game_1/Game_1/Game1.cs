using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private FollowCamera camera;

        private AnimationState currentAnimationState;
        private int idleFrameCount;
        private int movingFrameCount;
        private int currentIdleFrame;
        private int currentMovingFrame;
        private int idleFrameUpdateInterval = 15; // Adjust this value for idle animation speed
        private int movingFrameUpdateInterval = 5; // Adjust this value for moving animation speed
        private int frameUpdateCounter = 0;

        Texture2D spriteSheets;
        Texture2D spriteSheetsRunning;

        Vector2 playerPosition;
        Rectangle curentPostion;
        Vector2 frameSize;


        int counter;
        int activeFrame;
        int numFrame;

        bool isMoving;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            camera = new(Vector2.Zero);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the sprite sheet
            spriteSheets = Content.Load<Texture2D>("IDLE");
            spriteSheetsRunning = Content.Load<Texture2D>("RUN");

            // Initialize animation and player position
            activeFrame = 0;
            numFrame = 5;
            idleFrameCount = 5;
            movingFrameCount = 8;

            counter = 0;
            frameSize = new Vector2(30, 40);

            playerPosition = new Vector2(100, 100); // Starting position of the player
            curentPostion.X = (int) playerPosition.X;
            curentPostion.Y = (int) playerPosition.Y;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update player movement and animation
            UpdatePlayer(gameTime);

            // Update the camera to follow the player
            Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            camera.Follow(curentPostion, screenSize);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Determine sprite effects based on movement direction
            SpriteEffects spriteEffects = isMoving && playerDirection == Direction.Left
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Texture2D currentTexture = currentAnimationState == AnimationState.Idle ? spriteSheets : spriteSheetsRunning;
            int currentFrame = currentAnimationState == AnimationState.Idle ? currentIdleFrame : currentMovingFrame;

            // Scale factor
            float scale = 3.5f;

            _spriteBatch.Draw(
                currentTexture,
                new Vector2(playerPosition.X - camera.Position.X, playerPosition.Y - camera.Position.Y), // Apply camera offset
                new Rectangle(31 + (currentFrame * 93), 71, (int)frameSize.X, (int)frameSize.Y),
                Color.White,
                0f, // Rotation angle (0 for no rotation)
                new Vector2(frameSize.X / 2, frameSize.Y / 2), // Origin (center of the sprite)
                scale, // Scale
                spriteEffects, // Sprite effects (flipping)
                0f); // Layer depth


            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private enum Direction
        {
            None,
            Left,
            Right
        }

        private Direction playerDirection = Direction.None;

        private void UpdatePlayer(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            isMoving = false;
            playerDirection = Direction.None;

            // Move the player
            if (keyboardState.IsKeyDown(Keys.D))
            {
                playerPosition.X += 6f;
                isMoving = true;
                playerDirection = Direction.Right;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                playerPosition.X -= 6f;
                isMoving = true;
                playerDirection = Direction.Left;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                playerPosition.Y -= 6f;
                isMoving = true;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                playerPosition.Y += 6f;
                isMoving = true;
            }

            // Update the current animation state
            if (isMoving)
            {
                currentAnimationState = AnimationState.Moving;
            }
            else
            {
                currentAnimationState = AnimationState.Idle;
            }
            curentPostion.X = (int)playerPosition.X;
            curentPostion.Y = (int)playerPosition.Y;
            FrameCalculation();
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

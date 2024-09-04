using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace Game_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private Camera camera;

        private Vector2 playerPosition;
        private Direction playerDirection;
        private bool isMoving;


        private Dictionary<Vector2, int> bg;
        private KeyboardState previousKeyboardState;
        private Texture2D textureAtlas;

        private List<Rectangle> textureStored;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            textureStored = new()
            {
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(64, 0, 32, 32)
            };

            bg = loadMap("../../../Data/Sol.csv");
        }

        private Dictionary<Vector2, int> loadMap(string filepath) 
        {
            Dictionary<Vector2, int> result = new();

            StreamReader reader = new StreamReader(filepath);

            int y = 0;

            string line;
            while ((line = reader.ReadLine()) != null) 
            {
                string[] items = line.Split(',');

                for(int x = 0; x < items.Length; x++) 
                {
                    if (int.TryParse(items[x], out int value)){
                        if (value > -1) 
                        {
                            result[new Vector2(x, y)] = value;
                        }    
                    }
                }
                y++;
            }
            return result;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D idleTexture = Content.Load<Texture2D>("IDLE");
            Texture2D runningTexture = Content.Load<Texture2D>("RUN");

            textureAtlas = Content.Load<Texture2D>("Tileset Outside");

            playerPosition = new Vector2(100, 100); // Starting position of the player
            player = new Player(idleTexture, runningTexture, playerPosition);

            Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            camera = new Camera(screenSize);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdatePlayer(gameTime);

            // Update the camera to follow the player
            camera.Follow(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: camera.Transform, samplerState: SamplerState.PointClamp);

            //foreach (var tile in tilemap) 
            //{
            //    Rectangle dest = new Rectangle((int)tile.Key.X * 64, (int)tile.Key.Y * 32, 64, 64);

            //    Rectangle src = textureStored[tile.Value - 1];

            //    _spriteBatch.Draw(map, dest, src, Color.White);
            //}
            int display_Tilesize = 32;
            int num_tiles_per_row = 32;
            int pixel_per_tile = 32;
            foreach (var tile in bg)
            {
                Rectangle drect = new(
                    (int)tile.Key.X * display_Tilesize,
                    (int)tile.Key.Y * display_Tilesize,
                    display_Tilesize,
                    display_Tilesize
                );

                int x = tile.Value & num_tiles_per_row;
                int y = tile.Value / num_tiles_per_row;
                Rectangle src = new(
                    x * pixel_per_tile,
                    y* pixel_per_tile,
                    pixel_per_tile,
                    pixel_per_tile
                );

                _spriteBatch.Draw(textureAtlas, drect, src, Color.White);
            }

            player.Draw(_spriteBatch, Vector2.Zero, playerDirection); // No need to pass camera position anymore

            _spriteBatch.End();

            base.Draw(gameTime);
        }

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

            player.Update(gameTime, playerPosition, isMoving);
        }

       
    }
}

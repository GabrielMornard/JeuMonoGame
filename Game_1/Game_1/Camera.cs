using Microsoft.Xna.Framework;

namespace Game_1
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position { get; private set; }

        private Vector2 screenSize;

        public Camera(Vector2 screenSize)
        {
            this.screenSize = screenSize;
            Position = Vector2.Zero;
        }

        public void Follow(Sprite target)
        {
            // Calculate the camera position to center the target (e.g., the player)
            var targetPosition = target.position;

            // Adjust the camera position to center the target
            Position = new Vector2(targetPosition.X - (screenSize.X / 2), targetPosition.Y - (screenSize.Y / 2));

            // Create the transformation matrix to apply the camera offset
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}

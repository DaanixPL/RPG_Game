using Raylib_cs;
using System.Numerics;

namespace RPG_JKL
{
    public class Bullet
    {
        public Vector2 position;
        public Vector2 direction;
        public float speed = 800f;

        public Bullet(Vector2 startPosition, Vector2 direction)
        {
            this.position = startPosition;
            this.direction = direction;
        }

        public void Update(float deltaTime)
        {
            // Poruszanie pocisku
            position += direction * speed * deltaTime;
        }

        public void Draw()
        {
            Raylib.DrawCircleV(position, 5, Color.Red); // Rysowanie pocisku
        }
    }
}

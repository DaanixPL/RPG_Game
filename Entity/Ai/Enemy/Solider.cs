using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

namespace RPG_JKL.Entity.Ai.Enemy
{
    internal class Solider
    {
        private Texture2D soliderTexture = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\Ai\\Solider.png");

        private float speed = 200;
        private float attackRange = 150f;

        public Vector2 position;
        public float rotation = 0f;

        public float hp = 100;

        public bool isDead = false;

        private Vector2 gunTipOffset = new Vector2(182, 83); // Punkt końca lufy w teksturze
        private float scale = 0.9f; // Skalowanie tekstury

        private float fireRate = 0.5f; // Czas odnowienia strzału (w sekundach)
        private float lastShotTime = 0f;

        private Player targetPlayer;
        public List<Bullet> bullets = new List<Bullet>(); // Lista pocisków, które solider wystrzeliwuje

        public Solider(Player targetPlayer)
        {
            this.targetPlayer = targetPlayer;
            position = new Vector2((Constantly.mapWidth * Constantly.tileSize) / 2, (Constantly.mapHeight * Constantly.tileSize) / 2);
        }

        public void Draw()
        {
            if (!isDead)
            {
                Vector2 origin = new Vector2(soliderTexture.Width / 2, soliderTexture.Height / 2); // Środek tekstury
                Raylib.DrawTexturePro(
                    soliderTexture,
                    new Rectangle(0, 0, soliderTexture.Width, soliderTexture.Height),
                    new Rectangle(position.X, position.Y, soliderTexture.Width * scale, soliderTexture.Height * scale),
                    origin,
                    rotation,
                    Color.White
                );


                // Rysowanie pocisków
                foreach (var bullet in bullets)
                {
                    bullet.Draw(); // Rysowanie pocisków, które solider wystrzelił
                }
            }
        }

        public void FollowPlayer(float deltaTime)
        {
            // Obliczanie kierunku do gracza
            Vector2 direction = targetPlayer.position - position;
            float distance = direction.Length();

            // Normalizowanie kierunku
            if (distance > 0)
            {
                direction = Vector2.Normalize(direction);
                position += direction * speed * deltaTime;
            }

            // Obracanie w kierunku gracza
            if (distance > 0)
            {
                rotation = MathF.Atan2(direction.Y, direction.X) * (180.0f / MathF.PI);
            }
        }

        public void AttackPlayer(float deltaTime)
        {
            float distance = Vector2.Distance(position, targetPlayer.position);

            // Jeżeli gracz jest w zasięgu ataku, wykonaj akcję (np. strzał)
            if (distance <= attackRange)
            {
                // Wykonaj atak (np. strzał)
                lastShotTime += deltaTime;
                if (lastShotTime >= fireRate)
                {
                    ShootAtPlayer();
                    lastShotTime = 0f; // Resetowanie czasu od ostatniego strzału
                }
            }
        }

        private void ShootAtPlayer()
        {
            // Obliczanie kierunku strzału
            Vector2 direction = targetPlayer.position - position;
            direction = Vector2.Normalize(direction);

            // Obliczanie pozycji końca lufy
            Vector2 gunTip = position + gunTipOffset;
            gunTip.X += 175;
            gunTip.Y += 140;
            gunTip = RotatePoint(gunTip, position, rotation); // Obracamy punkt lufy


            // Tworzenie nowego pocisku
            bullets.Add(new Bullet(gunTip, direction));
        }

        // Metoda do obliczenia obrotu
        public static Vector2 RotatePoint(Vector2 point, Vector2 center, float angle)
        {
            float radian = MathF.PI / 180f * angle;
            float cosA = MathF.Cos(radian);
            float sinA = MathF.Sin(radian);

            float dx = point.X - center.X;
            float dy = point.Y - center.Y;

            return new Vector2(
                center.X + cosA * dx - sinA * dy,
                center.Y + sinA * dx + cosA * dy
            );
        }

        public void Update(float deltaTime)
        {
            if (!isDead)
            {
                // Śledzenie gracza
                FollowPlayer(deltaTime);

                // Atakowanie gracza, jeśli jest w zasięgu
                AttackPlayer(deltaTime);

                // Zaktualizowanie pocisków
                foreach (var bullet in bullets)
                {
                    bullet.Update(deltaTime); // Aktualizacja pozycji pocisku
                }

                // Usuwanie pocisków, które opuściły ekran
                bullets.RemoveAll(bullet => Vector2.Distance(bullet.position, position) > Constantly.renderBullets);
            }
        }
    }
}

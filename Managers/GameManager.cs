using Raylib_cs;
using RPG_JKL.Entity.Ai.Enemy;
using RPG_JKL.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RPG_JKL.Managers
{
    internal class GameManager
    {
        Player player;
        Solider solider;
        MapManager mapManager;
        Camera2D camera;
        SkillManager skillManager;
        DashSkill dashSkill;
        GameUI menu;

        List<Solider> enemies = new List<Solider>();

        public void Init()
        {
            Raylib.InitWindow(Constantly.windowWidht, Constantly.windowHeight, "RPG Game");
            Raylib.SetTargetFPS(60);

            player = new Player();
            solider = new Solider(player);
            dashSkill = new DashSkill();

            mapManager = new MapManager();
            skillManager = new SkillManager();
            menu = new GameUI(dashSkill,player);

            enemies.Add(solider);
            // skils
            skillManager.AddSkill(dashSkill);


            mapManager.Init();

            camera = new Camera2D
            {
                Offset = new Vector2(Constantly.windowWidht / 2, Constantly.windowHeight / 2), // Pozycja kamery
                Target = new Vector2(0.0f, 0.0f), // Punkt, na który kamera patrzy
                Rotation = 0.0f, // Rotacja kamery
                Zoom = 1.0f // Poziom powiększenia
            };
        }

        public void Update()
        {
            float deltaTime = Raylib.GetFrameTime();

            Vector2 movementDirection = new Vector2();

            if (Raylib.IsKeyDown(KeyboardKey.W)) { movementDirection.Y = -1; }
            if (Raylib.IsKeyDown(KeyboardKey.S)) { movementDirection.Y = 1; }
            if (Raylib.IsKeyDown(KeyboardKey.A)) { movementDirection.X = -1; }
            if (Raylib.IsKeyDown(KeyboardKey.D)) { movementDirection.X = 1; }

            if (Raylib.IsKeyPressed(KeyboardKey.C)) { skillManager.ActivateSkill(0, movementDirection, player); }
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift) && player.stamina > 0) {  player.isSprinting = true;  }
            else { player.isSprinting = false; }

            player.Sprint(deltaTime);
            player.Move(movementDirection, deltaTime);
            player.RotateTowardsMouse(camera);
            player.TryShoot(deltaTime); // Próbuj strzelić
            player.UpdateBullets(deltaTime);

            solider.Update(deltaTime);

            ShootCheck();

            skillManager.Update(player, deltaTime);

            camera.Target = player.position;
        }
        public void ShootCheck()
        {
            foreach (Bullet bullet in player.bullets)
            {
                if (Vector2.Distance(bullet.position, solider.position) < 30f) // 10f to próg odległości
                {
                    solider.hp -= 15;
                    Console.WriteLine("solider hp: " + solider.hp);
                    player.bullets.Remove(bullet);
                    return;
                }
            }
            foreach(Solider enemy in enemies)
            {
                if (enemy.hp <= 0)
                {
                    enemy.isDead = true;
                    enemies.Remove(enemy);
                    return;
                }
            }
        }

        public void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.BeginMode2D(camera);

            mapManager.Draw(camera);
            player.Draw();
            solider.Draw();

            Raylib.EndMode2D();

            menu.Draw();

            Raylib.EndDrawing();
        }
    }
}

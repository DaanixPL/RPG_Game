using System.Numerics;
using Raylib_cs;
using RPG_JKL.Skils;

namespace RPG_JKL.Skills
{
    internal class DashSkill : Template
    {

        private float dashDuration = 0.2f; // Czas trwania dasha
        private float dashCooldown = 5.0f;
        private float dashSpeed = 700.0f;  // Prędkość dasha
        private bool isDashing = false;
        private float dashTime = 0.0f;
        private Vector2 dashDirection;
        


        public DashSkill()
        {
            Cooldown = dashCooldown; // Czas odnowienia skilla
            lastUsedTime = -Cooldown;
        }

        public override void Activate(Vector2 direction, Player player)
        {
            if (!IsReady || direction == Vector2.Zero) return;

            isDashing = true;
            dashDirection = Vector2.Normalize(direction);
            dashTime = 0.0f;
            UseSkill(); // Ustaw czas użycia
        }
        public double GetRemainingCooldownTime()
        {
            return Math.Max(0, Cooldown - (Raylib.GetTime() - lastUsedTime));
        }


        public override void Update(Player player, float deltaTime)
        {
            if (isDashing)
            {
                dashTime += deltaTime;
                if (dashTime < dashDuration)
                {
                    player.position += dashDirection * dashSpeed * deltaTime; // Ruch dasha
                }
                else
                {
                    isDashing = false; // Koniec dasha
                }
            }
        }
    }
}

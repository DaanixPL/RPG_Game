using Raylib_cs;
using RPG_JKL.Managers;
using RPG_JKL.Skills;

namespace RPG_JKL
{
    internal class GameUI
    {
        private Texture2D DashIcon = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\Menu\\DashIcon.png");
        private DashSkill dashSkill;
        private Player player;

        public GameUI(DashSkill dashSkill, Player player)
        {
            this.dashSkill = dashSkill;
            this.player = player;
        }
        public void Draw()
        {
            Color semiTransparentBlack = new Color(0, 0, 0, 150);

            int x = Constantly.windowWidht / 2 - 300;  // Wyśrodkowanie kwadratu
            int y = Constantly.windowHeight - 150;     // Pozycja przy dolnej krawędzi ekranu

            DrawStatistic();
            // Rysowanie półprzezroczystego kwadratu
            Raylib.DrawRectangle(x, y, 600, 150, semiTransparentBlack);

            Raylib.DrawTexture(DashIcon, x + 25, y + 10, Color.White);

            double remainingCooldown = dashSkill.GetRemainingCooldownTime();
            string cooldownText;

            if (remainingCooldown <= 0)
            {
                cooldownText = "Ready";
            }
            else
            {
                // Zaokrąglamy czas do pełnych sekund i wyświetlamy
                int roundedCooldown = (int)Math.Ceiling(remainingCooldown);
                cooldownText = $"cd: {roundedCooldown}";
            }

            Raylib.DrawText(cooldownText, x + 120, y + 110, 15, Color.White); // Wyświetlanie tekstu obok ikony
        }
        public void DrawStatistic()
        {
            Color semiTransparentBlack = new Color(0, 0, 0, 150);

            int x = Constantly.windowWidht - 150;
            int y = 0;

            float stamina = player.ReturnStamina();

            float staminaPercentage = Math.Clamp(stamina / 100f, 0f, 1f); // Procent staminy
            int barWidth = (int)(staminaPercentage * 125); // Szerokość paska w pikselach
            int barHeight = 20; // Wysokość paska
            int barX = x + 12; // Pozycja X paska (taka sama jak kwadratu)
            int barY = y + 110; // Pozycja Y paska (na dole kwadratu)

            Raylib.DrawRectangle(x, y, 150, 150, semiTransparentBlack);

            Color staminaColor = stamina > 30 ? Color.Blue : Color.DarkBlue; // Zielony, jeśli stamine >30, inaczej czerwony

            // Rysowanie paska staminy
            Raylib.DrawRectangle(barX, barY, barWidth, barHeight, staminaColor);

            // Rysowanie ramki paska
            Raylib.DrawRectangleLines(barX, barY, 125, barHeight, Color.Black);

            float hp = player.ReturnHp();

            float hpPercentage = Math.Clamp(hp / 100f, 0f, 1f); // Procent staminy
            int hpWidth = (int)(hpPercentage * 150); // Szerokość paska w pikselach
            int hpHeight = 20; // Wysokość paska
            int hpX = x; // Pozycja X paska (taka sama jak kwadratu)
            int hpY = y + 130; // Pozycja Y paska (na dole kwadratu)

            Color hpColor = hp > 30 ? Color.Green : Color.Red; // Zielony, jeśli stamine >30, inaczej czerwony

            // Rysowanie paska staminy
            Raylib.DrawRectangle(hpX, hpY, hpWidth, hpHeight, hpColor);

            // Rysowanie ramki paska
            Raylib.DrawRectangleLines(hpX, hpY, 150, hpHeight, Color.Black);
        }
    }
}

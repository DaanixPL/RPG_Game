using Raylib_cs;
using System.Numerics;

namespace RPG_JKL.Skils
{
    internal abstract class Template
    {
        public float Cooldown { get; protected set; } // Czas odnowienia
        protected double lastUsedTime;                // Ostatni czas użycia

        public bool IsReady => Raylib.GetTime() - lastUsedTime >= Cooldown;

        public abstract void Activate(Vector2 direction, Player player);

        public virtual void Update(Player player, float deltaTime) { }

        protected void UseSkill()
        {
            lastUsedTime = Raylib.GetTime();
        }
    }
}

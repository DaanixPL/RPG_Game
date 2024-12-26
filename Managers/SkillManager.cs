using RPG_JKL.Skils;
using System.Collections.Generic;
using System.Numerics;

namespace RPG_JKL.Skills
{
    internal class SkillManager
    {
        private List<Template> skills = new List<Template>();

        public void AddSkill(Template skill)
        {
            skills.Add(skill);
        }

        public void ActivateSkill(int index, Vector2 direction, Player player)
        {
            if (index >= 0 && index < skills.Count)
            {
                skills[index].Activate(direction, player);
            }
        }

        public void Update(Player player, float deltaTime)
        {
            foreach (var skill in skills)
            {
                skill.Update(player, deltaTime);
            }
        }
    }
}

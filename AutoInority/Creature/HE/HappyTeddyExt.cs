﻿using AutoInority.Extentions;

namespace AutoInority.Creature
{
    internal class HappyTeddyExt : BaseCreatureExt
    {
        public override SkillTypeInfo[] SkillSets { get; } = new SkillTypeInfo[] { Insight, Attachment, Repression };

        public HappyTeddyExt(CreatureModel creature) : base(creature)
        {
        }

        public override bool CanWorkWith(AgentModel agent, SkillTypeInfo skill, out string message)
        {
            if (agent.GetRecentWorkedCreature()?.script is HappyTeddy)
            {
                message = string.Format(Angela.Creature.HappyTeddy, agent.Tag(), _creature.Tag(), skill.Tag());
                return false;
            }
            return base.CanWorkWith(agent, skill, out message);
        }
    }
}
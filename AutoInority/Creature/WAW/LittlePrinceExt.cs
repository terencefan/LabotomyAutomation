﻿using System.Linq;

using AutoInority.Extentions;

namespace AutoInority.Creature
{
    internal class LittlePrinceExt : ExpectNormalExt
    {
        private const int AGENT_MAX = 3;

        private const int SKILL_MAX = 2;

        public LittlePrinceExt(CreatureModel creature) : base(creature)
        {
        }

        public override bool CanWorkWith(AgentModel agent, SkillTypeInfo skill, out string message)
        {
            if (CenterBrain.FindLastRecords(agent, AGENT_MAX).Where(x => x.Creature == _creature).Count() == AGENT_MAX)
            {
                message = string.Format(Angela.Creature.YoungPrinceHasBuf, agent.Tag(), _creature.Tag(), skill.Tag());

                // WhenHasBuf(agent);
                return false;
            }
            else if (skill.rwbpType != RwbpType.W)
            {
                if (CenterBrain.FindLastRecords(_creature, SKILL_MAX).Where(r => r.Skill.rwbpType != RwbpType.W).Count() == SKILL_MAX)
                {
                    message = string.Format(Angela.Creature.YoungPrinceTwice, agent.Tag(), _creature.Tag(), skill.Tag());
                    return false;
                }
            }
            return base.CanWorkWith(agent, skill, out message);
        }
    }
}
﻿namespace AutoInority.Creature
{
    internal class WhiteNight : BaseCreatureExt
    {
        public override bool IsUrgent => QliphothCounter < 3;

        public override SkillTypeInfo[] SkillSets { get; } = new SkillTypeInfo[] { Insight, Attachment, Repression };

        public WhiteNight(CreatureModel creature) : base(creature)
        {
        }

        public override bool CheckConfidence(AgentModel agent, SkillTypeInfo skill)
        {
            return NormalConfidence(agent, skill) < Automaton.Instance.DeadConfidence && base.CheckConfidence(agent, skill);
        }
    }
}
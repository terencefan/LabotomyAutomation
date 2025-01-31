﻿namespace AutoInority.Creature
{
    internal class ScarecrowExt : ExpectNormalExt
    {
        public override bool AutoSuppress => true;

        public override SkillTypeInfo[] SkillSets { get; } = new SkillTypeInfo[] { Insight, Repression };

        public ScarecrowExt(CreatureModel creature) : base(creature)
        {
        }

        public override float ConfidenceMultiplifier(AgentModel agent, SkillTypeInfo skill) => agent.prudenceLevel > 2 ? 0.5f : 1;
    }
}
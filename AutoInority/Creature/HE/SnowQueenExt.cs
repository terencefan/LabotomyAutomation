﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoInority.Extentions;

namespace AutoInority.Creature
{
    internal class SnowQueenExt : GoodNormalExt
    {
        private static readonly int DummyGiftId = 1021;

        private static readonly int RealGiftId = 1023;

        private readonly FieldInfo _field;

        public override SkillTypeInfo[] SkillSets
        {
            get
            {
                if (IsFreezing)
                {
                    return new SkillTypeInfo[] { Instinct };
                }
                else if (IsFarming)
                {
                    return new SkillTypeInfo[] { Repression };
                }
                else
                {
                    return new SkillTypeInfo[] { Insight, Attachment };
                }
            }
        }

        private bool IsFreezing => (bool)_field.GetValue(_creature.script);

        private bool IsFarming => Automaton.Instance.FarmingCreatures.Contains(_creature);

        public SnowQueenExt(CreatureModel creature) : base(creature)
        {
            _field = typeof(SnowQueen).GetField("_isBlockWork", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public override bool CanWorkWith(AgentModel agent, SkillTypeInfo skill, out string message)
        {
            if (IsFreezing)
            {
                if (agent.fortitudeLevel < 5)
                {
                    message = Message(Angela.Creature.SnowQueenDual, agent, skill);
                    return false;
                }
            }
            else if (IsFarming) // a tricky way to get her buff.
            {
                message = "";
                return skill.rwbpType == RwbpType.P;
            }
            else if (agent.HasEquipment(DummyGiftId) && GoodConfidence(agent, skill) < DeadConfidence)
            {
                message = Message(Angela.Creature.SnowQueen, agent, skill);
                return false;
            }
            return base.CanWorkWith(agent, skill, out message);
        }

        public override bool TryGetEGOGift(out EquipmentTypeInfo gift)
        {
            gift = EquipmentTypeList.instance.GetData(RealGiftId);
            return gift != null;
        }

        public override bool CheckConfidence(AgentModel agent, SkillTypeInfo skill)
        {
            if (IsFarming)
            {
                // skip confidence check when farming.
                return CheckSurvive(agent, skill);
            }
            return base.CheckConfidence(agent, skill);
        }

        public override IEnumerable<AgentModel> FindAgents(bool extend = false)
        {
            if (IsFarming && !IsFreezing)
            {
                var agents = AgentManager.instance.GetAgentList().Where(x => x.IsAvailable() && x.HasEquipment(DummyGiftId));
                if (agents.Any())
                {
                    return agents;
                }
            }
            return base.FindAgents(extend);
        }
    }
}
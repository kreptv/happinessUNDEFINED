﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if SURVIVAL_ENGINE || SURVIVAL_ENGINE_ONLINE
using SurvivalEngine;
#endif

#if FARMING_ENGINE
using FarmingEngine;
#endif

#if SURVIVAL_ENGINE || SURVIVAL_ENGINE_ONLINE || FARMING_ENGINE

namespace DialogueQuests
{
    [CreateAssetMenu(fileName = "condition", menuName = "DialogueQuests/Conditions/Player Gold", order = 10)]
    public class ConditionGold : ConditionData
    {
        public override bool IsMet(NarrativeEvent evt, NarrativeCondition condition, Actor player, Actor triggerer) 
        {
            PlayerCharacter character = player.GetComponent<PlayerCharacter>();
            int i1 = character != null ? character.SaveData.gold : 0;
            return condition.CompareInt(i1, condition.value_int);
        }

        public override bool ShowValueInt()
        {
            return true;
        }

        public override bool ShowOperatorInt()
        {
            return true;
        }
    }
}

#endif

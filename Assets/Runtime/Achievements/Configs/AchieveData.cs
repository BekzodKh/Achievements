using System;
using System.Collections.Generic;

using UnityEngine;

using Sirenix.OdinInspector;

using Achievements.Conditions;

namespace Achievements.Configs
{
    [Serializable]
    public class AchieveData
    {
        public AchieveTypes AchieveType => _achieveType;
        public List<IAchieveCondition> AchieveConditions => _conditions;

        [LabelText("@_achieveType.AchieveId")] [SerializeField]
        private AchieveTypes _achieveType;

        [SerializeReference] private List<IAchieveCondition> _conditions;

        public AchieveData(AchieveTypes type, List<IAchieveCondition> conditions)
        {
            _achieveType = type;
            _conditions = conditions;
        }
    }
}
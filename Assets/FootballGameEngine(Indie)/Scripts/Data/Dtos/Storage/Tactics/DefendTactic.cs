using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tactics
{
    [Serializable]
    public class DefendTactic : Tactic
    {
        /// <summary>
        /// Distance ratio to push ahead
        /// </summary>
        [Range(1f, 2f)]
        [SerializeField]
        float _pushBackRatio = 1f;

        /// </summary>
        [Range(0.1f, 1f)]
        [SerializeField]
        float _tightPressFrequency = 0.5f;

        /// <summary>
        /// A reference to how fast this instance transist from defend to attack
        /// </summary>
        [Range(0.1f, 1f)]
        [SerializeField]
        float _transistIntoDefendSpeed = 0.5f;

        [SerializeField]
        DefenceTypeEnum _defendType;

        public float TransistIntoDefendSpeed { get => _transistIntoDefendSpeed; set => _transistIntoDefendSpeed = value; }
        public DefenceTypeEnum DefendType { get => _defendType; set => _defendType = value; }
        public float PushBackRatio { get => _pushBackRatio; set => _pushBackRatio = value; }
        public float TightPressFrequency { get => _tightPressFrequency; set => _tightPressFrequency = value; }
    }
}

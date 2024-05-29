using Assets.FootballGameEngine_Indie.Scripts.Utilities.Objects;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tactics
{
    [Serializable]
    public class AttackTactic : Tactic
    {
        /// <summary>
        /// A reference to the probability of making a long pass
        /// </summary>
        [Range(0.1f, 1f)]
        [SerializeField]
        float _forwardRunProbability = 0.5f;

        /// <summary>
        /// A reference to the probability of making a long pass
        /// </summary>
        [Range(0.1f, 1f)]
        [SerializeField]
        float _longBallProbability = 0.5f;

        /// <summary>
        /// Distance ratio to push ahead
        /// </summary>
        [SerializeField]
        [Range(1f, 2f)]
        float _pushAheadRatio = 1f;

        /// <summary>
        /// Frequency for pushing players upfield when in possesion
        /// </summary>
        [SerializeField]
        float _pushForwardFrequency = 0.5f;

        /// <summary>
        /// A reference to how fast this instance transist from defend to attack
        /// </summary>
        [Range(0.1f, 1f)]
        [SerializeField]
        float _transistIntoAttackSpeed = 0.5f;

        /// <summary>
        /// A reference to the short pass range
        /// </summary>
        [SerializeField]
        Range _shortPassRange = new Range(5f, 9f);

        [SerializeField]
        AttackTypeEnum _attackType;

        public AttackTypeEnum AttackType { get => _attackType; set => _attackType = value; }
        public Range ShortPassRange { get => _shortPassRange; set => _shortPassRange = value; }
        public float LongBallProbability { get => _longBallProbability; set => _longBallProbability = value; }
        public float ForwardRunProbability { get => _forwardRunProbability; set => _forwardRunProbability = value; }
        public float PushAheadRatio { get => _pushAheadRatio; set => _pushAheadRatio = value; }
        public float PushForwardFrequency { get => _pushForwardFrequency; set => _pushForwardFrequency = value; }
        public float TransistIntoAttackSpeed { get => _transistIntoAttackSpeed; set => _transistIntoAttackSpeed = value; }
    }
}

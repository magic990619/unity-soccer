using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Entities
{
    public class Pass
    {
        public float BallTimeToTarget { get; set; }
        public float KickPower { get; set; }

        public PassTypesEnum PassType { get; set; }

        public Vector3 FromPosition { get; set; }
        public Vector3 ToPosition { get; set; }

        public Player Receiver { get; set; }

        public override string ToString()
        {
            string stringValue = string.Format("BallTimeToTarget-{0} n/ KickPower-{1} n/ PassType-{2} n/ FromPosition-{3} n/ ToPosition-{4} n/ Receiver-{5}", BallTimeToTarget, KickPower, PassType, FromPosition, ToPosition, Receiver);
            return stringValue;
        }

        public void Init(float ballTimeToTarget, float kickPower, PassTypesEnum passTypesEnum, Vector3 fromPosition, Vector3 toPosition, Player receiver)
        {
            BallTimeToTarget = ballTimeToTarget;
            KickPower = kickPower;
            PassType = passTypesEnum;
            FromPosition = fromPosition;
            ToPosition = toPosition;
            Receiver = receiver;
        }
    }
}

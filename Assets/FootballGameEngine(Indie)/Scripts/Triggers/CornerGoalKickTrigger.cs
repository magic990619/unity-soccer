using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Triggers
{
    public class CornerGoalKickTrigger : BTrigger
    {
        public delegate void CollidedWithBall(Vector3 position);
        public new CollidedWithBall OnCollidedWithBall;

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if (other.tag == "Ball")
            {
                // cache the collision position
                CollidedWithBall temp = OnCollidedWithBall;
                if (temp != null)
                    temp.Invoke(other.transform.position);
            }
        }
    }
}

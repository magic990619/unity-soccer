using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Triggers
{
    public class BTrigger : MonoBehaviour
    {
        public Action OnCollidedWithBall;

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ball")
            {
                //invoke that the wall has collided with the ball
                Action temp = OnCollidedWithBall;
                if (temp != null)
                    temp.Invoke();
            }
        }
    }
}

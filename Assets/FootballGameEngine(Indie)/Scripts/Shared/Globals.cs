using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FootballGameEngine_Indie_.Scripts.Shared
{
    public static class Globals
    {
        /// <summary>
        /// The maximum velocity of the ball that we can catch it
        /// </summary>
        public static float MaxBallCatchableVelocity { get; } = 30f;

        /// <summary>
        /// Max distance the goalkeeper can move from his own goal
        /// </summary>
        public static float MaxWanderDistanceFromGoal { get; } = 30f;

        /// <summary>
        /// Maximum time we have to wait before we can update logic to protect the goal
        /// </summary>
        public static float MaxProtectGoalWaitTime { get; } = 1f;

        /// <summary>
        /// Time to recover from kick
        /// </summary>
        public static float RecoverFromKickWaitTime { get; } = 0.5f;

        /// <summary>
        /// The layer mask for the eighteen area
        /// </summary>
        public static string EighteenAreaLayerMask { get; } = "EighteenArea";

    }
}

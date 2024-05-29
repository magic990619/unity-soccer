using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.SubStates
{
    public class PrepareForGoalKick : BState
    {
        bool _waitForAttackingTeamToFinishSetup;

        float _waitTime;

        public override void Enter()
        {
            base.Enter();

            if(Owner.HasGoalKick == true)
            {
                // set the positions
                PlaceBallAtGoalKickPosition();
                Owner.PlaceEveryPlayerAtHomePosition(0.25f, Ball.Instance.NormalizedPosition);
                PlaceGoalKeeperAtPosition();

                // trigger state change
                Machine.ChangeState<TakeGoalKick>();
            }
            else
            {
                // wait for attacking team
                _waitForAttackingTeamToFinishSetup = true;
                _waitTime = 0.1f;
            }
        }

        public override void Execute()
        {
            base.Execute();

            if(_waitForAttackingTeamToFinishSetup == true)
            {
                _waitTime -= Time.deltaTime;
                if(_waitTime <= 0f)
                {
                    // update the position of the players
                    Owner.PlaceEveryPlayerAtHomePosition(0.65f, Ball.Instance.NormalizedPosition);

                    // trigger state change
                    Machine.ChangeState<DefendGoalKick>();
                }
            }
        }

        void PlaceBallAtGoalKickPosition()
        {
            // stop ball
            Ball.Instance.Trap();

            // find side to place ball
            Vector3 localBallPosition = Owner.Goal.transform.InverseTransformPoint(Owner.CachedBallPosition);
            bool isBallOnLeftSide = localBallPosition.x < 0;

            // place ball
            if (isBallOnLeftSide == true)
                Ball.Instance.Position = Owner.LeftSideGoalKickSpot.position;
            else
                Ball.Instance.Position = Owner.RightSideGoalKickSpot.position;

        }

        void PlaceGoalKeeperAtPosition()
        {
            // get the goalie
            TeamPlayer player = Owner.Players
                .Find(tM => tM.Player.PlayerType == PlayerTypes.Goalkeeper);

            // set player pos
            Vector3 position = Ball.Instance.Position + Owner.Opponent.Goal.transform.forward * player.Player.Radius;
            
            // place him
            player.CurrentHomePosition.position = position;
            player.Player.Position = position;

            // face ball
            Vector3 direction = Ball.Instance.Position - position;
            player.Player.Rotation = Quaternion.LookRotation(direction);

            //get the take kick of state and set the controlling player
            Machine.GetState<TakeGoalKick>().ControllingPlayer = player;
        }

        public FootballGameEngine_Indie.Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}

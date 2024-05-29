using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.PutBallBackIntoPlay.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.SubStates
{
    public class AttackCounterKick : BState
    {
        bool _hasSendMessage;

        float _lengthPitch = 90;
        float _waitTime;

        public override void Enter()
        {
            base.Enter();

            // set some stuff
            _hasSendMessage = false;
            _waitTime = 3f;

            // register to some events
            Owner.GoalKeeper.OnPutBallBackIntoPlay += Instance_OnPutBallBackIntoPlay;

            // move players upfield
            Owner.MovePlayersUpField(_lengthPitch, 0.15f, Ball.Instance.Position);
            Owner.Players.ForEach(tM =>
            {
                tM.Player.IsTeamInControl = true;
                ActionUtility.Invoke_Action(tM.Player.OnInstructedToGoToHome);
            });
        }

        public override void Execute()
        {
            base.Execute();

            // decrement time
            if (_waitTime > 0)
                _waitTime -= Time.deltaTime;

            // send message if appropriate
            if(_waitTime <= 0 
                && _hasSendMessage == false
                && Owner.GoalKeeper.GoalKeeperFSM.IsCurrentState<IdleMainState>() == true)
            {
                // set some stuff
                _hasSendMessage = true;

                // tell the keeper to put the ball back into play
                //Owner.GoalKeeper.GoalKeeperFSM.ChangeState<PutBallBackIntoPlayMainState>();
                Owner.GoalKeeper.Invoke_OnInstructedToPutBallBackIntoPlay();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister listening to some events
            Owner.Opponent.GoalKeeper.OnPutBallBackIntoPlay -= Instance_OnPutBallBackIntoPlay;
        }

        private void Instance_OnPutBallBackIntoPlay()
        {
            SuperMachine.ChangeState<AttackMainState>();
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

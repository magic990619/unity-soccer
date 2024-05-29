using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Wait.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates;
using RobustFSM.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState
{
    /// <summary>
    /// The team drops into it's own half and tries to place players between itself and the
    /// goal in the hope of making it difficult for the opposition to score
    /// </summary>
    public class DefendMainState : BState
    {
        bool _canDefend;

        float _waitTime;
        float _pitchLength;

        TeamPlayer _closestPlayerToBall;

        public Team Owner { get => ((TeamFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // pick some wait time
            _pitchLength = Pitch.Instance.Length;
            _waitTime = Mathf.Clamp(Random.value, 0.1f, Owner.TeamData.DefendTactic.TransistIntoDefendSpeed);

            // enable the support spots root
            Owner.PlayerSupportSpots.gameObject.SetActive(true);

            //listen to some team events
            Owner.OnGoalKeeperGainedBallControl += Instance_OnGoalKeeperGainedBallControl;
            Owner.OnGainPossession += Instance_OnGainPossession;
            Owner.OnMessagedToStop += Instance_OnMessagedToStop;
            Owner.OnMessagedToTakeThrowIn += Instance_OnMessagedToTakeThrowIn;

            // init the players home positions
            Owner.Players.ForEach(tM => ActionUtility.Invoke_Action(tM.Player.OnInstructedToGoToHome));
        }

        public override void Execute()
        {
            base.Execute();

            if (_canDefend == false)
            {
                _waitTime -= Time.deltaTime;
                if (_waitTime < 0) _canDefend = true;
            }
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            if (_canDefend)
            {
                // trigger closest player to ball to chase ball
                TriggerPlayerToChaseBall();
                //Owner.TriggerPlayersToPickOutThreats();
                MovePlayersDownField();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // enable the support spots root
            Owner.PlayerSupportSpots.gameObject.SetActive(false);

            //stop listening to some team events
            Owner.OnGoalKeeperGainedBallControl -= Instance_OnGoalKeeperGainedBallControl;
            Owner.OnGainPossession -= Instance_OnGainPossession;
            Owner.OnMessagedToStop -= Instance_OnMessagedToStop;
            Owner.OnMessagedToTakeThrowIn -= Instance_OnMessagedToTakeThrowIn;
        }

        private void Instance_OnGainPossession()
        {
            Machine.ChangeState<AttackMainState>();
        }

        public void Instance_OnGoalKeeperGainedBallControl()
        {
            // change state
            SuperMachine.GetState<CounterKickMainState>().GoalKeeperHasBall = true;
            SuperMachine.ChangeState<CounterKickMainState>();
        }

        private void MovePlayersDownField()
        {
            //loop through each player and update it's position
            foreach (TeamPlayer teamPlayer in Owner.Players)
            {
                //find the percentage to move the player upfield
                Vector3 ballGoalLocalPosition = Owner.Opponent.Goal.transform.InverseTransformPoint(Ball.Instance.transform.position);
                float playerMovePercentage = Mathf.Clamp01((ballGoalLocalPosition.z / _pitchLength) * Owner.TeamData.DefendTactic.PushBackRatio);

                //move the home position a similar percentage up the field
                Vector3 currentPlayerHomePosition = Vector3.Lerp(teamPlayer.AttackingHomePosition.transform.position,
                    teamPlayer.DefendingHomePosition.position,
                    playerMovePercentage);

                // only move upfield. 
                teamPlayer.CurrentHomePosition.position = currentPlayerHomePosition;
            }
        }

        private void Instance_OnMessagedToStop()
        {
            SuperMachine.ChangeState<WaitMainState>();
        }

        private void Instance_OnMessagedToTakeThrowIn()
        {
            SuperMachine.ChangeState<TakeThrowIn>();
        }

        private void TriggerPlayerToChaseBall()
        {
            // get the current closest player to ball
            TeamPlayer currClosestPlayerToPoint = Owner.GetClosestPlayerToPoint(Ball.Instance.NormalizedPosition);

            //update new player to attack ball
            if (currClosestPlayerToPoint != _closestPlayerToBall)
            {
                // message the closest player to go out of chaseball
                if (_closestPlayerToBall != null)
                    _closestPlayerToBall.Player.Invoke_OnIsNoLongerTheClosestPlayerToBall();

                // update to new closest player
                _closestPlayerToBall = currClosestPlayerToPoint;

                // raise the new player to say he is now the new closest player to ball
                _closestPlayerToBall.Player.Invoke_OnBecameTheClosestPlayerToBall();
            }
            else if (currClosestPlayerToPoint != null
                && currClosestPlayerToPoint.Player.InFieldPlayerFSM.IsCurrentState<ChaseBallMainState>() == false)
            {
                // raise the new player to say he is now the new closest player to ball
                _closestPlayerToBall.Player.Invoke_OnBecameTheClosestPlayerToBall();
            }
        }

    }
}

using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates
{
    /// <summary>
    /// Prepares the team for a throw-in depending on whether the team is
    /// attacking the throw-in or defending it
    /// </summary>
    public class PrepareForThrowIn : BState
    {
        float _waitTime = 0.1f;

        Vector3 _normalizedBallPosition;
        Vector3 _rotationVector;

        TeamPlayer _teamPlayer = null;

        public override void Enter()
        {
            base.Enter();

            // normalize ball pos 
            _normalizedBallPosition = new Vector3(Owner.CachedBallPosition.x,
                0f,
                Owner.CachedBallPosition.z);

            // check if I should take the throw in
            if (Owner.HasThrowIn == true)
            {
                // set wait time
                _waitTime = 0.3f;

                // update the position of the players
                PlaceControllingPlayerAtThrowInPositionForAttackingTeam();

                // recalculate player home
                Owner.MovePlayersUpField();

                // set the players at there position
                Owner.Players
                    .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer 
                        && p != _teamPlayer)
                    .ToList()
                    .ForEach(p =>
                    {
                        // update position and steering target
                        p.Player.transform.LookAt(_normalizedBallPosition);
                        p.Player.Position = p.CurrentHomePosition.transform.position;
                        p.Player.HomePosition.position = p.CurrentHomePosition.transform.position;
                    });
            }
            else
            {
                // set wait time
                _waitTime = 0.1f;

                // recalculate player home
                // Todo::Pass a dynamic value of the pitch length
                Owner.MovePlayersDownField();

                // update players to pick out threats
                Owner.Players
                    .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer)
                    .ToList()
                    .ForEach(p =>
                    {
                        // update position and steering target
                        p.Player.transform.LookAt(_normalizedBallPosition);
                        p.Player.Position = p.CurrentHomePosition.transform.position;
                        p.Player.HomePosition.position = p.CurrentHomePosition.transform.position;
                    });
            }
        }

        public override void Execute()
        {
            base.Execute();

            _waitTime -= Time.deltaTime;
            if(_waitTime <= 0f)
            {
                if(Owner.HasThrowIn)
                    Machine.ChangeState<TakeThrowIn>();
                else
                    Machine.ChangeState<DefendThrowIn>();
            }
        }
        public override void Exit()
        {
            base.Exit();

            //if (Owner.HasThrowIn == true)
            //{
            //    // set the players to support attacker
            //    Owner.Players
            //        .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer
            //            && p != _teamPlayer)
            //        .ToList()
            //        .ForEach(p =>
            //        {
            //            // go to support attacker state
            //            p.Player.InFieldPlayerFSM.ChangeState<SupportAttackerMainState>();
            //        });
            //}
            //else
            //{
            //    // update players to pick out threats
            //    Owner.Players
            //        .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer)
            //        .ToList()
            //        .ForEach(p =>
            //        {
            //            // set some stuff
            //            p.Player.InFieldPlayerFSM.GetState<PickOutThreatMainState>().IsSetPiece = true;
            //            p.Player.InFieldPlayerFSM.ChangeState<PickOutThreatMainState>();
            //        });
            //}
        }

        void PlaceControllingPlayerAtThrowInPositionForAttackingTeam()
        {
            // find the player to take kickoff
            Vector3 localBallPos = Owner.transform.InverseTransformPoint(_normalizedBallPosition);
            bool isBallOnLeftSide = localBallPos.x <= 0;

            // get the player
            if (isBallOnLeftSide == true)
            {
                _rotationVector = Vector3.RotateTowards(Owner.transform.forward, Owner.transform.right, 90f * Mathf.Deg2Rad, 0f);
                _teamPlayer = GetPlayer(Owner.TeamData.LeftSideThrowInTake);
            }
            else 
            {

                _rotationVector = Vector3.RotateTowards(Owner.transform.forward, Owner.transform.right, -90f * Mathf.Deg2Rad, 0f);
                _teamPlayer = GetPlayer(Owner.TeamData.RightSideThrowInTake);
            }

            // set the position
            _teamPlayer.Player.transform.position = _normalizedBallPosition;
            _teamPlayer.Player.transform.rotation = Quaternion.LookRotation(_rotationVector);

            //get the take kick of state and set the controlling player
            Owner.ControllingPlayer = _teamPlayer.Player;

            // set this as the ball owner
            Ball.Instance.CurrentOwner = _teamPlayer.Player;
        }

        TeamPlayer GetPlayer(int id)
        {
            return Owner.Players[id];
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

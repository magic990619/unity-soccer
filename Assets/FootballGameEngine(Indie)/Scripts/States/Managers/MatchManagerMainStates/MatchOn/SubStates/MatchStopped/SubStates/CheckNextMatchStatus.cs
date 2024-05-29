using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates.MatchStopped.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates;
using RobustFSM.Base;
using RobustFSM.Interfaces;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.SubStates
{
    public class CheckNextMatchStatus : BState
    {
        public override void Enter()
        {
            base.Enter();

            // run the logic depending on match status
            if (Owner.MatchStatus == MatchStatuses.CornerKick)
            {
                Machine.ChangeState<BroadcastCornerKick>();
            }
            else if (Owner.MatchStatus == MatchStatuses.GoalKick)
            {
                Machine.ChangeState<BroadcastGoalKick>();
            }
            else if (Owner.MatchStatus == MatchStatuses.GoalScored)
            {
                Machine.ChangeState<BroadcastGoalScored>();
            }
            else if (Owner.MatchStatus == MatchStatuses.HalfExhausted)
            {
                //if it's the first half then we have to switch sides
                if (Owner.CurrentHalf == 1)
                    Machine.ChangeState<BroadcastHalfTimeStatus>();
                else if (Owner.CurrentHalf == 2)
                {
                    // check if we have a winner
                    bool isThereAWinner = Owner.TeamAway.Goals != Owner.TeamHome.Goals;

                    //todo::implement logic to take penalties after the match

                    if (Owner.CurrGameTime == GameTimeEnum.NormalTime)
                    {
                        // go match over
                        if(isThereAWinner == true)
                            Machine.ChangeState<TriggerMatchOver>();
                        else
                            Machine.ChangeState<BroadcastNormalTimeIsOver>();
                    }
                    else if (Owner.CurrGameTime == GameTimeEnum.ExtraTime)
                    {
                        // simply go to match over
                        Machine.ChangeState<TriggerMatchOver>();
                    }
                }
            }
            else if(Owner.MatchStatus == MatchStatuses.ThrowIn)
            {
                Machine.ChangeState<BroadcastThrowIn>();
            }
        }

        /// <summary>
        /// Access the super state machine
        /// </summary>
        public IFSM SuperFSM
        {
            get
            {
                return (MatchManagerFSM)SuperMachine;
            }
        }

        /// <summary>
        /// Access the owner of the state machine
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}

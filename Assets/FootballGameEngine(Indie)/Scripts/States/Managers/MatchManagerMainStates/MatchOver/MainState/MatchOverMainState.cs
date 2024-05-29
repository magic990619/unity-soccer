using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOver.MainState
{
    public  class MatchOverMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            //raise the match-end event
            RaiseTheMatchOverEvent();
        }

        public void RaiseTheMatchOverEvent()
        {
            //prepare the message
            string message = string.Empty;

            //generate the message
            if (Owner.TeamAway.IsUserControlled)
            {
                if (Owner.TeamAway.Goals > Owner.TeamHome.Goals)
                    message = "You Won";
                else if (Owner.TeamAway.Goals < Owner.TeamHome.Goals)
                    message = "You Lost";
                else
                    message = "Draw";
            }
            else if (Owner.TeamHome.IsUserControlled)
            {
                if (Owner.TeamAway.Goals < Owner.TeamHome.Goals)
                    message = "You Won";
                else if (Owner.TeamAway.Goals > Owner.TeamHome.Goals)
                    message = "You Lost";
                else
                    message = "Draw";
            }
            else
            {
                if (Owner.TeamAway.Goals > Owner.TeamHome.Goals)
                    message = "Away Team Won";
                else if (Owner.TeamAway.Goals < Owner.TeamHome.Goals)
                    message = "Home Team Won";
                else
                    message = "Draw";
            }

            //raise the on-match-end-evet
            ActionUtility.Invoke_Action(message, Owner.OnEnterMatchOverState);
        }

        public override void Exit()
        {
            base.Exit();

            //raise the event that I have exited the match over state
            ActionUtility.Invoke_Action(Owner.OnExitMatchOverState);

        }

        /// <summary>
        /// Returns the owner of this instance
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

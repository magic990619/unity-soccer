using Assets.RobustFSM.Mono;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Init.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.MainState;

namespace Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities
{
    public class TeamFSM : MonoFSM<Team>
    {
        public override void AddStates()
        {
            //set the update frequency
            SetUpdateFrequency(Owner.AiUpdateFrequency);

            //add the states
            AddState<AttackMainState>();
            AddState<CornerKickMainState>();
            AddState<CounterKickMainState>();
            AddState<GoalKickMainState>();
            AddState<InitMainState>();
            AddState<DefendMainState>();
            AddState<KickOffMainState>();
            AddState<ThrowInMainState>();
            AddState<WaitMainState>();

            //set the initial state
            SetInitialState<InitMainState>();
        }
    }
}

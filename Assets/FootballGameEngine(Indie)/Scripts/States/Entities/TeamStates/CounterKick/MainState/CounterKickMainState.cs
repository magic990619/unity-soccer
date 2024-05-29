using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.MainState
{
    public class CounterKickMainState : BHState
    {
        bool _goalKeeperHasBall;

        public bool GoalKeeperHasBall { get => _goalKeeperHasBall; set => _goalKeeperHasBall = value; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<AttackCounterKick>();
            AddState<DefendCounterKick>();
            AddState<PrepareForCounterKick>();

            // set initial state
            SetInitialState<PrepareForCounterKick>();
        }

        public override void Exit()
        {
            base.Exit();

            // reset stuff
            _goalKeeperHasBall = false;
        }
    }
}

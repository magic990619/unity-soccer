using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using RobustFSM.Base;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.SubStates
{
    public class InitializeDataDependentEntities : BState
    {
        public override void Enter()
        {
            base.Enter();

            // initialize components that depend on the data
            InitializeWithData();

            // got to wait for wait for match on instruction
            Machine.ChangeState<WaitForContinueInstruction>();
        }

        public override void Exit()
        {
            base.Exit();

            // enable the initmainmenu loading submenu
            GraphicsManager.Instance.InitMainMenu.DisableLoadSubMenu();
        }

        void InitializeWithData()
        {
            InitializeGameManagerWithData();
            InitializeSoundManagerWithData();
        }

        void InitializeGameManagerWithData()
        {
            InitializeGameManagerTeamsDataWithData();
        }

        void InitializeGameManagerTeamsDataWithData()
        {

            GameManager.Instance.DefaultSettings = SuperMachine.GetState<InitMainState>().Settings;
            GameManager.Instance.TeamsData = SuperMachine.GetState<InitMainState>().TeamsData;
        }

        void InitializeSoundManagerWithData()
        {
            // get the sound settings
            SoundSettingsDto soundSettings = SuperMachine.GetState<InitMainState>().Settings.SoundSettings;

            // init the sound manager
            SoundManager.Instance.Init(soundSettings.IsSoundOn, soundSettings.MusicVolume, soundSettings.SfxVolume);
        }
    }
}

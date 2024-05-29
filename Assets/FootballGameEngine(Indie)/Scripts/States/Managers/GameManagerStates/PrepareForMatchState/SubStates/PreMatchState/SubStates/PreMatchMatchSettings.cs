using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates.PreMatchState.SubStates
{
    public class PreMatchMatchSettings : BState
    {
        public GameManager Owner { get => ((GameManagerFSM)SuperMachine).Owner; }

        public override void Initialize()
        {
            base.Initialize();

            // init
            Init();
        }

        public override void Enter()
        {
            base.Enter();

            // initialize the utility menu
            InitializeSettingsMenu();
            InitializeUtilityMenu();

            // enable the prematch match-settings menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchMatchSettingsSubMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // disable the prematch team-management menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchMatchSettingsSubMenu
                .Root
                .SetActive(false);
        }

        private void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true,
                false,
                "Match Settings",
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.GoToPreviousState();
                });
        }

        void Init()
        {
            InitBasicSettings();
        }

        void InitBasicSettings()
        {
            InitMatchDifficultyButtons();
            InitHalfLengthsButtons();
            InitOtherSettings();
        }

        void InitOtherSettings()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.ToggleIsRadarOn.onValueChanged.AddListener((bool value) =>
            {
                Owner.MatchDayMatchSettings.IsRadarOn = value;
            });
        }

        void InitializeSettingsMenu()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnFirst.isOn = (Owner.MatchDayMatchSettings.MatchDifficulty == MatchDifficultyEnum.Easy);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnThird.isOn = (Owner.MatchDayMatchSettings.MatchDifficulty == MatchDifficultyEnum.Hard);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnSecond.isOn = (Owner.MatchDayMatchSettings.MatchDifficulty == MatchDifficultyEnum.Normal);

            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnThird.isOn = (Owner.MatchDayMatchSettings.HalfLength == 5f);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnSecond.isOn = (Owner.MatchDayMatchSettings.HalfLength == 4f);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnFirst.isOn = (Owner.MatchDayMatchSettings.HalfLength == 3f);

            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.ToggleIsRadarOn.isOn = Owner.MatchDayMatchSettings.IsRadarOn;
        }

        #region InitHalfLengthButtons

        void InitHalfLengthsButtons()
        {
            this.InitFiveMinHalfLengthButton();
            this.InitFourMinHalfLengthButton();
            this.InitThreeMinHalfLengthButton();
        }

        void InitFiveMinHalfLengthButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnThird.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.HalfLength = 5f;
                }
            });
        }

        void InitFourMinHalfLengthButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnSecond.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.HalfLength = 2f;
                }
            });
        }

        void InitThreeMinHalfLengthButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.HalfLengthSettings.BtnFirst.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.HalfLength = 3f;
                }
            });
        }
        #endregion

        #region InitMatchDifficultyButtons

        void InitMatchDifficultyButtons()
        {
            InitMatchDifficultyEasyButton();
            InitMatchDifficultyHardButton();
            InitMatchDifficultyNormalButton();
        }

        void InitMatchDifficultyEasyButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnFirst.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.MatchDifficulty = MatchDifficultyEnum.Easy;
                }
            });
        }

        void InitMatchDifficultyHardButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnThird.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.MatchDifficulty = MatchDifficultyEnum.Hard;
                }
            });
        }

        void InitMatchDifficultyNormalButton()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchMatchSettingsSubMenu.DifficultySettings.BtnSecond.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.MatchDayMatchSettings.MatchDifficulty = MatchDifficultyEnum.Normal;
                }
            });
        }

        #endregion
    }
}

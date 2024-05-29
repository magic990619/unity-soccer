using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.SettingsState.MainState
{
    public class SettingsMainState :BState
    {

        public override void Initialize()
        {
            base.Initialize();

            // initialize state
            Init();
        }

        public override void Enter()
        {
            base.Enter();

            // initialize settings
            this.InitializeSettingsMenu();
            this.InitializeUtilityMenu();

            // enable the menus
            GraphicsManager.Instance.MenuManager.EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance.MenuManager.EnableMenu(GraphicsManager.Instance.SettingsMainMenu.ID);
            GraphicsManager.Instance.MenuManager.EnableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);
        }

        public override void Exit()
        {
            base.Exit();

            // save to file
            DataManager.Instance.SaveData(Owner.DefaultSettings, Owner.SettingsSaveFileName);

            // disable the menus
            GraphicsManager.Instance.MenuManager.DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance.MenuManager.DisableMenu(GraphicsManager.Instance.SettingsMainMenu.ID);
            GraphicsManager.Instance.MenuManager.DisableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);
        }

        public GameManager Owner
        {
            get
            {
                return ((GameManagerFSM)SuperMachine).Owner;
            }
        }

        void Init()
        {
            InitBasicSettings();
            InitSoundSettings();
        }

        void InitBasicSettings()
        {
            InitMatchDifficultyButtons();
            InitHalfLengthsButtons();
            InitOtherButtons();
        }

        void InitMatchDifficultyButtons()
        {
            InitMatchDifficultyEasyButton();
            InitMatchDifficultyHardButton();
            InitMatchDifficultyNormalButton();
        }

        void InitMatchDifficultyEasyButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyEasy.onValueChanged.AddListener((bool value) => 
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.MatchDifficulty = MatchDifficultyEnum.Easy;
                }
            });
        }

        void InitMatchDifficultyHardButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyHard.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.MatchDifficulty = MatchDifficultyEnum.Hard;
                }
            });
        }

        void InitMatchDifficultyNormalButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyNormal.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.MatchDifficulty = MatchDifficultyEnum.Normal;
                }
            });
        }

        void InitHalfLengthsButtons()
        {
            this.InitFiveMinHalfLengthButton();
            this.InitFourMinHalfLengthButton();
            this.InitThreeMinHalfLengthButton();
        }

        void InitFiveMinHalfLengthButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthFiveMins.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.HalfLength = 5f;
                }
            });
        }

        void InitFourMinHalfLengthButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthFourMins.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.HalfLength = 4f;
                }
            });
        }

        void InitThreeMinHalfLengthButton()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthThreeMins.onValueChanged.AddListener((bool value) =>
            {
                if (value)
                {
                    Owner.OnButtonClicked();
                    Owner.DefaultSettings.MatchSettings.HalfLength = 3f;
                }
            });
        }

        void InitOtherButtons()
        {
            GraphicsManager.Instance.SettingsMainMenu.ToggleRadar.onValueChanged.AddListener((bool value) =>
            {
                Owner.OnButtonClicked();
                Owner.DefaultSettings.MatchSettings.IsRadarOn = value;
            });
        }

        void InitSoundSettings()
        {
            this.InitMusicSlider();
            this.InitSfxSlider();
            this.InitSoundToggle();
        }

        void InitMusicSlider()
        {
            GraphicsManager.Instance.SettingsMainMenu.SliderMusicVolume.onValueChanged.AddListener((float value) =>
            {
                GameManager.Instance.DefaultSettings.SoundSettings.MusicVolume = value;
                SoundManager.Instance.MusicVolume = value;
            });
        }

        void InitSfxSlider()
        {
            GraphicsManager.Instance.SettingsMainMenu.SliderSoundVolume.onValueChanged.AddListener((float value) =>
            {
                GameManager.Instance.DefaultSettings.SoundSettings.SfxVolume = value;
                SoundManager.Instance.SoundVolume = value;
            });
        }

        private void InitSoundToggle()
        {
            GraphicsManager.Instance.SettingsMainMenu.ToggleSounds.onValueChanged.AddListener((bool value) =>
            {
                Owner.OnButtonClicked();
                GameManager.Instance.DefaultSettings.SoundSettings.IsSoundOn = value;
                SoundManager.Instance.IsSoundEnabled = value;
            });
        }

        void InitializeSettingsMenu()
        {
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyEasy.isOn = (Owner.DefaultSettings.MatchSettings.MatchDifficulty == MatchDifficultyEnum.Easy);
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyHard.isOn = (Owner.DefaultSettings.MatchSettings.MatchDifficulty == MatchDifficultyEnum.Hard);
            GraphicsManager.Instance.SettingsMainMenu.BtnDifficultyNormal.isOn = (Owner.DefaultSettings.MatchSettings.MatchDifficulty == MatchDifficultyEnum.Normal);
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthFiveMins.isOn = (Owner.DefaultSettings.MatchSettings.HalfLength == 5f);
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthFourMins.isOn = (Owner.DefaultSettings.MatchSettings.HalfLength == 4f);
            GraphicsManager.Instance.SettingsMainMenu.BtnHalfLengthThreeMins.isOn = (Owner.DefaultSettings.MatchSettings.HalfLength == 3f);
            GraphicsManager.Instance.SettingsMainMenu.SliderMusicVolume.value = SoundManager.Instance.MusicVolume;
            GraphicsManager.Instance.SettingsMainMenu.SliderSoundVolume.value = SoundManager.Instance.SoundVolume;
            GraphicsManager.Instance.SettingsMainMenu.ToggleSounds.isOn = SoundManager.Instance.IsSoundEnabled;
            GraphicsManager.Instance.SettingsMainMenu.ToggleRadar.isOn = Owner.DefaultSettings.MatchSettings.IsRadarOn;
        }

        void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true, false, "Settings", () =>
            {
                Owner.OnButtonClicked();
                SuperMachine.GoToPreviousState();
            }, null);
        }
    }
}

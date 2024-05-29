using SmartMenuManagement.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.SettingsMenu.MainMenu
{
    [Serializable]
    public class SettingsMainMenu : BMenu
    {
        [SerializeField]
        private Slider _sliderMusicVolume;

        [SerializeField]
        private Slider _sliderSoundVolume;

        [SerializeField]
        private Toggle btnHalfLengthFiveMins;

        [SerializeField]
        private Toggle btnHalfLengthFourMins;

        [SerializeField]
        private Toggle btnHalfLengthThreeMins;

        [SerializeField]
        private Toggle btnDifficultyEasy;

        [SerializeField]
        private Toggle btnDifficultyHard;

        [SerializeField]
        private Toggle btnDifficultyNormal;

        [SerializeField]
        private Toggle _toggleRadar;

        [SerializeField]
        private Toggle _toggleSounds;

        public Slider SliderMusicVolume { get => _sliderMusicVolume; set => _sliderMusicVolume = value; }
        public Slider SliderSoundVolume { get => _sliderSoundVolume; set => _sliderSoundVolume = value; }
        public Toggle BtnHalfLengthFiveMins { get => btnHalfLengthFiveMins; set => btnHalfLengthFiveMins = value; }
        public Toggle BtnHalfLengthFourMins { get => btnHalfLengthFourMins; set => btnHalfLengthFourMins = value; }
        public Toggle BtnHalfLengthThreeMins { get => btnHalfLengthThreeMins; set => btnHalfLengthThreeMins = value; }
        public Toggle BtnDifficultyEasy { get => btnDifficultyEasy; set => btnDifficultyEasy = value; }
        public Toggle BtnDifficultyHard { get => btnDifficultyHard; set => btnDifficultyHard = value; }
        public Toggle BtnDifficultyNormal { get => btnDifficultyNormal; set => btnDifficultyNormal = value; }
        public Toggle ToggleSounds { get => _toggleSounds; set => _toggleSounds = value; }
        public Toggle ToggleRadar { get => _toggleRadar; set => _toggleRadar = value; }
    }
}

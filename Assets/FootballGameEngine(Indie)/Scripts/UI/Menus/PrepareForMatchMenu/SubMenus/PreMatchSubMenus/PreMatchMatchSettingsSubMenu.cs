using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus
{
    [Serializable]
    public class PreMatchMatchSettingsSubMenu : BSubMenu
    {
        [SerializeField]
        Toggle _toggleIsRadarOn;

        [SerializeField]
        SettingsInfo _difficultySettings;

        [SerializeField]
        SettingsInfo _halfLengthSettings;

        public Toggle ToggleIsRadarOn { get => _toggleIsRadarOn; set => _toggleIsRadarOn = value; }
        public SettingsInfo DifficultySettings { get => _difficultySettings; set => _difficultySettings = value; }
        public SettingsInfo HalfLengthSettings { get => _halfLengthSettings; set => _halfLengthSettings = value; }
    }

    [Serializable]
    public class SettingsInfo
    {
        [SerializeField]
        Toggle _btnFirst;

        [SerializeField]
        Toggle _btnSecond;

        [SerializeField]
        Toggle _btnThird;

        public Toggle BtnFirst { get => _btnFirst; set => _btnFirst = value; }
        public Toggle BtnSecond { get => _btnSecond; set => _btnSecond = value; }
        public Toggle BtnThird { get => _btnThird; set => _btnThird = value; }
    }
}

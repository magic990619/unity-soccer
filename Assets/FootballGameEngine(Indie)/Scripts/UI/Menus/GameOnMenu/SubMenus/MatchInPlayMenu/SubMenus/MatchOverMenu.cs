using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus
{
    [Serializable]
    public class MatchOverMenu : BSubMenu
    {
        [SerializeField]
        Button _btnRestart;

        [SerializeField]
        Button _btnQuit;

        [SerializeField]
        Text _textInfo;

        public Text TextInfo { get => _textInfo; set => _textInfo = value; }
        public Button BtnRestart { get => _btnRestart; set => _btnRestart = value; }
        public Button BtnQuit { get => _btnQuit; set => _btnQuit = value; }
    }
}

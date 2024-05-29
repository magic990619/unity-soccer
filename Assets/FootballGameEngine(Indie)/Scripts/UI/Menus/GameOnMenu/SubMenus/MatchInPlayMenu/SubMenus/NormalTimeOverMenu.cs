using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus
{
    [Serializable]
    public class NormalTimeOverMenu : BSubMenu
    {
        [SerializeField]
        Button _btnContinue;

        [SerializeField]
        Button _btnRestart;

        [SerializeField]
        Button _btnQuit;

        [SerializeField]
        Text _txtHeader;

        [SerializeField]
        Text _txtInfo;

        [SerializeField]
        Text _txtContinueBtn;

        public Button BtnRestart { get => _btnRestart; set => _btnRestart = value; }
        public Button BtnContinueToNormalTime { get => _btnContinue; set => _btnContinue = value; }
        public Button BtnQuit { get => _btnQuit; set => _btnQuit = value; }
        public Text TxtHeader { get => _txtHeader; set => _txtHeader = value; }
        public Text TxtInfo { get => _txtInfo; set => _txtInfo = value; }
        public Text TxtContinueBtn { get => _txtContinueBtn; set => _txtContinueBtn = value; }
    }
}

using SmartMenuManagement.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.HomeMenu.MainMenu
{
    [Serializable]
    public class HomeMainMenu : BMenu
    {
        [SerializeField]
        private Button _btnExit;

        [SerializeField]
        private Button _btnKickOff;

        [SerializeField]
        private Button _btnKickOff2;

        [SerializeField]
        private Button _btnSettings;

        [SerializeField]
        private Button _btnTrophs;

        public Button BtnExit { get => _btnExit; set => _btnExit = value; }
        public Button BtnKickOff { get => _btnKickOff; set => _btnKickOff = value; }
        public Button BtnKickOff2 { get => _btnKickOff2; set => _btnKickOff2 = value; }
        public Button BtnSettings { get => _btnSettings; set => _btnSettings = value; }
        public Button BtnTrophy { get => _btnTrophs; set => _btnTrophs = value; }
    }
}

using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.ExitMenu.MainMenu
{
    [Serializable]
    public class ExitMainMenu : BMenu
    {
        [SerializeField]
        Button _btnCancelQuit;

        [SerializeField]
        Button _btnConfirmQuit;

        public Button BtnCancelQuit { get => _btnCancelQuit; set => _btnCancelQuit = value; }
        public Button BtnConfirmQuit { get => _btnConfirmQuit; set => _btnConfirmQuit = value; }
    }
}

using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.Init.SubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.Init.MainMenu
{
    [Serializable]
    public class InitMainMenu : BMenu
    {
        [SerializeField]
        LoadMenu _loadSubMenu;

        [SerializeField]
        WaitForContinueInstructionMenu _waitForContinueInstructionSubMenu;

        public LoadMenu LoadMenu { get => _loadSubMenu; set => _loadSubMenu = value; }
        public WaitForContinueInstructionMenu WaitForContinueInstruction { get => _waitForContinueInstructionSubMenu; set => _waitForContinueInstructionSubMenu = value; }

        public void DisableAll()
        {
            _loadSubMenu.Root.SetActive(false);
            _waitForContinueInstructionSubMenu.Root.SetActive(false);
        }

        public void DisableLoadSubMenu()
        {
            _loadSubMenu.Root.SetActive(false);
        }

        public void DisableWaitForContinueInstructionSubMenu()
        {
            _waitForContinueInstructionSubMenu.Root.SetActive(false);

        }

        public void EnableLoadSubMenu()
        {
            _loadSubMenu.Root.SetActive(true);
        }

        public void EnableWaitForContinueInstructionSubMenu()
        {
            _waitForContinueInstructionSubMenu.Root.SetActive(true);
        }
    }
}

using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.Init.SubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus
{
    [Serializable]
    public class InitMenu : BSubMenu
    {
        [SerializeField]
        LoadMenu _loadMenu;

        [SerializeField]
        WaitForContinueToMatchOnInstructionMenu _waitForContinueToMatchOnInstructionMenu;

        public LoadMenu LoadMenu { get => _loadMenu; set => _loadMenu = value; }
        public WaitForContinueToMatchOnInstructionMenu WaitForContinueInstructionMenu { get => _waitForContinueToMatchOnInstructionMenu; set => _waitForContinueToMatchOnInstructionMenu = value; }

        public void DisableChildren()
        {
            _loadMenu.Root.SetActive(false);
            _waitForContinueToMatchOnInstructionMenu.Root.SetActive(false);
        }
    }
}

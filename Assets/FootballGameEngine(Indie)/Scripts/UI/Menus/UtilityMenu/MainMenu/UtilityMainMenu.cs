using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.UtilityMenu.MainMenu
{
    [Serializable]
    public class UtilityMainMenu : BMenu
    {
        [SerializeField]
        private Button _btnBack;

        [SerializeField]
        private Button _btnContinue;

        [SerializeField]
        private Text _txtHeading;

        public void Init(bool isBackButtonActive, bool isContinueButtonActive, string heading, UnityAction onClickBackButton = null, UnityAction onClickContinueButton = null)
        {
            // remove any listeners
            _btnBack.onClick.RemoveAllListeners();
            _btnContinue.onClick.RemoveAllListeners();

            // check if we have something
            bool onClickBackButtonFlag = onClickBackButton != null;
            if (onClickBackButtonFlag)
                _btnBack.onClick.AddListener(onClickBackButton);
            
            bool onClickContinueButtonFlag = onClickContinueButton != null;
            if (onClickContinueButtonFlag)
                _btnContinue.onClick.AddListener(onClickContinueButton);
            
            // enable the appropriate menus
            _btnBack.gameObject.SetActive(isBackButtonActive);
            _btnContinue.gameObject.SetActive(isContinueButtonActive);

            // set the header
            _txtHeading.text = heading.ToUpper();
        }
    }
}

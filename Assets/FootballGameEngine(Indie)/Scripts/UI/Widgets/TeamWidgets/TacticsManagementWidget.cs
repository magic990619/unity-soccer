using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.TeamWidgets
{
    [Serializable]
    public class TacticsManagementWidget
    {
        [SerializeField]
        TacticSetting _attackTacticSetting;

        [SerializeField]
        TacticSetting _defenceTacticSetting;

        public void Init(TeamDto teamData, 
            UnityAction onClickNextAttackTactic = null,
            UnityAction onClickNextDefenceTactic = null,
            UnityAction onClickPrevAttackTactic = null,
            UnityAction onClickPrevDefenceTactic = null)
        {
            _attackTacticSetting.BtnNextButton.onClick.RemoveAllListeners();
            _attackTacticSetting.BtnPrevButton.onClick.RemoveAllListeners();

            _defenceTacticSetting.BtnNextButton.onClick.RemoveAllListeners();
            _defenceTacticSetting.BtnPrevButton.onClick.RemoveAllListeners();

            _attackTacticSetting.BtnNextButton.onClick.AddListener(() => { onClickNextAttackTactic?.Invoke(); });
            _attackTacticSetting.BtnPrevButton.onClick.AddListener(() => { onClickPrevAttackTactic?.Invoke(); });

            _defenceTacticSetting.BtnNextButton.onClick.AddListener(() => { onClickNextDefenceTactic?.Invoke(); });
            _defenceTacticSetting.BtnPrevButton.onClick.AddListener(() => { onClickPrevDefenceTactic?.Invoke(); });

            _attackTacticSetting.TextCurrTactic.text = teamData.AttackType.ToString();
            _defenceTacticSetting.TextCurrTactic.text = teamData.DefenceType.ToString();
        }
    }

    [Serializable]
    public class TacticSetting
    {
        [SerializeField]
        Button _btnPrevButton;

        [SerializeField]
        Button _btnNextButton;

        [SerializeField]
        Text _textCurrTactic;

        public Button BtnPrevButton { get => _btnPrevButton; set => _btnPrevButton = value; }
        public Button BtnNextButton { get => _btnNextButton; set => _btnNextButton = value; }
        public Text TextCurrTactic { get => _textCurrTactic; set => _textCurrTactic = value; }
    }
}

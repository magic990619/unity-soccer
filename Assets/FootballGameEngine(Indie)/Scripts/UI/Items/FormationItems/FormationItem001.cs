using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Items.FormationItems
{
    public class FormationItem001 : MonoBehaviour
    {
        [SerializeField]
        Text _formationId;

        [SerializeField]
        Text _formationName;

        [SerializeField]
        Toggle _itemToggle;

        public Text FormationName { get => _formationName; set => _formationName = value; }
        public Toggle ItemToggle { get => _itemToggle; set => _itemToggle = value; }


        public void Init(bool isOn, string formationId, string formationName, ToggleGroup toggleGroup, UnityAction<string> unityAction)
        {
            // init the ui items
            _formationId.text = formationId;
            _formationName.text = formationName;

            // set the item toggle
            _itemToggle.isOn = isOn;
            _itemToggle.group = toggleGroup;

            // if value changes then call the action with the formation id of this item
            _itemToggle.onValueChanged.AddListener((val) =>
            {
                if(val == true)
                {
                    unityAction.Invoke(_formationId.text);
                }
            });

        }
    }
}

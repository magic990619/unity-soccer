
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems
{
    public class PlayerItem002 : MonoBehaviour
    {
        [SerializeField]
        Button _button;

        [SerializeField]
        private GameObject _checkMark;

        [SerializeField]
        private Text _id;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _kitNumber;

        [SerializeField]
        private Text _role;

        public Text Name { get => _name; set => _name = value; }
        public Text Role { get => _role; set => _role = value; }
        public Text KitNumber { get => _kitNumber; set => _kitNumber = value; }
        public GameObject CheckMark { get => _checkMark; set => _checkMark = value; }
        public Button Button { get => _button; set => _button = value; }
        public Text Id { get => _id; set => _id = value; }

        public void Init(int id, string kitNumber, string name, string role)
        {
            _id.text = id.ToString();

            _kitNumber.text = kitNumber;
            _name.text = name;
            _role.text = role;

            _checkMark.gameObject.SetActive(false);
        }
    }
}

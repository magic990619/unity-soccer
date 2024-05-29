using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Tactics;
using Patterns.Singleton;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField]
        List<AttackTactic> _attackTactics;

        [SerializeField]
        List<DefendTactic> _defendTactics;

        [SerializeField]
        private List<TeamDto> _teams;

        public T GetData<T>(string name)
        {
            T result;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream saveFile = File.Open(Application.persistentDataPath + "/Data/" + name + ".binary", FileMode.Open);
                T data = (T)((object)formatter.Deserialize(saveFile));
                saveFile.Close();
                result = data;
            }
            catch
            {
                result = default(T);
            }
            return result;
        }

        public void SaveData<T>(T data, string name)
        {
            bool flag = !Directory.Exists(Application.persistentDataPath + "/Data");
            if (flag)
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Data");
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Create(Application.persistentDataPath + "/Data/" + name + ".binary");
            formatter.Serialize(saveFile, data);
            saveFile.Close();
        }

        public List<TeamDto> Teams
        {
            get
            {
                return this._teams;
            }
        }

        public List<AttackTactic> AttackTactics { get => _attackTactics; set => _attackTactics = value; }
        public List<DefendTactic> DefendTactics { get => _defendTactics; set => _defendTactics = value; }
    }
}

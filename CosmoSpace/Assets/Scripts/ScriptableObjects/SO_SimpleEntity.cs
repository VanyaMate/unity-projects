using System.Collections;
using UnityEngine;

namespace CS.SO
{
    [CreateAssetMenu(fileName = "so_simpleEntity_", menuName = "game/entitys/simple/create", order = 51)]
    public class SO_SimpleEntity : ScriptableObject
    {
        [SerializeField] private SimpleEntityGameObject _prefab;
        [SerializeField] private string _name;
        [SerializeField] private float _maxHp;

        public SimpleEntityGameObject prefab => _prefab;
        public new string name => _name;
        public float maxHp => _maxHp;
    } 
}
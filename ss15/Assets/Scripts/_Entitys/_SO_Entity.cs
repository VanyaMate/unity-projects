using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SS15.Entitys
{
    [CreateAssetMenu(fileName = "so_entity_", menuName = "ss15/entity/create", order = 51)]
    public class _SO_Entity : ScriptableObject
    {
        [Header("Entity props")]
        [SerializeField] private string _name;
        [SerializeField] private string _type;
        [SerializeField] private string _subtype;

        [Header("Textures")]
        [SerializeField] private Sprite _texture;
        [SerializeField] private Sprite _damagedTextures;

        [Header("Health")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private int _armor;
    }
}
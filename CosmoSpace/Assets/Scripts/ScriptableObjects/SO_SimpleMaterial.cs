using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.SO
{
    [CreateAssetMenu(fileName = "so_simpleMaterial_", menuName = "game/material/simple/create", order = 51)]
    public class SO_SimpleMaterial : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _type;
        [SerializeField] private List<string> _subtypes;

        [Header("----------------Common")]
        [SerializeField] private float _weightPerUnit;
        [SerializeField] private Material _texture;
        [SerializeField] private float _maxHp;


        [Header("----------------Hardness")]
        [SerializeField] private bool _h_canBeHardness;
        [SerializeField] private bool _h_canBeDeform;
        [SerializeField] private bool _h_canBeSplit;
        [SerializeField] private float _h_evaporationSpeed;

        [Header("Material")]
        [SerializeField] private CompoundMaterial _h_material;
        [SerializeField] private float _h_alphaMaterial;

        [Header("Fire")]
        [SerializeField] private bool _h_fireResistant;
        [SerializeField] private float _h_fireResist;
        [SerializeField] private float _h_fireStartTemperature;
        [SerializeField] private float _h_fireTemperature;

        [Header("Liquid")]
        [SerializeField] private bool _h_liquidResistant;
        [SerializeField] private float _h_liquidResist;

        [Header("Gas")]
        [SerializeField] private bool _h_gasResistant;
        [SerializeField] private float _h_gasResist;

        [Header("Radiation")]
        [SerializeField] private bool _h_radiationResistant;
        [SerializeField] private float _h_radiationResist;

        [Header("Toxic")]
        [SerializeField] private bool _h_toxicResistant;
        [SerializeField] private float _h_toxicResist;

        [Header("Explosive")]
        [SerializeField] private bool _h_explosive;
        [SerializeField] private float _h_explosiveTemperature;
        [SerializeField] private float _h_explosivePower;

        [Header("Temperature")]
        [SerializeField] private Vector2 _h_stateTemperature;

        [Header("----------------Liquid")]
        [SerializeField] private bool _l_canBeLiquid;
        [SerializeField] private float _l_dryingSpeed;
        [SerializeField] private float _l_evaporationSpeed;

        [Header("Material")]
        [SerializeField] private CompoundMaterial _l_material;
        [SerializeField] private float _l_alphaMaterial;

        [Header("Fire")]
        [SerializeField] private bool _l_fireResistant;
        [SerializeField] private float _l_fireResist;
        [SerializeField] private float _l_fireStartTemperature;
        [SerializeField] private float _l_fireTemperature;

        [Header("Liquid")]
        [SerializeField] private bool _l_liquidResistant;
        [SerializeField] private float _l_liquidResist;

        [Header("Gas")]
        [SerializeField] private bool _l_gasResistant;
        [SerializeField] private float _l_gasResist;

        [Header("Radiation")]
        [SerializeField] private bool _l_radiationResistant;
        [SerializeField] private float _l_radiationResist;

        [Header("Toxic")]
        [SerializeField] private bool _l_toxicResistant;
        [SerializeField] private float _l_toxicResist;

        [Header("Explosive")]
        [SerializeField] private bool _l_explosive;
        [SerializeField] private float _l_explosiveTemperature;
        [SerializeField] private float _l_explosivePower;

        [Header("Temperature")]
        [SerializeField] private Vector2 _l_stateTemperature;

        [Header("----------------Gas")]
        [SerializeField] private bool _g_canBeGas;
        [SerializeField] private float _g_maxLifeValue;

        [Header("Material")]
        [SerializeField] private CompoundMaterial _g_material;
        [SerializeField] private float _g_alphaMaterial;

        [Header("Fire")]
        [SerializeField] private bool _g_fireResistant;
        [SerializeField] private float _g_fireStartTemperature;
        [SerializeField] private float _g_fireTemperature;

        [Header("Toxic")]
        [SerializeField] private bool _g_toxicResistant;
        [SerializeField] private float _g_toxicResist;

        [Header("Explosive")]
        [SerializeField] private bool _g_explosive;
        [SerializeField] private float _g_explosiveTemperature;
        [SerializeField] private float _g_explosivePower;

        [Header("Temperature")]
        [SerializeField] private Vector2 _g_stateTemperature;

        public new string name => _name;
        public string type => _type;
        public List<string> subtypes => _subtypes;


        public float weightPerUnit => _weightPerUnit;
        public Material texture => _texture;
        public float maxHp => _maxHp;

        public bool H_canBeHardness { get => _h_canBeHardness; private set => _h_canBeHardness = value; }
        public bool H_canBeDeform { get => _h_canBeDeform; private set => _h_canBeDeform = value; }
        public bool H_canBeSplit { get => _h_canBeSplit; private set => _h_canBeSplit = value; }
        public CompoundMaterial H_material { get => _h_material; private set => _h_material = value; }
        public float H_alphaMaterial { get => _h_alphaMaterial; private set => _h_alphaMaterial = value; }
        public bool H_fireResistant { get => _h_fireResistant; private set => _h_fireResistant = value; }
        public float H_fireResist { get => _h_fireResist; private set => _h_fireResist = value; }        
        public float H_fireStartTemperature { get => _h_fireStartTemperature; private set => _h_fireStartTemperature = value; }
        public float H_fireTemperature { get => _h_fireTemperature; private set => _h_fireTemperature = value; }
        public bool H_liquidResistant { get => _h_liquidResistant; private set => _h_liquidResistant = value; }
        public float H_liquidResist { get => _h_liquidResist; private set => _h_liquidResist = value; }
        public bool H_gasResistant { get => _h_gasResistant; private set => _h_gasResistant = value; }
        public float H_gasResist { get => _h_gasResist; private set => _h_gasResist = value; }
        public bool H_radiationResistant { get => _h_radiationResistant; private set => _h_radiationResistant = value; }
        public float H_radiationResist { get => _h_radiationResist; private set => _h_radiationResist = value; }
        public bool H_toxicResistant { get => _h_toxicResistant; private set => _h_toxicResistant = value; }
        public float H_toxicResist { get => _h_toxicResist; private set => _h_toxicResist = value; }
        public bool H_explosive { get => _h_explosive; private set => _h_explosive = value; }
        public float H_explosiveTemperature { get => _h_explosiveTemperature; private set => _h_explosiveTemperature = value; }
        public float H_explosivePower { get => _h_explosivePower; private set => _h_explosivePower = value; }
        public Vector2 H_stateTemperature { get => _h_stateTemperature; private set => _h_stateTemperature = value; }
        public bool L_canBeLiquid { get => _l_canBeLiquid; private set => _l_canBeLiquid = value; }
        public float L_dryingSpeed { get => _l_dryingSpeed; private set => _l_dryingSpeed = value; }
        public CompoundMaterial L_material { get => _l_material; private set => _l_material = value; }
        public float L_alphaMaterial { get => _l_alphaMaterial; private set => _l_alphaMaterial = value; }
        public bool L_fireResistant { get => _l_fireResistant; private set => _l_fireResistant = value; }
        public float L_fireResist { get => _l_fireResist; private set => _l_fireResist = value; }
        public float L_fireStartTemperature { get => _l_fireStartTemperature; private set => _l_fireStartTemperature = value; }
        public float L_fireTemperature { get => _l_fireTemperature; private set => _l_fireTemperature = value; }
        public bool L_liquidResistant { get => _l_liquidResistant; private set => _l_liquidResistant = value; }
        public float L_liquidResist { get => _l_liquidResist; private set => _l_liquidResist = value; }
        public bool L_gasResistant { get => _l_gasResistant; private set => _l_gasResistant = value; }
        public float L_gasResist { get => _l_gasResist; private set => _l_gasResist = value; }
        public bool L_radiationResistant { get => _l_radiationResistant; private set => _l_radiationResistant = value; }
        public float L_radiationResist { get => _l_radiationResist; private set => _l_radiationResist = value; }
        public bool L_toxicResistant { get => _l_toxicResistant; private set => _l_toxicResistant = value; }
        public float L_toxicResist { get => _l_toxicResist; private set => _l_toxicResist = value; }
        public bool L_explosive { get => _l_explosive; private set => _l_explosive = value; }
        public float L_explosiveTemperature { get => _l_explosiveTemperature; private set => _l_explosiveTemperature = value; }
        public float L_explosivePower { get => _l_explosivePower; private set => _l_explosivePower = value; }
        public Vector2 L_stateTemperature { get => _l_stateTemperature; private set => _l_stateTemperature = value; }
        public bool G_canBeGas { get => _g_canBeGas; private set => _g_canBeGas = value; }
        public float G_maxLifeValue { get => _g_maxLifeValue; private set => _g_maxLifeValue = value; }
        public CompoundMaterial G_material { get => _g_material; private set => _g_material = value; }
        public float G_alphaMaterial { get => _g_alphaMaterial; private set => _g_alphaMaterial = value; }
        public bool G_fireResistant { get => _g_fireResistant; private set => _g_fireResistant = value; }
        public float G_fireStartTemperature { get => _g_fireStartTemperature; private set => _g_fireStartTemperature = value; }
        public float G_fireTemperature { get => _g_fireTemperature; private set => _g_fireTemperature = value; }
        public bool G_toxicResistant { get => _g_toxicResistant; private set => _g_toxicResistant = value; }
        public float G_toxicResist { get => _g_toxicResist; private set => _g_toxicResist = value; }
        public bool G_explosive { get => _g_explosive; private set => _g_explosive = value; }
        public float G_explosiveTemperature { get => _g_explosiveTemperature; private set => _g_explosiveTemperature = value; }
        public float G_explosivePower { get => _g_explosivePower; private set => _g_explosivePower = value; }
        public Vector2 G_stateTemperature { get => _g_stateTemperature; private set => _g_stateTemperature = value; }
    }
}
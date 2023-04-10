using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeUI : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public BuildingTypeSO BuildingTypeSo;
    public Image Icon => _icon;
}

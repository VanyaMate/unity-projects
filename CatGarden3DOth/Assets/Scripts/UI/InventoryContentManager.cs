using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContentManager : MonoBehaviour
{
    [SerializeField] private Transform _content;

    public Transform Content => _content;
}

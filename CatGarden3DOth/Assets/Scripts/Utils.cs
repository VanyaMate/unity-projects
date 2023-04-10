using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

namespace Common
{
    public static class Utils
    {
        public static RaycastHit GetMouseWorldHit()
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out raycastHit) ? raycastHit : default;
        }

        public static RaycastHit[] GetMouseWorldHits(LayerMask mask, float distance = 100f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, distance, mask);
            return hits.Length != 0 ? hits : default;
        }
    }
}
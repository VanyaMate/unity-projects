using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WG.Point
{
    public class PointInterface : MonoBehaviour
    {
        public void OnHover ()
        {
            transform.DOScale(Vector3.one * 1.5f, .2f);
        }

        public void OnExit ()
        {
            transform.DOScale(Vector3.one, .1f);
        }
    }
}

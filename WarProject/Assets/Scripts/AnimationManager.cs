using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WG.Animation
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}

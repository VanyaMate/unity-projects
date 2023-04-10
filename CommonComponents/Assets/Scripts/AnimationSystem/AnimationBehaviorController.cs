using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.AnimationSystem
{
    [RequireComponent(typeof(Animator))]
    public class AnimationBehaviorController : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            this._animator = GetComponent<Animator>();
        }

        protected void SetValue<T> (string name, T value) where T : IConvertible
        {
            System.Type type = typeof(T);

            if (type == typeof(float))
            {
                this._animator.SetFloat(name, (float)Convert.ChangeType(value, typeof(float)));
            }
            else if (type == typeof(bool))
            {
                this._animator.SetBool(name, (bool)Convert.ChangeType(value, typeof(bool)));
            }
        }

        protected void Trigger (string name)
        {
            this._animator.SetTrigger(name);
        }
    }
}
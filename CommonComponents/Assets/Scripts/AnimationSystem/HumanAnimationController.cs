using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.AnimationSystem
{
    public class HumanAnimationController : AnimationBehaviorController
    {
        [SerializeField] private float _animationMaxValue = 100f;

        public void Move(float horizontalDirection = 0, float verticalDirection = 0, float speed = 0)
        {
            this.SetValue<float>(
                name: "XVelocity",
                value: horizontalDirection * this._animationMaxValue
            );

            this.SetValue<float>(
                name: "ZVelocity",
                value: verticalDirection * this._animationMaxValue
            );

            this.SetValue<float>(
                name: "Speed",
                value: speed
            );
        }

        public void Jump()
        {
            this.Trigger("Jump");
        }

        // from animation
        public void JumpAddForce (float force = 1)
        {
            Debug.Log("Jump Add Force");
        }
    }
}
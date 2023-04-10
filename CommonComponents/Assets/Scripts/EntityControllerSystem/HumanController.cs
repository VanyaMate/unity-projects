using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.AnimationSystem;

namespace VM.EntitySystem
{
    public class HumanController : MonoBehaviour
    {
        protected HumanAnimationController _animationController;

        [Header("Direction")]
        [SerializeField] private Vector2 _direction = Vector2.zero;
        [SerializeField] private float _changeDirectionSpeedTime = .5f;

        [Header("Speed")]
        [SerializeField] private float _changeSpeedTime = .2f;
        [SerializeField] private float _speedChangeAmount = .25f;
        [SerializeField] private float _currentSpeed = 0f;

        // Direction
        private Vector2 _prevDirection = Vector2.zero;
        private float _currentChangesDirectionTime = 0f;

        // Speed
        private float _targetSpeed = 0f;
        private float _prevSpeed = 0f;
        private float _currentChangeSpeedTime = 0f;

        private void Awake()
        {
            this._animationController = GetComponent<HumanAnimationController>();
            this._targetSpeed = this._currentSpeed;
            this._prevSpeed = this._currentSpeed;
        }

        private void Update()
        {
            Vector2 direction = this.GetDirection();
            float speed = this.GetSpeed();
            bool jump = Input.GetKeyDown(KeyCode.Space);

            if (jump)
            {
                this._animationController.Jump();
            }

            this._animationController.Move(
                horizontalDirection: direction.x,
                verticalDirection: direction.y,
                speed: speed
            );

            this._currentSpeed = speed;
            this._direction = direction;
            this._currentChangesDirectionTime += Time.deltaTime;
            this._currentChangeSpeedTime += Time.deltaTime;
        }

        private Vector2 GetDirection ()
        {
            float horizontalSpeed = Input.GetAxisRaw("Horizontal");
            float verticalSpeed = Input.GetAxisRaw("Vertical");

            Vector2 moveDirection = new Vector2(horizontalSpeed, verticalSpeed);

            if (this._prevDirection != moveDirection)
            {
                this._currentChangesDirectionTime = 0;
                this._prevDirection = moveDirection;
            }

            return Vector2.Lerp(this._direction, moveDirection, this._currentChangesDirectionTime / this._changeDirectionSpeedTime);
        }

        private float GetSpeed ()
        {
            float speedChange = Input.mouseScrollDelta.y * this._speedChangeAmount;

            this._targetSpeed += speedChange;

            if (this._targetSpeed > 2)
            {
                this._targetSpeed = 2;
            }
            else if (this._targetSpeed < 0)
            {
                this._targetSpeed = 0;
            }

            if (speedChange != 0)
            {
                this._currentChangeSpeedTime = 0f;
                this._prevSpeed = this._currentSpeed;
            }

            return Mathf.Lerp(this._prevSpeed, this._targetSpeed, this._currentChangeSpeedTime / this._changeSpeedTime);
        }
    }
}
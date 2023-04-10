using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.Managers;

namespace CG.Player
{
    [Serializable]
    class MoveSettings
    {
        public float WalkMoveSpeed;
        public float RunMoveSpeed;
    }

    [RequireComponent(typeof(MoveManager))]
    public class PlayerMoveManager : MonoBehaviour
    {
        [SerializeField] private MoveSettings _moveSettings;
        [SerializeField] private MoveManager _moveManager;
        [SerializeField] private Animator _animator;

        [SerializeField] private bool _run = false;
        [SerializeField] private bool _walk = false;

        private void Awake()
        {
            this._moveManager = GetComponent<MoveManager>();
        }

        private void Start()
        {
            this._moveManager.onMoveEnd.AddListener(() => {
                this.AnimationEnd();
                this._walk = false;
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                this._run = true;
                this._moveManager.Speed = this._moveSettings.RunMoveSpeed;

                if (this._walk)
                {
                    this.AnimationStart("Run");
                }
            }
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                this._run = false;
                this._moveManager.Speed = this._moveSettings.WalkMoveSpeed;

                if (this._walk)
                {
                    this.AnimationStart("Walk");
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                bool pointAvailable = this._moveManager.MoveToPoint(
                    Common.Utils.GetMouseWorldHits(LayerMask.GetMask("Map"))[0].point, 
                    this._run ? 
                        this._moveSettings.RunMoveSpeed : 
                        this._moveSettings.WalkMoveSpeed
                );

                if (pointAvailable)
                {
                    this._walk = true;
                    this.AnimationStart(this._run ? "Run" : "Walk");
                }
            }
            
            Vector3 velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                velocity += new Vector3(1, 0, -1);
            }

            if (Input.GetKey(KeyCode.W))
            {
                velocity += new Vector3(-1, 0, -1);
            }   

            if (Input.GetKey(KeyCode.D))
            {
                velocity += new Vector3(-1, 0, 1);
            }

            if (Input.GetKey(KeyCode.S))
            {
                velocity += new Vector3(1, 0, 1);
            }

            if (velocity != Vector3.zero)
            {
                this._walk = true;
                this.AnimationStart(this._run ? "Run" : "Walk");
                this._moveManager.SetVelocity(
                    velocity.normalized, 
                    this._run ? 
                        this._moveSettings.RunMoveSpeed : 
                        this._moveSettings.WalkMoveSpeed
                );
            }
        }

        private void AnimationStart(string type)
        {
            this.AnimationEnd();
            this._animator.SetBool(type, true);
        }

        private void AnimationEnd()
        {
            this._animator.SetBool("Walk", false);
            this._animator.SetBool("Run", false);
        }
    }
}

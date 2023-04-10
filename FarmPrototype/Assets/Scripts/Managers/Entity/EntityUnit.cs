using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VM.Controller;
using VM.Managers.Road;
using VM.Player;

namespace VM.Managers.EntityTools
{
    [RequireComponent(typeof(CharacterControllerBased))]
    public class EntityUnit : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterControllerBased _cc;
        [SerializeField] private Animator _animator;

        [Header("Props")]
        [SerializeField] private float _speed;
        [SerializeField] private float _checkMovePositionDistance;

        [Header("Moveto props")]
        [SerializeField] private bool _moveToPosition = false;
        [SerializeField] private Transform _moveToObject;

        [Header("Car")]
        [SerializeField] protected RoadCar _car = null;
        [SerializeField] private bool _insideCar;

        private float _currentCheckTimer = 0;

        public bool insideCar
        {
            get => !this._cc.cc.enabled;
            set
            {
                this._cc.cc.enabled = !value;
            }
        }

        public UnityEvent OnStop = new UnityEvent();
        public UnityEvent OnMove = new UnityEvent();

        private void Awake()
        {
            this._cc = GetComponent<CharacterControllerBased>();
            this.insideCar = this._insideCar;

            if (this._car != null)
            {
                this._car.AddEntity(this, false);
            }
        }

        private void Start()
        {
            this.MoveTo(PlayerManager.Instance.transform);
        }

        private void Update()
        {
            if (this._moveToPosition)
            {
                Vector3 direction = (this._moveToObject.position - transform.position).normalized;
                this._cc.Move(direction * this._speed);
                this._cc.RotateTo(direction);

                if (this._currentCheckTimer > this._checkMovePositionDistance)
                {
                    List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, 1.5f));

                    if (colliders.Find(x => x.transform == this._moveToObject) != null)
                    {
                        this.Stop();
                    }

                    this._currentCheckTimer = 0;
                }

                this._currentCheckTimer += Time.deltaTime;
            }
        }

        public void SetCar(RoadCar car, bool inside = false)
        {
            this._car = car;
            this.insideCar = inside;
        }

        public void MoveTo (Transform moveToObject)
        {
            this._moveToPosition = true;
            this._moveToObject = moveToObject;
            this._animator.SetBool("run", true);
            this.OnMove.Invoke();
        }

        public void Stop ()
        {
            this._moveToPosition = false;
            this._animator.SetBool("run", false);
            this._cc.Move(Vector3.zero);
            this.OnStop.Invoke();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    [SerializeField] private BotSideSO _botInfo;
    [SerializeField] private Transform _tower;
    [SerializeField] private Transform _chassis;
    [SerializeField] private Gun _gun;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private GunSO _gunType;
    private float _recheckCooldown = 1f;
    private float _recheckTimer = 0;

    public BotSideSO Info { get => _botInfo; set => _botInfo = value; }
    public Transform Tower => _tower;
    public Transform Chassis => _chassis;
    public Gun Gun => _gun;

    private void Awake()
    {
        this._navMeshAgent = GetComponent<NavMeshAgent>();
        this._gunType = this._gun.GunType;
    }

    private void Update()
    {
        this._recheckTimer += Time.deltaTime;

        if (this._recheckTimer > this._recheckCooldown)
        {
            this._recheckTimer = 0;

            Collider[] targets = this.FindAllTargets();

            foreach (Collider target in targets)
            {
                Debug.Log(target);
                
                if (target.transform.GetComponent<Bot>() && target.transform.GetComponent<Bot>().Info.Name != this._botInfo.Name)
                {
                    this._navMeshAgent.isStopped = true;
                    this._gun.FireTo(target.transform.position);
                    break;
                }
                else if (target.transform.GetComponent<Goal>() != null)
                {
                    this._navMeshAgent.SetDestination(target.transform.position);
                }
            }
        }
    }

    private Collider[] FindAllTargets()
    {
        return Physics.OverlapSphere(transform.position, 2500f, LayerMask.GetMask(new string[] { "Goal", "TankTarget" }));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool _isPlayer;
    [SerializeField] private GunSO _gunType;
    [SerializeField] private Transform _gunTower;
    [SerializeField] private Transform _gunTrunk;
    [SerializeField] private Transform _gunBulletDeparture;

    public bool IsPlayer { get => _isPlayer; set => _isPlayer = value; }
    public GunSO GunType => _gunType;

    private float _coolDown = 0f;

    private void Awake()
    {
        this._isPlayer = false;
    }

    private void Update()
    {
        this._coolDown -= Time.deltaTime;

        if (this._isPlayer == true)
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();
            bool inRange;

            Vector3 direction = this.DirectionOfLaunchForArc(
                mouseWorldPosition,
                this._gunBulletDeparture.position,
                this._gunType.AmmoStartSpeed,
                this._gunType.Mounted,
                out inRange
            );

            float trunkDirection = this.TrunkRotateTo(direction);
            float towerDirection = this.TowerRotateTo(mouseWorldPosition);

            if (Input.GetMouseButtonDown(0))
            { 
                this.FireTo(Utils.GetMouseWorldPosition());
            }
        }
    }

    public void FireTo (Vector3 target)
    {
        Vector3 mouseWorldPosition = target;
        bool inRange;

        Vector3 direction = this.DirectionOfLaunchForArc(
            mouseWorldPosition,
            this._gunBulletDeparture.position,
            this._gunType.AmmoStartSpeed,
            this._gunType.Mounted,
            out inRange
        );

        float trunkDirection = this.TrunkRotateTo(direction);
        float towerDirection = this.TowerRotateTo(mouseWorldPosition);

        if (inRange && this._coolDown < 0)
        {
            Transform bullet = Instantiate(this._gunType.AmmoType.Prefab, this._gunBulletDeparture.position, Quaternion.identity);
            Transform explostion = Instantiate(this._gunType.GunExplostionEffect.transform, this._gunBulletDeparture.position, Quaternion.identity);

            bullet.GetComponent<Rigidbody>().AddForce(direction * this._gunType.AmmoStartSpeed, ForceMode.Impulse);
            explostion.localRotation = Quaternion.Euler(0, towerDirection - 90, trunkDirection);

            this._coolDown = this._gunType.FireRate;
        }
    }

    private float TrunkRotateTo(Vector3 target)
    {
        float deg = Mathf.Atan(target.y) * Mathf.Rad2Deg;
        this._gunTrunk.localRotation = Quaternion.Euler(0, 0, deg);
        return deg;
    }

    private float TowerRotateTo(Vector3 target)
    {
        float deg = Mathf.Atan2(this._gunTower.position.x - target.x, this._gunTower.position.z - target.z) * Mathf.Rad2Deg;
        this._gunTower.rotation = Quaternion.Euler(0, deg + 90, 0);
        return deg;
    }

    private Vector3 DirectionOfLaunchForArc(Vector3 targetPos, Vector3 launcherPos, float startSpeed, bool mounted, out bool inRange)
    {
        Vector3 targetDirection = targetPos - launcherPos;
        targetDirection.y = 0f;
        Quaternion targetDirRot = Quaternion.LookRotation(targetDirection);
        Vector3 targetLocalPos = Quaternion.Inverse(targetDirRot) * (targetPos - launcherPos);
        targetLocalPos.z = Mathf.Abs(targetLocalPos.z);

        float x = targetLocalPos.z;
        float y = targetLocalPos.y;

        float v = startSpeed;

        const float g = 9.81f;

        float ang;

        float root = Mathf.Sqrt(v * v * v * v - g * (g * (x * x) + 2 * y * (v * v)));
        if (root > 0)
        {
            float upP;
            if (mounted)
            {
                upP = v * v + root;
            }
            else
            {
                upP = v * v - root;
            }
            float dnP = g * x;
            float divRes = upP / dnP;
            float theta = Mathf.Atan(divRes);
            ang = theta * Mathf.Rad2Deg;
            inRange = true;
        }
        else
        {
            ang = 45f;
            inRange = false;
        }

        Vector3 laubchDir = targetDirRot * Quaternion.Euler(-ang, 0, 0) * Vector3.forward;
        return laubchDir;
    }
}

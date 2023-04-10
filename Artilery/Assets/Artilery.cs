using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artilery : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _targetPos;
    [SerializeField] private float _startSpeed;
    [SerializeField] private bool _mounted;

    [SerializeField] private float _upTime;
    [SerializeField] private float _currentUpTime;

    [SerializeField] private Transform _bullet;
    [SerializeField] private bool _autoFire;

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Map")
                {
                    bool inRange;
                    Vector3 direction = this.DirectionOfLaunchForArc(hit.point, this._startPosition.transform.position, this._startSpeed, this._mounted, out inRange);

                    Transform bullet = Instantiate(this._bullet, this._startPosition.transform.position, Quaternion.Euler(direction.x, direction.y, direction.z));

                    bullet.GetComponent<Rigidbody>().AddForce(direction * this._startSpeed, ForceMode.Impulse);
                }
            }
        }

        this._currentUpTime += Time.deltaTime;

        if (this._currentUpTime > this._upTime)
        {
            Vector3 target = Vector3.zero;
            float minDis = 100;
            bool targetFinded = false;
            
            Collider[] targets = Physics.OverlapSphere(transform.position, 50f);
            foreach (Collider targ in targets)
            {
                if (targ.tag == "Actor")
                {
                    float dist = Mathf.Sqrt(Mathf.Pow(transform.position.x - targ.transform.position.x, 2) + Mathf.Pow(transform.position.z - targ.transform.position.z, 2));
                    Debug.Log("CheckDist: " + dist + " from " + targ.name);

                    if (dist < minDis)
                    {
                        minDis = dist;
                        target = targ.transform.position;
                        targetFinded = true;
                    }
                }
            }

            if (targetFinded && this._autoFire)
            { 

                bool inRange;
                Vector3 direction = this.DirectionOfLaunchForArc(target, transform.position, this._startSpeed, this._mounted, out inRange);

                Debug.Log(direction);
                Debug.Log(inRange);

                if (inRange)
                {
                    Transform bullet = Instantiate(this._bullet, transform.position, Quaternion.Euler(direction.x, direction.y, direction.z));

                    bullet.GetComponent<Rigidbody>().AddForce(direction * this._startSpeed, ForceMode.Impulse);

                    this._currentUpTime = 0;
                }
            }
        }
    }

    private Vector3 DirectionOfLaunchForArc(Vector3 targetPos, Vector3 launcherPos, float startSpeed, bool mounted, out bool inRange)
    {
        Vector3 targetDirection = targetPos - launcherPos;
        targetDirection.y = 0f;
        Quaternion targetDirRot = Quaternion.LookRotation(targetDirection);
        Vector3 targetLocalPos = Quaternion.Inverse(targetDirRot) * (targetPos - launcherPos);
        targetLocalPos.z = Mathf.Abs(targetLocalPos.z);
        Debug.DrawRay(launcherPos, targetLocalPos);

        float x = targetLocalPos.z;
        float y = targetLocalPos.y;

        float v = startSpeed;

        const float g = 3f;

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
        Debug.DrawRay(launcherPos, laubchDir, Color.green);
        Debug.DrawRay(launcherPos, targetDirRot * Vector3.forward, Color.red);
        return laubchDir;
    }
}

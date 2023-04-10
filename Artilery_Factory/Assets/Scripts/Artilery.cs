using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class ArtilerySettings
{
    public Transform Launcher;
    public float StartSpeed;
    public bool Mounted;
}

public class Artilery : MonoBehaviour
{
    [SerializeField] private bool _isPlayer;
    [SerializeField] private Transform _bullet;
    [SerializeField] private ArtilerySettings _artSettings;

    private void Update()
    {
        if (this._isPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool inRange;

                Transform bullet = Instantiate(this._bullet, this._artSettings.Launcher.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(
                    this.DirectionOfLaunchForArc(
                        Utils.GetMouseWorldPosition(),
                        this._artSettings.Launcher.position,
                        this._artSettings.StartSpeed,
                        this._artSettings.Mounted,
                        out inRange
                    ) * this._artSettings.StartSpeed,
                    ForceMode.Impulse
                );
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
        Debug.DrawRay(launcherPos, laubchDir, Color.green);
        Debug.DrawRay(launcherPos, targetDirRot * Vector3.forward, Color.red);
        return laubchDir;
    }
}

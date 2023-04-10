using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationsEffect : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _stepClips;
    [SerializeField] private GameObject _stepEffect;

    public void PlayStep ()
    {
        AudioSource.PlayClipAtPoint(
            clip: this._stepClips[Random.Range(0, this._stepClips.Count)], 
            position: transform.position
        );

        Instantiate(this._stepEffect, transform.position, Quaternion.identity);
    }
}

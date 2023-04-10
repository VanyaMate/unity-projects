using FB.Score;
using FB.Spawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FB.Player
{
    public class PlayerTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // For Bonus
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // For score
            ScoreManager.Instance.AddPoint(1);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // For die
            MapManager.Instance.StopGame();
        }
    }
}
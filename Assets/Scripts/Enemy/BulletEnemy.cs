using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = Vector2.down * (speed + EnemyMove.speed);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player")
        {
            if (!PlayerPrefs.HasKey("TopScore") || PlayerPrefs.GetInt("TopScore") < Spawner.score)
            {
                PlayerPrefs.SetInt("TopScore", Spawner.score);
                Debug.Log(PlayerPrefs.GetInt("TopScore"));
            }

            Debug.Log("Lose");
            Destroy(gameObject);
        }
    }
}

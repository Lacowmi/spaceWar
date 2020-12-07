using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animEnemy;
    public AudioSource explosion;
    public AudioSource ricochet;
    private bool isEnemyDestroying = false;
    private int hp = Weapon.damage;
    private int cost = 5;

    private void Start()
    {
        if(Spawner.waveIndex > 15)
        {
            cost = 10;
            hp = 2 * Weapon.damage;
        }
        else if(Spawner.waveIndex > 30)
        {
            cost = 20;
            hp = 3 * Weapon.damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D hittedObject)
    {
        if (hittedObject.name == "Player" && !isEnemyDestroying)
        {
            if(!PlayerPrefs.HasKey("TopScore") || PlayerPrefs.GetInt("TopScore") < Spawner.score)
            {
                PlayerPrefs.SetInt("TopScore", Spawner.score);
                Debug.Log(PlayerPrefs.GetInt("TopScore"));
            }

            Debug.Log("Game over");
        }
        else if (hittedObject.gameObject.CompareTag("PlayerBullet") && !isEnemyDestroying)
        {
            Destroy(hittedObject.gameObject);

            ricochet.Play();
            hp -= Weapon.damage;

            if(hp <= 0)
            {
                isEnemyDestroying = true;
                Spawner.score += cost;

                animEnemy.cullingMode = AnimatorCullingMode.CullCompletely;
                animEnemy.SetTrigger("isExplosion");
                explosion.Play();

                Destroy(gameObject, 0.6f);
            }
        }
        else if (hittedObject.tag == "DeathZone")
        {
            Destroy(gameObject);
        }
    }
}

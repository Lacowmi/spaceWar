using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animMeteor;
    public AudioSource explosion;
    public AudioSource ricochet;
    bool isMeteorDestroying = false;
    Vector2 direction;
    private float meteorSpeed = 1f;
    private int hp = Weapon.damage;

    void Start()
    {
        if (Spawner.waveIndex > 15)
        {
            meteorSpeed = Random.Range(3f, 4f);
            hp = 2 * Weapon.damage;
        }
        else
        {
            meteorSpeed = Random.Range(2f, 3f);

        }


        if (transform.transform.position.x > 0)
        {
            direction = new Vector2(Random.Range(-3f, -1f), -1);
        }
        else
        {
            direction = new Vector2(Random.Range(1f, 3f), -1); ;
        }


        if (Spawner.waveIndex < 20)
        {
            rb.velocity = direction.normalized * meteorSpeed;
        }
        else
        {
            rb.velocity = Vector2.down * meteorSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D hittedObject)
    {
        if (hittedObject.name == "Player" && !isMeteorDestroying)
        {
            if (!PlayerPrefs.HasKey("TopScore") || PlayerPrefs.GetInt("TopScore") < Spawner.score)
            {
                PlayerPrefs.SetInt("TopScore", Spawner.score);
                Debug.Log(PlayerPrefs.GetInt("TopScore"));
            }

            Debug.Log("Game over");
        }
        else if (hittedObject.gameObject.CompareTag("PlayerBullet") && !isMeteorDestroying)
        {
            Destroy(hittedObject.gameObject);
            ricochet.Play();
            hp -= Weapon.damage;

            if(hp <= 0)
            {
                isMeteorDestroying = true;

                animMeteor.cullingMode = AnimatorCullingMode.CullCompletely;
                explosion.Play();
                animMeteor.Play("Explosion");

                Destroy(gameObject, 0.6f);
            }
        }
        else if (hittedObject.tag == "DeathZone")
        {
            Destroy(gameObject);
        }
    }
}

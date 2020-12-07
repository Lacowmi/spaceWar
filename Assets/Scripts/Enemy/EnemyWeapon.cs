using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private bool abilityToShoot;
    public Transform firePointLeft;
    public Transform firePointRight;
    public GameObject bullet;
    public AudioSource shootSound;
    public AudioSource explosion;
    private DateTime timeShoot = DateTime.Now;
    private float intervalOfShoots;
    private bool onView = false;

    void Start()
    {
        AbilityToShoot(Spawner.waveIndex);
    }

    void Update()
    {
        if(abilityToShoot)
            Attack();
    }

    private void AbilityToShoot(int wave)
    {
        int chance = UnityEngine.Random.Range(1, 100);
        if (chance <= 20 + wave * 2)
        {
            abilityToShoot = true;
        }
        else
        {
            abilityToShoot = false;
        }
    }

    private void BulletCreation()
    {
        shootSound.Play();

        Instantiate(bullet, firePointLeft.position, firePointLeft.rotation);
        Instantiate(bullet, firePointRight.position, firePointRight.rotation);
    }


    private void Attack()
    {

        intervalOfShoots = UnityEngine.Random.Range(1f,2.5f);
        GameObject player = GameObject.Find("Player");

        TimeSpan difference = DateTime.Now - timeShoot;

        if (transform.position.x - player.transform.position.x >= -1f && transform.position.x - player.transform.position.x <= 1f && player.transform.position.y < transform.position.y)
        {
            if (difference.TotalSeconds > intervalOfShoots && !explosion.isPlaying && onView)
            {
                timeShoot = DateTime.Now;
                Invoke("BulletCreation", 0.3f);
            }
        }
    }

    private void OnBecameVisible()
    {
        onView = true;
    }
}

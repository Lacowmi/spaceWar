using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 5f;
    public AudioSource shootSound;
    public AudioSource explosion;
    private float intervalOfShoots;
    private DateTime timeShoot = DateTime.Now;
    private bool onView = false;


    void Start()
    {
        intervalOfShoots = UnityEngine.Random.Range(1f, 2.5f);
    }

    void Update()
    {
        Attack();
    }

    private void BulletCreation()
    {
        shootSound.Play();

        Instantiate(bullet, GameObject.Find("BossFirePointLeft").transform.position, GameObject.Find("BossFirePointLeft").transform.rotation);
        Instantiate(bullet, GameObject.Find("BossFirePointRight").transform.position, GameObject.Find("BossFirePointRight").transform.rotation);
    }


    private void Attack()
    {
        GameObject player = GameObject.Find("Player");

        TimeSpan difference = DateTime.Now - timeShoot;

        if (CheckObjectInView(player))
        {
            if (difference.TotalSeconds > intervalOfShoots && !explosion.isPlaying && onView)
            {
                timeShoot = DateTime.Now;
                BulletCreation();
            }
        }
    }

    private bool CheckObjectInView(GameObject obj)
    {
        if (transform.position.x - obj.transform.position.x >= -3f && transform.position.x - obj.transform.position.x <= 3f && obj.transform.position.y < transform.position.y)
            return true;
        return false;
    }

    private void OnBecameVisible()
    {
        onView = true;
    }
}

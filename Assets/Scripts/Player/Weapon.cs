using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static int damage = 5;
    public Transform firePointLeft;
    public Transform firePointRight;
    public GameObject bullet;
    public AudioSource shootSound;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GameObject.Find("Complete") == null)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootSound.Play();

        Instantiate(bullet, firePointLeft.position, firePointLeft.rotation);
        Instantiate(bullet, firePointRight.position, firePointRight.rotation);
    }
}

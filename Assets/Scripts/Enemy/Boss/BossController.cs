using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public static bool isBossDestroying = false;
    public Animator bossAnim;
    public AudioSource explosion;
    public AudioSource ricochet;
    public Image barFill;
    public GameObject bar;
    private int hp = 500;
    private int cost = 75;

    private void Start()
    {
        if(Spawner.waveIndex > 15)
        {
            cost = 100;
        }    
    }

    public void Update()
    {
        barFill.fillAmount = (float)hp / 500;
    }

    private void OnTriggerEnter2D(Collider2D hittedObject)
    {
        if (hittedObject.name == "Player" && !isBossDestroying)
        {
            if (!PlayerPrefs.HasKey("TopScore") || PlayerPrefs.GetInt("TopScore") < Spawner.score)
            {
                PlayerPrefs.SetInt("TopScore", Spawner.score);
                Debug.Log(PlayerPrefs.GetInt("TopScore"));
            }

            Debug.Log("Game over");
        }
        else if (hittedObject.gameObject.CompareTag("PlayerBullet") && !isBossDestroying)
        {
            Destroy(hittedObject.gameObject);

            ricochet.Play();
            hp -= Weapon.damage;
            ShowBar();

            if (hp <= 0)
            {
                HideBar();
                isBossDestroying = true;
                Spawner.score += cost;

                gameObject.transform.localScale = new Vector3(1, 1, 1);

                bossAnim.cullingMode = AnimatorCullingMode.CullCompletely;
                explosion.Play();
                bossAnim.Play("BossExplosion");

                Destroy(gameObject, 0.8f);
            }
        }
        else if (hittedObject.tag == "DeathZone")
        {
            Destroy(gameObject);
        }
    }

    private void ShowBar()
    {
        if(bar.active == false)
        {
            bar.SetActive(true);
            Invoke("HideBar", 1f);
        }
    }

    private void HideBar()
    {
        bar.SetActive(false);
    }
}

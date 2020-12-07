using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static bool coop = false;
    public GameObject mercury, venus, sun;
    public GameObject mainMenu, options;
    public AudioSource click;
    public AudioSource music;
    public Toggle isMusic;
    private GameObject volSlider;
    private float musicVolume;
    private float musicFirstVolume = 0.5f;

    void Start()
    {
        //score
        if (PlayerPrefs.HasKey("TopScore"))
            PrintScore();

        //music check saves
        if (PlayerPrefs.HasKey("IsMusicOn"))
        {
            isMusic.isOn = Convert.ToBoolean(PlayerPrefs.GetString("IsMusicOn"));
            music.enabled = isMusic.isOn;
        }
        else
        {
            music.enabled = true;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            music.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
            music.volume = musicFirstVolume;
    }

    void Update()
    {
        mercury.transform.RotateAround(sun.transform.position, new Vector3(0, 0, 1), Time.deltaTime * 23f);
        venus.transform.RotateAround(sun.transform.position, new Vector3(0, 0, 1), Time.deltaTime * 15f);

        mercury.transform.GetChild(0).transform.RotateAround(mercury.transform.GetChild(0).transform.position, new Vector3(0, 0, -1), Time.deltaTime * 23f);
        venus.transform.GetChild(0).transform.RotateAround(venus.transform.GetChild(0).transform.position, new Vector3(0, 0, -1), Time.deltaTime * 15f);
    }

    private void PrintScore()
    {
        if (PlayerPrefs.HasKey("TopScore"))
        {
            if (PlayerPrefs.GetInt("TopScore") > 9999)
            {
                GameObject.Find("BestScore").transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("TopScore").ToString();
            }
            else if (PlayerPrefs.GetInt("TopScore") > 999)
            {
                GameObject.Find("BestScore").transform.GetChild(0).GetComponent<Text>().text = "0" + PlayerPrefs.GetInt("TopScore").ToString();
            }
            else if (PlayerPrefs.GetInt("TopScore") > 99)
            {
                GameObject.Find("BestScore").transform.GetChild(0).GetComponent<Text>().text = "00" + PlayerPrefs.GetInt("TopScore").ToString();
            }
            else if (PlayerPrefs.GetInt("TopScore") > 9)
            {
                GameObject.Find("BestScore").transform.GetChild(0).GetComponent<Text>().text = "000" + PlayerPrefs.GetInt("TopScore").ToString();
            }
            else
                GameObject.Find("BestScore").transform.GetChild(0).GetComponent<Text>().text = "0000" + PlayerPrefs.GetInt("TopScore").ToString();
        }
    }
    public void Controller(Button btn)
    {
        switch (btn.name)
        {
            case "Sun":
                Application.LoadLevel("Game");
                break;
            case "Mercury":
                Application.LoadLevel("Game");
                coop = true;
                break;
            case "Venus":
                options.SetActive(true);
                mainMenu.SetActive(false);

                volSlider = GameObject.Find("MusicVolume");
                volSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume") * 2;
                break;
            case "BackBtn":
                options.SetActive(false);
                mainMenu.SetActive(true);
                break;
        }
        click.Play();
    }

    public void TurnMusic()
    {
        music.enabled = !music.enabled;

        PlayerPrefs.SetString("IsMusicOn", music.enabled.ToString());
    }

    public void SetVolume()
    {
        musicVolume = volSlider.GetComponent<Slider>().value / 2;
        music.volume = musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }
}

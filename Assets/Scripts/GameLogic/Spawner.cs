using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class Spawner : MonoBehaviour
{
    public Text waveTextUI;
    public GameObject meteor;
    public GameObject leftSpawnPoint, rightSpawnPoint;
    public GameObject enemy;
    public GameObject boss;
    public GameObject wavePanel;
    public GameObject completePanel;
    public GameObject scorePanel;
    public static int waveIndex;
    public static  Queue<int> typesOfWaves = new Queue<int>();        // 1-array,2-array,3-diagonal,4-solo,5-meteors
    public static int score = 0;
    private Camera camera;
    private Vector3 enemyWidth;
    private Vector3 endsOfWorld;
    private int arrayWaveSize;
    private int countOfDiagonalLines = 6;
    private float timeOfSpawningMeteors = 20, timeBtwMeteors = 1f;
    private float timeOfSpawningSolo = 10, timeBtwSolo = 0.8f;
    private int countOfSoloInLine = 1;
    private float intervalBtwDiagonal = 3f;
    private float distanceBtwnEnemies;
    private bool waveState = false;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }


    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        endsOfWorld = camera.ScreenToWorldPoint(new UnityEngine.Vector3(0, 0, 0));

        enemyWidth = enemy.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().bounds.size;
        arrayWaveSize = Convert.ToInt32(-endsOfWorld.x / enemyWidth.x);
        distanceBtwnEnemies = (-endsOfWorld.x / (arrayWaveSize + 1)) * 2;

        typesOfWaves.Enqueue(1);
        typesOfWaves.Enqueue(2);
        typesOfWaves.Enqueue(3);
        typesOfWaves.Enqueue(4);
        typesOfWaves.Enqueue(5);

        waveIndex = 1;
        SpawnEnemy();
    }

    void Update()
    {
        waveTextUI.text = waveIndex + " Wave";
        PrintScore();

        if (!IsEnemyAlive() && !waveState)
        {
            completePanel.SetActive(true);
            wavePanel.SetActive(false);
            scorePanel.SetActive(false);
        }

        if(waveIndex > 20)
        {
            countOfDiagonalLines = 10;
        }

        if (completePanel.active && Input.GetKeyDown("space"))
        {
            waveIndex++;
            completePanel.SetActive(false);
            wavePanel.SetActive(true);
            scorePanel.SetActive(true);
            SpawnEnemy();
        }
    }

    private void PrintScore()
    {
        if (score > 9999)
        {
            scorePanel.transform.GetChild(0).GetComponent<Text>().text = score.ToString();
        }
        else if (score > 999)
        {
            scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Score: 0" + score.ToString();
        }
        else if (score > 99)
        {
            scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Score: 00" + score.ToString();
        }
        else if (score > 9)
        {
            scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Score: 000" + score.ToString();
        }   else
            scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Score: 0000" + score.ToString();

    }

    private void SpawnEnemy()
    {
        float xSpawn = endsOfWorld.x + distanceBtwnEnemies;
        float ySpawn = -endsOfWorld.y - 1f;


        if (waveIndex % 10 == 0)   //boss
        {
            Vector2 spawnPoint = Camera.main.ScreenToWorldPoint(new UnityEngine.Vector3(0, 0, 0));
            spawnPoint.x = 0;
            spawnPoint.y = -spawnPoint.y + 5;

            Instantiate(boss,spawnPoint, Quaternion.identity);
        }
        else if(typesOfWaves.Peek() == 1 || typesOfWaves.Peek() == 2)//--------------------array
        {
            ySpawn += 5.5f;
            GameObject[,] arrayWave = new GameObject[arrayWaveSize, 4];

            for (int i = 0; i < arrayWaveSize; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arrayWave[i, j] = enemy;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < arrayWaveSize; j++)
                {
                    Instantiate(arrayWave[j, i], new Vector2(xSpawn, ySpawn), Quaternion.identity);
                    xSpawn += distanceBtwnEnemies;
                }
                xSpawn = endsOfWorld.x + distanceBtwnEnemies;
                ySpawn -= 1.2f;
            }

            GameObject[] waveArray = GameObject.FindGameObjectsWithTag("EnemyParent");
            GameObject.Find("ArrayWave").transform.position = new Vector3(0,10,0);

            foreach (GameObject gm in waveArray)
            {
                gm.transform.parent = GameObject.Find("ArrayWave").transform;
            }

            GameObject.Find("ArrayWave").GetComponent<Animation>().Stop();
            GameObject.Find("ArrayWave").GetComponent<Animation>().Play();
        }
        else if (typesOfWaves.Peek() == 3)//--------------------------------------------diagonal
        {
            WaveStateChecker();
            Invoke("WaveStateChecker", intervalBtwDiagonal * countOfDiagonalLines);

            for (int i = 0; i < countOfDiagonalLines; i++)
            {
                Invoke("SpawnDiagonal", i * intervalBtwDiagonal);
            }

            if(waveIndex % 10 == 0 && waveIndex < 41){
                countOfDiagonalLines++;
            }
        }
        else if (typesOfWaves.Peek() == 4)//-----------------------------------------------solo
        {
            WaveStateChecker();
            Invoke("WaveStateChecker", timeOfSpawningSolo);

            SpawnSoloEnemy();

            if (waveIndex % 10 == 0 && waveIndex < 41)
            {
                countOfSoloInLine++;
                if (timeBtwSolo >= 0.6f)
                    timeBtwSolo -= 0.1f;
            }

            timeOfSpawningSolo += 2f;
        }
        else if (typesOfWaves.Peek() == 5)//--------------------------------------------meteors
        {
            WaveStateChecker();
            Invoke("WaveStateChecker", timeOfSpawningMeteors);

            MeteorSpawn();

            timeOfSpawningMeteors += 2f;
            if (timeBtwMeteors >= 0.5f)
                timeBtwMeteors -= 0.1f;
        }
        typesOfWaves.Enqueue(typesOfWaves.Dequeue());   //throw back wave type
    }

    private void WaveStateChecker()
    {
        waveState = !waveState;
    }

    private Vector2 MeteorCoordinates(bool side)   //1-left | 2-right
    {
        float x, y;

        if(waveIndex < 20)                         //random vector
        {
            x = -endsOfWorld.x + 2;
            y = UnityEngine.Random.Range(-3, -endsOfWorld.y + 3);

            if (side)
                x = -x;
        }
        else                                    //down vector
        {
            x = UnityEngine.Random.Range(endsOfWorld.x + enemyWidth.x, -endsOfWorld.x - enemyWidth.x);
            y = -endsOfWorld.y + 2;
        }

        return new Vector2(x,y);
    }

    private void MeteorSpawn()
    {
        if (waveState)
        {
            if (waveIndex < 20)
            {
                Instantiate(meteor, MeteorCoordinates(true), Quaternion.identity);
                Instantiate(meteor, MeteorCoordinates(false), Quaternion.identity);
            }
            else
            {
                Instantiate(meteor, MeteorCoordinates(false), Quaternion.identity);
            }
            Invoke("MeteorSpawn", timeBtwMeteors);
        }
    }

    private bool IsEnemyAlive()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null && GameObject.FindGameObjectWithTag("Meteor") == null)
        {
            return false;
        }
        return true;
    }

    private void SpawnDiagonal()
    {
        float xSpawn = endsOfWorld.x + distanceBtwnEnemies / 2;
        float ySpawn = -endsOfWorld.y + 1f;
        int sideSpawn = UnityEngine.Random.Range(0, 4);

        for (int j = 1; j <= arrayWaveSize; j++)
        {
            if (sideSpawn > 2)
            {
                Instantiate(enemy, new Vector2(xSpawn + j * distanceBtwnEnemies, ySpawn + j * 0.4f), Quaternion.identity);
            }
            else
            {
                Instantiate(enemy, new Vector2(-xSpawn - j * distanceBtwnEnemies, ySpawn + j * 0.4f), Quaternion.identity);
            }
        }
    }

    private void SpawnSoloEnemy()
    {
        float ySpawn = -endsOfWorld.y + 2f;

        if (waveState)
        {
            for (int i = 0; i < countOfSoloInLine; i++)
            {
                Instantiate(enemy, new Vector2(UnityEngine.Random.Range(endsOfWorld.x + enemyWidth.x, -endsOfWorld.x - enemyWidth.x), ySpawn), Quaternion.identity);
            }

            Invoke("SpawnSoloEnemy", timeBtwSolo);
        }
    }
}

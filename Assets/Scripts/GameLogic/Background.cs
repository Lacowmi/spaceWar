using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Camera camera;
    public GameObject leftWall, rightWall, leftSpawnPoint, rightSpawnPoint, floor;
    public GameObject bossDownWall, bossUpWall, bossLeftWall, bossRightWall;

    void Start()
    {
        SettingBorders();
    }

    void FixedUpdate()
    {

    }

    void SettingBorders()
    {
        var endOfCamera = camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));

        bossUpWall.transform.position = new Vector2(bossUpWall.transform.position.x,-endOfCamera.y + 0.5f);
        bossDownWall.transform.position = new Vector2(bossDownWall.transform.position.x,-2);

        leftWall.transform.position = new Vector2(endOfCamera.x - 0.5f,0);            //выравнивание стенок
        rightWall.transform.position = new Vector2(-endOfCamera.x + 0.5f, 0);

        bossLeftWall.transform.position = leftWall.transform.position;
        bossRightWall.transform.position = rightWall.transform.position;

        floor.transform.position = new Vector2(0, endOfCamera.y - 4);

        rightSpawnPoint.transform.position = new Vector2(-endOfCamera.x - 1.5f, -endOfCamera.y + 5);
        leftSpawnPoint.transform.position = new Vector2(endOfCamera.x + 1.5f, -endOfCamera.y + 5);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public GameObject Player;
    private Vector3 targetplayer;
    public float direccion;
    public float smoothed;

    void Update()
    {
        targetplayer = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetplayer, smoothed * Time.deltaTime);
    }
}

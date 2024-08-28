using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private TextMeshProUGUI TextLife;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private float timeSpeedFactor = 2.0f;
    private float time;
    private bool isColliding = false;

    private void Update()
    {
        time += Time.deltaTime * timeSpeedFactor;
        TimerText.text = "Time " + time.ToString("F2");
    }
    public void ChangeColorBlue()
    {
        if (!isColliding)
        {
            Player.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
    public void ChangerColorRed()
    {
        if(!isColliding)
        {
            Player.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void ChangerColorYellow()
    {
        if (!isColliding)
        {
            Player.GetComponent<SpriteRenderer>().color = Color.yellow;
        }      
    }
    public void UpdateLifePlayer(int life)
    {
        TextLife.text = "x" + life;
    }
}

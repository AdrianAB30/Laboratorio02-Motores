using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject Player;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject pauseButon;
    [SerializeField] private GameObject menuPause;
    [SerializeField] private GameObject resultsPanel; 
    [SerializeField] private TMP_Text resultsText; 
    [SerializeField] private TMP_Text finalTimeText;
    [SerializeField] private TMP_Text TextScore;

    [Header("Time Elements")]
    [SerializeField] private float timeSpeedFactor = 2.0f;
    [SerializeField] private float shakeDuration = 0.5f;  
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float shakeInterval = 1.0f;

    private float time;
    private float shakeTimer;
    private float shakeDurationTimer;
    private Vector3 originalCameraPos;
    private bool isShaking = false;
    private bool isPaused = false;
    private bool gameEnded = false;

    private void OnEnable()
    {
        Player_Controller.OnChangedScore += UpdateScore;
        Player_Controller.OnPlayerWin += HandlePlayerWin;
        Player_Controller.OnPlayerLoose += HandlePlayerLoose;
    }
    private void OnDisable()
    {
        Player_Controller.OnChangedScore -= UpdateScore;
        Player_Controller.OnPlayerWin += HandlePlayerWin;
        Player_Controller.OnPlayerLoose += HandlePlayerLoose;
    }
    private void Start()
    {
        if (mainCamera != null)
        {
            originalCameraPos = mainCamera.transform.localPosition;
        }
        shakeTimer = shakeInterval; 
        shakeDurationTimer = shakeDuration; 
    }

    private void Update()
    {
        if (TimerText != null)
        {
            time += Time.deltaTime * timeSpeedFactor;
            TimerText.text = "Time " + time.ToString("F2");
        }
    
        if (mainCamera != null)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0 && !isShaking)
            {
                isShaking = true;
                shakeDurationTimer = shakeDuration; 
            }

            if (isShaking)
            {
                shakeDurationTimer -= Time.deltaTime;

                if (shakeDurationTimer > 0)
                {
                    float x = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
                    float y = UnityEngine.Random.Range(-1f, 1f) * shakeMagnitude;
                    mainCamera.transform.localPosition = new Vector3(x, y, originalCameraPos.z);
                }
                else
                {
                    mainCamera.transform.localPosition = originalCameraPos;
                    isShaking = false;
                    shakeTimer = shakeInterval;
                }
            }
        }
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if(pauseButon != null)
        {
            pauseButon.SetActive(false);
        }
        if(menuPause != null)
        {
            menuPause.SetActive(true);
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if(pauseButon!= null)
        {
            pauseButon.SetActive(true);
        }
        if(menuPause != null)
        {
            menuPause.SetActive(false);
        }
    }
    public void Restart(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void QuitAplication()
    {
        Application.Quit();
        Debug.Log("Saliendo del Programa");
    }
    public void EndLevel(bool hasWon)
    {
        gameEnded = true;
        Time.timeScale = 0f;

        if (hasWon)
        {
            resultsText.text = "GANASTE EL NIVEL";
        }
        else
        {
            resultsText.text = "PERDISTE";
        }

        finalTimeText.text = TimerText.text;

        if(resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }
    }
    public void HandlePlayerWin()
    {
        EndLevel(true);
        Debug.Log("Evento de victoria recibido en GameManager");
    }
    public void HandlePlayerLoose()
    {
        EndLevel(false);
        Debug.Log("Evento de derrota recibido en GameManager");
    }
    public void ChangeColorBlue()
    {
        if (Player != null)
        {
            Player_Controller player = Player.GetComponent<Player_Controller>();
            if (player != null && !player.isColliding)
            {
                Player.GetComponent<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    public void ChangerColorRed()
    {
        if (Player != null)
        {
            Player_Controller player = Player.GetComponent<Player_Controller>();
            if (player != null && !player.isColliding)
            {
                Player.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    public void ChangerColorYellow()
    {
        if (Player != null)
        {
            Player_Controller player = Player.GetComponent<Player_Controller>();
            if (player != null && !player.isColliding)
            {
                Player.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
    public void UpdateScore(int score)
    {
        if (TextScore != null)
        {
            TextScore.text = "Puntaje: " + score;
        }
    }
}
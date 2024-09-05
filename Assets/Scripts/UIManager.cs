using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text TextLife;
    [SerializeField] private Slider lifeBar;

    private void OnEnable()
    {
        Player_Controller.OnPlayerLifeUpdate += UpdateLifePlayer;
    }

    public void UpdateLifePlayer(int life)
    {
        if(TextLife != null)
        {
            TextLife.text = "x" + life;
        }
        if(lifeBar != null)
        {
            lifeBar.value = life;
        }
    }
    private void OnDisable()
    {
        Player_Controller.OnPlayerLifeUpdate -= UpdateLifePlayer;
    }
}

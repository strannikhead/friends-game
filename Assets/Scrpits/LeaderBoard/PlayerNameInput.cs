using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField inputField;
    private string playerName;

    public void UpdateResults()
    {
        var scoreManager = FindObjectOfType<ScoreManager>();
        GetPlayerName();
        scoreManager.AddScore(playerName, Game.score);
        SceneManager.LoadScene("MainMenu");
    }

    // review(29.06.2024): На самом деле этот метод не получает имя игрока, а устанавливает его. Стоит ли тогда в принципе хранить имя в поле?
    private void GetPlayerName()
    {
        playerName = inputField.text;
        Debug.Log("Имя игрока: " + playerName);
    }
}
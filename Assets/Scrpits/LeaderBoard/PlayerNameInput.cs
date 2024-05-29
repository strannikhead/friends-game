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
    
    private void GetPlayerName()
    {
        playerName = inputField.text;
        Debug.Log("Имя игрока: " + playerName);
    }
}
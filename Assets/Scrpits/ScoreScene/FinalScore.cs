using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    private TextMeshProUGUI textObject;
    // Start is called before the first frame update
    void Start()
    {
        textObject = gameObject.GetComponent<TextMeshProUGUI>();
        
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.AddScore("Player", Game.score);

    }

    // Update is called once per frame
    void Update()
    {
        // review(26.05.2024): Как часто обновляется score? Действительно ли стоит перезаписывать текст на каждый Update?
        textObject.text = Game.score.ToString();
    }
}

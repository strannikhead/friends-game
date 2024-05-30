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
        // refactored (30.05.2024) // review(26.05.2024): Как часто обновляется score? Действительно ли стоит перезаписывать текст на каждый Update?
        if (int.Parse(textObject.text) != Game.score)
        {
            textObject.text = Game.score.ToString();
        }
    }
}

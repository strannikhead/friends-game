using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//refactored (30.05.2024) // review(26.05.2024): Название файла не соответствует названию класа
public class Remaininglife : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //refactored (30.05.2024) // review(26.05.2024): Действительно ли есть необходимость при каждом обновлении генерировать строку? Может быть, изменять текст только при изменении кол-ва жизней?
        if (int.Parse(text.text.Split(':')[1]) != Game.lives)
        {
            text.text = $"Remaining lives: {Game.lives}";
        }
    }
}

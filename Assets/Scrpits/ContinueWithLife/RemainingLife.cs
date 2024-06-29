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
        if (int.Parse(text.text.Split(':')[1]) != Game.lives) // review(29.06.2024): Наверное, тут можно было обойтись без парсинга, а просто сохранять дополнительно еще старое значение в отедльное поле. Что будет, ели вы захотите поменять текст? Снова будете придумывать алгоритм парсинга?
        {
            text.text = $"Осталось жизней: {Game.lives}";
        }
    }
}

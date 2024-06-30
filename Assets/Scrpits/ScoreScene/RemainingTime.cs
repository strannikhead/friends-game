using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingTime : MonoBehaviour
{
    private TextMeshProUGUI textObject;
    private Calculator calculator;
    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        calculator = FindAnyObjectByType<Calculator>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = (calculator.elapsedTime - calculator.countedTime).ToString("0.00"); // review(29.06.2024): Как будто можно было в калькулятор добавить метод GetRemainingTime
    }
}

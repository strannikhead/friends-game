using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemainingBonus : MonoBehaviour
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
        textObject.text = calculator.timeBonus.ToString("0");
    }
}

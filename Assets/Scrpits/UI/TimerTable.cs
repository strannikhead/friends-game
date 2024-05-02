using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerTable : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textObject;
    void Start()
    {
        TimeSystem.Start();
        textObject = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = $"Time: {Math.Round(TimeSystem.ElapsedTime, 2)}";
    }
}

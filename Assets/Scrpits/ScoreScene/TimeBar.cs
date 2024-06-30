using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private Calculator calculator;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.fillAmount = 0;
        image.color = new Color(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        //(29.05.2024) refactored // review(26.05.2024): var
        var barProgress = calculator.countedTime / calculator.timeTreshold;
        image.fillAmount = barProgress < 1 ? barProgress : 1;      // review(29.06.2024): Я больше люблю Math.Min(barProgress, 1);
        var red = barProgress < 0.5f ? barProgress * 2 : 1;        // review(29.06.2024): Math.Min(barProgress * 2, 1);
        var green = barProgress > 0.5f ? 1 : 1 - 2 * barProgress;
        image.color = new Color(red, green, 0);
    }
}

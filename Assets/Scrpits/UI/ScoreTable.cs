using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTable : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textObject;
    void Start()
    {
        textObject = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = $"Score: {ScoreSystem.score}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintChangeTrigger : MonoBehaviour
{
    private HintChanger changer;
    // Start is called before the first frame update
    void Start()
    {
        changer = FindAnyObjectByType<HintChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CompareTag("Change")) 
            { 
                changer.ShowNextHint();
            }
            else if (CompareTag("Disable"))
            {
                changer.DisableHint();
            }
        }
    }
}

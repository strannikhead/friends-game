using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    private Player player;
    private Image bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Image>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = player.dashCharge;
    }
}

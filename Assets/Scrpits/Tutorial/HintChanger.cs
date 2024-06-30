using System.Collections;
using System.Linq;
using UnityEngine;

public class HintChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hints;
    private int pointer;
    // Start is called before the first frame update
    void Start()
    {
        pointer = 0;
        Debug.Log(hints.Length);
        hints = hints.OrderBy(x =>
        {
            var name = x.name;
            return int.Parse(name.Substring(0, name.Length - 4));
        }).ToArray();
        Debug.Log(hints.Length);
    }

    public void ShowNextHint()
    {
        hints[pointer].SetActive(false);
        pointer++; // review(29.06.2024): А что если pointer >= hints.Length?
        hints[pointer].SetActive(true);
    }

    public void DisableHint() 
    {
        hints[pointer].SetActive(false);
    }
}

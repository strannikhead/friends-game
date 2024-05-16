using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableBlock : MonoBehaviour
{
    [SerializeField]
    private float vanishTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Vanish());
        }
    }

    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(vanishTime);
        Destroy(gameObject);
    }
}

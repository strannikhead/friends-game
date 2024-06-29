using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject prefab;
    [SerializeField]
    private float delay;
    [SerializeField]
    private GameObject target;
    private bool isSpawning;
    // Start is called before the first frame update
    void Start()
    {
        isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null && !isSpawning) // review(29.06.2024): Разве после спавна target не всегда будет != null? Если так, то можно было оставить только проверку на isSpawning, потому что судя по подсказкам Rider сравнение на null GameObject-ов - затратная операция
        {
            isSpawning = true;
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(delay);
        target = Instantiate(prefab, transform.position, Quaternion.identity);
        isSpawning = false;
    }
}

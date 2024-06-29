using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearingWall : MonoBehaviour
{
    // review(29.06.2024): Неиспользуемые методы лучше убирать
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
        // review(29.06.2024): Тут разве не должна быть проверка на игрока?
        gameObject.SetActive(false);
    }
}

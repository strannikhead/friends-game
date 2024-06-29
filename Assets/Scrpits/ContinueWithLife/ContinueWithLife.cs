using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueWithLife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Decrement());
    }

    // review(29.06.2024): Ненужные методы стоит удалять
    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Decrement()
    {
        yield return new WaitForSeconds(0.5f);
        Game.lives--;             // review(29.06.2024): Изменять публичные поля - плохо, как и сами публичные поля
        StartCoroutine(Unload()); // review(29.06.2024): А зачем стартовать корутину в корутине?
    }
    private IEnumerator Unload()
    {
        yield return new WaitForSeconds(0.5f);
        // review(29.06.2024): Я бы выделил отдельный хелпер, который бы внутри себя инкапсулировал вызов этих двух методов
        SceneManager.UnloadSceneAsync(MapModel.PlayerPosition.sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

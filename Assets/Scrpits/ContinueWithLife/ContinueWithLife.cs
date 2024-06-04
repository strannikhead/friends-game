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

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Decrement()
    {
        yield return new WaitForSeconds(0.5f);
        Game.lives--;
        StartCoroutine(Unload());
    }
    private IEnumerator Unload()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(MapModel.PlayerPosition.sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

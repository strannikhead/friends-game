using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueWithLife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        Game.lives--;
        StartCoroutine(Unload());
    }
    private IEnumerator Unload()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(MapModel.playerPos.sceneName);
        SceneManager.UnloadSceneAsync(MapModel.playerPos.sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

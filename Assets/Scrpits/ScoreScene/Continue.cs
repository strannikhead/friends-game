using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unload()
    {
        SceneManager.UnloadSceneAsync(SceneManager
            .GetActiveScene()
            .GetRootGameObjects()
            .Select(x => x.gameObject.GetComponent<ScenePointer>())
            .Where(x => x != null)
            .FirstOrDefault()
            .player
            .GetComponent<MainMapPlayer>()
            .location
            .sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

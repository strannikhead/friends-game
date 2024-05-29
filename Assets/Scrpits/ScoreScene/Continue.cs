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
        SceneManager.UnloadSceneAsync(MapModel.PlayerPosition.sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

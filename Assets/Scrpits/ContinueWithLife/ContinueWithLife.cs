using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueWithLife : MonoBehaviour
{
    [SerializeField]
    private Button button;
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
        button.interactable = false;
        StartCoroutine(Unload());
    }
    private IEnumerator Unload()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(MapModel.PlayerPosition.sceneName);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

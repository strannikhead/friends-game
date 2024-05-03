using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    public string sceneName;
    public void ChangeToScene()
    {
        ScoreSystem.score = 0;
        SceneManager.LoadScene(sceneName);
    }
}

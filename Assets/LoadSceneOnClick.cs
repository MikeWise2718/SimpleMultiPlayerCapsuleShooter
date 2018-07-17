using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene:" + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadScene0()
    {
        var sceneName = "GameScene";
        Debug.Log("Loading scene0:" + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
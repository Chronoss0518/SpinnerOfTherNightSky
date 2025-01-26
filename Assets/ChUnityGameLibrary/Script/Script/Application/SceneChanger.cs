using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    string loadScenePath = "";
    int loadSceneNo = 0;

    public void SceneChangeFromPath(string _loadScenePath)
    {
        SceneManager.LoadScene(_loadScenePath);
    }
    public void SceneChangeFromNo(int _loadSceneNo)
    {
        SceneManager.LoadScene(_loadSceneNo);
    }
    public void SceneChangeFromPath()
    {
        SceneManager.LoadScene(loadScenePath);
    }
    public void SceneChangeFromNo()
    {
        SceneManager.LoadScene(loadSceneNo);
    }
}

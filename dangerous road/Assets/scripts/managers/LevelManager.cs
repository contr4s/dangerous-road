using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager: MonoBehaviour {

    private void Start()
    {
        UnPause();
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        GameplaySoundManager.SetPauseToAllSounds(true);
        CarSpawnManager.canTurn = false;
    }

    public static void UnPause()
    {
        Time.timeScale = 1;
        GameplaySoundManager.SetPauseToAllSounds(false);
        CarSpawnManager.canTurn = true;
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }    

    public static void LoadSceneWithTransition(string sceneName)
    {
        Loader.LoadSceneWithTransition(sceneName);
    }

    public static void OpenOverlay(GameObject overlay) {
        overlay.SetActive(true);
    }

    public static void CloseOverlay(GameObject overlay) {
        overlay.SetActive(false);
    }  
}

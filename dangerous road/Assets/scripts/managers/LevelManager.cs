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
        GameplaySoundManager.ReactToChangingTimescale();
    }

    public static void UnPause()
    {
        Time.timeScale = 1;
        GameplaySoundManager.ReactToChangingTimescale();
    }

    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void OpenOverlay(GameObject overlay) {
        overlay.SetActive(true);
    }

    public static void CloseOverlay(GameObject overlay) {
        overlay.SetActive(false);
    }  

    public static void ButtonUnlock(Button button) {
        button.interactable = true;
    }

    public static void ButtonLock(Button button) {
        button.interactable = false;
    }
}

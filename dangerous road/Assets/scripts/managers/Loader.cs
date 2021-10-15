using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public const string loadingScreenSceneName = "Loading Screen";
    public const float defaultWaitTime = 4;

    private static Loader S { get; set; }

    [SerializeField] private string mainMenuSceneName = "Main Menu(new)";

    private void Awake()
    {
        S = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(LoadSceneThroughLoadingScreen(mainMenuSceneName));
    }

    public static void LoadSceneWithTransition(string sceneName)
    {
        S.StartCoroutine(LoadSceneThroughLoadingScreen(sceneName));
    }

    private static IEnumerator LoadSceneThroughLoadingScreen(string sceneName)
    {
        LevelManager.LoadScene(loadingScreenSceneName);
        yield return new WaitForSeconds(defaultWaitTime);
        yield return SceneManager.LoadSceneAsync(sceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
}

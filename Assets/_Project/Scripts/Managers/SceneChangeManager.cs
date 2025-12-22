using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager Instance { get; private set; }
    
    [SerializeField] private float sceneFadeDuration;
    [SerializeField] private float fadeDuration;
    private SceneFade _sceneFade;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _sceneFade = GetComponentInChildren<SceneFade>();
    }

    public void RestartScene(string sceneToLoad) => LoadNextStage(sceneToLoad);

    public void LoadNextStage(string sceneToLoad)
    {
        StopAllCoroutines();
        StartCoroutine(LoadNextStageCoroutine(sceneToLoad));
    }

    private IEnumerator LoadNextStageCoroutine( string sceneToLoad)
    {
        Debug.Log("Next Scene Change Starting");
        yield return StartCoroutine(_sceneFade.FadeOutCoroutine(sceneFadeDuration));

        yield return new WaitForSeconds(fadeDuration);
        
        Debug.Log($"Loading next additive scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

        yield return StartCoroutine(_sceneFade.FadeInCoroutine(sceneFadeDuration));

        Debug.Log("Completed Scene Change");
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnPauseMenuActive();
        }
    }

    private void OnPauseMenuActive()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);

        GameUtility.PauseMenuDisableLogic("PauseMenu");
    }

    public void OnResumeButtonClicked()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        
        GameUtility.PauseMenuEnableLogic("PauseMenu");
    }
    
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SceneChangeManager.Instance.RestartScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}

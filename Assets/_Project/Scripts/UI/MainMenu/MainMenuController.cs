using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private bool _hasClicked = false;
    
    public void OnPlayButtonClicked()
    {
        if (_hasClicked) return;
        _hasClicked = true;
        SceneChangeManager.Instance.LoadNextStage("PreScreenScene");
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}

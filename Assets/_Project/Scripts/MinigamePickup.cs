using UnityEngine;

public class MinigamePickup : MonoBehaviour
{
    [SerializeField] private int amount;
    
    public GameObject minigameCanvas;
    private bool _playerInside = false;
    private MiniGame _currentGame;
    
    private void HandleMiniGameSuccess()
    {
        CleanupSubscriptions();
        GameUtility.PauseMenuEnableLogic();
        MiniGameManager.Instance.OnMiniGameComplete(1);
        Destroy(gameObject);
    }
    
    private void HandleMiniGameFail()
    {
        CleanupSubscriptions();
        GameUtility.PauseMenuEnableLogic();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        if (_playerInside) return;

        _playerInside = true;

        HandleMiniGameStart();
    }

    private void HandleMiniGameStart()
    {
        Debug.Log("Spawning minigame");
        GameUtility.PauseMenuDisableLogic();

        var go = Instantiate(minigameCanvas);
        _currentGame = go.GetComponent<MiniGame>();

        if (_currentGame == null)
        {
            Debug.LogError("Minigame prefab is missing MiniGame component!");
            Destroy(go);
            GameUtility.PauseMenuEnableLogic();
            return;
        }

        _currentGame.OnMiniGameSuccess += HandleMiniGameSuccess;
        _currentGame.OnMiniGameFail += HandleMiniGameFail;
    }
    
    private void CleanupSubscriptions()
    {
        if (_currentGame == null) return;

        _currentGame.OnMiniGameSuccess -= HandleMiniGameSuccess;
        _currentGame.OnMiniGameFail -= HandleMiniGameFail;
        _currentGame = null;
    }

    private void OnDestroy()
    {
        CleanupSubscriptions();
    }
    
    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(!otherCollider.CompareTag("Player")) return;
        _playerInside = false;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private Button quitButton;
    private bool isPaused = false;
    private BoxCollider2D catCollider;

    void Start()
    {
        pauseOverlay.SetActive(false);
        quitButton.onClick.AddListener(QuitGame);

        GameObject cat = GameObject.FindWithTag("cat");
        if (cat != null)
        {
            catCollider = cat.GetComponent<BoxCollider2D>();
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseOverlay.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        BoxCollider2D[] allColliders = FindObjectsOfType<BoxCollider2D>();
        foreach (var collider in allColliders)
        {
            collider.enabled = !isPaused;
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
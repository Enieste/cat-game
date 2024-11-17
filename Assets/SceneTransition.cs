using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string targetScene;

    private void OnMouseDown()
    {
        // Store cat state if needed before transition
        CatController currentCat = FindObjectOfType<CatController>();
        if (currentCat != null)
        {
            // Save any state to GameManager if needed
            // GameManager.Instance.isHungry = currentCat.isHungry;
        }

        SceneManager.LoadScene(targetScene);
    }
}
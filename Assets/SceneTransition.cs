using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    public string targetScene;
    private TextMeshPro textMesh;
    private Vector3 originalScale;
    private float hoverScale = 1.1f;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalScale = transform.localScale;
    }   

    private void OnMouseEnter()
    {
        transform.localScale = originalScale * hoverScale;
        textMesh.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        transform.localScale = originalScale;
        textMesh.color = Color.white;
    }

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
using UnityEngine;
using System.Linq;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private HungerSystem hungerSystem;
    private SpriteRenderer catSprite;
    private HungerState state;

    private float goodThreshold = 60f;
    private float warningThreshold = 20f;

    [SerializeField] private Color goodColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color badColor = Color.red;

    private BoxCollider2D catCollider;
    [SerializeField] private PauseMenuController pauseMenuController;

    void Start()
    {
        GameObject catIcon = GameObject.FindWithTag("cat_icon");
        if (catIcon != null)
        {
            catSprite = catIcon.GetComponent<SpriteRenderer>();
            catCollider = catIcon.GetComponent<BoxCollider2D>();
            if (catCollider == null)
            {
                catCollider = catIcon.AddComponent<BoxCollider2D>();
                catCollider.size = catSprite.sprite.bounds.size;
            }
        }
        else
        {
            Debug.LogError("No object with tag 'cat_icon' found!");
        }
    }

    void Update()
    {
        if (hungerSystem != null)
        {
            state = hungerSystem.GetCurrentState();
            UpdateCatColor();
        }
    }

    private void UpdateCatColor()
    {
        if (catSprite == null || state == null) return;

        int minState = new[] { state.Saturation, state.Pets, state.Plays }.Min();

        Color newColor;
        if (minState >= goodThreshold)
        {
            newColor = goodColor;
        }
        else if (minState >= warningThreshold)
        {
            newColor = warningColor;
        }
        else
        {
            newColor = badColor;
        }

        newColor.a = catSprite.color.a;
        catSprite.color = newColor;
    }

    private void OnMouseDown()
    {
        if (pauseMenuController != null)
        {
            pauseMenuController.TogglePause();
        }
        else
        {
            Debug.LogError("PauseMenuController reference is missing!");
        }
    }
}
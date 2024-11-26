using UnityEngine;
using System.Linq;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private HungerSystem hungerSystem;
    private SpriteRenderer catSprite;
    private HungerState state;

    [SerializeField] private float goodThreshold = 60f;
    [SerializeField] private float warningThreshold = 20f;

    [SerializeField] private Color goodColor = Color.green;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color badColor = Color.red;

    void Start()
    {
        GameObject catIcon = GameObject.FindWithTag("cat_icon");
        if (catIcon != null)
        {
            catSprite = catIcon.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("No object with tag 'cat_icon' found!");
        }

        if (hungerSystem != null)
        {
            state = hungerSystem.GetCurrentState();
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
}
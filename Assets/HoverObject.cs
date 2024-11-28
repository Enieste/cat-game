using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private float thicknessMultiplier = 0.01f;
    [SerializeField] private Color outlineColor = new Color(1f, 0.5f, 0f, 1f); // Orange by default

    private SpriteRenderer spriteRenderer;
    private Material outlineMaterial;
    private Material defaultMaterial;
    private static readonly int OutlineColorProperty = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineWidthProperty = Shader.PropertyToID("_OutlineWidth");

    private float currentThickness;
    private Color currentColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        // Create material instance
        outlineMaterial = new Material(Shader.Find("Custom/SpriteOutline"));
        outlineMaterial.mainTexture = spriteRenderer.sprite.texture;

        // Set initial values
        UpdateOutlineProperties();
    }

    void Update()
    {
        // Check if values changed
        if (currentThickness != thicknessMultiplier || currentColor != outlineColor)
        {
            UpdateOutlineProperties();
        }
    }

    private void UpdateOutlineProperties()
    {
        float objectWidth = spriteRenderer.bounds.size.x;
        float objectHeight = spriteRenderer.bounds.size.y;
        float averageSize = (objectWidth + objectHeight) / 2f;
        float outlineWidth = averageSize * thicknessMultiplier;

        outlineMaterial.SetFloat(OutlineWidthProperty, outlineWidth);
        outlineMaterial.SetColor(OutlineColorProperty, outlineColor);

        // Store current values
        currentThickness = thicknessMultiplier;
        currentColor = outlineColor;
    }

    void OnMouseEnter()
    {
        spriteRenderer.material = outlineMaterial;
    }

    void OnMouseExit()
    {
        spriteRenderer.material = defaultMaterial;
    }

    void OnDestroy()
    {
        if (outlineMaterial != null)
        {
            Destroy(outlineMaterial);
        }
    }
}
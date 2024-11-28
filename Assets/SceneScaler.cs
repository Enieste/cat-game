using UnityEngine;

public class SceneScaler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private float targetAspect = 16f / 9f;  // 1920/1080

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        ApplyScaling();
    }

    private void ApplyScaling()
    {
        // Get current aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // Create camera scaling rect
        if (scaleHeight < 1.0f)
        {
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
    }
}
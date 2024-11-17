using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    void OnMouseDown()
    {
        // Find the cat in the current scene
        CatController cat = FindObjectOfType<CatController>();
        if (cat != null)
        {
            // When clicked, make the cat move to this object
            Vector3 targetPos = transform.position;
            cat.MoveTo(targetPos);
        }
    }
}
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public GameObject catPrefab;
    public Transform spawnPoint;

    void Start()
    {
        // Just check if there's already a cat in the scene
        if (FindObjectOfType<CatController>() == null)
        {
            SpawnInitialCat();
        }
    }

    void SpawnInitialCat()
    {
        if (catPrefab != null && spawnPoint != null)
        {
            Instantiate(catPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Missing cat prefab or spawn point reference in CatSpawner!");
        }
    }
}
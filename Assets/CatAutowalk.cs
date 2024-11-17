namespace DefaultNamespace
{
    using UnityEngine;
using System.Collections;

public class CatAutowalk : MonoBehaviour
{
    private CatController cat;
    
    [Header("Movement Settings")]
    [Tooltip("Minimum time in seconds the cat will wait before moving to a new position")]
    [SerializeField] private float minIdleTime = 5f;
    
    [Tooltip("Maximum time in seconds the cat will wait before moving to a new position")]
    [SerializeField] private float maxIdleTime = 10f;

    private void Start()
    {
        Debug.Log("STARTING");
        cat = GetComponent<CatController>();
        if (cat != null)
        {
            StartCoroutine(RandomWaypointMovement());
        }
        else
        {
            Debug.Log("CatAutowalk requires a CatController component on the same GameObject!");
        }
    }

    private IEnumerator RandomWaypointMovement()
    {
        Debug.Log("Started RandomWaypointMovement coroutine");
        while (true)
        {
            // Wait while cat is moving
            while (cat.IsMoving())
            {
                yield return null;
            }

            // Random delay between minIdleTime and maxIdleTime seconds while idle
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            float timer = 0f;
            
            Debug.LogError(timer);

            while (timer < idleTime)
            {
                // Check if cat started moving for some other reason
                if (cat.IsMoving())
                {
                    timer = 0f;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // Only proceed if we reached the full idle time
            if (timer >= idleTime)
            {
                // Get all waypoints
                GameObject[] waypoints = GameObject.FindGameObjectsWithTag("waypoint");
                
                GameObject randomWaypoint = waypoints[Random.Range(0, waypoints.Length)];
                cat.MoveTo(randomWaypoint.transform.position);
            }
        }
    }

    // Optional: Method to change idle time range at runtime
    public void SetIdleTimeRange(float min, float max)
    {
        minIdleTime = Mathf.Max(0, min);
        maxIdleTime = Mathf.Max(minIdleTime, max);
    }
}
    
}
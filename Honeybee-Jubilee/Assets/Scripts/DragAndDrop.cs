using UnityEngine;
using System.Collections.Generic; // Needed for using Lists

public class DragAndDrop : MonoBehaviour
{
    public GameObject trailPrefab; // Prefab for the trail object
    public float trailSpawnInterval = 0.1f; // Time interval between spawning trail objects

    private Camera mainCamera;
    private TrailRenderer currentTrail;
    private float lastTrailSpawnTime;
    private List<GameObject> trailPool = new List<GameObject>(); // Object pool for trails
    private int poolSize = 10; // Initial size of the pool
    private bool isDragging = false; // Added a flag to check if dragging has started

    void Start()
    {
        mainCamera = Camera.main;
        InitializeTrailPool();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse is over the GameObject when the click starts
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
            }
        }

        if (isDragging)
        {
            FollowMouse();
        }

        // When mouse button is released, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void FollowMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        transform.position = mousePosition;

        if (Time.time - lastTrailSpawnTime > trailSpawnInterval)
        {
            SpawnTrailObject();
            lastTrailSpawnTime = Time.time;
        }
    }

    void SpawnTrailObject()
    {
        GameObject trail = GetPooledTrail();
        if (trail != null)
        {
            trail.transform.position = transform.position;
            trail.SetActive(true);
            currentTrail = trail.GetComponent<TrailRenderer>();
        }
        // Optionally, handle the case where trail is null (all trails in use and pool at max size)
    }
    // Initializes the pool of trail objects
    void InitializeTrailPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trail.SetActive(false);
            trailPool.Add(trail);
        }
    }

    // Returns an inactive trail object from the pool, if available
    GameObject GetPooledTrail()
    {
        for (int i = 0; i < trailPool.Count; i++)
        {
            if (!trailPool[i].activeInHierarchy)
            {
                return trailPool[i];
            }
        }

        // Return null if poolSize is reached and all trails are active, instead of expanding the pool
        if (trailPool.Count >= poolSize)
        {
            return null; // No available trail, and pool size limit reached
        }

        // Expand the pool if under max size and no available inactive objects
        GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        trail.SetActive(false);
        trailPool.Add(trail);
        return trail;
    }
}
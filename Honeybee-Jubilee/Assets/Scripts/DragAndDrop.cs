using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DragAndDrop : MonoBehaviour
{
    public GameObject trailPrefab;
    public float trailSpawnInterval = 0.1f;
    public Material trailMaterial;
    public Color trailColor = Color.white;
    public Color outOfBoundsColor = Color.red; // Color when out of bounds
    public float startWidth = 0.1f;
    public float endWidth = 0.0f;
    public float timeToLive = 1.0f;
    public Collider2D targetObjectCollider; // Collider of the target object

    public Collider2D endtargetObjectCollider; 

    private Camera mainCamera;
    private TrailRenderer currentTrail;
    private float lastTrailSpawnTime;
    private List<GameObject> trailPool = new List<GameObject>();
    private int poolSize = 10;
    private bool isDragging = false;
    private Vector3 initialPosition;

    void Start()
    {
        mainCamera = Camera.main;
        InitializeTrailPool();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

void FollowMouse()
{
    Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0f;

    if (!targetObjectCollider.OverlapPoint(mousePosition))
    {
        if (currentTrail != null)
        {
            currentTrail.material.color = outOfBoundsColor; // Change trail color to red
        }
        isDragging = false; // Optionally stop dragging if out of bounds
    }
    else
    {
        transform.position = mousePosition;
        if (Time.time - lastTrailSpawnTime > trailSpawnInterval)
        {
            SpawnTrailObject();
            lastTrailSpawnTime = Time.time;
        }
    }

    // Check collision with endtargetObject
    if (endtargetObjectCollider != null && endtargetObjectCollider.OverlapPoint(mousePosition))
    {
        // Load the GameScene2 when colliding with endtargetObject
        SceneManager.LoadScene("GameScene2");
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
            SetupTrailRenderer(currentTrail);
        }
    }

    void InitializeTrailPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject trail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trail.SetActive(false);
            TrailRenderer trRenderer = trail.GetComponent<TrailRenderer>();
            if (trRenderer == null)
            {
                trRenderer = trail.AddComponent<TrailRenderer>();
            }
            SetupTrailRenderer(trRenderer);
            trailPool.Add(trail);
        }
    }

    GameObject GetPooledTrail()
    {
        foreach (GameObject trail in trailPool)
        {
            if (!trail.activeInHierarchy)
            {
                return trail;
            }
        }

        // If all trails are active and the pool is at max size, do not expand the pool further
        return null;
    }

    private void SetupTrailRenderer(TrailRenderer tr)
    {
        tr.material = trailMaterial;
        tr.startColor = trailColor;
        tr.endColor = trailColor;
        tr.startWidth = startWidth;
        tr.endWidth = endWidth;
        tr.time = timeToLive;
        // setting the renderQueue to make the trail render above other objects
        tr.material.renderQueue = 3000;
        tr.sortingLayerName = "Foreground";
        // Set the order within the layer (Higher values are rendered on top)
        tr.sortingOrder = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TrailConfigurator : MonoBehaviour
{
    public Material trailMaterial; // Material for the trail
    public Color trailColor = Color.white; // Color of the trail
    public float startWidth = 0.1f; // Starting width of the trail
    public float endWidth = 0.0f; // Ending width of the trail
    public float timeToLive = 1.0f; // Time the trail will be visible

    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = gameObject.AddComponent<TrailRenderer>();

        trailRenderer.material = trailMaterial;
        // 使轨迹材质渲染在默认渲染队列的前面
        trailRenderer.material.renderQueue = 2999; // 默认值是3000

        trailRenderer.startColor = trailColor;
        trailRenderer.endColor = trailColor;
        trailRenderer.startWidth = startWidth;
        trailRenderer.endWidth = endWidth;
        trailRenderer.time = timeToLive;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, 0);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float point = 0f;

            if (plane.Raycast(ray, out point))
            {
                Vector3 mousePos = ray.GetPoint(point);
                transform.position = mousePos;
            }
        }
    }
}
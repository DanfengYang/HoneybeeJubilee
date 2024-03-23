using UnityEngine;

public class TrailConfigurator : MonoBehaviour
{
    public Material trailMaterial1; // 轨迹在边界内的材质
    public Material trailMaterial2; // 轨迹在边界外的材质
    public Color trailColor1 = Color.white; // 轨迹在边界内的颜色
    public Color trailColor2 = Color.red; // 轨迹在边界外的颜色
    public float startWidth = 0.1f; // 轨迹起始宽度
    public float endWidth = 0.0f; // 轨迹结束宽度
    public float timeToLive = 1.0f; // 轨迹可见时间
    public Collider2D targetObject; // 用于比较鼠标位置的2D对象
    public Collider2D endtargetObject; // 用于比较鼠标位置的2D对象

    private TrailRenderer trailRenderer; // 轨迹渲染器
    private bool isColliding = false; // 标志位，用于跟踪碰撞状态

    void Start()
    {
        trailRenderer = gameObject.AddComponent<TrailRenderer>(); // 添加轨迹渲染器组件
        trailRenderer.material = trailMaterial1; // 设置轨迹材质
        trailRenderer.startColor = trailColor1; // 设置轨迹起始颜色
        trailRenderer.endColor = trailColor1; // 设置轨迹结束颜色
        trailRenderer.startWidth = startWidth; // 设置轨迹起始宽度
        trailRenderer.endWidth = endWidth; // 设置轨迹结束宽度
        trailRenderer.time = timeToLive; // 设置轨迹可见时间
    }

    void Update()
    {
        if (!isColliding && Input.GetMouseButton(0)) // 如果未发生碰撞且鼠标左键按下
        {
            Plane plane = new Plane(Vector3.up, 0); // 创建平面用于射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 从鼠标位置发射射线
            float point = 0f;

            if (plane.Raycast(ray, out point)) // 射线与平面相交
            {
                Vector3 mousePos = ray.GetPoint(point); // 获取鼠标位置点
                if (targetObject == null || !targetObject.OverlapPoint(mousePos)) // 如果目标对象为空或鼠标位置超出边界
                {
                    // 设置轨迹材质和颜色为超出边界的材质和颜色
                    isColliding = true;
                    trailRenderer.material = trailMaterial2;
                    trailRenderer.startColor = trailColor2;
                    trailRenderer.endColor = trailColor2;
                }
                else
                {
                    // 设置轨迹材质和颜色为边界内的材质和颜色
                    trailRenderer.material = trailMaterial1;
                    trailRenderer.startColor = trailColor1;
                    trailRenderer.endColor = trailColor1;
                }
            }
        }

    }
}

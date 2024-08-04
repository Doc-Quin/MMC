using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Runtime.ExceptionServices;
using System;
using Unity.VisualScripting;

public class wandInteraction : MonoBehaviour
{
    Vector3 currentPos;
    Vector3 lastPos;
    public bool isDrawing = false;
    private List<Vector3> drawPositions = new List<Vector3>();
    private List<Vector2> projectedPositions = new List<Vector2>();
    public GameObject drawPostion;
    public Camera camera;
    private LineRenderer line;
    public Material mat;
    public PlayerInput playerInput;
    private Plane projectionPlane;
    private float originalTimeScale;
    private float originalFixedDeltaTime;
    public bool inFreeze = false;
    public UISystem UIScript;
    

    void Start()
    {
        originalTimeScale = Time.timeScale;
        originalFixedDeltaTime = Time.fixedDeltaTime;
        line = new GameObject().AddComponent<LineRenderer>();
        line.positionCount = 0;
        line.startWidth = 0.002f;
        line.endWidth = 0.002f;
        line.material = mat;
        UIScript.energyName.SetActive(true);
        UIScript.dynamicEnergy.SetActive(true);
        UIScript.maxEnergy = 2;
    }

    void Update()
    {
        if (playerInput.actions["StartDraw"].triggered && UIScript.energyPoint == 2)
        {
            Debug.Log("开始绘制");
            isDrawing = true;

            lastPos = drawPostion.transform.position;
            realTimeDrawLine(line, lastPos);
            drawPositions.Add(lastPos);
        }

        if (playerInput.actions["EndDraw"].triggered && isDrawing)
        {
            Debug.Log("结束绘制");
            isDrawing = false;

            UpdateProjectionPlane();

            // 检查是否符合Z字形
            if (CheckZShape(projectedPositions, 30f))
            {
                Debug.Log("Trigger Spell Successfull.");
                
                if (!inFreeze) {
                    StartCoroutine(slowTimeSpell()); 
                    UIScript.setEnergyPoint(0, UIScript.maxEnergy);
                    UIScript.setDynamicEnergySlider(0f, UIScript.maxEnergy);
                }
            }
            else
            {
                Debug.LogError("Trigger Spell Failed.");
            }

            drawPositions.Clear();
            projectedPositions.Clear();
            lastPos = Vector3.zero;
            currentPos = Vector3.zero;
            line.positionCount = 0;
        }

        if (isDrawing)
        {
            currentPos = drawPostion.transform.position;
            if (Vector3.Distance(lastPos, currentPos) > 0.001f)
            {
                drawPositions.Add(currentPos);
                lastPos = currentPos;
                realTimeDrawLine(line, currentPos);
            }
        }
    }

    private void realTimeDrawLine(LineRenderer line, Vector3 currentPos)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, currentPos);
    }

    private void UpdateProjectionPlane()
    {
        // 使用摄像机的当前世界位置和朝向定义平面
        projectionPlane = new Plane(camera.transform.forward, drawPostion.transform.position);
        
        foreach (var point in drawPositions)
        {
            Vector2 projectedPoint = ProjectPointToPlane(point, projectionPlane);
            projectedPositions.Add(projectedPoint);
        }
    }

    private Vector2 ProjectPointToPlane(Vector3 point, Plane plane)
    {
        float distance = projectionPlane.GetDistanceToPoint(point);
        
        // Project the point onto the plane
        Vector3 projectedPoint = point - distance * projectionPlane.normal;
        
        // Convert the projected point to screen coordinates
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(projectedPoint);

        // Return the screen coordinates as a Vector2
        return new Vector2(screenPoint.x, screenPoint.y);
    }

    private bool CheckZShape(List<Vector2> points, double toleranceAngle)
    {
        if (points.Count < 3)
        {
            return false; // 至少需要三个点才能形成Z形
        }

        int turnPoint = 0;
        double previousAngle = 0;
        bool initialAngleSet = false;
        Vector2 startPoint = points[0];
        bool? lastTurnDirection = null; // 用于存储最后一次转向的方向，true表示右转，false表示左转

        for (int i = 1; i < points.Count; i++)
        {
            Vector2 currentPoint = points[i];
            
            double slope;
            if (startPoint.x == currentPoint.x)
            {
                slope = double.PositiveInfinity; // 处理垂直线的情况
            }
            else
            {
                slope = (currentPoint.y - startPoint.y) / (currentPoint.x - startPoint.x);
            }

            double angleRad = Math.Atan(slope);
            double angleDeg = angleRad * (180.0 / Math.PI);

            if (!initialAngleSet)
            {
                previousAngle = angleDeg;
                initialAngleSet = true;
                continue;
            }

            double angleDifference = angleDeg - previousAngle;

            if (Math.Abs(angleDifference) > toleranceAngle)
            {
                bool currentTurnDirection = angleDifference > 0; // true表示右转，false表示左转

                if (lastTurnDirection != null && lastTurnDirection == currentTurnDirection)
                {
                    // 如果连续两次转向方向相同，则不符合Z字形
                    return false;
                }

                turnPoint++;
                startPoint = currentPoint; // 更新起始点为转折点
                previousAngle = angleDeg;
                initialAngleSet = false; // 重新计算与新起始点的角度
                lastTurnDirection = currentTurnDirection; // 记录当前转向方向
            }
        }

        return turnPoint == 2;
    }


    IEnumerator slowTimeSpell()
    {
        inFreeze = true;
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log($"时间缩放减少到: {Time.timeScale}");

        yield return new WaitForSecondsRealtime(3);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        Debug.Log($"时间缩放重置为: {Time.timeScale}");

        inFreeze = false;
    }
}

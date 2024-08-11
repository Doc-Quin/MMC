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
            //Debug.Log("Start Drawing");
            isDrawing = true;

            lastPos = drawPostion.transform.position;
            realTimeDrawLine(line, lastPos);
            drawPositions.Add(lastPos);
        }

        if (playerInput.actions["EndDraw"].triggered && isDrawing)
        {
            //Debug.Log("End Drawing");
            isDrawing = false;

            UpdateProjectionPlane();

            // Check if it complies with the Z shape
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
        // Defines a plane using the camera's current world position and orientation
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
            return false; // At least three points are needed to form a Z shape
        }

        int turnPoint = 0;
        double previousAngle = 0;
        bool initialAngleSet = false;
        Vector2 startPoint = points[0];
        bool? lastTurnDirection = null; // Used to store the direction of the last turn, true means turn right, false means turn left

        for (int i = 1; i < points.Count; i++)
        {
            Vector2 currentPoint = points[i];
            
            double slope;
            if (startPoint.x == currentPoint.x)
            {
                slope = double.PositiveInfinity; // Handling vertical lines with 'N'
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
                bool currentTurnDirection = angleDifference > 0; // true means turn right, false means turn left

                if (lastTurnDirection != null && lastTurnDirection == currentTurnDirection)
                {
                    // If two consecutive turns are in the same direction, it does not conform to the Z-shape.
                    return false;
                }

                turnPoint++;
                startPoint = currentPoint; // Update the starting point to the turning point
                previousAngle = angleDeg;
                initialAngleSet = false; // Recalculate the angle with respect to the new starting point
                lastTurnDirection = currentTurnDirection; // Record the current turning direction
            }
        }

        return turnPoint == 2;
    }


    IEnumerator slowTimeSpell()
    {
        inFreeze = true;
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        //Debug.Log($"Time scaling is reduced to: {Time.timeScale}");

        yield return new WaitForSecondsRealtime(3);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        //Debug.Log($"Time scaling reset to: {Time.timeScale}");

        inFreeze = false;
    }
}

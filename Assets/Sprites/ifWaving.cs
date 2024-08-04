using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ifWaving : MonoBehaviour
{
    public GameObject body;
    public float thresholdSpeed = 3.0f; // 设定的速度阈值，超过此值认为是在挥砍
    public float offsetThreshold; // 偏移阈值，小于此值认为无偏移方向
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    public bool inAttacking = false;
    public bool attackLeft = false;
    public bool attackRight = false;
    public bool attackUp = false;
    public bool attackDown = false;
    public int offsetDirection = -1; // -1 表示无偏移方向，0-右上，1-左上，2-左下，3-右下

    // Update is called once per frame
    void Start()
    {
        if (body == null)
        {
            body = this.gameObject;
        }
        lastPosition = body.transform.position;

        offsetThreshold = 3.0f;
    }

    void Update()
    {
        // 计算速度
        Vector3 currentPosition = body.transform.position;
        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;

        // 判断速度是否超过阈值
        if (currentVelocity.magnitude > thresholdSpeed)
        {
            inAttacking = true;
            DetermineSwingDirection(currentVelocity);
        }
        else
        {
            inAttacking = false;
            attackDown = false;
            attackUp = false;
            attackLeft = false;
            attackRight = false;
            offsetDirection = -1; // 无偏移方向
        }

        // 更新上一次位置
        lastPosition = currentPosition;
    }

    private void DetermineSwingDirection(Vector3 velocity)
    {
        // 屏幕空间的左右方向
        float horizontalSpeed = Vector3.Dot(velocity, Camera.main.transform.right);
        // 屏幕空间的上下方向
        float verticalSpeed = Vector3.Dot(velocity, Camera.main.transform.up);

        // 判断主方向
        if (Mathf.Abs(horizontalSpeed) > Mathf.Abs(verticalSpeed))
        {
            if (horizontalSpeed > 0)
            {
                attackRight = true;
                attackLeft = false;
                attackUp = false;
                attackDown = false;
            }
            else if (horizontalSpeed < 0)
            {
                attackLeft = true;
                attackRight = false;
                attackUp = false;
                attackDown = false;
            }
        }
        else
        {
            if (verticalSpeed > 0)
            {
                attackUp = true;
                attackDown = false;
                attackLeft = false;
                attackRight = false;
            }
            else if (verticalSpeed < 0)
            {
                attackDown = true;
                attackUp = false;
                attackLeft = false;
                attackRight = false;
            }
        }

        // 判断偏移方向
        if (Mathf.Abs(horizontalSpeed) < offsetThreshold && Mathf.Abs(verticalSpeed) < offsetThreshold)
        {
            offsetDirection = -1; // 无偏移方向
        }
        else if (horizontalSpeed > 0 && verticalSpeed > 0)
        {
            offsetDirection = 0; // 右上
        }
        else if (horizontalSpeed < 0 && verticalSpeed > 0)
        {
            offsetDirection = 1; // 左上
        }
        else if (horizontalSpeed < 0 && verticalSpeed < 0)
        {
            offsetDirection = 2; // 左下
        }
        else if (horizontalSpeed > 0 && verticalSpeed < 0)
        {
            offsetDirection = 3; // 右下
        }
        else
        {
            offsetDirection = -1; // 无偏移方向
        }
    }
}

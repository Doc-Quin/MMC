using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ifWaving : MonoBehaviour
{
    public GameObject body;
    public float thresholdSpeed = 3.0f; // Set the speed threshold, exceeding which is considered as slashing
    public float offsetThreshold; // Offset threshold, if the value is less than this value, it is considered that there is no offset direction
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    public bool inAttacking = false;
    public bool attackLeft = false;
    public bool attackRight = false;
    public bool attackUp = false;
    public bool attackDown = false;
    public int offsetDirection = -1; // -1 means no offset direction, 0-upper right, 1-upper left, 2-lower left, 3-lower right

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
        // Calculate speed
        Vector3 currentPosition = body.transform.position;
        currentVelocity = (currentPosition - lastPosition) / Time.deltaTime;

        // Determine whether the speed exceeds the threshold
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
            offsetDirection = -1; // No offset direction
        }

        // Update last location
        lastPosition = currentPosition;
    }

    private void DetermineSwingDirection(Vector3 velocity)
    {
        // The left and right direction of the screen space
        float horizontalSpeed = Vector3.Dot(velocity, Camera.main.transform.right);
        // Up and down direction in screen space
        float verticalSpeed = Vector3.Dot(velocity, Camera.main.transform.up);

        // Determine the main direction
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

        // Determine the offset direction
        if (Mathf.Abs(horizontalSpeed) < offsetThreshold && Mathf.Abs(verticalSpeed) < offsetThreshold)
        {
            offsetDirection = -1; // No offset direction
        }
        else if (horizontalSpeed > 0 && verticalSpeed > 0)
        {
            offsetDirection = 0; // Top right
        }
        else if (horizontalSpeed < 0 && verticalSpeed > 0)
        {
            offsetDirection = 1; // Top left
        }
        else if (horizontalSpeed < 0 && verticalSpeed < 0)
        {
            offsetDirection = 2; // Lower left
        }
        else if (horizontalSpeed > 0 && verticalSpeed < 0)
        {
            offsetDirection = 3; // Bottom right
        }
        else
        {
            offsetDirection = -1; // No offset direction
        }
    }
}

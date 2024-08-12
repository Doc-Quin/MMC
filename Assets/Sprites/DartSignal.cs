using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DartSignal : MonoBehaviour
{
    public bool ifHit;
    public int focusBorderType;
    public GameObject focusObject;
    public Rigidbody dartBody;
    public int directionMessage;
    public float rotationSpeed; // Rotation speed
    public float initialSpeed;    // Initial flight speed
    public float curveAmount;  // Arc offset
    public int offsetDirection;
    public Camera camera;
    public GameObject XRController;
    public Vector3 focusPos;
    public EnemyController ecScript;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 100f;
        initialSpeed = 1.5f;
        curveAmount = 0.5f; // Adjust the curve offset
        
        if(ifHit){

            ecScript = GameObject.Find("EnemyController").GetComponent<EnemyController>();

            if(focusBorderType == 0){
                focusObject = ecScript.currentRightObj;
            }

            if(focusBorderType == 1){
                focusObject = ecScript.currentLeftObj;
            }
        }
        

        // Gives the dart an initial velocity
        Vector3 initialDirection = camera.transform.forward;
        dartBody.velocity = initialDirection * initialSpeed;

        // Set the vertical speed according to directionMessage
        Vector3 verticalSpeed = Vector3.zero;
        switch (directionMessage)
        {
            case 0: // up
                verticalSpeed = camera.transform.up;
                break;
            case 1: // down
                verticalSpeed = -camera.transform.up;
                break;
            case 2: // left
                verticalSpeed = -camera.transform.right;
                break;
            case 3: // right
                verticalSpeed = camera.transform.right;
                break;
        }

        // Gives the dart a smaller vertical velocity
        dartBody.velocity += verticalSpeed * curveAmount * initialSpeed * 0.3f;

        // Get the speed in the offset direction
        Vector3 offsetDir = Vector3.zero;
        switch (offsetDirection)
        {
            case 0: // Top right
                offsetDir = camera.transform.up + camera.transform.right;
                // Rotate 45 degrees counterclockwise along the Z axis
                transform.Rotate(0, 0, 45f);
                break;
            case 1: // Top left
                offsetDir = camera.transform.up - camera.transform.right;
                transform.Rotate(0, 0, -45f);
                break;
            case 2: // Lower left
                offsetDir = -camera.transform.up - camera.transform.right;
                transform.Rotate(0, 0, 45f);
                break;
            case 3: // Bottom right
                offsetDir = -camera.transform.up + camera.transform.right;
                transform.Rotate(0, 0, -45f);
                break;
        }
        
        // Freeze the Z rotation of a rigid body
        dartBody.constraints = RigidbodyConstraints.FreezeRotationZ;

        // Gives the dart a small offset in the direction of the velocity
        dartBody.velocity += offsetDir.normalized * initialSpeed * curveAmount * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(dartBody != null){

            // Gives the dart a small offset in the direction of the velocity
            Vector3 rotatedUp = transform.TransformDirection(Vector3.up);
        
            // Adding Torque
            dartBody.AddTorque(rotatedUp * rotationSpeed * Time.deltaTime, ForceMode.VelocityChange);

            if(focusObject != null){

                focusPos = focusObject.transform.position;
                // Calculate the direction to the target
                Vector3 directionToTarget = (focusObject.transform.position - transform.position).normalized;

                // Gradually adjust the speed of the dart to track the target object
                Vector3 newVelocity = Vector3.Lerp(dartBody.velocity, directionToTarget * initialSpeed, Time.deltaTime * 4); // Fast lerp speed.
                dartBody.velocity = newVelocity;
            }else{
                Vector3 directionToTarget = (XRController.transform.position + new Vector3(0, 0.5f, 3f) - transform.position).normalized;
                Vector3 newVelocity = Vector3.Lerp(dartBody.velocity, directionToTarget * initialSpeed, Time.deltaTime * 4); // Fast lerp speed.
                dartBody.velocity = newVelocity;
                if (transform.position.z >= 2)
                {        
                    Destroy(gameObject); // Destory dart object when it goes out of the screen.
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject != XRController){
            Destroy(gameObject);
        }
    }
}

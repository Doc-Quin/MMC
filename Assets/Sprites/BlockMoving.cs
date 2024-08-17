using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoving : MonoBehaviour
{
    public float movespeed;
    private float originalMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        movespeed = -0.5f;
        originalMoveSpeed = movespeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(EnvironmentController.isPaused){
            return;
        }
        
        transform.Translate(Vector3.forward * movespeed * Time.fixedDeltaTime);
    }

    public void ResetMoveSpeed()
    {
        movespeed = originalMoveSpeed;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        movespeed = newSpeed;
    }
}

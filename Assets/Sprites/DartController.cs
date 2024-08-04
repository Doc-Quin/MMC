using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DartController : MonoBehaviour
{
    public bool ifHit;
    public GameObject XRController;
    public Camera cam;
    public GameObject focusObject;
    public int focusBorderType;
    public SpawnBlock spawnBlockScript;
    public ifWaving wavingScrpit;
    public GameObject dartPrefab;
    public PlayerInput playerInput;
    public bool isShooting = false;
    public bool isBorned = false;
    public EnemyController ecScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(focusBorderType == 0){
            focusObject = ecScript.currentRightObj;
        }

        if(focusBorderType == 1){
            focusObject = ecScript.currentLeftObj;
        }
            
        if(playerInput.actions["StartDraw"].triggered){
            isShooting = true;
        }

        if(focusObject != null && focusObject.transform.position.z - cam.transform.position.z > 0.2f && focusObject.transform.position.z - cam.transform.position.z > 1f){
            ifHit = true;
        }else{
            ifHit = false;
        }
        
        if(isShooting && wavingScrpit.inAttacking && !isBorned){
            GameObject newDart = Instantiate(dartPrefab, transform.position, Quaternion.identity);

            if(wavingScrpit.attackUp){newDart.GetComponent<DartSignal>().directionMessage = 0;}
            if(wavingScrpit.attackDown){newDart.GetComponent<DartSignal>().directionMessage = 1;}
            if(wavingScrpit.attackLeft){newDart.GetComponent<DartSignal>().directionMessage = 2;}
            if(wavingScrpit.attackRight){newDart.GetComponent<DartSignal>().directionMessage = 3;}
            
            newDart.GetComponent<DartSignal>().focusBorderType = focusBorderType;
            newDart.GetComponent<DartSignal>().camera = cam;
            newDart.GetComponent<DartSignal>().XRController = XRController;
            newDart.GetComponent<DartSignal>().offsetDirection = wavingScrpit.offsetDirection;
            newDart.GetComponent<DartSignal>().ifHit = ifHit;
            
            isBorned = true;
               
        }

        if(playerInput.actions["EndDraw"].triggered && isShooting){
            isShooting = false;
            isBorned = false;
        }
    }
}

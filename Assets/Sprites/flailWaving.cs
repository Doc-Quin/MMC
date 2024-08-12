using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class flailWaving : MonoBehaviour
{
    public bool ifpressed = false;
    public GameObject Head;
    public GameObject body;
    public ifWaving wavingScript;
    public UISystem UIScript;
    public float moveSpeed = 1.0f;
    public float swingDistance;  // Distance of reciprocating motion
    private bool isSwinging = false;  // Preventing Coroutines from Starting Again
    public GameObject backPoint;
    public Vector3 localStartPosition;
    public Vector3 localEndPosition;
    public PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        localStartPosition = backPoint.transform.position;
        localEndPosition = new Vector3(localStartPosition.x, localStartPosition.y, localStartPosition.z + swingDistance);
        UIScript.energyName.SetActive(true);
        UIScript.dynamicEnergy.SetActive(true);
        UIScript.maxEnergy = 2;
    }

    // Update is called once per frame
    void Update()
    {
        localStartPosition = backPoint.transform.position;
        localEndPosition = new Vector3(localStartPosition.x, localStartPosition.y, localStartPosition.z + swingDistance);
        Head.transform.position = new Vector3(backPoint.transform.position.x, backPoint.transform.position.y, Head.transform.position.z);

        if(playerInput.actions["StartDraw"].triggered){
            ifpressed = true;
            if(UIScript.energyPoint == 2 && !isSwinging){
                StartCoroutine(flailSwing());
            }
        }

        if(playerInput.actions["EndDraw"].triggered && ifpressed){
            ifpressed = false;
        }
    }

    void FixedUpdate()
    {
        
    }
    
    IEnumerator flailSwing(){
        isSwinging = true;

        while(Head.transform.position.z < localEndPosition.z){
            Head.transform.position = Vector3.MoveTowards(Head.transform.position, localEndPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while(Head.transform.position.z > localStartPosition.z){
            Head.transform.position = Vector3.MoveTowards(Head.transform.position, localStartPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        UIScript.setEnergyPoint(0, UIScript.maxEnergy);
        UIScript.setDynamicEnergySlider(0, UIScript.maxEnergy);
        
        isSwinging = false;
    }
}

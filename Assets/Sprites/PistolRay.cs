using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolRay : MonoBehaviour
{
    public UISystem UIScript;
    public Block blockScript;
    public Bomb bombScript;
    public PlayerInput playerInput;
    public GameObject focusItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f)){
            Debug.DrawLine(transform.position, hit.point, Color.red);

            blockScript = hit.collider.gameObject.GetComponent<Block>();
            bombScript = hit.collider.gameObject.GetComponent<Bomb>();

            if(blockScript != null){
                focusItem = blockScript.fatherGO;
                
                if(playerInput.actions["PistolTrigger"].triggered){
                    UIScript.setScorePoint(UIScript.scorePoint + 1);
                    UIScript.setDynamicScore(UIScript.scorePoint);
                    Destroy(focusItem);
                }
            }

            if(bombScript != null){
                focusItem = bombScript.fatherGO;
                
                if(playerInput.actions["PistolTrigger"].triggered){
                    UIScript.setHealthPoint(UIScript.healthPoint - 1, UIScript.maxHealth);
                    UIScript.setDynamicHealth(UIScript.healthPoint);
                    Destroy(focusItem);
                }
            }
            
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green);
    }
}

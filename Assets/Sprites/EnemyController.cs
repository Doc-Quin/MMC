using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject currentLeftObj;
    public GameObject currentRightObj;
    public GameObject weaponLeftBase;
    public GameObject weaponRightBase;
    public List<GameObject> weaponLeftBox;
    public List<GameObject> weaponRightBox;
    public GameObject currentLeftWeapon;
    public GameObject currentRightWeapon;
    public UISystem UIScript;
    public List<GameObject> leftObjList;
    public List<GameObject> rightObjList;
    // Start is called before the first frame update
    void Start()
    {
        weaponLeftBase.GetChildGameObjects(weaponLeftBox);
        weaponRightBase.GetChildGameObjects(weaponRightBox);
        foreach(GameObject wp in weaponLeftBox){
            if(wp.activeSelf){
                currentLeftWeapon = wp;
            }
        }
        foreach(GameObject wp in weaponRightBox){
            if(wp.activeSelf){
                currentRightWeapon = wp;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLeftObj == null && leftObjList.Count > 0){
            currentLeftObj = leftObjList[leftObjList.Count - 1];
        }

        if(currentRightObj == null && rightObjList.Count > 0){
            currentRightObj = rightObjList[rightObjList.Count - 1];
        }
    }
}

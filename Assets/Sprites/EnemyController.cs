using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject currentLeftObj;
    public GameObject currentRightObj;
    public GameObject weaponBase;
    public List<GameObject> weaponBox;
    public GameObject currentWeapon;
    public UISystem UIScript;
    public List<GameObject> leftObjList;
    public List<GameObject> rightObjList;
    // Start is called before the first frame update
    void Start()
    {
        weaponBase.GetChildGameObjects(weaponBox);
        foreach(GameObject wp in weaponBox){
            if(wp.activeSelf){
                currentWeapon = wp;
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

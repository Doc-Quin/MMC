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
    public int blockNumber;
    // Start is called before the first frame update
    void Start()
    {
        weaponLeftBase.GetChildGameObjects(weaponLeftBox);
        weaponRightBase.GetChildGameObjects(weaponRightBox);

        switch (PlayerSetData.weaponID)
        {
            case 0:
                currentLeftWeapon = weaponLeftBox[0];
                currentRightWeapon = weaponRightBox[0];
                break;
                
            case 1:
                currentLeftWeapon = weaponLeftBox[1];
                currentRightWeapon = weaponRightBox[1];
                break;

            case 2:
                currentLeftWeapon = weaponLeftBox[2];
                currentRightWeapon = weaponRightBox[2];
                break;

            case 3:
                currentLeftWeapon = weaponLeftBox[3];
                currentRightWeapon = weaponRightBox[3];
                break;

            case 4:
                currentLeftWeapon = weaponLeftBox[4];
                currentRightWeapon = weaponRightBox[4]; 
                break;
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

        if(blockNumber <= 1){
            UIScript.showSuccessUI();
        }
    }
}

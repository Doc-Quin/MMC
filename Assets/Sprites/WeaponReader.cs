using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReader : MonoBehaviour
{
    public int currentWeaponID;
    public GameObject LongSword;
    public GameObject Wand;
    public GameObject Pistol;
    public GameObject Flail;
    public GameObject Dart;

    void Start(){
        currentWeaponID = PlayerSetData.weaponID;
        switch(currentWeaponID){
            case 0:
                LongSword.SetActive(true);
                break;
                
            case 1:
                Wand.SetActive(true);
                break;
                
            case 2:
                Pistol.SetActive(true);
                
                break;
                
            case 3:
                Flail.SetActive(true);
                break;
                
            case 4:
                Dart.SetActive(true);
                break;
        }
    }
}

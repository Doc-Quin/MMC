using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponReader : MonoBehaviour
{
    public int currentWeaponID;
    public GameObject LongSword;
    public GameObject Wand;
    public GameObject Pistol;
    public GameObject Flail;
    public GameObject Dart;
    public PlayerInput playerInput;
    public UISystem UIScript;   

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

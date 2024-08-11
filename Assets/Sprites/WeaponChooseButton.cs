using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChooseButton : MonoBehaviour
{
    public GameObject weaponChoosePanel;
    public GameObject musicSheet;
    public void chooseLongSword(){
        PlayerSetData.weaponID = 0;
        weaponChoosePanel.SetActive(false);
        musicSheet.SetActive(true);
    }

    public void chooseWand(){
        PlayerSetData.weaponID = 1;
        weaponChoosePanel.SetActive(false);
        musicSheet.SetActive(true);
    }

    public void choosePistol(){
        PlayerSetData.weaponID = 2;
        weaponChoosePanel.SetActive(false);
        musicSheet.SetActive(true);
    }

    public void chooseFlail(){
        PlayerSetData.weaponID = 3;
        weaponChoosePanel.SetActive(false);
        musicSheet.SetActive(true);
    }

    public void chooseDart(){
        PlayerSetData.weaponID = 4;
        weaponChoosePanel.SetActive(false);
        musicSheet.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public GameObject startButton;
    public GameObject creditButton;
    public GameObject weaponChoice;

    public void Jump(){
        startButton.SetActive(false);
        creditButton.SetActive(false);
        weaponChoice.SetActive(true);
    }
}

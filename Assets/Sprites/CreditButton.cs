using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditButton : MonoBehaviour
{
    public GameObject startButton;
    public GameObject creditButton;
    public GameObject creditPanel;
    public void triggerCredit(){
        startButton.SetActive(false);
        creditButton.SetActive(false);
        creditPanel.SetActive(true);
        
    }
}

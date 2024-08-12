using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject startButton;
    public GameObject creditButton;
    public GameObject creditPanel;
    public void Back()
    {
        startButton.SetActive(true);
        creditButton.SetActive(true);
        creditPanel.SetActive(false);
    }
}

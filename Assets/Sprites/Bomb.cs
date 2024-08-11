using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public UISystem UIScript;
    public GameObject fatherGO;
    public GameObject ec;
    public EnemyController ecScript;
    public GameObject playerWeapon;
    public int posType;

    // Start is called before the first frame update
    void Start()
    {
        ec = GameObject.Find("EnemyController"); 
        ecScript = ec.GetComponent<EnemyController>();
        UIScript = ecScript.UIScript;
        playerWeapon = ecScript.currentLeftWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < 0.5f){
            ecScript.blockNumber--;
            Destroy(fatherGO);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered Burst!");
        
        if(playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail")){
            UIScript.setEnergyPoint(UIScript.energyPoint - 1, UIScript.maxEnergy);
            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
        }

        UIScript.setHealthPoint(UIScript.healthPoint - 1, UIScript.maxHealth);
        UIScript.setDynamicHealth(UIScript.healthPoint);
        
        ecScript.blockNumber--;
        Destroy(fatherGO);
    }
}

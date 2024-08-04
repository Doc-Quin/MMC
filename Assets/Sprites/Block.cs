using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Block : MonoBehaviour
{
    public ifWaving wavingScript;
    public UISystem UIScript;
    public GameObject playerWeapon;
    public GameObject ec;
    public EnemyController ecScript;
    public GameObject fatherGO;
    public int directionAttribute;
    public int posType;

    // Start is called before the first frame update
    void Start()
    {
        ec = GameObject.Find("EnemyController"); 
        ecScript = ec.GetComponent<EnemyController>();
        UIScript = ecScript.UIScript;

        if(ecScript.currentWeapon != null){
            playerWeapon = ecScript.currentWeapon;

            if(playerWeapon.name.Equals("Sword") || playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail") || playerWeapon.name.Equals("Dart")){
                wavingScript = playerWeapon.GetComponent<ifWaving>();
            }

            if(playerWeapon.name.Equals("Pistol")){

            }

            if(playerWeapon.name.Equals("Dart")){
            
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < 0.3f){
            Debug.Log("Missed!"); 
            UIScript.setHealthPoint(UIScript.healthPoint - 1, UIScript.maxHealth);
            UIScript.setDynamicHealth(UIScript.healthPoint);
            Destroy(fatherGO);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Body")){
            HandleCollision(0, null);
        }

        if(other.gameObject.tag.Equals("FlailHead")){
            HandleCollision(1, null);
        }

        if(other.gameObject.tag.Equals("Dart")){
            HandleCollision(2, other);
        }
    }

    private void HandleCollision(int colliderType, Collider other)
    {
        //Collider type: 0 - body, 1 - flail head, 2 - dart body
        if (wavingScript != null && wavingScript.inAttacking)
        {
            switch(directionAttribute){
                case 0:{
                    if (wavingScript.attackUp){
                        if(playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail")){
                            UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
                            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
                        }

                        UIScript.setScorePoint(UIScript.scorePoint + 1);
                        UIScript.setDynamicScore(UIScript.scorePoint);
                        
                        Destroy(fatherGO);
                    }
                    break;
                }

                case 1:{
                    if (wavingScript.attackDown){
                        if(playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail")){
                            UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
                            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
                        }

                        UIScript.setScorePoint(UIScript.scorePoint + 1);
                        UIScript.setDynamicScore(UIScript.scorePoint);

                        Destroy(fatherGO);;
                    }
                    break;
                }

                case 2:{
                    if (wavingScript.attackLeft){
                        if(playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail")){
                            UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
                            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
                        }

                        UIScript.setScorePoint(UIScript.scorePoint + 1);
                        UIScript.setDynamicScore(UIScript.scorePoint);

                        Destroy(fatherGO);
                    }
                    break;
                }
                
                case 3:{
                    if (wavingScript.attackRight){
                        if(playerWeapon.name.Equals("Wand") || playerWeapon.name.Equals("Flail")){
                            UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
                            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
                        }

                        UIScript.setScorePoint(UIScript.scorePoint + 1);
                        UIScript.setDynamicScore(UIScript.scorePoint);

                        Destroy(fatherGO);
                    }
                    break;
                }
            }
        }

        if(colliderType == 1 && UIScript.energyPoint == 2){
            UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
            UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
            Destroy(fatherGO);
        }

        if(colliderType == 2 && other.gameObject.tag.Equals("Dart")){
            if(directionAttribute == other.GetComponent<DartSignal>().directionMessage){
                UIScript.setEnergyPoint(UIScript.energyPoint + 1, UIScript.maxEnergy);
                UIScript.setDynamicEnergySlider(UIScript.energyPoint, UIScript.maxEnergy);
                Destroy(fatherGO);
            }
        }
    }
}

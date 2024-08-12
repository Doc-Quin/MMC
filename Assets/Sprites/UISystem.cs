using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{   
    public GameObject EnemyController;

    #region Energy
        public GameObject energyName;
        public GameObject dynamicEnergy;
        public int energyPoint;
        public Slider energySlider;
        public int maxEnergy;
    #endregion

    #region Score
        public GameObject scoreName;
        public GameObject dynamicScore;
        public int scorePoint;
    #endregion

    #region Health
        public GameObject healthName;
        public GameObject dynamicHealth;
        public int healthPoint;
        public int maxHealth;
    #endregion

    #region FailUI
        public GameObject FailText;
        public GameObject BackToMenu;
    #endregion

    #region SuccessUI
        public GameObject SuccessText;
    #endregion

    void Start()
    {
        healthPoint = 30;
        scorePoint = 0;
        setDynamicScore(scorePoint);
        setHealthPoint(healthPoint, maxHealth);
        setDynamicHealth(healthPoint);
    }

    #region Energy
    public void setEnergyPoint(int value, int maxEnergy){
        if(value <= maxEnergy && value >= 0){energyPoint = value;}
    }
    public void setDynamicEnergySlider(float value, int maxEnergy){
        if(energyPoint <= maxEnergy){energySlider.value = value / maxEnergy;}
        else{energySlider.value = value;}
    }
    #endregion

    #region Score
    public void setScorePoint(int value){
        if(value >= 0){scorePoint = value;}
    }
    public void setDynamicScore(int value){        
        dynamicScore.GetComponent<TextMeshProUGUI>().text = value.ToString();  
    }
    #endregion

    #region Health
    public void setHealthPoint(int value, int maxHealth){
        if(value < 0){
            EnemyController.SetActive(false);
            FailText.SetActive(true);
            BackToMenu.SetActive(true);
        }

        if(value <= maxHealth && value >= 0){healthPoint = value;}
    }
    public void setDynamicHealth(int value){        
        dynamicHealth.GetComponent<TextMeshProUGUI>().text = value.ToString();  
    }
    #endregion

    #region SuccessUI
    public void showSuccessUI(){
        EnemyController.SetActive(false);
        SuccessText.SetActive(true);
        BackToMenu.SetActive(true);
    }
    #endregion
}

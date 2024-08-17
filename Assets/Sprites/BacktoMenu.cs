using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    public void Jump(){
        EnvironmentController.isPaused = false;
        SceneManager.LoadScene(0);
    }
}

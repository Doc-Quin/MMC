using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnvironmentController : MonoBehaviour
{
    public UISystem uiScript;
    public static bool isPaused = false;
    public PlayerInput playerInput;
    public AudioSource audioSource;
    public float waitCheckSeconds = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMusicWithDelay(5f));
    }

    void Update(){
        if (playerInput.actions["PauseGame"].WasPressedThisFrame()){
            if(isPaused){
                audioSource.UnPause();
                uiScript.hidePauseUI();
                isPaused = false;
            }
            else{
                audioSource.Pause();
                uiScript.showPauseUI();
                isPaused = true;
            }
        }
    }

    IEnumerator PlayMusicWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.clip = PlayerSetData.musicFile;
        audioSource.Play();
    }
}

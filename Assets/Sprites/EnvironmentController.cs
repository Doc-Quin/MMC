using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMusicWithDelay(5f));
    }

    IEnumerator PlayMusicWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.clip = PlayerSetData.musicFile;
        audioSource.Play();
    }
}

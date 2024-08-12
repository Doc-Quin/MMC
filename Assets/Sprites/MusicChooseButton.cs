using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicChooseButton : MonoBehaviour
{
    public void Send01(){
        PlayerSetData.musicSheetDatafile = Resources.Load<TextAsset>("01");
        PlayerSetData.musicFile = Resources.Load<AudioClip>("Canon_In_D");
        SceneManager.LoadScene(1);
    }

    public void Send02(){
        PlayerSetData.musicSheetDatafile = Resources.Load<TextAsset>("02");
        PlayerSetData.musicFile = Resources.Load<AudioClip>("Mother1");
        SceneManager.LoadScene(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicChooseButton : MonoBehaviour
{
    public void Send01(){
        PlayerSetData.musicSheetDatafile = Resources.Load<TextAsset>("01");
        SceneManager.LoadScene(1);
    }
}

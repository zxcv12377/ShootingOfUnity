using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectButtonMgr : MonoBehaviour
{
    public void OnClick()
    {
        if(DataMgr.instance.currentCharacter != Character.None)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

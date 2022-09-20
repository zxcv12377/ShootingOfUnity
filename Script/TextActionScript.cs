using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextActionScript : MonoBehaviour
{

    [SerializeField]
    private GameObject text;

    private byte i = 255;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textAction());
    }
    private void Update()
    {
        text.GetComponent<Text>().color = new Color32(255, 255, 255, i);
    }

    IEnumerator textAction()
    {
        i -= 15;            
        yield return new WaitForSeconds(0.05f);
        if(i == 0)
        {
            StartCoroutine(textActionR());
        }
        else
        {
            StartCoroutine(textAction());
        }
    }
    IEnumerator textActionR()
    {
        i += 15;
        yield return new WaitForSeconds(0.05f);
        if (i == 255)
        {
            StartCoroutine(textAction());
        }
        else
        {
            StartCoroutine(textActionR());
        }
    }
}

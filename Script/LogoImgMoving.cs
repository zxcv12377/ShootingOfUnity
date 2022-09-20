using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LogoImgMoving : MonoBehaviour, IPointerClickHandler
{

    public bool starting = false;
    [SerializeField]
    private GameObject gameLogo;
    [SerializeField]
    private GameObject StartButton;
    [SerializeField]
    private GameObject PressAnyButton;
    int i = 0;
    int bi = -660;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (starting == false)
        {
            starting = true;
            StartCoroutine(LogoMove());
            StartCoroutine(ButtonMove());
            Destroy(PressAnyButton);

        }
    }
    IEnumerator LogoMove()
    {

        i += 3;
        gameLogo.GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(i, 0, 0);
        yield return new WaitForSeconds(0.005f);
        if (i >= 450)
        {
            StopCoroutine(LogoMove());
        }
        else
        {
            StartCoroutine(LogoMove());
        }
    }

    IEnumerator ButtonMove()
    {
        bi += 3;
        StartButton.GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(bi, 0, 0);
        yield return new WaitForSeconds(0.0025f);
        if (bi >= 300)
        {
            StopCoroutine(ButtonMove());
        }
        else
        {
            StartCoroutine(ButtonMove());
        }
    }
}

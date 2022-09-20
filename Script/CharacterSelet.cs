using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelet : MonoBehaviour
{
    public GameObject selecCharacter;
    public Character character;
    Animator anim;
    public Light spotLight;
    public CharacterSelet[] chars;

    private void Start()
    {
        anim = selecCharacter.GetComponent<Animator>();
        if (DataMgr.instance.currentCharacter == character) OnSelect();
        else OnDeSelect();
    }
    private void OnMouseUpAsButton()
    {
        DataMgr.instance.currentCharacter = character;
        OnSelect();
        for(int i = 0; i < chars.Length; i++)
        {
            if (chars[i] != this) chars[i].OnDeSelect();
        }
    }
    private void OnDeSelect()
    {
        anim.SetBool("isSelect", false);
        spotLight.intensity = 0;
    }
    private void OnSelect()
    {
        anim.SetBool("isSelect", true);
        spotLight.intensity = 2;
    }
}

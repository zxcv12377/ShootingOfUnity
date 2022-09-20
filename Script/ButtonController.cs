using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject imageColor;
    [SerializeField]
    private GameObject textColor;
    [SerializeField]
    private GameObject PauseImage;
    [SerializeField]
    private GameObject Continue;
    [SerializeField]
    private GameObject EndGame;
    [SerializeField]
    private GameObject btn_Pause;

    private bool isPause;
    
    public PlayerController player;
    //private GameObject playerAnim;
    //private Animation Anim;

    public enum ButtonType { Fire, Jump, Reload, Pause, Continue, EndGame};
    public ButtonType buttonType;

    private void Update()
    {
        if(buttonType == ButtonType.Fire)
        {
            if (player.currentBullet <= 0 && player.isReload == false)
            {
                textColor.GetComponent<Text>().text = ("Reload");
                textColor.GetComponent<Text>().fontSize = 60;
            }
            else
            {
                textColor.GetComponent<Text>().text = ("Fire");
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonType != ButtonType.Pause)
        {
            imageColor.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            textColor.GetComponent<Text>().color = new Color32(50, 50, 50, 100);
        }
        switch (buttonType)
        {
            case ButtonType.Fire:
                if (!player.isReload && !player.isJump)
                {
                    if (player.currentBullet <= 0)
                    {
                        player.Reload();
                    }
                    else
                    {
                        player.isShoot = true;
                        player.animator.SetBool("isShoot", player.isShoot);
                    }
                }
                break;
            case ButtonType.Jump:
                if (!player.isJump && !player.isReload)
                {
                    player.isJump = true;
                    player.Jump();
                }
                break;
            case ButtonType.Reload:
                player.Reload();
                break;
            case ButtonType.Pause:
                if(isPause == false)
                {
                    PauseImage.GetComponent<Image>().color = new Color32(0, 0, 0, 125);
                    Time.timeScale = 0;
                    isPause = true;
                    Continue.SetActive(true);
                    EndGame.SetActive(true);
                }
                break;
            case ButtonType.Continue:
                break;
            case ButtonType.EndGame:
                break;
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonType != ButtonType.Pause)
        {
            imageColor.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            textColor.GetComponent<Text>().color = new Color32(50, 50, 50, 255);
        }
        switch (buttonType)
        {
            case ButtonType.Fire:
                player.isShoot = false;
                player.animator.SetBool("isShoot", player.isShoot);
                player.muzzleStop();
                break;
            case ButtonType.Jump:
                break;
            case ButtonType.Reload:
                break;
            case ButtonType.Pause:
                break;
            case ButtonType.Continue:
                isPause = btn_Pause.GetComponent<ButtonController>().isPause;
                if (isPause == true)
                {
                    PauseImage.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                    Time.timeScale = 1;
                    btn_Pause.GetComponent<ButtonController>().isPause = false;
                    Continue.SetActive(false);
                    EndGame.SetActive(false);
                }
                break;
            case ButtonType.EndGame:
                GameQuit();
                break;
        }
    }

    public void GameQuit()
    {
        Application.Quit();        
    }
}

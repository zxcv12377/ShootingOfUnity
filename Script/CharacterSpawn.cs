using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject player;
    public GameObject ammo;

    public VirtualJoystick joyStick;
    public TouchPanelController touchPanel;
    public ButtonController buttonFire;
    public ButtonController buttonJump;
    public ButtonController buttonReload;

    private PlayerController controller;


    void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter]);
        player.transform.position = transform.position;
        controller = player.GetComponent<PlayerController>();
        // 스폰시 없는 컴포넌트들을 삽입
        joyStick.controller = controller;
        touchPanel.controller = controller;
        buttonFire.player = controller;
        buttonJump.player = controller;
        buttonReload.player = controller;
        controller.Text = ammo;
        ///////////////////////////////
    }
}

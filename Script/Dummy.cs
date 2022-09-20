using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    public void StartFire()
    {
        player.Fire();
    }

    public void StopJump()
    {
        player.JumpCnt();
    }

    public void StopReload()
    {
        player.StopReload();
    }

    public void muzzleOn()
    {
        player.muzzleOn();
    }
    public void MuzzleStop()
    {
        player.muzzleStop();
    }

    public void Landing()
    {
        player.Landing();
    }
    
    public void endLanding()
    {
        player.endLanding();
    }
}

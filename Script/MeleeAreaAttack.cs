using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAreaAttack : MonoBehaviour
{
    public GameObject enemy;
    public EnemyContoller enemyController;

    public float meleeAttackDamage;
    private void Start()
    {
        meleeAttackDamage = enemyController.Damage;
    }
}

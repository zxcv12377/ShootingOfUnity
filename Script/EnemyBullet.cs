using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public GameObject Enemy;
    public EnemyContoller enemyContoller;

    public float BulletDamage;
    private void Start()
    {
        BulletDamage = enemyContoller.Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground" /*|| collision.gameObject.tag == "Player"*/)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}

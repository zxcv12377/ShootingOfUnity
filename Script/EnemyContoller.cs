using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContoller : MonoBehaviour
{

    public float currentHP;
    public float maxHP;
    public float Damage;
    public float targetRange;
    public float targetRadius;
    public float BulletSpeed;
    //public float speed;
    public bool isChase;
    public bool isAttack;

    private Transform target;

    [SerializeField]
    private Transform enemyBody;

    private GameObject player;
    public PlayerController playerController;
    public GameObject bullet;
    public BoxCollider meleeArea;
    public Transform ShootPoint;


    Rigidbody rb;
    Material mat;
    BoxCollider boxCollider;
    NavMeshAgent nav;
    Animator anim;

    public float firstdelay;
    public float enabletime;
    public float AttackSpeed;
    

    public enum EnemyType { Boximon, AssaultMech};
    public EnemyType enemyType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        while(player == null)
        {
            yield return new WaitForSeconds(0.2f);
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
            if(player != null)
            {
                ChaseStart();
            }
        }
    }

    private void Targetting()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        targetRadius,
                                                        transform.forward,
                                                        targetRange,
                                                        LayerMask.GetMask("Player"));


        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine(Attack());
        }

    }
    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        switch (enemyType)
        {
            case EnemyType.Boximon:
                yield return new WaitForSeconds(firstdelay);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(enabletime);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(AttackSpeed);
                break;
            case EnemyType.AssaultMech:
                yield return new WaitForSeconds(0.7f);
                GameObject instantBullet = Instantiate(bullet, ShootPoint.position, ShootPoint.rotation);
                EnemyBullet Ebullet = instantBullet.GetComponent<EnemyBullet>();
                Ebullet.enemyContoller = this;
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = ShootPoint.forward * BulletSpeed;
                break;
            default:
                break;
        }
        //enemyBody.forward = target.position;
        

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    private void FixedUpdate()
    {
        Targetting();
        FreezeRotation();
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isMove", true);
    }

    void ChaseStop()
    {
        isChase = false;
        anim.SetBool("isMove", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        target = player.transform;
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
            if(nav.isStopped == true)
            {
                nav.velocity = Vector3.zero;
            }
        }
    }

    private void FreezeRotation()
    {
        if (isChase)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Enemy Hit!");
            HitDamage(playerController.reactVector);
            Destroy(collision.gameObject);
        }
    }

    public void HitDamage(Vector3 reacVector)
    {
        currentHP -= playerController.playerInfo.Damage;
        Debug.Log(currentHP);
        BoxCollider enemyColli = GetComponent<BoxCollider>();
        //enemyColli.enabled = false;
        if (currentHP > 0)
        {
            reacVector = reacVector.normalized;
            reacVector += Vector3.up;
            rb.AddForce(reacVector * 5, ForceMode.Impulse);
        }
        else
        {
            enemyColli.enabled = false;
            anim.SetTrigger("doDie");
            ChaseStop();
            Destroy(gameObject, 1.2f);
        }
    }

    //IEnumerator OnDamaged(Vector3 reacVector)
    //{
    //    BoxCollider enemyColli = GetComponent<BoxCollider>();
    //    enemyColli.enabled = false;
    //    yield return new WaitForSeconds(0.1f);
    //    if(currentHP > 0)
    //    {
    //        reacVector = reacVector.normalized;
    //        reacVector += Vector3.up;
    //        rb.AddForce(reacVector * 5, ForceMode.Impulse);
    //    }
    //    else
    //    {
    //        anim.SetTrigger("doDie");
    //        ChaseStop();
    //        Destroy(gameObject, 1.2f);
    //    }
    //}
}

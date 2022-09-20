using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // ����
    [SerializeField]
    private Transform ShootPoint; // RayCast �߻�Ǵ� �� �Ѿ� ������ ��
    
    [SerializeField]
    private Transform CharacterBody; // ĳ���� ��ü
    [SerializeField]
    private Transform cameraArm; // ī�޶��
    [HideInInspector]
    public Animator animator;
    public ParticleSystem muzzleFlash;
    private Rigidbody rb;
    [SerializeField]
    private GameObject Bullet;

    // �̵� / ������ ���� ����
    private Vector3 m_LastPosition; // ��ġ(�ӵ� ���Ҷ� ���)
    
    //private float gravity = -9.81f; // �߷�
    [SerializeField]
    private float jumpforce = 3.0f; // �ٴ� ��
    public float moveSpeed = 5.0f; // �̵� �ӵ�

    // �ִϸ��̼��� ���� ����
    [HideInInspector]
    public bool isWalk = false; // �ȱ�/�ٱ�
    [HideInInspector]
    public bool isShoot = false; // �߻�����
    [HideInInspector]
    public bool isJump = false; // ����������
    [HideInInspector]
    public bool isReload = false; // ����������
    [HideInInspector]
    public bool isLanding = false;

    // �Ѱ� ���õ� ����
    [HideInInspector]
    public GameObject Text; // źâ�� UI
    //[HideInInspector]
    public int maxBullet; // �ִ� źȯ
    //[HideInInspector]
    public int currentBullet; // ���� źȯ
    //private float range = 20.0f; // ��Ÿ�
    public Vector3 reactVector;
    private float bulletSpeed = 20f;

    //private bool check = true;
    private bool isBorder;
    // ĳ���� ����
    public struct PlayerInfo
    {
        public int Level;
        public float CurrentHP;
        public float MaxHP;
        public float Damage;
        public float Armor;

        public float CurrentEXP;
        public float MaxEXP;
    }
    public PlayerInfo playerInfo;

    public enum DamageType { Auto, Single, Burst}
    public DamageType damageType;

    private EnemyContoller enemyController;

    // Start is called before the first frame update
    private void Awake()
    {
        muzzleStop();
        animator = CharacterBody.GetComponent<Animator>();
        rb = CharacterBody.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerInfo.MaxHP = 100f;
        playerInfo.CurrentHP = playerInfo.MaxHP;
        playerInfo.Damage = 10f;

    }

    private void FixedUpdate()
    {
        StopToWall();
        FreezeRotation();
    }

    // Update is called once per frame
    private void Update()
    {
        Text.GetComponent<Text>().text = (currentBullet + " / " + maxBullet);
        if (currentBullet <= 0)
        {
            isShoot = false;
            animator.SetBool("isShoot", isShoot);
            //muzzleFlash.Stop();
            
        }
        if (!isShoot)
        {
            muzzleStop();
        }
        
    }
    float GetSpeed() // �ӵ� ���ϴ� �Լ�
    {
        float speed = (((transform.position - m_LastPosition).magnitude) / Time.deltaTime);
        m_LastPosition = transform.position;

        return speed;
    }

    public void MoveTo(Vector2 inputDirection) // �������� �����ϴ� �Լ�
    {
        if (!isLanding)
        {
            Vector2 moveInput = inputDirection;
            bool isMove = moveInput.magnitude != 0;
            animator.SetFloat("Move", moveInput.magnitude);

            if (isMove || isBorder)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

                CharacterBody.forward = moveDir;
                transform.position += moveDir * Time.deltaTime * moveSpeed;
                if (GetSpeed() < 3)
                {
                    isWalk = true;
                }
                else
                {
                    isWalk = false;
                }
                animator.SetBool("isWalk", isWalk);
            }
        }
    }

    public void LookAround(Vector2 InputDirection) // ī�޶� �̵� �Լ�
    {
        Vector2 mouseDelta = InputDirection;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if(x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y - mouseDelta.x, camAngle.z);
    }

    public void Jump() // �����Լ�
    {
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        animator.SetBool("isJump", isJump);
    }

    public void JumpCnt() // ���� ���� �Լ� (�ִϸ��̼� �̺�Ʈ���� �����)
    {
        isJump = false;
        animator.SetBool("isJump", isJump);
    }

    public void Fire() // �߻� �Լ� (�ִϸ��̼� �̺�Ʈ���� �����)
    {
        if(currentBullet > 0)
        {
            GameObject intantBullet = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
            Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = ShootPoint.forward * bulletSpeed;
            currentBullet--;
        }
    }

    public void Reload() // ���� �Լ�
    {
        if(currentBullet != maxBullet)
        {
            isReload = true;
            animator.SetBool("isReload", isReload);
        }
    }

    public void StopReload() // ���� �ִϸ��̼� ������ �Լ� (�ִϸ��̼� �̺�Ʈ���� �����)
    {
        isReload = false;
        animator.SetBool("isReload", isReload);
        currentBullet = maxBullet;
    }

    public void muzzleOn()
    {
        muzzleFlash.gameObject.SetActive(true);
        muzzleFlash.time = 0.12f;
        muzzleFlash.Play();

    }

    public void muzzleStop()
    {
        muzzleFlash.gameObject.SetActive(false);
        muzzleFlash.Stop();
    }

    private void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }
    private void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }

    public void OnTriggerEnter(Collider other)
    {

        enemyController = other.GetComponent<EnemyContoller>();
        

        if (other.tag == "EnemyBullet")
        {
            EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();
            playerInfo.CurrentHP -= enemyBullet.BulletDamage;
            Debug.Log("EnemyBullet Hit! \n ���� HP : " + playerInfo.CurrentHP);
        }
        else if (other.tag == "Enemy")
        {
            MeleeAreaAttack meleeAttack = other.GetComponent<MeleeAreaAttack>();
            playerInfo.CurrentHP -= meleeAttack.meleeAttackDamage;
            Debug.Log("Enemy Melee Hit! \n ���� HP : " + playerInfo.CurrentHP);
        }
        if (playerInfo.CurrentHP <= 0)
        {
            Die();
        }
    }

    public void endLanding()
    {
        isLanding = false;
    }

    public void Landing()
    {
        isLanding = true;
    }

    private void Die() // �÷��̾� ����� ����Ǵ� �Լ�
    {
        Debug.Log("Player Die");
    }
}



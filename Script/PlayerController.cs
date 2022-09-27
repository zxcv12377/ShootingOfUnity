using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // 변수
    [SerializeField]
    private Transform ShootPoint; // RayCast 발사되는 곳 총알 나가는 곳
    
    [SerializeField]
    private Transform CharacterBody; // 캐릭터 본체
    [SerializeField]
    private Transform cameraArm; // 카메라암
    [HideInInspector]
    public Animator animator;
    public ParticleSystem muzzleFlash;
    private Rigidbody rb;
    [SerializeField]
    private GameObject Bullet;

    // 이동 / 움직임 관련 변수
    private Vector3 m_LastPosition; // 위치(속도 구할때 사용)
    
    //private float gravity = -9.81f; // 중력
    [SerializeField]
    private float jumpforce = 3.0f; // 뛰는 힘
    public float moveSpeed = 5.0f; // 이동 속도

    // 애니메이션을 위한 변수
    [HideInInspector]
    public bool isWalk = false; // 걷기/뛰기
    [HideInInspector]
    public bool isShoot = false; // 발사할지
    [HideInInspector]
    public bool isJump = false; // 점프중인지
    [HideInInspector]
    public bool isReload = false; // 장전중인지
    [HideInInspector]
    public bool isLanding = false;

    // 총과 관련된 변수
    [HideInInspector]
    public GameObject Text; // 탄창수 UI
    //[HideInInspector]
    public int maxBullet; // 최대 탄환
    //[HideInInspector]
    public int currentBullet; // 현재 탄환
    //private float range = 20.0f; // 사거리
    public Vector3 reactVector;
    private float bulletSpeed = 20f;

    //private bool check = true;
    private bool isBorder;
    // 캐릭터 인포
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
    private void StopToWall()
    {
        Debug.DrawRay(CharacterBody.transform.position, CharacterBody.transform.forward * 1, Color.black);
        isBorder = Physics.Raycast(CharacterBody.transform.position, CharacterBody.transform.forward, 1, LayerMask.GetMask("Wall"));
    }
    private void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
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
    float GetSpeed() // 속도 구하는 함수
    {
        float speed = (((transform.position - m_LastPosition).magnitude) / Time.deltaTime);
        m_LastPosition = transform.position;

        return speed;
    }

    public void MoveTo(Vector2 inputDirection) // 움직임을 관리하는 함수
    {
        if (!isLanding)
        {
            Vector2 moveInput = inputDirection;
            bool isMove = moveInput.magnitude != 0;
            animator.SetFloat("Move", moveInput.magnitude);

            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                CharacterBody.forward = moveDir;
                if (!isBorder)
                {
                    transform.position += moveDir * Time.deltaTime * moveSpeed;
                }

                
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

    public void LookAround(Vector2 InputDirection) // 카메라 이동 함수
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

    public void Jump() // 점프함수
    {
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        animator.SetBool("isJump", isJump);
    }

    public void JumpCnt() // 점프 종료 함수 (애니메이션 이벤트에서 사용중)
    {
        isJump = false;
        animator.SetBool("isJump", isJump);
    }

    public void Fire() // 발사 함수 (애니메이션 이벤트에서 사용중)
    {
        if(currentBullet > 0)
        {
            GameObject intantBullet = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
            Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = ShootPoint.forward * bulletSpeed;
            currentBullet--;
        }
    }

    public void Reload() // 장전 함수 
    {
        if(currentBullet != maxBullet)
        {
            isReload = true;
            animator.SetBool("isReload", isReload);
        }
    }

    public void StopReload() // 장전 애니메이션 끝내는 함수 (애니메이션 이벤트에서 사용중)
    {
        isReload = false;
        animator.SetBool("isReload", isReload);
        currentBullet = maxBullet;
    }

    public void muzzleOn() // (애니메이션 이벤트에서 사용중)
    {
        muzzleFlash.gameObject.SetActive(true);
        muzzleFlash.time = 0.12f;
        muzzleFlash.Play();

    }

    public void muzzleStop() // (애니메이션 이벤트에서 사용중)
    {
        muzzleFlash.gameObject.SetActive(false);
        muzzleFlash.Stop();
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            enemyController = other.GetComponent<EnemyContoller>();


            if (other.tag == "EnemyBullet")
            {
                EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();
                playerInfo.CurrentHP -= enemyBullet.BulletDamage;
                Debug.Log("EnemyBullet Hit! \n 현재 HP : " + playerInfo.CurrentHP);
            }
            else if (other.tag == "Enemy")
            {
                MeleeAreaAttack meleeAttack = other.GetComponent<MeleeAreaAttack>();
                playerInfo.CurrentHP -= meleeAttack.meleeAttackDamage;
                Debug.Log("Enemy Melee Hit! \n 현재 HP : " + playerInfo.CurrentHP);
            }
            if (playerInfo.CurrentHP <= 0)
            {
                Die();
            }
        }
    }

    public void endLanding() // (애니메이션 이벤트에서 사용중)
    {
        isLanding = false;
    }

    public void Landing() // (애니메이션 이벤트에서 사용중)
    {
        isLanding = true;
    }

    private void Die() // 플레이어 사망시 실행되는 함수
    {
        Debug.Log("Player Die");
    }
}



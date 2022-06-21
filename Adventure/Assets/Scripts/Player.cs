using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject playerDustRight;
    [SerializeField] private GameObject playerDustLeft;
    [SerializeField] private GameObject dust;
    [SerializeField] private LayerMask enemyMask;
    private Animator anim;
    private SpriteRenderer sprRender;
    private Rigidbody2D rig;

    [Header("Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float attackDistance;
    [SerializeField] private int life;
    private bool IsDead { get; set; }
    private bool DamagedRun { get; set; }
    private bool Damaged { get; set; }
    private bool IsAttack { get; set;}
    private bool Effect { get; set;}
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        sprRender = rig.GetComponent<SpriteRenderer>();
        dust = (GameObject)Instantiate(Resources.Load("Prefab/Dust"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            Attack();
        }              
    }

    private void FixedUpdate()
    {
        if (!IsDead)
        {
            Move();
        }        
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(moveX, moveY).normalized;

        direction = direction * speed * Time.deltaTime;
        rig.MovePosition(rig.position + direction);

        if (direction.x > 0)
        {
            if (!IsAttack)
            {
                anim.SetBool("run", true);
            }
            DamagedRun = true;
            StartCoroutine("DustEffect");
            playerDustRight.SetActive(true);
            playerDustLeft.SetActive(false);
            sprRender.flipX = false;
        }
        else if (direction.x < 0)
        {
            if (!IsAttack)
            {
                anim.SetBool("run", true);
            }
            DamagedRun = true;
            StartCoroutine("DustEffect");
            playerDustRight.SetActive(false);
            playerDustLeft.SetActive(true);
            sprRender.flipX = true;
        }
        else if (direction.y > 0)
        {
            if (!IsAttack)
            {
                anim.SetBool("run", true);
            }
            DamagedRun = true;
            StartCoroutine("DustEffect");
        }
        else if (direction.y < 0)
        {
            if (!IsAttack)
            {
                anim.SetBool("run", true);
            }
            DamagedRun = true;
            StartCoroutine("DustEffect");
        }

        else
        {
            if (!IsAttack)
            {
                anim.SetBool("attack", false);
            }
            DamagedRun = false;
            anim.SetBool("run", false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("CastAttack");
        }
    }
    IEnumerator CastAttack()
    {
        RaycastHit2D hit;
        if (!IsAttack)
        {
            IsAttack = true;
            if (!Damaged)
            {
                anim.SetBool("run", false);
                anim.SetBool("damageRun", false);
                anim.SetBool("damage", false);
                anim.SetBool("attack", true);
            }           

            if (!sprRender.flipX)
            {
                hit = Physics2D.Raycast(transform.position, Vector2.right, attackDistance, enemyMask);
                
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.left, attackDistance, enemyMask);
            }

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Slime"))
                {
                    FindObjectOfType<Slime>().Damage();
                }
            }

            yield return new WaitForSeconds(.32f);

            anim.SetBool("attack", false);
            IsAttack = false;
        }
    }

    public void Damage()
    {
        StartCoroutine("IsGettingDamagede");
    }

    IEnumerator IsGettingDamagede()
    {
        if (!Damaged)
        {
            Damaged = true;
            life--;            
            if(life > 0)
            {
                if (!DamagedRun)
                {
                    anim.SetBool("run", false);
                    anim.SetBool("attack", false);
                    anim.SetBool("damageRun", false);
                    anim.SetBool("damage", true);
                    yield return new WaitForSeconds(1f);
                    anim.SetBool("damage", false);
                    Damaged = false;

                }
                else
                {
                    anim.SetBool("run", false);
                    anim.SetBool("attack", false);
                    anim.SetBool("damageRun", true);
                    anim.SetBool("damage", false);
                    yield return new WaitForSeconds(.46f);
                    anim.SetBool("damageRun", false);
                    Damaged = false;
                }
            }
            else
            {
                anim.SetTrigger("dead");
                IsDead = true;
            }                                                         
        }        
    }

    IEnumerator DustEffect()
    {
        if (!Effect)
        {
            Effect = true;
            yield return new WaitForSeconds(.01f);

            dust.SetActive(true);
            if (playerDustRight.activeInHierarchy)
            {
                dust.transform.position = playerDustRight.transform.position;
            }
            else if (playerDustLeft.activeInHierarchy)
            {
                dust.transform.position = playerDustLeft.transform.position;
            }

            yield return new WaitForSeconds(.22f);
            dust.SetActive(false);
            Effect = false;
        }      
    }
}

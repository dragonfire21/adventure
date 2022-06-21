using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Slime : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LayerMask playerMask;
    private SpriteRenderer sprRender;
    private Animator anim;

    [Header("Attributes")]
    [SerializeField] private float radius;
    [SerializeField] private float life;
    private bool isHiting;
    private bool IsAttack { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Collider2D c = Physics2D.OverlapCircle(transform.position, radius, playerMask);
        if(c != null)
        {
            float distance = Vector2.Distance(transform.position, c.transform.position);
            Vector2 direction = c.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);

            if(angle > 1.5f || angle < -1.5f)
            {
                sprRender.flipX = true;
            }
            else
            {
                sprRender.flipX = false;
            }

            if(distance < 1.7f)
            {
                StartCoroutine("CastAttack");
            }
            else
            {
                anim.SetBool("walk", true);
            }

            if (!isHiting)
            {
                transform.position = Vector2.MoveTowards(transform.position, c.transform.position, .045f);
            }                      
        }
        else
        {
            anim.SetBool("walk", false);
        }

    }

    IEnumerator CastAttack()
    {
        if (!IsAttack)
        {
            IsAttack = true;
            anim.SetBool("walk", false);
            anim.SetBool("damage", false);
            anim.SetBool("attack", true);          
            yield return new WaitForSeconds(.5f);
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
        isHiting = true;
        life--;
        if (life > 0)
        {
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            anim.SetBool("damage", true);
        }
        else
        {
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            anim.SetBool("damage", false);
            anim.SetTrigger("dead");
            yield return new WaitForSeconds(.4f);
            Destroy(gameObject);

        }
        yield return new WaitForSeconds(.3f);
        isHiting = false;
        anim.SetBool("damage", false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

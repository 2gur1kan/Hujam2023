using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacing : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCastTime = 1f;
    private bool waitforattack;
    
    [Header("Ground Check Lenght")]
    [SerializeField] private float ground = .5f;
    [SerializeField] private float wall = .7f;

    [Header("Movment Value")]
    [SerializeField] private float speed = 1;

    private Rigidbody2D rb2d;
    private Animator anim;
    private EnemyHealth health;
    private bool isGrounded = false;
    private bool turn = true;
    private float wait = 0;

    //attack
    private bool attack;
    [SerializeField] private float attackRange = 1.5f;

    public bool stop;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        Move();
    }

    private void Turn()
    {
        if (turn)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speed *= -1;
            clock(.5f);
        }
    }

    IEnumerator clock(float time)
    {
        turn = false;
        yield return new WaitForSeconds(time);
        turn = true;
    }

    private void Move()
    {
        if(health.Health <=0)
        {
            rb2d.velocity = Vector2.zero;
            stop = true;
        }
        else CreateRayCast();

        if (!stop)
        {  
            if (isGrounded) rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            else rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }
    }

    private void CreateRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground, LayerMask.GetMask("Ground"));
        Debug.DrawRay(transform.position, Vector2.down * ground, Color.red);
        isGrounded = hit.collider != null && hit.collider.CompareTag("Ground");

        if (isGrounded) rb2d.gravityScale = 0f;
        else rb2d.gravityScale = 2f;

        if (!isGrounded && wait <= 0)
        {
            Turn();
            wait = 1f;
        }
        else wait -= Time.deltaTime;

        if(transform.localScale.x > 0)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.right * attackRange, Color.red);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.left * attackRange, Color.red);
        }

        attack = hit.collider != null && hit.collider.CompareTag("Player");

        if (hit.collider != null && (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy"))) Turn();

        if (attack && !waitforattack)
        {
            attackAnimPlay();
        }
    }

    private void CreateRayCast(int damage)
    {
        RaycastHit2D hit;

        if (transform.localScale.x > 0)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange * 1.2f, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.right * attackRange, Color.red);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left, attackRange * 1.2f, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, Vector2.left * attackRange, Color.red);
        }

        attack = hit.collider != null && hit.collider.CompareTag("Player");

        if (attack)
        {
            hit.collider.GetComponent<Health>().Damage(damage);
            waitforattack = true;
        }
    }

    private void attackAnimPlay()
    {
        rb2d.velocity = Vector2.zero;
        anim.SetBool("Attack", true);
        stop = true;
        waitforattack = true;

        StartCoroutine(takeDamage(damage));
    }

    IEnumerator takeDamage(int damage)
    {

        yield return new WaitForSeconds(.2f);
        anim.SetBool("Attack", false);
        SoundDataBaseController.Instance.PlaySound(SoundEnum.HITSLUG);
        CreateRayCast(damage);
        yield return new WaitForSeconds(damageCastTime);
        waitforattack = false;
        stop = false;
    }
}

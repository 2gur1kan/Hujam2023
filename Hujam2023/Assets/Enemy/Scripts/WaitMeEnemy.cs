using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitMeEnemy : MonoBehaviour
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
    private bool isGrounded = false;
    private bool turn = true;
    private bool TurnLeft;
    private bool TurnRight = true; 
    private bool PlayerRight;

    private float wait = 0;

    [Header ("Attack")]
    private bool attack;
    [SerializeField] private float attackRange = 1.5f;
    public float detectionDistance;
    public float damageDelay;

    public bool stop;
    private bool detectPlayer;
    private bool waitAttack;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectPlayer();
        Move();
    }

    private void DetectPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < detectionDistance && player.transform.position.y > transform.position.y - 1f)
            {
                detectPlayer = true;
                if (!player.GetComponent<Health>().dead && !GetComponent<EnemyHealth>().Hit)
                {
                    if (player.transform.position.x - transform.position.x < 0) speed = -Mathf.Abs(speed);
                    else speed = Mathf.Abs(speed);

                    if (turn)
                    {
                        if (player.transform.position.x > transform.position.x + 1)
                        {
                            PlayerRight = true;
                        }
                        else if (player.transform.position.x < transform.position.x - 1)
                        {
                            PlayerRight = false;
                        }
                    }
                }
                else detectPlayer = false;
            }
            else detectPlayer = false;
        }
    }

    private void Move()
    {
        CreateRayCast();
        if (detectPlayer && !waitAttack && !attack)
        {
            Turn();
            anim.SetBool("Walk", true);
            turn = true;
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }
        else anim.SetBool("Walk", false);
    }

    private void Turn()
    {
        if (!TurnRight && TurnLeft && PlayerRight)
        {
            ChangeDirection();
            TurnRight = true;
            TurnLeft = false;
        }
        else if (!TurnLeft && TurnRight && !PlayerRight)
        {
            ChangeDirection();
            TurnLeft = true;
            TurnRight = false;
        }
    }

    private void ChangeDirection()
    {
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    private void CreateRayCast()
    {
        RaycastHit2D hit;

        if (transform.localScale.x > 0)
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), attackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(1, 0) * attackRange, Color.red);

        }
        else
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), attackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(-1, 0) * attackRange, Color.red);
        }

        attack = hit.collider != null && hit.collider.CompareTag("Player");

        if (attack && !waitforattack)
        {
            attackAnimPlay();
        }
    }

    private void attackAnimPlay()
    {
        rb2d.velocity = Vector2.zero;

        stop = true;
        waitforattack = true;

        StartCoroutine(takeDamage(damage));
    }

    IEnumerator takeDamage(int damage)
    {
        yield return new WaitForSeconds(.6f);
        anim.SetBool("Attack", true);
        anim.SetBool("Walk", false);
        StartCoroutine(DamageDelay(damage));
        yield return new WaitForEndOfFrame();
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(damageCastTime);
        waitforattack = false;
        stop = false;
    }
    IEnumerator DamageDelay(int damage)
    {
        yield return new WaitForSeconds(damageDelay);
        
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

        if (attack && GetComponent<EnemyHealth>().Health > 0)
        {
            hit.collider.GetComponent<Health>().Damage(damage);
            waitforattack = true;
        }
    }
}

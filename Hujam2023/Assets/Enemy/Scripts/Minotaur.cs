using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    private bool attack;
    private bool tire;
    private bool waitAttack;
    [SerializeField] private float TireTime = 5f;
    [SerializeField] private float AttackRange = 1f;

    [Header("Attack-1")]//Balta saldýrýsý
    [SerializeField] private GameObject Attack1GO;
    [SerializeField] private int Attack1Damage = 2;
    [SerializeField] private float Attack1CastTime = 1f;
    [SerializeField] private float Attack1WaitTime = 5f;
    private bool Attack1wait;

    [Header("Attack-2")]//Yere balta vurma
    [SerializeField] private GameObject Attack2GO;
    [SerializeField] private int Attack2Damage = 1;
    [SerializeField] private float Attack2CastTime = 1f;
    [SerializeField] private float Attack2WaitTime = 10f;
    [SerializeField] private bool Attack2wait;

    [Header("Attack-3")]//Balta ile dönme
    [SerializeField] private GameObject Attack3GO;
    [SerializeField] private int Attack3Damage = 3;
    [SerializeField] private float Attack3CastTime = 1f;
    [SerializeField] private float Attack3WaitTime = 20f;
    [SerializeField] private float Attack3SpinTime = 4f;
    [SerializeField] private bool Attack3wait;

    [Header("Movment Value")]
    [SerializeField] private float speed = 1;
    private float moveSpeed;
    private bool TurnRight = true;
    private bool TurnLeft = false;
    private bool PlayerRight = true;

    [Header("Player Detection")]
    [SerializeField] private float detectionDistance = 10;
    private bool detectPlayer;

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool turn = true;

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

            if (distance < detectionDistance && player.transform.position.y > transform.position.y -2f)
            {
                detectPlayer = true;
                if (!player.GetComponent<Health>().dead)
                {
                    if (player.transform.position.x - transform.position.x < 0) moveSpeed = -speed;
                    else moveSpeed = speed;

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
        if (!tire && detectPlayer && !waitAttack && !tire && !attack)
        {
            Turn();
            turn = true;
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else if (waitAttack)
        {
            StartCoroutine(WaitAttack());
            turn = true;
            Turn();
        }
        else if (tire)
        {
            turn = false;
            anim.SetTrigger("Tire");
        }
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
        RaycastHit2D hit2;

        if (transform.localScale.x > 0)
        {
            hit2 = Physics2D.Raycast(transform.position, new Vector2(1,-.7f), AttackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(1, -.5f) * AttackRange, Color.red);
            hit = Physics2D.Raycast(transform.position, new Vector2(1,.7f), AttackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(1, .5f) * AttackRange, Color.red);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(-1, -.7f), AttackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(-1, -.5f) * AttackRange, Color.red);
            hit2 = Physics2D.Raycast(transform.position, new Vector2(-1, .7f), AttackRange, LayerMask.GetMask("Player"));
            Debug.DrawRay(transform.position, new Vector2(-1, .5f) * AttackRange, Color.red);
        }

        attack = hit.collider != null && hit.collider.CompareTag("Player");
        if(!attack) attack = hit2.collider != null && hit2.collider.CompareTag("Player");

        if(Attack1wait && Attack2wait && Attack3wait)
        {
            anim.SetBool("Tire", true);
            StartCoroutine(Tire());
        }
        else if(!tire && !waitAttack)
        {
            SelectAttack();
        }
    }

    private void SelectAttack()
    {
        if(!Attack1wait && attack)
        {
            waitAttack = true;
            StartCoroutine(Attack1());
        }
        else if (!Attack2wait && detectPlayer)
        {
            waitAttack = true;
            StartCoroutine(Attack2());
        }
        else if (GetComponent<EnemyHealth>().maxHealth/2 >= GetComponent<EnemyHealth>().Health && !Attack3wait && attack)
        {
            waitAttack = true;
            StartCoroutine(Attack3());
        }
    }

    IEnumerator Attack1()
    {
        Attack1wait = true;
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(Attack1CastTime);
        SoundDataBaseController.Instance.PlaySound(SoundEnum.SWORDATTACK);
        anim.ResetTrigger("Attack1");
        GameObject attack1GO = Instantiate(Attack1GO, CalculateDirectionX(1f), transform.rotation);
        attack1GO.GetComponent<AttackBox>().damage = Attack1Damage;
        StartCoroutine(WaitAttack());
        yield return new WaitForSeconds(Attack1WaitTime);
        Attack1wait = false;
    }

    IEnumerator Attack2()
    {
        Attack2wait = true;
        anim.SetTrigger("Attack2");
        SoundDataBaseController.Instance.PlaySound(SoundEnum.ROAR);
        yield return new WaitForSeconds(Attack2CastTime);
        anim.ResetTrigger("Attack2");
        GameObject attack2GO = Instantiate(Attack2GO, CalculateDirectionY(1f), transform.rotation);
        attack2GO.GetComponent<AttackBox>().damage = Attack2Damage;
        attack2GO.GetComponent<AttackBox>().DestroyTime = 3f;
        attack2GO.GetComponent<Roll>().Right = PlayerRight;

        if(GetComponent<EnemyHealth>().maxHealth / 2 >= GetComponent<EnemyHealth>().Health)
        {
            anim.SetTrigger("Attack2");
            SoundDataBaseController.Instance.PlaySound(SoundEnum.ROAR);
            yield return new WaitForSeconds(Attack2CastTime);
            anim.ResetTrigger("Attack2");
            attack2GO = Instantiate(Attack2GO, CalculateDirectionY(1f), transform.rotation);
            attack2GO.GetComponent<AttackBox>().damage = Attack2Damage;
            attack2GO.GetComponent<AttackBox>().DestroyTime = 3f;
            attack2GO.GetComponent<Roll>().Right = PlayerRight;
        }

        StartCoroutine(WaitAttack());
        yield return new WaitForSeconds(Attack2WaitTime);
        Attack2wait = false;
    }

    IEnumerator Attack3()
    {
        Attack3wait = true;
        anim.SetTrigger("Attack3");
        yield return new WaitForSeconds(Attack3CastTime);
        anim.ResetTrigger("Attack3");

        GameObject attack3GO = Instantiate(Attack3GO, CalculateDirectionY(.5f), transform.rotation);
        attack3GO.GetComponent<AttackBox>().damage = Attack1Damage;
        attack3GO.GetComponent<AttackBox>().DestroyTime = Attack3SpinTime;
        StartCoroutine(Attack3SpinSound(Attack3SpinTime));
        yield return new WaitForSeconds(Attack3SpinTime);
        anim.SetTrigger("SpinStop");

        StartCoroutine(WaitAttack());
        yield return new WaitForSeconds(Attack3WaitTime);
        anim.ResetTrigger("SpinStop");
        Attack3wait = false;
    }

    IEnumerator Attack3SpinSound(float spintime)
    {
        if (spintime > 0)
        {
            SoundDataBaseController.Instance.PlaySound(SoundEnum.SWORDATTACK);
            yield return new WaitForSeconds(.2f);
        }
        else spintime -= Time.deltaTime;
    }

    private Vector3 CalculateDirectionX(float value)
    {
        Vector3 vector = transform.position;

        if (PlayerRight) vector.x += value;
        else vector.x -= value;

        return vector;
    }
    private Vector3 CalculateDirectionY(float value)
    {
        Vector3 vector = transform.position;
        vector.y -= value;
        return vector;
    }

    IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(2f);
        waitAttack = false;
    }

    IEnumerator Tire()
    {
        yield return new WaitForSeconds(1f);
        tire = true;
        yield return new WaitForSeconds(TireTime);
        tire = false;
        anim.SetBool("Tire", false);
    }
}

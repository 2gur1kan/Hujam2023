using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private AttackValue Sword;

    private bool attack = true;
    private float attackCastTime = 0;
    private float attackChargeCast = 0;


    private Rigidbody2D rb2d;
    private PlayerMovment MovCS;
    private bool jumped;

    public BasicAttackTypeEnum BA;
    public AttackDirectionEnum AD;

    public int AddDamage = 0;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        MovCS = GetComponent<PlayerMovment>();
    }

    private void FixedUpdate()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (attack && !MovCS.Stuned())
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                attackChargeCast = 0;
                MovCS.DontMove = true;
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                attack = false;
                SellectType();
            }
            else if (Input.GetKey(KeyCode.X))// x den çekildiðinde karakter stunda deðilse tutma sürene göre saldýr 
            {  
                attackChargeCast += Time.deltaTime;
            }
        }
        else
        {
            if (attackCastTime <= 0) attack = true;
            else attackCastTime -= Time.deltaTime;
        }
    }

    private void SellectType()
    {
        switch (BA)
        {
            case BasicAttackTypeEnum.SwordAttack:
                swordAttack();
                break;
            default:
                break;
        }
    }

    private void swordAttack()
    {
        if (attackChargeCast < .4f)
        {
            attackCastTime = this.Sword.attackCastTime;

            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0f;
            AttackDirection();
            GameObject Sword = Instantiate(this.Sword.basicAttack, transform.position, transform.rotation);
            Sword.GetComponent<SwordAttack>().AD = this.AD;
            Sword.GetComponent<SwordAttack>().AddDamage(AddDamage);
            Sword.GetComponent<SwordAttack>().character = gameObject;
        }
        else
        {
            //Charge Attack
            attack = true;
        }
    }

    private void AttackDirection()
    {
        if (Input.GetAxis("Vertical") < 0 && !MovCS.IsGrounded)//zýplarken aþþaðýya vuruþ
        {
            AD = AttackDirectionEnum.Down;
        }
        else if (Input.GetAxis("Vertical") > 0)// yukarýya vuruþ
        {
            AD = AttackDirectionEnum.Up;
        }
        else
        {
            if (transform.localScale.x > 0)// saða vur
            {
                AD = AttackDirectionEnum.Right;
            }
            else//sola vur
            {
                AD = AttackDirectionEnum.Left;
            }
        }
    }
}

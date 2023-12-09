using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private AttackValue Sword;

    private bool attack = true;
    private bool attackClick = false;
    private float attackCastTime = 0;
    private float attackChargeCast = 0;
    
    private Rigidbody2D rb2d;
    private PlayerMovment MovCS;

    public BasicAttackTypeEnum BA;
    public AttackDirectionEnum AD;

    public int AddDamage = 0;

    public float AttackChargeCast { get => attackChargeCast; set => attackChargeCast = value; }
    public bool Attack { get => attack; set => attack = value; }
    public bool AttackClick { get => attackClick; set => attackClick = value; }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        MovCS = GetComponent<PlayerMovment>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (attack && !MovCS.Stuned() && Input.GetKeyDown(KeyCode.X))
        {
            attackChargeCast = 0;
            attackClick = true;

            if (!MovCS.IsGrounded) AttackForMe();
        }
        else if (Input.GetKeyUp(KeyCode.X) && attack && attackClick)
        {
            AttackForMe();
        }
        else if (Input.GetKey(KeyCode.X) && attack)// x den çekildiðinde karakter stunda deðilse tutma sürene göre saldýr 
        {
            attackChargeCast += Time.deltaTime;
        }
        else
        {
            if (attackCastTime <= 0) attack = true;
            else attackCastTime -= Time.deltaTime;
        }
    }

    private void AttackForMe()
    {
        MovCS.DontMove = true;
        attack = false;
        attackClick = false;
        SellectType();
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

    private void WaitAttak()
    {
        attackCastTime = this.Sword.attackCastTime;
    }

    public void WaitAttak(float waitTime)
    {
        attackCastTime = waitTime;
    }

    private void swordAttack()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0f;

        WaitAttak();

        AttackDirection();

        if (attackChargeCast < 1 || !MovCS.IsGrounded)
        {   
            GameObject Sword = Instantiate(this.Sword.basicAttack, transform.position, transform.rotation);
            Sword.GetComponent<SwordAttack>().AD = this.AD;
            Sword.GetComponent<SwordAttack>().AddDamage(AddDamage);
            Sword.GetComponent<SwordAttack>().character = gameObject;
        }
        else
        {

            GameObject Sword = Instantiate(this.Sword.basicChargeAttack, transform.position, transform.rotation);
            Sword.GetComponent<SwordAttack2>().character = gameObject;
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
        else if(transform.localScale.x > 0)// saða vur
        {
            AD = AttackDirectionEnum.Right;
        }
        else//sola vur
        {
            AD = AttackDirectionEnum.Left;
        }
    }
}

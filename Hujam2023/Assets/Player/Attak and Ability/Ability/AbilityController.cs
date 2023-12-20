using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private AbilityValue UFO;

    public AbilityTypeEnum AbilityType;

    private bool attack = true;
    private float attackCastTime = 0;

    private Rigidbody2D rb2d;
    private PlayerMovment MovCS;

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
            AttackForMe();
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
        SellectType();
    }

    private void SellectType()
    {
        switch (AbilityType)
        {
            case AbilityTypeEnum.UFO:
                uFO();
                break;
            default:
                break;
        }
    }

    public void WaitAttak(float waitTime)
    {
        attackCastTime += waitTime;
    }

    private void uFO()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.gravityScale = 0f;

        attackCastTime = this.UFO.abilityCastTime;

        GameObject UFO = Instantiate(this.UFO.ability, transform.position, transform.rotation);
    }
}

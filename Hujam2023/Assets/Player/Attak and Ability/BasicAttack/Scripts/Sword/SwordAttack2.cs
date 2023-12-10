using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack2 : MonoBehaviour
{
    private bool attack;
    public int damage = 3;
    public float destroyTime = .2f;

    public GameObject character;
    [SerializeField] private GameObject HitEffect;

    private void Start()
    {
        StartCoroutine(DestroyTime(destroyTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            collision.GetComponent<EnemyHealth>().Damage(damage);
            Instantiate(HitEffect, transform.position, transform.rotation);
            push(collision);
        }

        if (collision.GetComponent<Spine>())
        {
            character.GetComponent<PlayerMovment>().JumpBool = true;
        }
    }

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }

    IEnumerator DestroyTime(float time)
    {
        yield return new WaitForSeconds(time);
        destroyObject();
    }

    private void destroyObject()
    {
        character.GetComponent<PlayerMovment>().DontMove = false;
        character.GetComponent<Rigidbody2D>().gravityScale = 2f;
        character.GetComponent<BasicAttack>().Attack = false;
        character.GetComponent<BasicAttack>().AttackClick = false;
        character.GetComponent<BasicAttack>().WaitAttak(.5f);
        Destroy(gameObject);
    }
    private void push(Collider2D collision)
    {
        Vector2 collisionPoint = collision.transform.position;
        Vector2 center = transform.position;

        Vector2 pushDirection = center - collisionPoint;
        pushDirection.Normalize();

        float pushForce = 4f;

        collision.GetComponent<Rigidbody2D>().AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
    }
}

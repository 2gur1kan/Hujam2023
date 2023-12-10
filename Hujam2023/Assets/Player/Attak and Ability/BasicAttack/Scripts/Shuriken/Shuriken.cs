using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private bool attack = false;
    public int damage = 2;
    public float destroyTime = 3f;
    public float moveSpeed = 3f;

    public GameObject character;
    public Transform target;
    public AttackDirectionEnum AD;

    [SerializeField] private GameObject HitEffect;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Start()
    {
        character.GetComponent<PlayerMovment>().DontMove = false;
        rb = GetComponent<Rigidbody2D>();
        if(target == null) SelectDirection();
        StartCoroutine(DestroyTime(destroyTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            collision.GetComponent<EnemyHealth>().Damage(damage);
            Instantiate(HitEffect, transform.position, transform.rotation);
        }

        if(collision != null)
        {
            destroyObject();
        }
    }

    private void Update()
    {
        if (target != null) connectTarget();

        move();
    }

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }

    IEnumerator DestroyTime(float time)
    {
        yield return new WaitForSeconds(time);
        character.GetComponent<Rigidbody2D>().gravityScale = 2f;
        destroyObject();
    }

    private void move()
    {
        Vector2 movement = direction * moveSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
    }

    private void destroyObject()
    {
        Destroy(gameObject);
    }

    private void connectTarget()
    {
        Vector2 direction = target.position - transform.position;

        direction.Normalize();
    }

    private void SelectDirection()
    {
        switch (AD)
        {
            case AttackDirectionEnum.Down:
                direction = Vector2.down;
                break;
            case AttackDirectionEnum.Up:
                direction = Vector2.up;
                break;
            case AttackDirectionEnum.Right:
                direction = Vector2.right;
                break;
            case AttackDirectionEnum.Left:
                direction = Vector2.left;
                break;
        }
    }
}

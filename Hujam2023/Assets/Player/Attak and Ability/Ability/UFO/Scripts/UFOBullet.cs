using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBullet : MonoBehaviour
{
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private float speed = 15;
    public int damage;
    public GameObject target;

    private Rigidbody2D rb;
    private Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == null) Destroy(gameObject);
    }

    private void Update()
    {
        move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            collision.GetComponent<EnemyHealth>().Damage(damage);
            Instantiate(HitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (collision.tag != "Player" && collision != null)
        {
            Destroy(gameObject);
        }
    }

    private void move()
    {
        direction = target.transform.position - transform.position;

        direction.Normalize();

        Vector2 movement = direction * speed * Time.deltaTime;

        rb.MovePosition(rb.position + movement);
    }
}

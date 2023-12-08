using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public bool Right;
    [SerializeField] private float speed = 6f;
    private float moveSpeed = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!Right) ChangeDirection();
    }

    void Update()
    {
        if (moveSpeed < speed) moveSpeed += Time.deltaTime*5;

        if(Right) rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        else rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    private void ChangeDirection()
    {
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}

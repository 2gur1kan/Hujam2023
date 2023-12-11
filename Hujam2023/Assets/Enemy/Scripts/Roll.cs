using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public bool Right;
    [SerializeField] private float speed = 6f;
    private float moveSpeed = 0f;
    private float timer = 1;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!Right) ChangeDirection();
    }

    void Update()
    {
        timer += 2 * Time.deltaTime;
        transform.localScale = Vector3.one * timer;

        if (moveSpeed < speed) moveSpeed += Time.deltaTime * 5;

        if(Right) rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        else rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            StartCoroutine(destrotime(2f));
        } 
        else if (collision.tag != "Ground")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator destrotime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void ChangeDirection()
    {
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    [SerializeField] private GameObject Effect;
    public float DestroyTime = .5f;
    public int damage = 1;

    private void Start()
    {
        StartCoroutine(destroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            collision.GetComponent<Health>().Damage(damage);
            push(collision);
        }

        if (collision.CompareTag("Player"))
        {
            Instantiate(Effect, collision.transform.position, transform.rotation);
        }
    }

    private void push(Collider2D collision)
    {
        Vector2 collisionPoint = collision.transform.position;
        Vector2 center = transform.position;

        Vector2 pushDirection = center - collisionPoint;
        pushDirection.Normalize();

        float pushForce = 5f;

        collision.GetComponent<Rigidbody2D>().AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}

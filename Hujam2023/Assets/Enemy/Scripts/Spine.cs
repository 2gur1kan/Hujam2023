using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            if(GetComponent<EnemyHealth>() && GetComponent<EnemyHealth>().Health > 0)
            {
                collision.GetComponent<Health>().Damage(damage);
                push(collision);
            }
            else if (!GetComponent<EnemyHealth>())
            {
                collision.GetComponent<Health>().Damage(damage);
                push(collision);
            }
        }
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

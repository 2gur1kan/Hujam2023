using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private bool demageable = true;
    [SerializeField] private int health = 3;
    public int maxHealth;

    [Header("Hasar Alma")]
    [SerializeField] private float staggerTime = .2f;
    private bool hit;
    private bool dead;

    public int Health { get => health; set => health = value; }
    public bool Hit { get => hit; set => hit = value; }

    private void Start()
    {
        maxHealth = health;
    }

    public void Damage(int damage)
    {
        if(demageable && !hit && health > 0 && !dead)
        {
            hit = true;
            health -= damage;

            if(health <= 0)
            {
                Dead();
            }
            else
            {
                StartCoroutine(Hiting());
            }
        }
    }

    private void Dead()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().mass = 100;
        GetComponent<Animator>().SetTrigger("Dead");
        GetComponent<Collider2D>().enabled = false;

        GameObject.FindWithTag("Player").GetComponent<Health>().Heal();

        dead = true;
        StartCoroutine(destroy());
    }

    IEnumerator Hiting()
    {
        hit = true;
        GetComponent<Animator>().SetTrigger("Hit");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(staggerTime);
        GetComponent<Animator>().ResetTrigger("Hit");
        hit = false;
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5);
        {
            Destroy(gameObject);
        }
    }
}


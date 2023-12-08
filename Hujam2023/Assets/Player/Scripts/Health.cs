using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private bool demageable = true;
    public bool dead;

    [Header("Hasar Alma")]
    [SerializeField] private float staggerTime = .4f;
    [SerializeField] private bool damageBlock = false;
    private bool hit;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public bool DamageBlock { get => damageBlock; set => damageBlock = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal()
    {
        if(currentHealth < maxHealth) currentHealth++;
    }

    public void Damage(int damage)
    {
        if (demageable && !hit && currentHealth > 0 && !dead)
        {
            if (damageBlock && damage > 0) damage -= 1;

            hit = true;
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Dead();
            }
            else
            {
                GetComponent<Animator>().SetTrigger("Hit");
                StartCoroutine(Hiting());
            }
        }
    }

    private void Dead()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("Dead");
        GetComponent<PlayerMovment>().Dead = true;
        GetComponent<PlayerMovment>().Stun = true;

        if(!GetComponent<PlayerMovment>().IsGrounded) GetComponent<Collider2D>().enabled = false;
        dead = true;
    }

    IEnumerator Hiting()
    {
        gameObject.GetComponent<PlayerMovment>().Stun = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        yield return new WaitForSeconds(staggerTime);
        GetComponent<Animator>().ResetTrigger("Hit");
        hit = false;
        gameObject.GetComponent<PlayerMovment>().Stun = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 2f;
    }
}

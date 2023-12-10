using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    [SerializeField] private GameObject trap;

    [SerializeField] private bool PressurePlate;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PressurePlate)
        {
            trap.GetComponent<Rigidbody2D>().gravityScale = 2f;
        }
        else if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().Damage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovment>() != null)
        {
            if (!collision.GetComponent<PlayerMovment>().IsGrounded)
            {
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }
}

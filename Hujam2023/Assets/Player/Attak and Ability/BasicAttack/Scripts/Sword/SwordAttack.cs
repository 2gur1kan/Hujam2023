using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private bool attack = false;
    public int damage = 2;
    public float destroyTime = .2f;

    public GameObject character;
    public AttackDirectionEnum AD;

    [SerializeField] private GameObject HitEffect;

    [Header("Next Attack")]
    [SerializeField] private GameObject NextAttack;

    public AudioClip mySound;

    private void Start()
    {
        SelectDirection();
        StartCoroutine(DestroyTime(destroyTime));

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = mySound;
        audioSource.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) attack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            collision.GetComponent<EnemyHealth>().Damage(damage);
            Instantiate(HitEffect, transform.position, transform.rotation);
            push(collision);
        }

        if (collision.GetComponent<Spine>())
        {
            if(AD == AttackDirectionEnum.Down) pushUp(character);
            character.GetComponent<PlayerMovment>().JumpBool = true;
        }
    }

    public void AddDamage(int damage)
    {
        this.damage += damage;
    }

    IEnumerator DestroyTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (attack) NextAttackCreate();
        else character.GetComponent<Rigidbody2D>().gravityScale = 2f;
        destroyObject();
    }

    private void NextAttackCreate()
    {
        if(AD == AttackDirectionEnum.Down || AD == AttackDirectionEnum.Up) character.GetComponent<Rigidbody2D>().gravityScale = 2f;
        else
        {
            GameObject NextAttack = Instantiate(this.NextAttack, transform.position, transform.rotation);
            NextAttack.GetComponent<SwordAttack2>().AddDamage(damage - 2);
            NextAttack.GetComponent<SwordAttack2>().character = character;
        }

    }

    private void destroyObject()
    {
        character.GetComponent<PlayerMovment>().DontMove = false;
        Destroy(gameObject);
    }

    private void SelectDirection()
    {
        switch (AD)
        {
            case AttackDirectionEnum.Down:
                DownAttack();
                break;
            case AttackDirectionEnum.Up:
                UpAttack();
                break;
            case AttackDirectionEnum.Right:
                RightAttack();
                break;
            case AttackDirectionEnum.Left:
                LeftAttack();
                break;
        }
    } 

    private void DownAttack()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - transform.localScale.z, transform.position.z);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + 180f, currentRotation.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }
    private void UpAttack()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.z, transform.position.z);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x + 180f, currentRotation.y , currentRotation.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }
    private void RightAttack()
    {
        transform.position = new Vector3(transform.position.x + transform.localScale.z, transform.position.y, transform.position.z);
    }
    private void LeftAttack()
    {
        transform.position = new Vector3(transform.position.x - transform.localScale.z, transform.position.y, transform.position.z);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + 180f, currentRotation.z);
        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void push(Collider2D collision)
    {
        Vector2 collisionPoint = collision.transform.position;
        Vector2 center = transform.position;

        Vector2 pushDirection = center - collisionPoint;
        pushDirection.Normalize();

        float pushForce = 3f;

        collision.GetComponent<Rigidbody2D>().AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
    }

    private void pushUp(GameObject go)
    {
        float pushForce = 3f;

        go.GetComponent<Rigidbody2D>().AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private float Wait = 2f;

    [Header("Attack Walue")]
    [SerializeField] private GameObject BasicAttack;

    [SerializeField] private int damage = 2;
    [SerializeField] private float attackCastTime = .5f;
    private bool attack;

    [Header("Enemy Detection")]
    [SerializeField] private float detectionDistance = 10;
    private GameObject target;

    private void Start()
    {
        StartCoroutine(destroy());
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (!attack)
        {
            attack = true;
            SelectTarget();
            if (target != null)
            {
                GameObject AttackGO = Instantiate(BasicAttack, transform.position, transform.rotation);
                AttackGO.GetComponent<UFOBullet>().target = this.target;
                AttackGO.GetComponent<UFOBullet>().damage = this.damage;
            }
            else attack = false;

        }
    }

    private void SelectTarget()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemys)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < detectionDistance && target == null)
                {
                    target = enemy;

                    break;
                }
            }
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(Wait);
        Destroy(gameObject);
    } 

    public void AddWaitTime(float Wait)
    {
        this.Wait += Wait;
    }
}

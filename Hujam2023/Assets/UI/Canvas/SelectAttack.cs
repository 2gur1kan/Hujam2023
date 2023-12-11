using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelectAttack : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 0f;
    }

    public void SetSwordAttack()
    {
        player.GetComponent<BasicAttack>().BA = BasicAttackTypeEnum.SwordAttack;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SetShurikenAttack()
    {
        player.GetComponent<BasicAttack>().BA = BasicAttackTypeEnum.ShurikenAttack;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetAbility(AbilityTypeEnum BA)
    {
        //player.GetComponent<BasicAttack>().BA = BA;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerValuesManager : MonoBehaviour
{
    [SerializeField] private PlayerValueDataBase playerValues;
    [SerializeField] private GameObject player;

    public static PlayerValuesManager instance;

    public PlayerValueDataBase PlayerValues { get => playerValues; set => playerValues = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerValues()
    {
        instance.playerValues.level++;

        instance.playerValues.BasicAttackType = player.GetComponent<BasicAttack>().BA;
        instance.playerValues.AddDamage = player.GetComponent<BasicAttack>().AddDamage;

        instance.playerValues.MaxHealt = player.GetComponent<Health>().MaxHealth;
        instance.playerValues.DamageBlock = player.GetComponent<Health>().DamageBlock;

        instance.playerValues.DashForce = player.GetComponent<PlayerMovment>().DashForce1;
    }

    public void LoadPlayerValues()
    {
        player.GetComponent<BasicAttack>().BA = playerValues.BasicAttackType;
        player.GetComponent<BasicAttack>().AddDamage = playerValues.AddDamage;

        player.GetComponent<Health>().MaxHealth = playerValues.MaxHealt;
        player.GetComponent<Health>().DamageBlock = playerValues.DamageBlock;

        player.GetComponent<PlayerMovment>().DashForce1 = playerValues.DashForce;
    }

    public void ResetValue()
    {
        playerValues = ScriptableObject.CreateInstance<PlayerValueDataBase>();
    }
}

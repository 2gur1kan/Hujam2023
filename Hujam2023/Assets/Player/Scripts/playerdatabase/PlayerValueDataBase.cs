using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerValueDataBase", menuName = "PlayerValueDataBase")]
public class PlayerValueDataBase : ScriptableObject
{
    public int level = 1;

    //Basic Attack
    public BasicAttackTypeEnum BasicAttackType = BasicAttackTypeEnum.SwordAttack;
    public int AddDamage = 0;

    //Health
    public int MaxHealt = 5;
    public bool DamageBlock = false;

    //Charecter Movement
    public float DashForce = 3;
}

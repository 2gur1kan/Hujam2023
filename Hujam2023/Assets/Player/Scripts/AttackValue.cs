using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackValue
{
    public BasicAttackTypeEnum basicAttackType;

    public GameObject basicAttack;
    public GameObject basicChargeAttack;

    public float attackCastTime;
}

public enum BasicAttackTypeEnum
{
    SwordAttack,
    RangeAttack
}

public enum AttackDirectionEnum
{
    Up,
    Down,
    Right,
    Left
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityValue : MonoBehaviour
{
    public AbilityTypeEnum abilityTypeEnum;

    public GameObject ability;

    public float abilityCastTime = 20;
}

public enum AbilityTypeEnum
{
    None,
    UFO,
    digersaldiri
}

public enum ElementTypeEnum
{
    None,
    Fire,
    Ice,
    Lightining
}

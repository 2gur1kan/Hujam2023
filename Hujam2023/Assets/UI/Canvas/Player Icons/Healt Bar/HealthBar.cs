using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private SpriteRenderer SR;

    [SerializeField] private Sprite Empty;
    [SerializeField] private Sprite Full;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();

        SR.sprite = Full;
    }

    public void Hit()
    {
        SR.sprite = Empty;
    }

    public void Heal()
    {
        SR.sprite = Full;
    }

}

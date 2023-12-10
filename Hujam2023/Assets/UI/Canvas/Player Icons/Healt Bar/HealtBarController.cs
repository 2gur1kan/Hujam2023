using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtBarController : MonoBehaviour
{
    public int maxHealth = 5; // Toplam can miktarı
    public int currentHealt; // Mevcut can miktarı
    private Health player;

    public List<HealthBar> HealthBars; // Can kapsüllerini tutan dizi

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        currentHealt = player.CurrentHealth;

        if(maxHealth != currentHealt)
        {
            SetBars();
        }
    }

    private void SetBars()
    {
        if(maxHealth > currentHealt)
        {
            HealthBars[maxHealth - 1].Hit();
            maxHealth--;
        }
        else
        {
            HealthBars[maxHealth - 1].Heal();
            maxHealth++;
        }
    }
}

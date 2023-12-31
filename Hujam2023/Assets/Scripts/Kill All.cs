using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAll : MonoBehaviour
{
    [SerializeField] private List<EnemyHealth> list;
    [SerializeField] private GameObject Unlock;

    private void Update()
    {
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].Health <= 0)
                {
                    list.RemoveAt(i);
                }
            }
        }
        else Unlock.SetActive(true);
    }

}

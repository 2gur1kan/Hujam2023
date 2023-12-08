using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private void Update()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit") && !GetComponent<Animator>().IsInTransition(0))
        {
            Destroy(gameObject);
        }
    }
}

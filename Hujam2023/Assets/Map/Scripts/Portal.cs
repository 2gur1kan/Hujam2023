using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject OtherPortal;
    public bool wait;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !wait)
        {
            collision.transform.position = OtherPortal.transform.position;
            wait = true;
            OtherPortal.GetComponent<Portal>().wait = true;

            StartCoroutine(waitTime());
        }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(2f);
        wait = false;
        OtherPortal.GetComponent<Portal>().wait = false;
    }
}

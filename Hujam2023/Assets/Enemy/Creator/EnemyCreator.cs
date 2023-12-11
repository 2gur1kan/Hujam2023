using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> List;

    private void Start()
    {
        if (List != null) SpawnEnemy();

        Destroy(gameObject);
    }

    private void SpawnEnemy()
    {
        int random = Random.Range(0, List.Count);

        Instantiate(List[random], transform.position, transform.rotation);
    }
}

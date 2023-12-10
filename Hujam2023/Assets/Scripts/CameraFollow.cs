using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float size = 10;
    [SerializeField] private float transitionSpeed = 10f;

    private Camera _camera;
    private bool fall;
    private bool jump;

    public float Size { get => size; set => size = value; }
    public Vector3 OffSet { get => offSet; set => offSet = value; }

    private void Start()
    {
        offSet.z += -11;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 targetPosition;

        targetPosition = player.transform.position + offSet;
        /*
        if (player.GetComponent<CharacterMovment>().height - player.transform.position.y > 5) fall = true;
        else fall = false;

        if (fall)
        {
            targetPosition = new Vector3(
                player.transform.position.x + offSet.x,
                player.transform.position.y + offSet.y - 6,
                player.transform.position.z + offSet.z
            );                 
        }
        else
        {
            targetPosition = new Vector3(
                player.transform.position.x + offSet.x,
                player.GetComponent<CharacterMovment>().height + offSet.y,
                player.transform.position.z + offSet.z
            );
        }
        */
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);

        _camera.orthographicSize = size;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    [SerializeField] Vector3 offset = new Vector3(0, 6, -7);
    [HideInInspector] public Vector3 shakeOffset = Vector3.zero;

    void LateUpdate()
    {
        transform.position = player.transform.position + offset + shakeOffset;
    }
}

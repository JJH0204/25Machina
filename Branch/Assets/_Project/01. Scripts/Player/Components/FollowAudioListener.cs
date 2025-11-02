using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAudioListener : MonoBehaviour
{
    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform rotationTarget;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = positionTarget.position + offset;
        transform.rotation = rotationTarget.rotation;
    }
}

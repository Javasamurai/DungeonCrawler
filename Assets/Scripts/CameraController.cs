using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField]
    private BoxCollider2D collider2D;

    private void Update()
    {
        var position = target.position;
        var targetBounds = collider2D.bounds;
        position.x = Mathf.Clamp(position.x, targetBounds.min.x, targetBounds.max.x);
        position.y = Mathf.Clamp(position.y, targetBounds.min.y, targetBounds.max.y);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}

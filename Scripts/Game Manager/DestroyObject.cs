using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private float _xBoundary = 70.0f;
    private float _yMinBoundary = -30.0f;
    private float _yMaxBoundary = 410.0f;

    private void Update()
    {
        if (IsObjectOutOfBounds())
            Destroy(gameObject);
    }

    private bool IsObjectOutOfBounds()
    {
        return transform.position.y < _yMinBoundary ||
            transform.position.y > _yMaxBoundary ||
            transform.position.x > _xBoundary ||
            transform.position.x < -_xBoundary;
    }
}

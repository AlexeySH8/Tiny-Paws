using UnityEngine;

public class MoonMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _xMinBorder = -145f;
    private float _xMaxBorder = 400f;
    private bool _movingRight = true;

    private void Update()
    {
        MoveMoon();
    }

    private void MoveMoon()
    {
        if (transform.position.x >= _xMaxBorder)
            _movingRight = false;
        else if (transform.position.x <= _xMinBorder)
            _movingRight = true;
        float direction = _movingRight ? 1f : -1f;
        transform.position += Vector3.right * direction * _speed * Time.deltaTime;
    }
}

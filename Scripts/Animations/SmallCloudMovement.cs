using UnityEngine;

public class SmallCloudMovement : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;

    private float _yMinPos = -20f;
    private float _yMaxPos = -160f;

    private float _xMinPos = -760f;
    private float _xMaxPos = 1300f;
    private float _xOffset = 1000f;

    private void Start()
    {
        ResetPosition();
    }

    private void Update()
    {
        MoveSmallCloud();
    }

    private void MoveSmallCloud()
    {
        if (transform.position.x < _xMinPos)
            ResetPosition();
        transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
    }

    private void ResetPosition()
    {
        Vector3 newPos = new Vector3(
            Random.Range(_xMaxPos, _xMaxPos - _xOffset),
            Random.Range(_yMinPos, _yMaxPos),
            transform.position.z);
        transform.position = newPos;
    }
}

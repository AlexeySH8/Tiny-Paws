using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private GameObject _observedObject;
    private float _xBoundary = 14.0f;
    private float _minYBoundary = 0f;
    private float _maxYBoundary = 340f;
    private Vector3 _velocity = Vector3.zero;
    private float _zPosition;
    private float _yOffset;

    private void Start()
    {
        _yOffset = transform.position.y;
        _zPosition = transform.position.z;
    }

    private void LateUpdate()
    {
        ObserveObject();
    }

    private void ObserveObject()
    {
        var targetX = Mathf.Clamp(_observedObject.transform.position.x,
            -_xBoundary, _xBoundary);
        var targetY = Mathf.Clamp(_observedObject.transform.position.y + _yOffset,
            _minYBoundary, _maxYBoundary);

        Vector3 targetPosition = new Vector3(targetX, targetY, _zPosition);

        transform.position = Vector3.SmoothDamp(
            transform.position, targetPosition, ref _velocity, _smoothTime);
    }
}

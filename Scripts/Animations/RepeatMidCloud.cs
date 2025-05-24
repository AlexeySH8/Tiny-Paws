using UnityEngine;

public class RepeatMidCloud : MonoBehaviour
{
    [SerializeField] float _scrollSpeed;
    [SerializeField] int _segmentCount;

    private float _segmentWidth;

    private void Start()
    {
        var colider = GetComponent<BoxCollider2D>();
        _segmentWidth = colider.size.x / _segmentCount;
        ResetPosition();
    }

    private void Update()
    {
        MoveCloud();
    }

    private void MoveCloud()
    {
        if (transform.position.x < -_segmentWidth / 2)
            ResetPosition();
        transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
    }

    private void ResetPosition()
    {
        transform.position = new Vector3(_segmentWidth / 2, transform.position.y, transform.position.z);
    }
}

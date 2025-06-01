using UnityEngine;
using UnityEngine.UI;

public class CameraSetting : MonoBehaviour
{
    private const string ZOOM_VALUE = "zoomValue";
    public float ZPosition { get; private set; }
    [SerializeField] private Slider _zoomSlider;
    private float _default = -33f;

    private void Start()
    {
        LoadValue();
    }

    public void SetZoomValue()
    {
        float value = _zoomSlider.value;
        ZPosition = value;
        PlayerPrefs.SetFloat(ZOOM_VALUE, value);
        PlayerPrefs.Save();
    }

    private void LoadValue()
    {
        _zoomSlider.value = PlayerPrefs.GetFloat(ZOOM_VALUE, _default);
        SetZoomValue();
    }
}

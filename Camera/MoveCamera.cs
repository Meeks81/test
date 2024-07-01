using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _sensetivity;
    [SerializeField] private float _keysSensetivity;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float zoomMin = 3;
    [SerializeField] private float zoomMax = 10;
    [SerializeField] private float minYRotate = 20f;
    [SerializeField] private float maxYRotate = 80f;
    [SerializeField] private float _zoomSensetivity;
    [Space]
    [SerializeField] private Camera _camera;

    private bool _isCustomSettings = false;

    private ValueRange _x;
    private ValueRange _y;
    private ValueRange _zoom;

    private void Start()
    {
        _x = new ValueRange(0, 1800f);
        _y = new ValueRange(0, 90f);
        _zoom = new ValueRange(zoomMax, 30f);
    }

    private void LateUpdate()
    {
        Vector3 movement = Vector3.zero;

        if (HotKey.GetKey(HotKeyType.CameraMoveForward))
            movement.z++;
        if (HotKey.GetKey(HotKeyType.CameraMoveBack))
            movement.z--;
        if (HotKey.GetKey(HotKeyType.CameraMoveLeft))
            movement.x--;
        if (HotKey.GetKey(HotKeyType.CameraMoveRight))
            movement.x++;

        if (_isCustomSettings == false)
        {
            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            _zoom.target = Mathf.Clamp(_zoom.target - mouseScroll * _zoomSensetivity, zoomMin, zoomMax);
        }
        _zoom.Update();
        _offset.z = -_zoom.current;

        if (HotKey.GetKey(HotKeyType.CameraRotateLeft))
            _x.target += _keysSensetivity * Time.deltaTime;
        if (HotKey.GetKey(HotKeyType.CameraRotateRight))
            _x.target -= _keysSensetivity * Time.deltaTime;

        if (Input.GetMouseButton(1))
        {
            _x.target += Input.GetAxis("Mouse X") * _sensetivity;
        }
        if (_isCustomSettings == false)
            _y.target = Mathf.Lerp(minYRotate, maxYRotate, 1f / (zoomMax - zoomMin) * (_zoom.current - zoomMin));
        _x.Update();
        _y.Update();
        _camera.transform.eulerAngles = new Vector3(_y.current, _x.current, 0);
        transform.eulerAngles = new Vector3(0, _x.current, 0);

        transform.Translate(movement * _speed * Time.deltaTime);

        Vector3 cameraPosition = _camera.transform.rotation * _offset + transform.position;
        _camera.transform.position = cameraPosition;
    }

    public void SetCustomSettings(float zoom, float axisY)
    {
        _zoom.target = zoom;
        _y.target = axisY;

        _isCustomSettings = true;
    }

    public void TurnCustomSettingsOff()
    {
        _isCustomSettings = false;
    }

    private class ValueRange
    {
        public float current;
        public float target;
        public float speed;

        public ValueRange(float value, float speed)
        {
            current = value;
            target = value;
            this.speed = speed;
        }

        public void Update()
        {
            current = Mathf.MoveTowards(current, target, speed * Time.deltaTime);
        }
    }

}

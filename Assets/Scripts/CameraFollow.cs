using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _minOrthographicSize = 5f;
    [SerializeField] private float _maxOrthographicSize = 15f;

    void Start()
    {
        _maxOrthographicSize = _offset.z;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = _player.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _offset.z -= scroll * 6f;
        _offset.z = Mathf.Clamp(_offset.z, _maxOrthographicSize, _minOrthographicSize);
    }
}

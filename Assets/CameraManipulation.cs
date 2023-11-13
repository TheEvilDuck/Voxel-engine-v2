using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{
    [SerializeField]float _minCameraAngle;
    [SerializeField]float _maxCameraAngle;
    private Quaternion _yQuat;
    private float _cameraAngle = 0;
    private Transform _cameraTransform;

    private void Awake() 
    {
        _cameraTransform = transform;
    }
    
    public void UpdateMouseMove(Vector2 mouseMove)
    {
		_cameraAngle += mouseMove.y;
		_cameraAngle = Mathf.Clamp(_cameraAngle, _minCameraAngle, _maxCameraAngle);
		_yQuat = Quaternion.AngleAxis(_cameraAngle, Vector3.left);
    }

    private void Update() 
    {
        _cameraTransform.localRotation = _yQuat;
    }
}

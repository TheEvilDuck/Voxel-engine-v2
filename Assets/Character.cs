using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class Character : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 2f;

    private Vector2 _moveVector;
    private Rigidbody _rigidBody;
    private float _rotationAngle;
    private Quaternion _xQuat;
    private Transform _transform;

    public Vector3 Position {get => _transform.position;}

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    public void SetMoveVector(Vector2 moveVector)
    {
        _moveVector = moveVector;
        if (_moveVector.magnitude<0.2f)
            _moveVector = Vector2.zero;
    }

    public void UpdateMouseMove(Vector2 mouseMove)
    {
		_rotationAngle += mouseMove.x;
		_xQuat = Quaternion.AngleAxis(_rotationAngle, Vector3.up);
    }
    private void FixedUpdate() 
    {

        Vector3 direction = _transform.forward*_moveVector.y + _transform.right*_moveVector.x;
        direction.Normalize();
        _rigidBody.velocity = new Vector3(direction.x*_moveSpeed,_rigidBody.velocity.y,direction.z*_moveSpeed);
        _rigidBody.rotation = _xQuat.normalized;
    }
}

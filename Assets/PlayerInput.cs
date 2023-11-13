using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> moveVectorChanged;
    public event Action<Vector2> mouseVectorChanged;
    public event Action leftMouseButtonPressed;
    public event Action rightMouseButtonPressed;

    private Vector2 _moveVector;
    private Vector2 _mouseVector;
    void Update()
    {
        Vector2 moveVectorNew = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        if (moveVectorNew!=_moveVector)
        {
            _moveVector = moveVectorNew;
            moveVectorChanged?.Invoke(_moveVector);
        }
        Vector2 mouseVectornew = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        if (mouseVectornew!=_mouseVector)
        {
            _mouseVector = mouseVectornew;
            mouseVectorChanged?.Invoke(_mouseVector);
        }
        if (Input.GetMouseButtonDown(0))
            leftMouseButtonPressed?.Invoke();
        if (Input.GetMouseButtonDown(1))
            rightMouseButtonPressed?.Invoke();
        
    }
}

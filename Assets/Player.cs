using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputActions input;
    private Vector3 moveDir;

    [SerializeField] private float moveSpeed;

    private void Awake() {
        input = new InputActions();

        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        moveDir = Vector3.zero;
    }

    private void OnEnable() {
        input.Enable();
    }

    private void OnDisable() {
        input.Disable();
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (obj.ReadValue<Vector2>().magnitude != 0) {
            moveDir = obj.ReadValue<Vector2>();
        } 
    }

    private void Update() {

        transform.position += (moveDir * moveSpeed * Time.deltaTime);
    }


}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EndingScreen : MonoBehaviour
{
    InputActions InputActions;
    private void Awake()
    {
        InputActions = new InputActions();
        InputActions.Enable();
    }
    private void Start()
    {
        InputActions.Player.ExitGame.performed += ExitGame_performed;
    }

    private void ExitGame_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}

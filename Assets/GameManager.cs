using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private InputActions input;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject sceneLoaderObject;

    private bool isPaused;

    public event EventHandler OnGamePaused;

    private void Awake() {
        if (instance == null) {
            instance = this;        
        } else {
            Destroy(gameObject);
        }

        input = new InputActions();
    }

    private void OnEnable() {
        input.Enable();
    }

    private void OnDisable() {
        input.Disable();
    }

    private void Start() {
        input.Player.Pause.performed += Pause_performed;
        input.Player.Restart.performed += Restart_performed;
        input.Player.ExitGame.performed += ExitGame_performed;
    }

    private void ExitGame_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Application.Quit();
    }

    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        ReloadLevel();
    }

    public void ShowGameOverScreen() {
        PauseGame();
        gameOverCanvas.SetActive(true);
    }

    public void ReloadLevel() {
        UnpauseGame();
        sceneLoaderObject.SetActive(true);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (isPaused) {
            UnpauseGame();
        } else {
            PauseGame();
        }
    }

    public void PauseGame() {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

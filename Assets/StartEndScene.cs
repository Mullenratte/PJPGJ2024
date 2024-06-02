using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class StartEndScene : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    int transitionTime = 3;
    bool isTransitioning;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent<Player>(out Player player)) {
            StartCoroutine(LoadEndScreenSceneAfterSeconds(transitionTime));
            isTransitioning = true;
        }
    }

    private void Update() {
        if (isTransitioning) {
            globalLight.intensity = Mathf.MoveTowards(globalLight.intensity, 0f, Time.deltaTime / transitionTime);
        }
    }

    IEnumerator LoadEndScreenSceneAfterSeconds(int seconds) {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("EndScreenScene", LoadSceneMode.Single);

    }
}

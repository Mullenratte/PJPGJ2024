using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable() {
        SceneManager.LoadScene("ArneScene", LoadSceneMode.Single);
    }
}

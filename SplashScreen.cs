using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] float timeSplashScreen = 1f;

    void Start()
    {
        Invoke("LoadFirstScene", timeSplashScreen);
    }
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}

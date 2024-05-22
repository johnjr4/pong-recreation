using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenu : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField]
    private float fadeOutTime = 0.5f;

    public void LeaveCheatMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        StartCoroutine(AudioFader.FadeOut(audioSource, fadeOutTime));
    }
}

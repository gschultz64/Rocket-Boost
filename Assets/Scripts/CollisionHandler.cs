using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay = 2f;
    [SerializeField] private AudioClip crash;
    [SerializeField] private AudioClip finish;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Collided with a friendly object");
                break;
            case "Finish":
                StartLoadSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        // TODO add particle effect on crash
        GetComponent<Movement>().enabled = false;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(crash);
        }
        else
        {
            audioSource.Stop();
        }
        Invoke(nameof(ReloadLevel), loadDelay);
    }

    private void StartLoadSequence()
    {
        // TODO add particle effect on finish
        GetComponent<Movement>().enabled = false;
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(finish);
        }
        else
        {
            audioSource.Stop();
        }
        Invoke(nameof(LoadNextLevel), loadDelay);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}

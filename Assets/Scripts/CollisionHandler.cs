using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float loadDelay = 2f;
    [SerializeField] private AudioClip crash;
    [SerializeField] private AudioClip finish;
    [SerializeField] private ParticleSystem crashParticles;
    [SerializeField] private ParticleSystem finishParticles;

    private AudioSource audioSource;

    private bool isTransitioning = false;
    private bool collisionToggle = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ApplyDebugKeys();
    }

    private void ApplyDebugKeys() // may want to add more for testing game behaviors!
    {
        if (Input.GetKey(KeyCode.L))
        {
            Debug.Log("L key has been pressed, loading next level...");
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("C key has been pressed, collisions now turned off.");
            collisionToggle = !collisionToggle; // toggle collision on/off with bool
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionToggle ) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartLoadSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    public void StartLoadSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finish);
        finishParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(LoadNextLevel), loadDelay);
    }
    private void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel), loadDelay);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}

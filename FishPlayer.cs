using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishPlayer : MonoBehaviour
{
    Rigidbody myRigidbody;

    //AUDIO
    AudioSource myAudioSource;
    [SerializeField] AudioClip soundThrust;
    [SerializeField] AudioClip soundHit;
    [SerializeField] AudioClip soundWin;

    //PARTICLES
    [SerializeField] ParticleSystem particlesThrust;
    [SerializeField] ParticleSystem particlesExplode;
    [SerializeField] ParticleSystem particlesWin;

    // ANIMATOR
    Animator animator;

    [SerializeField] float rotationThrust = 10f;
    [SerializeField] float multiplyThrust = 10f;
    [SerializeField] float levelLoadDelay = 1f;

    int currentSceneIndex;
    
    enum StatesPlayer {Alive, Dead, Transition};

    StatesPlayer state = StatesPlayer.Alive;
    
    bool collisionEnabled = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isLilyAlive", true);
        state = StatesPlayer.Alive;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();        
    }

    void Update()
    {
        if(state == StatesPlayer.Alive)
        {
            CheckThrust();
            CheckRotate();

            if (Debug.isDebugBuild) 
            {
                LevelUp();
                CollisionControl();
            }
        }        
    }    

    private void CheckThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            myAudioSource.Stop();
            particlesThrust.Stop();
        }
    }

    private void ApplyThrust()
    {
        myRigidbody.AddRelativeForce(Vector3.up * multiplyThrust * Time.deltaTime);

        particlesThrust.Play();        

        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(soundThrust, 1);
        }
    }

    private void CheckRotate()
    {
        myRigidbody.angularVelocity = Vector3.zero;
        float rotationSpeed = Time.deltaTime * rotationThrust;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {            
            transform.Rotate(Vector3.forward*rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
    }

    private void LevelUp()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
    }

    private void CollisionControl()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionEnabled = !collisionEnabled; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != StatesPlayer.Alive)
        {
            return;
        }
        
        switch (collision.collider.tag)
        {
            case "Friendly":
                break;
            case "LandingPadEnd":
                HandleWin();
                break;
            default:
                if (collisionEnabled == true)
                {
                    HandleDeath();
                }
                break;
        }
    }
    private void HandleWin()
    {
        state = StatesPlayer.Transition;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(soundWin);
        particlesWin.Play();
        
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void HandleDeath()
    {
        state = StatesPlayer.Dead;
        particlesExplode.Play();
        myAudioSource.Stop();        
        myAudioSource.PlayOneShot(soundHit, 1);
        animator.SetBool("isLilyAlive", false);
        Invoke("ReloadScene", levelLoadDelay);
    }
        

    private void LoadNextScene()
    {
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

}

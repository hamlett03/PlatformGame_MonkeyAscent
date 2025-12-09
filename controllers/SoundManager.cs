using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    instance = go.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    
    [Header("Audio clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip transitionSound;

    [SerializeField] private AudioClip jumpRealeseSound;
    [SerializeField] private AudioClip jumpLandingSound;
    [SerializeField] private AudioClip hardLandingSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip climbSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip toucanSound;

    [SerializeField] private float walkSoundInterval = 0.3f;
    private float walkSoundTimer = 0f;
    private bool isWalking = false;

    private void Awake()
    {
        // singleton pattern - ensure only one instance exist
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        if (music != null && musicSource != null)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }

        if (ambient != null && ambient != null)
        {
            ambientSource.clip = ambient;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    private void Update()
    {
        // handle walking sound loop
        if (isWalking)
        {
            walkSoundTimer -= Time.deltaTime;
            if (walkSoundTimer <= 0f)
            {
                PlayWalkSound();
                walkSoundTimer = walkSoundInterval;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayJumpRealese()
    {
        PlaySFX(jumpRealeseSound);
    }

    public void PlayJumpLanding()
    {
        PlaySFX(jumpLandingSound);
    }

    public void PlayHardLanding()
    {
        PlaySFX(hardLandingSound);
    }

    // camera snap
    public void PlayCameraSnap()
    {
        PlaySFX(transitionSound);
    }

    // walking sound
    public void StartWalkingSound()
    {
        if (!isWalking)
        {
            isWalking = true;
            walkSoundTimer = 0f;
        }
    }

    public void StopWalkingSound()
    {
        isWalking = false;
        walkSoundTimer = 0f;
    }

    private void PlayWalkSound()
    {
        PlaySFX(walkSound);
    }

    // climb sound
    public void PlayClimb()
    {
        PlaySFX(climbSound);
    }
}

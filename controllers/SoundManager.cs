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
    [SerializeField] private float climbSoundInterval = 0.3f;
    private float walkSoundTimer = 0f;
    private float climbSoundTimer = 0f;
    private bool isWalking = false;
    private bool isClimbing = false;
    private bool isMusicMuted = false;
    private bool isSfxMuted = false;
    private bool isAmbientMuted = false;


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

        // handle climbing sound loop
        if (isClimbing)
        {
            climbSoundTimer -= Time.deltaTime;
            if (climbSoundTimer <= 0f)
            {
                PlayClimb();
                climbSoundTimer = climbSoundInterval;
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

    public void StartClimbSound()
    {
        if (!isClimbing)
        {
            isClimbing = true;
            climbSoundTimer = 0f;
        }
    }

    public void StopClimbSound()
    {
        isClimbing = false;
        climbSoundTimer = 0f;
    }

    // power up sound
    public void PlayPowerUpSound()
    {
        PlaySFX(powerUpSound);
    }

    // dash sound
    public void PlayDashSound()
    {
        PlaySFX(dashSound);
    }

    // menu
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
    }

    public void ToggleSfx()
    {
        isSfxMuted = !isSfxMuted;
        sfxSource.mute = isSfxMuted;
    }

    public void ToggleAmbient()
    {
        isAmbientMuted = !isAmbientMuted;
        ambientSource.mute = isAmbientMuted;
    }

    public bool IsMusicMuted() => isMusicMuted;
    public bool IsSfxMuted() => isSfxMuted;
    public bool IsAmbientMuted() => isAmbientMuted;
}

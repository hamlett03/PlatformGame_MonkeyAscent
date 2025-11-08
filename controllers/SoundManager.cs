using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;
    
    [Header("Audio clip")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip ambient;
    [SerializeField] private AudioClip transitionSound;

    [SerializeField] private AudioClip jumpLandingSound;
    [SerializeField] private AudioClip hardLandingSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip powerUpSound;
    [SerializeField] private AudioClip climbSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip toucanSound;

    private void Start() 
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public AudioClip JumpLandingSound => jumpLandingSound;
    public AudioClip HardLandingSound => hardLandingSound;
    public AudioClip DashSound => dashSound;
    public AudioClip WalkSound => walkSound;
}

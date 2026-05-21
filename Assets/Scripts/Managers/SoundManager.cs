using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] public AudioClip DeadClickSFX;
    [SerializeField] public AudioClip GoodClickSFX;
    [SerializeField] public AudioClip SlideInSFX;
    [SerializeField] public AudioClip SlideOutSFX;
    [SerializeField] public AudioClip VoiceAudioClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            PlayClip(DeadClickSFX);
    }

    public void PlayClip(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void StopAudioClip()
    {
        if (sfxAudioSource.isPlaying)
        {
            sfxAudioSource.Stop();
        }
    }
}

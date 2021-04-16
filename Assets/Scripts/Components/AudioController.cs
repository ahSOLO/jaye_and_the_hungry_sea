using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Singleton
    public static AudioController aC;

    // Audio Sources
    public AudioSource musicSource;
    public AudioSource sFXSource;
    public AudioSource ambientSource;

    // Music Clips
    public AudioClip firstLevelMusic;
    public AudioClip secondLevelMusic;
    public AudioClip thirdLevelMusic;

    // Sound Effects
    public AudioClip bottlePickUp;
    public AudioClip[] boatCreak;
    public AudioClip[] hitHardDebris;
    public AudioClip[] hitEnemy;

    void OnEnable()
    {
        // Assign singleton - destroy all duplicates in existence
        if (aC == null) aC = this;
        else Destroy(gameObject);
    }

    public void PlayMusic(AudioClip clip, float volume) // Plays music on music audiosource
    {
        musicSource.volume = volume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void Play(AudioSource audioSource, AudioClip[] audioClips, float volume) // Plays random clip from array on specified audiosource
    {
        AudioClip clip = GetRandomClip(audioClips);
        audioSource.PlayOneShot(clip, volume);
    }

    public void Play(AudioSource audioSource, AudioClip audioClip, float volume) // Plays one shot clip on specified audiosource
    {
        audioSource.PlayOneShot(audioClip, volume);
    }

    public AudioSource PlayRandomSFXAtPoint(AudioClip[] audioClips, Vector3 position, float volume)
    {
        GameObject tempAudioClip = new GameObject("TempAudio");
        tempAudioClip.transform.position = position;
        AudioSource aSource = tempAudioClip.AddComponent<AudioSource>();
        AudioClip audioClip = GetRandomClip(audioClips);
        aSource.clip = audioClip;
        aSource.volume = volume;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        aSource.Play();
        Destroy(tempAudioClip, audioClip.length);
        return aSource;
    }

    public AudioSource PlaySFXAtPoint(AudioClip audioClip, Vector3 position, float volume)
    {
        GameObject tempAudioClip = new GameObject("TempAudio");
        tempAudioClip.transform.position = position;
        AudioSource aSource = tempAudioClip.AddComponent<AudioSource>();
        aSource.clip = audioClip;
        aSource.volume = volume;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        aSource.Play();
        Destroy(tempAudioClip, audioClip.length);
        return aSource;
    }

    public AudioClip GetRandomClip(AudioClip[] audioClips) // Gets a random audioclip from an audioclip array
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}

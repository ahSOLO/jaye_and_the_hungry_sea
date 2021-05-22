using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class AudioController : MonoBehaviour
{
    // Static var
    public static AudioController aC;

    // Audio Sources
    public AudioSource musicSource;
    public AudioSource sFXSource;
    public AudioSource rainSource1;
    public AudioSource rainSource2;
    public AudioSource rainSource3;

    // Music Clips
    public AudioClip titleMusic;
    public AudioClip firstLevelMusic;
    public AudioClip secondLevelMusic;
    public AudioClip thirdLevelMusic;
    public AudioClip fourthLevelMusic;
    public AudioClip endingMusic;

    // Sound Effects
    public AudioClip bottlePickUp;
    public AudioClip[] boatCreak;
    public AudioClip[] hitHardDebris;
    public AudioClip[] hitEnemy;
    public AudioClip rainSoft;
    public AudioClip rainHard;
    public AudioClip rainThunder;
    public AudioClip[] thunderOneShot;
    public AudioClip playerDeath;
    public AudioClip openNotes;
    public AudioClip closeNotes;
    public AudioClip[] changeNotes;
    public AudioClip[] cutsceneBarks;
    public AudioClip splash;
    public AudioClip clickButton;
    public AudioClip skullAttack;
    public AudioClip detachBodies;
    public AudioClip touchWOH;

    // Player reference
    private GameObject player;
    [SerializeField] private FloatVariable playerMaxSpeed;
    [SerializeField] private FloatVariable playerFastRowMultiplier;

    void OnEnable()
    {
        if (aC == null) aC = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject); // Persist between scenes
        SceneManager.sceneLoaded += OnSceneLoad;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Default music for each scene
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "0_Title")
        {
            aC.PlayMusic(titleMusic, 0f);
            StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
        }

        else if (SceneManager.GetActiveScene().name == "1_Introduction")
        {
            aC.Play(sFXSource, splash, 0.5f);
            aC.PlayMusic(firstLevelMusic, 0f);
            StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
        }

        else if (SceneManager.GetActiveScene().name == "2_Level1")
        {
            if (!aC.musicSource.isPlaying || aC.musicSource.clip != aC.firstLevelMusic)
            {
                aC.PlayMusic(firstLevelMusic, 0f);
                StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
            }
            player = GameObject.FindGameObjectWithTag("Player");
        }

        else if (SceneManager.GetActiveScene().name == "3_Cutscene1")
        {
            aC.PlayMusic(secondLevelMusic, 0f);
            StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
        }

        else if (SceneManager.GetActiveScene().name == "4_Level2")
        {
            if (!aC.musicSource.isPlaying || aC.musicSource.clip != aC.secondLevelMusic)
            {
                aC.PlayMusic(secondLevelMusic, 0f);
                StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
            }
            player = GameObject.FindGameObjectWithTag("Player");
        }

        else if (SceneManager.GetActiveScene().name == "5_Cutscene2")
        {
            aC.PlayMusic(thirdLevelMusic, 0f);
            StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
        }

        else if (SceneManager.GetActiveScene().name == "6_Level3")
        {
            if (!aC.musicSource.isPlaying || aC.musicSource.clip != aC.thirdLevelMusic)
            {
                aC.PlayMusic(thirdLevelMusic, 0f);
                StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
            }
            player = GameObject.FindGameObjectWithTag("Player");
        }

        else if (SceneManager.GetActiveScene().name == "7_Cutscene3")
        {
            aC.PlayMusic(fourthLevelMusic, 0f);
            StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
        }

        else if (SceneManager.GetActiveScene().name == "8_Level4")
        {
            if (!aC.musicSource.isPlaying || aC.musicSource.clip != aC.fourthLevelMusic)
            {
                aC.PlayMusic(fourthLevelMusic, 0f);
                StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0.55f));
            }
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void PlayMusic(AudioClip clip, float volume) // Plays clip on music audiosource
    {
        musicSource.volume = volume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void Play(AudioSource audioSource, AudioClip[] audioClips, float volume) // Plays random clip from array on specified audiosource
    {
        AudioClip audioClip = GetRandomClip(audioClips);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void Play(AudioSource audioSource, AudioClip audioClip, float volume) // Plays one shot clip on specified audiosource
    {
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public AudioSource PlayRandomSFXAtPoint(AudioClip[] audioClips, Vector3 position, float volume)
    {
        GameObject tempAudioClip = new GameObject("TempAudio");
        tempAudioClip.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
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
        tempAudioClip.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
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

    public void PauseActiveSources()
    {
        PauseIfActive(sFXSource);
        PauseIfActive(rainSource1);
        PauseIfActive(rainSource2);
        PauseIfActive(rainSource3);
    }

    private void PauseIfActive(AudioSource audioSource)
    {
        if (audioSource.isPlaying == true)
        {
            audioSource.Pause();
        }
    }

    public void UnPauseAudioSources()
    {
        sFXSource.UnPause();
        rainSource1.UnPause();
        rainSource2.UnPause();
        rainSource3.UnPause();
    }

    public void FadeRainSources(float target, float timer)
    {
        StartCoroutine(FadeAudioSource.StartFade(rainSource1, timer, target));
        StartCoroutine(FadeAudioSource.StartFade(rainSource2, timer, target));
        StartCoroutine(FadeAudioSource.StartFade(rainSource3, timer, target));
    }

    public void PlayOneShotThunder(bool isFlashing)
    {
        if (isFlashing) aC.PlayRandomSFXAtPoint(aC.thunderOneShot, player.transform.position, 0.5f);
    }

    public void PlayCreakSound()
    {
        aC.PlayRandomSFXAtPoint(boatCreak, player.transform.position, 0.4f);
    }

    public void PlayBottlePickupSound()
    {
        aC.PlaySFXAtPoint(bottlePickUp, player.transform.position, 0.25f);
    }

    public void PlayHitEnemySound(Collision2D collision)
    {
        aC.PlayRandomSFXAtPoint(hitEnemy, collision.contacts[0].point, 0.4f);
    }

    public void PlayHitSkullSound()
    {
        aC.PlaySFXAtPoint(skullAttack, player.transform.position, 0.5f);
    }

    public void PlayTouchWOHSound()
    {
        aC.PlaySFXAtPoint(touchWOH, player.transform.position, 1f);
    }

    public void PlayHitHardDebrisSound(Collision2D collision)
    {
        float impactVolume = collision.relativeVelocity.sqrMagnitude / Mathf.Pow(playerMaxSpeed.Value * playerFastRowMultiplier.Value, 2) * 0.6f;
        aC.PlayRandomSFXAtPoint(hitHardDebris, collision.contacts[0].point, impactVolume);
    }

    public void PlayDetachBodiesSound()
    {
        aC.PlaySFXAtPoint(detachBodies, player.transform.position, 0.5f);
    }

    public void LevelEnd()
    {
        StartCoroutine(FadeAudioSource.StartFade(musicSource, 3f, 0f));
        aC.FadeRainSources(0f, 3f);
    }
}

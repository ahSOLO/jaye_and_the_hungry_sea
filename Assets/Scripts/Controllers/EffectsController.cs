using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    // Static var
    public static EffectsController eC;

    // Object References
    public List<GameObject> lightningEffects;
    public List<GameObject> ripplesTilemapObjs;
    public GameObject surfaceWaveTilemapObj;
    public GameObject boatLight;
    public GameObject player;
    public GameObject background;
    public GameObject lightningGenerator;
    public GameObject lantern;
    public GameObject particleRainObj;
    public GameObject particleRipplesObj;
    public GameObject globalLightObj;
    public GameObject skullCircle;
    public GameObject skullLeftEyeLightObj;
    public GameObject skullRightEyeLightObj;
    // public GameObject fadeToBlackImgObj;

    // States
    public enum RainState { off, soft, medium, heavy, thunderStorm };
    public RainState rState;

    // Lightning vars
    public bool isFlashing;
    private float lightningTimer;

    // Renderers
    private SpriteRenderer bgRenderer;
    // private Image fadeToBlackImg; 

    // Tilemap
    private Tilemap surfaceWaveTilemap;

    // Lights
    private Light2D lanternLight;

    // Particle Systems
    private ParticleSystem particleRain;
    private ParticleSystem particleRipples;

    // Audio vars
    private float fadeDuration = 2f;
    private float rainSoftVolume = 0.12f;
    private float rainHardVolume = 0.15f;
    private float rainThunderVolume = 0.20f;

    private void OnEnable()
    {
        eC = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFlashing = false;
        lightningTimer = 0f;

        particleRain = particleRainObj.GetComponent<ParticleSystem>();
        particleRipples = particleRipplesObj.GetComponent<ParticleSystem>();

        //Get Renderers
        bgRenderer = background.GetComponent<SpriteRenderer>();
        surfaceWaveTilemap = surfaceWaveTilemapObj.GetComponent<Tilemap>();

        lanternLight = lantern.GetComponent<Light2D>();

        // Start Rain - dependent on rState - set in Inspector
        Rain();
    }

    // Update is called once per frame
    void Update()
    {
        // Lightning color inversion effect
        if (isFlashing == true)
        {
            bgRenderer.color = Color.white;
            surfaceWaveTilemap.color = Color.black;
            if (lantern != null) lantern.transform.localPosition = new Vector3(-100, -100, 4);
            if (skullCircle != null) skullCircle.SetActive(false);
            if (skullLeftEyeLightObj != null) skullLeftEyeLightObj.SetActive(false);
            if (skullRightEyeLightObj != null) skullRightEyeLightObj.SetActive(false);
            UIManager.uIM.SetHeartIcon(false);
        }

        else
        {
            bgRenderer.color = Color.black;
            surfaceWaveTilemap.color = Color.white;
            if (lantern != null) lantern.transform.localPosition = new Vector3(0, 0.833f, 4);
            if (skullCircle != null) skullCircle.SetActive(true);
            if (skullLeftEyeLightObj != null) skullLeftEyeLightObj.SetActive(true);
            if (skullRightEyeLightObj != null) skullRightEyeLightObj.SetActive(true);
            UIManager.uIM.SetHeartIcon(true);
        }

        // Debug lightning
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(-8f, 8f), player.transform.position.y + Random.Range(2.0f, 7f), 0);
            Instantiate(lightningEffects[Random.Range(0, lightningEffects.Count)], spawnPosition, Quaternion.identity, lightningGenerator.transform);
        }
        */

        // Debug rain
        /*
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            var rainEm = particleRain.emission;
            var ripplesEm = particleRipples.emission;

            if (!weatherToggleDecrease)
            {
                IncreaseRainState();
                if (rState == RainState.thunderStorm)
                {
                    weatherToggleDecrease = true;
                }
            } 
            else
            {
                DecreaseRainState();
                if (rState == RainState.off)
                {
                    weatherToggleDecrease = false;
                }
            }
            Rain();
        }
        */

        Lightning();
    }

    void Rain()
    {
        var rainEm = particleRain.emission;
        var ripplesEm = particleRipples.emission;

        // Rain switches between 5 different states
        switch (rState)
        {
            case RainState.off:
                ripplesTilemapObjs[0].SetActive(false);
                particleRainObj.SetActive(false);
                particleRipplesObj.SetActive(false);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, 0f));

                break;
            case RainState.soft:
                particleRainObj.SetActive(true);
                particleRipplesObj.SetActive(true);

                rainEm.rateOverTime = 225f;
                ripplesEm.rateOverTime = 175f;

                ripplesTilemapObjs[0].SetActive(false);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource1, AudioController.aC.rainSoft, 0f);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, rainSoftVolume));

                break;
            case RainState.medium:
                particleRainObj.SetActive(true);
                particleRipplesObj.SetActive(true);

                rainEm.rateOverTime = 550f;
                ripplesEm.rateOverTime = 300f;

                ripplesTilemapObjs[0].SetActive(false);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, 0f));
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource2, AudioController.aC.rainHard, 0f);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, rainHardVolume));

                break;
            case RainState.heavy:
                particleRainObj.SetActive(true);
                particleRipplesObj.SetActive(true);

                rainEm.rateOverTime = 1000f;
                ripplesEm.rateOverTime = 650f;

                ripplesTilemapObjs[0].SetActive(true);
                lightningTimer = 2f;

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource3, AudioController.aC.rainThunder, AudioController.aC.rainSource3.volume);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, rainHardVolume));

                break;
            case RainState.thunderStorm:
                particleRainObj.SetActive(true);
                particleRipplesObj.SetActive(true);

                rainEm.rateOverTime = 1600f;
                ripplesEm.rateOverTime = 1000f;

                ripplesTilemapObjs[0].SetActive(true);

                if (!AudioController.aC.rainSource3.isPlaying) AudioController.aC.Play(AudioController.aC.rainSource3, AudioController.aC.rainThunder, AudioController.aC.rainSource3.volume);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, rainThunderVolume));

                break;
        }
    }

    void Lightning()
    {
        lightningTimer -= Time.deltaTime;
        if (lightningTimer <= 0f && rState >= RainState.heavy && player != null)
        {            
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(-9f, 9f), player.transform.position.y + Random.Range(2.0f, 9f), 0);
            Instantiate(lightningEffects[Random.Range(0, lightningEffects.Count)], spawnPosition, Quaternion.identity, lightningGenerator.transform);

            float timerMin = 3f;
            float timerMax = 7f;

            if (rState == RainState.thunderStorm)
            {
                timerMin = 2f;
                timerMax = 3.5f;
            }

            AudioController.aC.PlayRandomSFXAtPoint(AudioController.aC.thunderOneShot, player.transform.position, 0.5f);

            lightningTimer = Random.Range(timerMin, timerMax);
        }
    }

    public void IncreaseRainState()
    {
        switch (rState)
        {
            case RainState.off:
                rState = RainState.soft;
                break;
            case RainState.soft:
                rState = RainState.medium;
                break;
            case RainState.medium:
                rState = RainState.heavy;
                break;
            case RainState.heavy:
                rState = RainState.thunderStorm;
                break;
            case RainState.thunderStorm:
                break;
        }
        Rain();
    }

    public void DecreaseRainState()
    {
        switch (rState)
        {
            case RainState.off:
                break;
            case RainState.soft:
                rState = RainState.off;
                break;
            case RainState.medium:
                rState = RainState.soft;
                break;
            case RainState.heavy:
                rState = RainState.medium;
                break;
            case RainState.thunderStorm:
                rState = RainState.heavy;
                break;
        }
        Rain();
    }

    public IEnumerator PlayerDeath(float timer, Vector3 playerPosition)
    {
        while (timer > 0)
        {
            globalLightObj.SetActive(false);
            if (SkullAI.sAI != null) SkullAI.sAI.gameObject.SetActive(false);
            lanternLight.pointLightInnerRadius = Mathf.Lerp(lanternLight.pointLightInnerRadius, 0f, Time.deltaTime * 2f);
            lanternLight.pointLightOuterRadius = Mathf.Lerp(lanternLight.pointLightOuterRadius, 0f, Time.deltaTime * 2f);
            timer -= Time.deltaTime;
            yield return null;
        }

        AudioController.aC.PlaySFXAtPoint(AudioController.aC.playerDeath, playerPosition, 0.4f);
        
        yield return new WaitForSeconds(4f);

        GameController.gC.RestartScene();
    }
}

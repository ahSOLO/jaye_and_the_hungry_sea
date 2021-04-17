using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    // Singleton variable
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

    // States
    private enum rainState { off, soft, medium, heavy, thunderStorm };
    private rainState rState;

    // Lightning vars
    public bool isFlashing;
    private float lightningTimer;

    // Renderers
    private SpriteRenderer bgRenderer;

    // Tilemap
    private Tilemap surfaceWaveTilemap;

    // Lights
    private Light2D lanternLight;

    // Lantern Variables
    public float lanternInnerRadius;
    public float lanternOuterRadius;

    // Particle Systems
    private ParticleSystem particleRain;
    private ParticleSystem particleRipples;

    // Audio vars
    private float fadeDuration = 2f;
    private float rainSoftVolume = 0.15f;
    private float rainHardVolume = 0.20f;
    private float rainThunderVolume = 0.25f;

    // Debug
    private bool weatherToggleDecrease = false;

    private void OnEnable()
    {
        //Assign Singleton
        if (eC == null) eC = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rState = rainState.off;
        isFlashing = false;
        lightningTimer = 0f;

        lanternLight = lantern.GetComponent<Light2D>();

        particleRain = particleRainObj.GetComponent<ParticleSystem>();
        particleRipples = particleRipplesObj.GetComponent<ParticleSystem>();

        //Get Renderers
        bgRenderer = background.GetComponent<SpriteRenderer>();
        surfaceWaveTilemap = surfaceWaveTilemapObj.GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lightning
        if (isFlashing == true)
        {
            bgRenderer.color = Color.white;
            surfaceWaveTilemap.color = Color.black;
            lanternLight.pointLightOuterRadius = 9f;
            lanternLight.pointLightInnerRadius = 9f;
        }

        else
        {
            bgRenderer.color = Color.black;
            surfaceWaveTilemap.color = Color.white;
            lanternLight.pointLightInnerRadius = lanternInnerRadius;
            lanternLight.pointLightOuterRadius = lanternOuterRadius;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(-8f, 8f), player.transform.position.y + Random.Range(2.0f, 7f), 0);
            Instantiate(lightningEffects[Random.Range(0, lightningEffects.Count)], spawnPosition, Quaternion.identity, lightningGenerator.transform);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            var rainEm = particleRain.emission;
            var ripplesEm = particleRipples.emission;

            if (!weatherToggleDecrease)
            {
                IncreaseRainState();
                if (rState == rainState.thunderStorm)
                {
                    weatherToggleDecrease = true;
                }
            } 
            else
            {
                DecreaseRainState();
                if (rState == rainState.off)
                {
                    weatherToggleDecrease = false;
                }
            }
            Rain();
        }
        Lightning();
    }

    void Rain()
    {
        var rainEm = particleRain.emission;
        var ripplesEm = particleRipples.emission;
        switch (rState)
        {
            case rainState.off:
                ripplesTilemapObjs[0].SetActive(false);
                particleRainObj.SetActive(false);
                particleRipplesObj.SetActive(false);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, 0f));

                break;
            case rainState.soft:
                particleRainObj.SetActive(true);
                particleRipplesObj.SetActive(true);

                rainEm.rateOverTime = 175f;
                ripplesEm.rateOverTime = 125f;

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource1, AudioController.aC.rainSoft, 0f);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, rainSoftVolume));

                break;
            case rainState.medium:
                rainEm.rateOverTime = 400f;
                ripplesEm.rateOverTime = 250f;
                ripplesTilemapObjs[0].SetActive(false);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource1, fadeDuration, 0f));
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource2, AudioController.aC.rainHard, 0f);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, rainHardVolume));

                break;
            case rainState.heavy:
                rainEm.rateOverTime = 700f;
                ripplesEm.rateOverTime = 450f;
                ripplesTilemapObjs[0].SetActive(true);

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource2, fadeDuration, 0f));
                AudioController.aC.Play(AudioController.aC.rainSource3, AudioController.aC.rainThunder, AudioController.aC.rainSource3.volume);
                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, rainHardVolume));

                break;
            case rainState.thunderStorm:
                rainEm.rateOverTime = 1200f;
                ripplesEm.rateOverTime = 800f;

                StartCoroutine(FadeAudioSource.StartFade(AudioController.aC.rainSource3, fadeDuration, rainThunderVolume));

                break;
        }
    }

    void Lightning()
    {
        lightningTimer -= Time.deltaTime;
        if (lightningTimer <= 0f && rState >= rainState.heavy)
        {            
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(-8f, 8f), player.transform.position.y + Random.Range(2.0f, 7f), 0);
            Instantiate(lightningEffects[Random.Range(0, lightningEffects.Count)], spawnPosition, Quaternion.identity, lightningGenerator.transform);

            float timerMin = 4f;
            float timerMax = 8f;

            if (rState == rainState.thunderStorm)
            {
                timerMin = 2f;
                timerMax = 4f;
            }

            lightningTimer = Random.Range(timerMin, timerMax);
        }
    }

    public void IncreaseRainState()
    {
        switch (rState)
        {
            case rainState.off:
                rState = rainState.soft;
                break;
            case rainState.soft:
                rState = rainState.medium;
                break;
            case rainState.medium:
                rState = rainState.heavy;
                break;
            case rainState.heavy:
                rState = rainState.thunderStorm;
                break;
            case rainState.thunderStorm:
                break;
        }
        Rain();
    }

    public void DecreaseRainState()
    {
        switch (rState)
        {
            case rainState.off:
                break;
            case rainState.soft:
                rState = rainState.off;
                break;
            case rainState.medium:
                rState = rainState.soft;
                break;
            case rainState.heavy:
                rState = rainState.medium;
                break;
            case rainState.thunderStorm:
                rState = rainState.heavy;
                break;
        }
        Rain();
    }

    public IEnumerator PlayerDeath(float timer, Vector3 playerPosition)
    {
        while (timer > 0)
        {
            globalLightObj.SetActive(false);
            lanternInnerRadius = Mathf.Lerp(lanternInnerRadius, 0f, Time.deltaTime * 2f);
            lanternOuterRadius = Mathf.Lerp(lanternOuterRadius, 0f, Time.deltaTime * 2f);
            timer -= Time.deltaTime;
            yield return null;
        }

        AudioController.aC.PlaySFXAtPoint(AudioController.aC.playerDeath, playerPosition, 0.4f);
        yield break;
    }
}

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

    // States
    private enum rainState { off, soft, medium, heavy };
    private rainState rState;

    public bool isFlashing;

    // Renderers
    private SpriteRenderer bgRenderer;

    // Tilemap
    private Tilemap surfaceWaveTilemap;

    // Lights
    private Light2D lanternLight;

    // Lantern Variables
    [SerializeField] private float lanternInnerRadius;
    [SerializeField] private float lanternOuterRadius;

    // Particle Systems
    private ParticleSystem particleRain;
    private ParticleSystem particleRipples;

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
            switch (rState)
            {
                case rainState.heavy:
                    rState = rainState.off;
                    ripplesTilemapObjs[0].SetActive(false);
                    particleRainObj.SetActive(false);
                    particleRipplesObj.SetActive(false);
                    break;
                case rainState.off:
                    rState = rainState.soft;
                    particleRainObj.SetActive(true);
                    particleRipplesObj.SetActive(true);
                    
                    rainEm.rateOverTime = 150f;
                    ripplesEm.rateOverTime = 100f;
                    break;
                case rainState.soft:
                    rState = rainState.medium;

                    rainEm.rateOverTime = 300f;
                    ripplesEm.rateOverTime = 200f;
                    break;
                case rainState.medium:
                    rState = rainState.heavy;

                    rainEm.rateOverTime = 600f;
                    ripplesEm.rateOverTime = 400f;
                    ripplesTilemapObjs[0].SetActive(true);
                    break;
            }
        }
    }
}

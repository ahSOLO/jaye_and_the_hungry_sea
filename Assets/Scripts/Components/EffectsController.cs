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
    public List<GameObject> rainEffects;
    public List<GameObject> lightningEffects;
    public List<GameObject> ripplesTilemapObjs;
    public GameObject surfaceWaveTilemapObj;
    public GameObject boatLight;
    public GameObject player;
    public GameObject background;
    public GameObject lightningGenerator;
    public GameObject lantern;

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

    private void OnEnable()
    {
        //Assign Singleton
        if (eC == null) eC = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        rState = rainState.soft;
        isFlashing = false;

        lanternLight = lantern.GetComponent<Light2D>();

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
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(-7f, 7f), player.transform.position.y + Random.Range(2.0f, 4f), 0);
            Instantiate(lightningEffects[Random.Range(0, lightningEffects.Count)], spawnPosition, Quaternion.identity, lightningGenerator.transform);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            switch (rState)
            {
                case rainState.off:
                    rState = rainState.soft;
                    rainEffects[2].SetActive(false);
                    break;
                case rainState.soft:
                    rState = rainState.medium;
                    rainEffects[0].SetActive(true);
                    break;
                case rainState.medium:
                    rState = rainState.heavy;
                    rainEffects[0].SetActive(false);
                    rainEffects[1].SetActive(true);
                    break;
                case rainState.heavy:
                    rState = rainState.off;
                    rainEffects[1].SetActive(false);
                    rainEffects[2].SetActive(true);
                    break;
            }
        }
    }
}

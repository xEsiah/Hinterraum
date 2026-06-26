using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class AtmosphereManager : MonoBehaviour
{
    public static AtmosphereManager instance;

    public Volume globalVolume;
    public Light directionalLight;
    
    public AudioClip backroomAmbientSound;
    public AudioClip breathingSound;
    public AudioClip heartbeatSound;
    
    public float delayBeforeStress = 30f;

    public float vignetteStart = 0.2f;
    public float vignetteEnd = 0.6f;
    
    public float dofStart = 5f;
    public float dofEnd = 1.5f;
    
    public float exposureStart = 0f;
    public float exposureEnd = -1.5f;
    
    public float lightStart = 1f;
    public float lightEnd = 0.2f;

    public float heartbeatSlowInterval = 1.5f;
    public float heartbeatFastInterval = 0.4f;

    private AudioSource audioSource;
    private Vignette vignette;
    private DepthOfField depthOfField;
    private ColorAdjustments colorAdjustments;

    private float maxTime;
    private float currentTime;
    
    private float breathingTimer;
    private float heartbeatTimer;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backroomAmbientSound;
        audioSource.loop = true;
        audioSource.Play();

        maxTime = GameManager.instance.playthroughDuration;
        
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
            globalVolume.profile.TryGet(out depthOfField);
            globalVolume.profile.TryGet(out colorAdjustments);
        }
        
        ResetAtmosphere();
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        float elapsedTime = maxTime - currentTime;

        float tensionFactor = 0f;
        if (elapsedTime > delayBeforeStress)
        {
            float stressDuration = maxTime - delayBeforeStress;
            float currentStressTime = elapsedTime - delayBeforeStress;
            tensionFactor = Mathf.Pow(currentStressTime / stressDuration, 3f);
        }

        UpdateVisuals(tensionFactor);
        UpdateLighting(tensionFactor);

        breathingTimer -= Time.deltaTime;
        if (breathingTimer <= 0f)
        {
            if (breathingSound != null) audioSource.PlayOneShot(breathingSound);
            breathingTimer = Random.Range(4f, 8f);
        }

        if (tensionFactor > 0f)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f)
            {
                if (heartbeatSound != null) audioSource.PlayOneShot(heartbeatSound);
                heartbeatTimer = Mathf.Lerp(heartbeatSlowInterval, heartbeatFastInterval, tensionFactor);
            }
        }
    }

    public void ResetAtmosphere()
    {
        maxTime = GameManager.instance.playthroughDuration;
        currentTime = maxTime;
        
        breathingTimer = Random.Range(2f, 5f);
        heartbeatTimer = heartbeatSlowInterval;
        
        UpdateVisuals(0f);
        UpdateLighting(0f);
        
        if (audioSource != null)
        {
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
        }
    }

    private void UpdateVisuals(float factor)
    {
        if (vignette != null) vignette.intensity.value = Mathf.Lerp(vignetteStart, vignetteEnd, factor);
        if (depthOfField != null) depthOfField.focusDistance.value = Mathf.Lerp(dofStart, dofEnd, factor);
        if (colorAdjustments != null) colorAdjustments.postExposure.value = Mathf.Lerp(exposureStart, exposureEnd, factor);
    }

    private void UpdateLighting(float factor)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = Mathf.Lerp(lightStart, lightEnd, factor);
        }
    }
}
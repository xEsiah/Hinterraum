using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class AtmosphereManager : MonoBehaviour
{
    public static AtmosphereManager instance;

    public Volume globalVolume;
    public Light directionalLight;
    public AudioClip tensionSound;
    
    public AudioClip randomSound1;
    public AudioClip randomSound2;
    
    public float delayBeforeStress = 30f;
    public float randomSoundStartRemaining = 80f;
    public float randomSoundEndRemaining = 5f;

    private AudioSource audioSource;
    private Vignette vignette;
    private DepthOfField depthOfField;
    private ColorAdjustments colorAdjustments;

    private float maxTime;
    private float currentTime;
    
    private float randomTimer;
    private bool playFirstRandom = true;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = tensionSound;
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

        UpdateAudio(tensionFactor);
        UpdateVisuals(tensionFactor);
        UpdateLighting(tensionFactor);

        if (currentTime <= randomSoundStartRemaining && currentTime >= randomSoundEndRemaining)
        {
            randomTimer -= Time.deltaTime;
            if (randomTimer <= 0f)
            {
                audioSource.PlayOneShot(playFirstRandom ? randomSound1 : randomSound2);
                playFirstRandom = !playFirstRandom;
                
                float progress = 1f - ((currentTime - randomSoundEndRemaining) / (randomSoundStartRemaining - randomSoundEndRemaining));
                randomTimer = Mathf.Lerp(4f, 0.5f, progress);
            }
        }
    }

    public void ResetAtmosphere()
    {
        maxTime = GameManager.instance.playthroughDuration;
        currentTime = maxTime;
        randomTimer = 2f;
        UpdateAudio(0f);
        UpdateVisuals(0f);
        UpdateLighting(0f);
    }

    private void UpdateAudio(float factor)
    {
        audioSource.pitch = Mathf.Lerp(0.8f, 2.0f, factor);
        audioSource.volume = Mathf.Lerp(0f, 1.0f, factor); 
    }

    private void UpdateVisuals(float factor)
    {
        if (vignette != null) vignette.intensity.value = Mathf.Lerp(0.2f, 0.6f, factor);
        if (depthOfField != null) depthOfField.focusDistance.value = Mathf.Lerp(5f, 1.5f, factor);
        if (colorAdjustments != null) colorAdjustments.postExposure.value = Mathf.Lerp(0f, -1.5f, factor);
    }

    private void UpdateLighting(float factor)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = Mathf.Lerp(1f, 0.2f, factor);
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    public Animator playerAnimator;
    public Collider playerCollider;
    
    public Camera menuCamera;
    public Camera cinematicCamera;
    public Camera cinematicCameraTop;
    public VideoPlayer cinematicVideoPlayer;
    public GameObject[] cinematicObjects;
    
    [Header("Audio")]
    public AudioSource bgmSource;
    public float fadeDuration = 2f;

    private string targetScene = "Backrooms";
    private bool isStarting = false;

    public void LoadEasyLevel()
    {
        GameSettings.isHardMode = false;
        StartCinematic();
    }

    public void LoadHardcoreLevel()
    {
        GameSettings.isHardMode = true;
        StartCinematic();
    }

    private void StartCinematic()
    {
        if (isStarting) return;
        isStarting = true;

        if (menuCamera != null) menuCamera.gameObject.SetActive(false);
        if (cinematicCamera != null) cinematicCamera.gameObject.SetActive(true);

        foreach (GameObject obj in cinematicObjects) if (obj != null) obj.SetActive(true);

        if (cinematicVideoPlayer != null) cinematicVideoPlayer.Play();
        
        if (bgmSource != null) StartCoroutine(FadeOutAudio());

        StartCoroutine(CinematicRoutine());
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }
        bgmSource.Stop();
    }

    private IEnumerator CinematicRoutine()
    {
        yield return new WaitForSeconds(5f);
        if (playerCollider != null) playerCollider.enabled = false;
        if (playerAnimator != null) playerAnimator.SetTrigger("Fall");
        if (cinematicCameraTop != null) cinematicCameraTop.gameObject.SetActive(true);
        if (cinematicCamera != null) cinematicCamera.gameObject.SetActive(false);
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetScene)) SceneManager.LoadScene(targetScene);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
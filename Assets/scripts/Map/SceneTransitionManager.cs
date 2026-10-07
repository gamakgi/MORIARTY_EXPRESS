using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;

    private const float FadeTime = 0.4f;
    private const float BlackHoldTime = 0.35f;

    private Image fadeImage;
    private bool isTransitioning;
    private bool firstSceneLoaded;

    public static bool IsTransitioning
    {
        get { return instance != null && instance.isTransitioning; }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateManager()
    {
        if (instance != null)
        {
            return;
        }

        GameObject managerObject = new GameObject("SceneTransitionManager");
        instance = managerObject.AddComponent<SceneTransitionManager>();
        DontDestroyOnLoad(managerObject);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        CreateFadeScreen();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void CreateFadeScreen()
    {
        GameObject canvasObject = new GameObject(
            "SceneFadeCanvas",
            typeof(RectTransform),
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        canvasObject.transform.SetParent(transform, false);

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 32767;

        CanvasScaler canvasScaler = canvasObject.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920f, 1080f);

        GameObject imageObject = new GameObject(
            "BlackScreen",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));

        imageObject.transform.SetParent(canvasObject.transform, false);

        RectTransform imageRect = imageObject.GetComponent<RectTransform>();
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;
        imageRect.localScale = Vector3.one;

        fadeImage = imageObject.GetComponent<Image>();
        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage.raycastTarget = false;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 처음 실행한 씬은 그대로 보여주고, 다음 씬부터 검정 화면에서 밝힌다.
        if (firstSceneLoaded == false)
        {
            firstSceneLoaded = true;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public static void LoadScene(string sceneName)
    {
        if (instance == null || instance.isTransitioning)
        {
            return;
        }

        instance.StartCoroutine(instance.FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isTransitioning = true;
        fadeImage.raycastTarget = true;

        float timer = 0f;
        while (timer < FadeTime)
        {
            timer += GetFrameTime();
            SetAlpha(Mathf.Clamp01(timer / FadeTime));
            yield return null;
        }

        SetAlpha(1f);

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        if (loading == null)
        {
            Debug.LogError("씬을 불러오지 못했습니다: " + sceneName);
            SetAlpha(0f);
            fadeImage.raycastTarget = false;
            isTransitioning = false;
            yield break;
        }

        while (loading.isDone == false)
        {
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        isTransitioning = true;
        fadeImage.raycastTarget = true;
        SetAlpha(1f);

        float holdTimer = 0f;
        while (holdTimer < BlackHoldTime)
        {
            holdTimer += GetFrameTime();
            yield return null;
        }

        float timer = 0f;
        while (timer < FadeTime)
        {
            timer += GetFrameTime();
            SetAlpha(1f - Mathf.Clamp01(timer / FadeTime));
            yield return null;
        }

        SetAlpha(0f);
        fadeImage.raycastTarget = false;
        isTransitioning = false;
    }

    private float GetFrameTime()
    {
        return Mathf.Min(Time.unscaledDeltaTime, 0.05f);
    }

    private void SetAlpha(float alpha)
    {
        fadeImage.color = new Color(0f, 0f, 0f, alpha);
    }
}

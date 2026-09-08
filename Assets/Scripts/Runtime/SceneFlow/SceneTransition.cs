using System.Collections;
using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    #region 인스펙터
    [Header("페이드 옵션")]
    [SerializeField] private CanvasGroup _fadeGroup;
    [SerializeField] private bool _useUnscale = true;

    [Header("로딩 텍스트")] 
    [SerializeField] private TMP_Text _loadingText;

    #endregion

    #region 내부 변수
    private Coroutine _loadingCoroutine;
    #endregion

    public void InitFadeGroup()
    {
        if (_fadeGroup == null)
        {
            Debug.LogWarning("캔버스 그룹 null / 인스펙터 확인");

            return;
        }

        _fadeGroup.alpha = 0.0f;
        _fadeGroup.blocksRaycasts = false;
        _fadeGroup.interactable = false;

        SetLoadingText("");
    }

    public void SetLoadingText(string text)
    {
        if (_loadingText == null)
        {
            return;
        }

        _loadingText.text = text;
    }

    public IEnumerator Co_Fade(float targetAlpha, float fadeDuration = 0.5f, bool blockRaycast = true)
    {
        if (_fadeGroup == null)
        {
            Debug.LogWarning("캔버스 그룹 null / 확인 요망");

            yield break;
        }

        if (_loadingCoroutine != null)
        {
            StopCoroutine(_loadingCoroutine);
            _loadingCoroutine = null;
        }

        _loadingCoroutine = StartCoroutine(Co_Fade_Internal(targetAlpha, fadeDuration));

        yield return _loadingCoroutine;

        _loadingCoroutine = null;
    }

    private IEnumerator Co_Fade_Internal(float targetAlpha, float fadeDuration, bool blockRaycast = true)
    {
        _fadeGroup.blocksRaycasts = blockRaycast;

        if (fadeDuration <= 0.0f)
        {
            _fadeGroup.alpha = targetAlpha;
            _fadeGroup.blocksRaycasts = (targetAlpha >= 0.99f);

            yield break;
        }
        
        float startAlpha = _fadeGroup.alpha;
        float time = 0.0f;

        while (time < fadeDuration)
        {
            float dt = _useUnscale ? Time.unscaledDeltaTime : Time.deltaTime;
            time += dt;

            float lerp = Mathf.Clamp01(time / fadeDuration);
            _fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, lerp);

            yield return null;
        }

        _fadeGroup.alpha = targetAlpha;
        _fadeGroup.blocksRaycasts = (targetAlpha >= 0.99f);

    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    #region 인스펙터
    [Header("페이드 옵션")]
    [SerializeField] private CanvasGroup _fadeGroup;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private bool _useUnscale = true;

    [Header("로딩 텍스트")] 
    [SerializeField] private TMP_Text _loadingText;

    #endregion

    #region 내부 변수

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



}

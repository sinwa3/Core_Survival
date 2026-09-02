using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISound : MonoBehaviour
{
    #region 인스펙터
    [Header("사운드 클립")]
    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _levelUpClip;

    [SerializeField] private PlayerLevel _levelUpEvent;
    #endregion

    #region 내부 변수
    private AudioSource _audioSource;
    #endregion

    private void OnEnable()
    {
        if (_levelUpEvent != null)
        {
            _levelUpEvent.OnLevelUp += LevelUpSoundPlay;
        }
    }

    private void OnDisable()
    {
        if (_levelUpEvent != null)
        {
            _levelUpEvent.OnLevelUp -= LevelUpSoundPlay;
        }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning("AudioSource 컴포넌트 null (UISound) / 확인 요망");
            enabled = false;

            return;
        }

        _audioSource.ignoreListenerPause = true;


    }

    void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        for(int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            button.onClick.AddListener(ClickSoundPlay);
        }
    }

    private void ClickSoundPlay()
    {
        if(_clickClip == null)
        {
            return;
        }

        _audioSource.PlayOneShot(_clickClip);
    }

    private void LevelUpSoundPlay()
    {
        if(_levelUpClip == null)
        {
            return;
        }

        _audioSource.PlayOneShot(_levelUpClip);
    }
}

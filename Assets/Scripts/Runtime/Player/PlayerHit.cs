using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    #region 인스펙터
    [Header("참조")]
    [SerializeField] private Player _player;
    // 확인용
    [SerializeField] private Renderer[] _renderers;

    [Header("깜빡임")]
    [SerializeField] private float _blinkInterval = 0.08f;

    [Header("사운드")]
    [SerializeField] private AudioSource _audioSource;
    #endregion

    private void OnEnable()
    {
        if (_player != null)
        {
            _player.OnDamaged += OnDamaged;
        }
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.OnDamaged -= OnDamaged;
        }
    }


    private void Awake()
    {
        if (_player == null)
        {
            Debug.LogWarning("플레이어 null (PlayerHit) / 인스펙터 확인");
            enabled = false;

            return;
        }

        _renderers = GetComponentsInChildren<Renderer>();

        if (_renderers == null || _renderers.Length == 0)
        {
            Debug.LogWarning("렌더러 비어있음 (PlayerHit) / 인스펙터 확인");
            enabled = false;

            return;
        }
    }


    private void OnDamaged()
    {
        if (_audioSource != null && _player.IsInvincible)
        {
            _audioSource.Play();
        }

        StartCoroutine(Co_Blink());
    }
    
    private IEnumerator Co_Blink()
    {
        while(_player.IsInvincible)
        {
            for(int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].enabled = !_renderers[i].enabled;
            }

            yield return new WaitForSeconds(_blinkInterval);
        }

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].enabled = true;
        }
    }
}

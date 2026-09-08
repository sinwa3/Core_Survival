using System.Collections.Generic;
using UnityEngine;

public class DamageTextPooling : MonoBehaviour
{
    #region 인스펙터
    [Header("옵션")]
    [SerializeField] private DamageText _damageTextPrefab;
    [SerializeField] private int _prewarmCount = 40;

    [Header("측정")]
    [SerializeField] private bool _usePool = true;
    #endregion

    #region 내부 변수
    private Queue<DamageText> _textPool = new Queue<DamageText>();
    private Queue<DamageText> _activeText = new Queue<DamageText>();
    #endregion

    private void Awake()
    {
        if (_usePool)
        {
            Prewarm();
        }
    }

    private void Prewarm()
    {
        if (_damageTextPrefab == null)
        {
            Debug.LogWarning("텍스트 프리팹 null (DamageTextPooling) / 프리웜 불가");

            return;
        }

        for (int i = 0; i < _prewarmCount; i++)
        {
            _textPool.Enqueue(CreateDamageText());
        }

        Debug.Log("데미지 텍스트 프리웜 성공");
    }

    private DamageText CreateDamageText()
    {
        DamageText damageText = Instantiate(_damageTextPrefab, transform);

        damageText.SetOwnerPool(this);
        damageText.gameObject.SetActive(false);

        return damageText;
    }

    public DamageText GetDamageText(float damage, Vector3 enemyPos)
    {
        if (_damageTextPrefab == null)
        {
            Debug.LogWarning("텍스트 프리팹 null (DamageTextPooling) / 생성 불가");

            return null;
        }

        DamageText text = PickText();

        if (text == null)
        {
            return text;
        }

        if (_usePool)
        {
            _activeText.Enqueue(text);
        }

        text.gameObject.SetActive(true);
        text.OnSpawn(damage, enemyPos);

        return text;
    }

    private DamageText PickText()
    {
        if (!_usePool)
        {
            return CreateDamageText();
        }

        if (_textPool.Count > 0)
        {
            return _textPool.Dequeue();
        }

        DamageText text = ReUseText();

        if (text != null)
        {
            return text;
        }

        return CreateDamageText();
    }

    private DamageText ReUseText()
    {
        while (_activeText.Count > 0)
        {
            DamageText text = _activeText.Dequeue();

            if (!text.IsPooled)
            {
                return text;
            }
        }

        return null;
    }

    public void ReturnDamageText(DamageText text)
    {
        if (text == null)
        {
            Debug.LogWarning("텍스트 null (DamageTextPooling)/ 반환 불가");

            return;
        }

        if (text.IsPooled)
        {
            return;
        }

        if (!_usePool)
        {
            Destroy(text.gameObject);

            return;
        }

        text.OnDespawn();
        text.gameObject.SetActive(false);
        _textPool.Enqueue(text);

        ClearReturnText();
    }

    private void ClearReturnText()
    {
        while (_activeText.Count > 0 && _activeText.Peek().IsPooled)
        {
            _activeText.Dequeue();
        }
    }



}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region 인스펙터
    [Header("텍스트")]
    [SerializeField] private TMP_Text _text;

    [Header("색")]
    [SerializeField] private Color _hoverColor = Color.white;

    #endregion

    #region 내부 변수
    private Color _baseColor;
    #endregion

    private void Awake()
    {
        if (_text == null)
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        if (_text == null)
        {
            Debug.LogWarning("텍스트 null (ButtonHover) / 인스펙터 확인");
            enabled = false;

            return;
        }

        _baseColor = _text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = _baseColor;
    }
}

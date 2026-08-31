using TMPro;
using UnityEngine;

public class AliveTime : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _aliveTime;
    #endregion

    #region 내부 변수
    private int _preSecond = -1;
    #endregion

    private void Awake()
    {
        if (_gameManager == null)
        {
            Debug.LogWarning("게임 매니저 null (AliveTime) / 인스펙터 확인");
            enabled = false;

            return;
        }

        if (_aliveTime == null)
        {
            _aliveTime = GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        float time = _gameManager.PlayTime;
        int minute = Mathf.FloorToInt(time / 60);
        int second = Mathf.FloorToInt(time % 60);

        if (second == _preSecond)
        {
            return;
        }

        _preSecond = second;
        _aliveTime.text = $"생존: {minute}:{second:00}";
    }
}

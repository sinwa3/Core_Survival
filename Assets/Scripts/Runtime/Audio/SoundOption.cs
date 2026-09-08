using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    #region 인스펙터
    [Header("패널")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("슬라이더")]
    [SerializeField] private Slider _volumeSlider;

    [Header("샘플 사운드")]
    [SerializeField] private AudioClip _sample;
    [SerializeField] private float _sampleInterval = 0.12f;
    #endregion

    #region 내부 변수
    private AudioSource _audioSource;
    private float _nextSampleTime;
    #endregion

    private const string VolumeKey = "MasterVolume";

    public static void LoadVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat(VolumeKey, 1.0f);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning("오디오 소스 null (SoundOption) / 컴포넌트 확인");

            return;
        }
    }

    void Start()
    {
        Hide();

        if (_volumeSlider == null)
        {
            Debug.LogWarning("슬라이더 null (SoundOption) / 인스펙터 확인");

            return;
        }

        _volumeSlider.SetValueWithoutNotify(AudioListener.volume);

        _volumeSlider.onValueChanged.AddListener(VolumeChanged);
    }

    private void VolumeChanged(float value)
    {
        AudioListener.volume = value;

        PlaySample();
    }

    private void PlaySample()
    {
        if (_audioSource == null || _sample == null)
        {
            return;
        }

        if (Time.time < _nextSampleTime)
        {
            return;
        }

        _nextSampleTime = Time.time + _sampleInterval;
        _audioSource.PlayOneShot(_sample);
    }

    public void OptionOpen()
    {

        if (_canvasGroup == null)
        {
            Debug.LogWarning("캔버스 그룹 null (SoundOption) / 인스펙터 확인");

            return;
        }

        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void OptionClose()
    {
        Hide();

        PlayerPrefs.SetFloat(VolumeKey, AudioListener.volume);
        PlayerPrefs.Save();
    }

    private void Hide()
    {
        if (_canvasGroup == null)
        {
            Debug.LogWarning("캔버스 그룹 null (SoundOption) / 인스펙터 확인");

            return;
        }

        _canvasGroup.alpha = 0.0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}

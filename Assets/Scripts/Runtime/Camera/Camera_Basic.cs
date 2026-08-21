using UnityEngine;

public class Camera_Basic : MonoBehaviour
{
    public enum POV
    {
        FirstPerson,
        ThirdPerson,
        TopView
    }


    #region 인스펙터
    [Header("시점")]
    [SerializeField] private POV _POV = POV.ThirdPerson;

    [Header("찍을 타겟")]
    [SerializeField] private Transform _target;

    [Header("카메라")]
    [SerializeField] private Camera _camera;

    [Header("카메라 관련 설정")]
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0.0f, 2.0f, -5.0f);
    [SerializeField] private Vector3 _firstOffset = new Vector3(0.0f, 1.0f, 0.5f);
    [SerializeField] private Vector3 _topOffset = new Vector3(0.0f, 10.0f, -2.0f);
    [SerializeField] private float _sensitive = 3.0f;
    [SerializeField] private float _cameraLookHeight = 1.2f;

    [Header("샤프니스")]
    [Min(0.1f)]
    [SerializeField] private float _sharpness = 10.0f;
    #endregion

    private Transform _camTr;
    private float _orbitYaw;
    private float _orbitPitch;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        if (_target == null)
        {
            Debug.Log("타겟 null / 인스펙터 확인");
            enabled = false;
            return;
        }

        _camTr = _camera.transform;
    }

    void Start()
    {
        _orbitYaw = 0.0f;
        _orbitPitch = 10.0f;
    }

    void LateUpdate()
    {
        switch (_POV)
        {
            case POV.FirstPerson:
                TickFirst();
                break;
            case POV.ThirdPerson:
                TickThird();
                break;
            case POV.TopView:
                TickTop();
                break;
        }
    }
    private void TickTop()
    {
        Vector3 desirePos;
        Quaternion desireRot;

        CalcPoseTop(out desirePos, out desireRot);

        ApplyPose(desirePos, desireRot);
    }

    private void CalcPoseTop(out Vector3 desirePos, out Quaternion desireRot)
    {
        desirePos = _target.position + _topOffset;
        Quaternion rot = Quaternion.LookRotation(_target.position - desirePos);
        desireRot = rot;
    }


    private void TickFirst()
    {
        Vector3 desirePos;
        Quaternion desireRot;

        CalcPoseFirst(out desirePos, out desireRot);

        ApplyPose(desirePos, desireRot);
    }
    private void CalcPoseFirst(out Vector3 desirePos, out Quaternion desireRot)
    {
        desirePos = _target.position + _target.rotation * _firstOffset;

        if (Input.GetMouseButton(1))
        {
            _orbitYaw += Input.GetAxis("Mouse X") * _sensitive;
            _orbitPitch -= Input.GetAxis("Mouse Y") * _sensitive;
        }

        _orbitYaw = Mathf.Clamp(_orbitYaw, -45.0f, 45.0f);
        _orbitPitch = Mathf.Clamp(_orbitPitch, -40.0f, 40.0f);

        Quaternion rot = Quaternion.Euler(_orbitPitch, _orbitYaw, 0.0f);

        desireRot = _target.rotation * rot;
    }

    private void ApplyPose(Vector3 desirePos, Quaternion desireRot)
    {
        float t = GetSmooth();

        _camTr.position = Vector3.Lerp(_camTr.position, desirePos, t);
        _camTr.rotation = Quaternion.Slerp(_camTr.rotation, desireRot, t);
    }

    private void TickThird()
    {
        Vector3 desirePos;
        Quaternion desireRot;

        CalcPoseThird(out desirePos, out desireRot);

        ApplyPose(desirePos, desireRot);
    }

    private void CalcPoseThird(out Vector3 desirePos, out Quaternion desireRot)
    {
        if (Input.GetMouseButton(1))
        {
            _orbitYaw += Input.GetAxis("Mouse X") * _sensitive;
            _orbitPitch -= Input.GetAxis("Mouse Y") * _sensitive;
        }

        _orbitPitch = Mathf.Clamp(_orbitPitch, -30.0f, 30.0f);

        Quaternion orbit = Quaternion.Euler(_orbitPitch, _orbitYaw, 0);
        desirePos = _target.position + orbit * _cameraOffset;
        Vector3 lookPos = _target.position + Vector3.up * _cameraLookHeight;

        desireRot = Quaternion.LookRotation(lookPos - desirePos, Vector3.up);
    }

    private float GetSmooth()
    {
        return 1f - Mathf.Exp(-_sharpness * Time.deltaTime);
    }

}

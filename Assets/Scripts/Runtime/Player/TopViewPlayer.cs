using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopViewPlayer : MonoBehaviour
{
    #region 인스펙터
    [Header("이동 / 회전 속도")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _avoidSpeed = 5.0f;
    [SerializeField] private float _rotateSharpness = 10.0f;

    [Header("대시 옵션")]
    [SerializeField] private float _dashMultiple = 3.0f;
    [SerializeField] private float _dashDuration = 0.14f;

    [Header("참조")]
    [SerializeField] private CharacterController _control;
    [SerializeField] private Animator _animator;

    [Header("중력")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundStick = -2.0f;

    [Header("키 설정")]
    [SerializeField] private KeyCode _dashKey = KeyCode.Space;

    #endregion

    #region 내부변수
    private bool _isDashing = false;
    private float _verticalVelocity;
    #endregion

    private void Awake()
    {
        _control = GetComponent<CharacterController>();

        if (_control == null)
        {
            Debug.LogWarning("캐릭터 컨트롤러 null / 확인 필요");
            enabled = false;

            return;
        }

        _animator = GetComponentInChildren<Animator>();

        if (_animator == null)
        {
            Debug.LogWarning("애니메이터 null / 확인 필요");
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (_isDashing)
        {



            return;
        }

        

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = new Vector3(h, 0, v);
        inputDir = inputDir.normalized;

        // 대시 키 누르면 대시
        if (Input.GetKeyDown(_dashKey) && inputDir.sqrMagnitude > 0.0001f)
        {
            StartCoroutine(Co_Dash(inputDir));
        }
        
        // 중력 계산
        TickGravity();

        Vector3 velocity = inputDir * _moveSpeed;
        velocity.y = _verticalVelocity;

        _control.Move(velocity * Time.deltaTime);

        // 회전 시키기
        TickRotate(inputDir);
    }

    // 대시 코루틴
    private IEnumerator Co_Dash(Vector3 dir)
    {
        _isDashing = true;

        if (dir.sqrMagnitude < 0.0001f)
        {
            yield break;
        }

        float timer = 0.0f;

        while (timer < _dashDuration)
        {
            timer += Time.deltaTime;

            // 대시 이동속도 계산
            Vector3 velocity = dir * _moveSpeed * _dashMultiple;

            // 대시 중에도 떨어지게
            TickGravity();
            velocity.y = _verticalVelocity;

            _control.Move(velocity * Time.deltaTime);

            // 다음 프레임 대기
            yield return null;    
        }

        // 대시 종료
        _isDashing = false;
    }

    private void TickRotate(Vector3 inputDir)
    {
        if (inputDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion rot = Quaternion.LookRotation(inputDir, Vector3.up);

        float sharpness = GetSharpness();

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, sharpness);
    }

    private float GetSharpness()
    {
        return 1.0f - Mathf.Exp(-_rotateSharpness * Time.deltaTime);
    }

    private void TickGravity()
    {
        if (_control.isGrounded)
        {
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = _groundStick;
            }
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);

    }

}

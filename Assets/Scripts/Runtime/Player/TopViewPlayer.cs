using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPlayer : MonoBehaviour
{
    #region 인스펙터
    [Header("이동 / 회전 속도")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _avoidSpeed = 5.0f;
    [SerializeField] private float _rotateSharpness = 10.0f;

    [Header("참조")]
    [SerializeField] private CharacterController _control;
    [SerializeField] private Animator _animator;

    [Header("중력")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundStick = -2.0f;

    #endregion

    #region 내부변수
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
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = new Vector3(h, 0, v);
        inputDir = inputDir.normalized;

        bool isAvoid = Input.GetKeyDown(KeyCode.Space);
        float speed = _moveSpeed * (isAvoid ? _avoidSpeed : 1.0f);
        
        TickGravity();

        Vector3 velocity = inputDir * speed;
        velocity.y = _verticalVelocity;

        _control.Move(velocity * Time.deltaTime);

        TickRotate(inputDir);
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

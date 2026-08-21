using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPlayer : MonoBehaviour
{
    #region 인스펙터
    [Header("이동 / 회전 속도")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 10.0f;

    [Header("참조")]
    [SerializeField] private CharacterController _control;
    [SerializeField] private Animator _animator;

    #endregion

    #region 내부변수

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

            return;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        ControlPlayer();
    }

    private void ControlPlayer()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = new Vector3(h, 0, v);
        Vector3 moveDir = inputDir.normalized;

        _rb.MovePosition(_rb.position + moveDir * _moveSpeed * Time.fixedDeltaTime);

        if (moveDir == Vector3.zero)
        {
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        Quaternion rot = Quaternion.Slerp(_rb.rotation, targetRot, _rotateSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(rot);
    }
}

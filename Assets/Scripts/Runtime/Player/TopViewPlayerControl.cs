using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewPlayerControl : MonoBehaviour
{
    #region 인스펙터
    [Header("이동 / 회전 속도")]
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _rotateSharpness = 10.0f;

    [Header("대시 옵션")]
    [SerializeField] private float _dashMultiple = 3.0f;
    [SerializeField] private float _dashDuration = 0.14f;
    [SerializeField] private float _dashCooltime = 1.0f;

    [Header("참조")]
    [SerializeField] private CharacterController _control;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyScanner _scanner;

    [Header("중력")]
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundStick = -2.0f;

    [Header("키 설정")]
    [SerializeField] private KeyCode _dashKey = KeyCode.Space;

    [Header("애니메이션")]
    [SerializeField] private string _paramRun = "bRun";
    [SerializeField] private string _paramDash = "tDash";
    [SerializeField] private bool _hasDash = true;

    [Header("스텟")]
    [SerializeField] private Player _player;
    

    #endregion

    #region 내부변수
    private bool _isDashing = false;
    private float _dashUseTime = 0.0f;
    private float _verticalVelocity;
    private int _hashRun;
    private int _hashDash;
    #endregion

    private void Reset()
    {
        _control = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        if (_control == null)
        {
            _control = GetComponent<CharacterController>();
        }

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        _hashRun = Animator.StringToHash(_paramRun);

        if (_hasDash)
        {
            _hashDash = Animator.StringToHash(_paramDash);
        }

        if (_scanner == null)
        {
            _scanner = GetComponentInChildren<EnemyScanner>();
        }

        if (_player == null)
        {
            _player = GetComponent<Player>();
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

        // 중력 계산
        TickGravity();

        bool canDash = Time.time >= _dashUseTime + _dashCooltime;

        // 대시 키 누르면 대시
        if (Input.GetKeyDown(_dashKey) && inputDir.sqrMagnitude > 0.0001f && canDash)
        {
            _dashUseTime = Time.time;

            if (_hasDash)
            {
                _animator.SetTrigger(_hashDash);
            }

            StartCoroutine(Co_Dash(inputDir));
        }

        Vector3 velocity = inputDir * _moveSpeed * _player.PlayerStats.speed;
        velocity.y = _verticalVelocity;

        _control.Move(velocity * Time.deltaTime);

        bool isRunning = (inputDir.sqrMagnitude > 0.001f);

        // 회전 시키기
        TickRotate(inputDir);

        _animator.SetBool(_hashRun, isRunning);
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
            Vector3 velocity = dir * _moveSpeed * _player.PlayerStats.speed * _dashMultiple;

            // 대시 중에도 떨어지게
            velocity.y = _verticalVelocity;

            _control.Move(velocity * Time.deltaTime);

            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            float sharpness = GetSharpness(_rotateSharpness);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _dashMultiple * sharpness);

            // 다음 프레임 대기
            yield return null;    
        }

        // 대시 종료
        _isDashing = false;
    }

    private void TickRotate(Vector3 inputDir)
    {
        Transform enemyTr = _scanner.GetNearest();

        if (enemyTr != null)
        {
            inputDir = enemyTr.position - transform.position;
        }

        if (inputDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion rot = Quaternion.LookRotation(inputDir, Vector3.up);

        float sharpness = GetSharpness(_rotateSharpness);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, sharpness);
    }

    private float GetSharpness(float sharpness)
    {
        return 1.0f - Mathf.Exp(-sharpness * Time.deltaTime);
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

    private void OnGUI()
    {
        float cool = _dashUseTime + _dashCooltime - Time.time;

        if (cool < 0.0f)
        {
            cool = 0.0f;
        }

        GUIStyle label = new GUIStyle();

        label.fontSize = 40;
        label.normal.textColor = Color.white;

        GUI.Box(new Rect(10, 10, 400, 150), "");
        GUI.Label(new Rect(60, 50, 600, 300), $"[대시 쿨] {cool : 0.00}", label);
    }

}

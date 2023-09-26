using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class TesterMoveBehavior : MonoBehaviour, ISkill
{
    private const string RUN_ANIMATION_KEY = "isRun";
    private const float DELAY_MOVE_TIME = 1.5F;

    private int _moveSpeed;
    private int _rotationSpeed = 5;
    private bool _isMove = false;
    private CharacterController _characterController;
    private ReactiveProperty<Vector3> _moveDirection;
    private CompositeDisposable _disposables;
    private Animator _animator;
    private Vector3 _desiredRotation;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _moveDirection = new ReactiveProperty<Vector3>();
        _disposables = new CompositeDisposable();
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        Subcribe();
        StartCoroutine(DelayMove());
    }

    public void InitData(int moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    private void Update() => MoveControl();
    private void Subcribe()
    {
        _moveDirection.Subscribe(vector =>
        {
            if (vector != Vector3.zero)
                _animator.SetBool(RUN_ANIMATION_KEY, true);
            else
                _animator.SetBool(RUN_ANIMATION_KEY, false);
        }).AddTo(_disposables);
    }
    private IEnumerator DelayMove()
    {
        yield return new WaitForSeconds(DELAY_MOVE_TIME);
        _isMove = true;
    }
    private void MoveControl()
    {
        if (!_isMove) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Tính toán hướng di chuyển
        _moveDirection.Value = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        _moveDirection.Value *= _moveSpeed;

        // Áp dụng di chuyển vào Character Controller
        _characterController.Move(_moveDirection.Value * Time.deltaTime);
        DesiredRotation(_moveDirection.Value);
    }
    private void DesiredRotation(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
            _desiredRotation = Quaternion.LookRotation(moveDirection.normalized).eulerAngles;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_desiredRotation), Time.deltaTime * _rotationSpeed);

    }

    private void OnDestroy()
    {
        _disposables.Clear();   
    }
}

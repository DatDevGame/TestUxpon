using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class CompMoveBehavior : MonoBehaviour, ISkill
{
    private const string RUN_ANIMATION_KEY = "isRun";
    private const float DISTANCE_STOP = 1.8F;

    private int _moveSpeed;
    private ReactiveProperty<float> _distanceTarget;
    private CompositeDisposable _disposables;
    private CompositeDisposable _moveAnimationDisposables;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Transform _target;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _distanceTarget = new ReactiveProperty<float>();
        _disposables = new CompositeDisposable();
        _moveAnimationDisposables = new CompositeDisposable();
    }
    private void Start()
    {
        Subcribe();
    }
    private void Subcribe()
    {
        _distanceTarget.Where(v => _target != null)
            .Subscribe(distance =>
            {
                if (distance > DISTANCE_STOP)
                    _animator.SetBool(RUN_ANIMATION_KEY, true);
                else
                    _animator.SetBool(RUN_ANIMATION_KEY, false);
            }).AddTo(_moveAnimationDisposables);

        Gameplay.Instance.CurrentState
            .Subscribe(state =>
            {
                _navMeshAgent.enabled = state == GamePlayState.Running;
            }).AddTo(_disposables);

        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Won || state == GamePlayState.Lost)
            .Subscribe(state =>
            {
                _moveAnimationDisposables.Clear();
                _animator.SetBool(RUN_ANIMATION_KEY, false);
            }).AddTo(_disposables);
    }
    public void InitData(Transform target, int moveSpeed)
    {
        _moveSpeed = moveSpeed;
        _navMeshAgent.speed = _moveSpeed;
        _target = target;

        OnStartAfterInitData();
    }

    private void OnStartAfterInitData()
    {
        StartCoroutine(UpdateMove());
    }
    private IEnumerator UpdateMove()
    {
        while (_target != null)
        {
            _distanceTarget.Value = (_target.position - transform.position).magnitude;
            if (_distanceTarget.Value > DISTANCE_STOP)
                MoveControl(_target.position);

            yield return null;
        }
        //Call When Target Dead
        _navMeshAgent.isStopped = true;
        _animator.SetBool(RUN_ANIMATION_KEY, false);
    }

    private void MoveControl(Vector3 target)
    {
        if (Gameplay.Instance.CurrentState.Value != GamePlayState.Running)
        {
            _navMeshAgent.enabled = false;
            return;
        }
        _navMeshAgent.destination = target;
    }
    private void OnDestroy()
    {
        _disposables.Clear();
    }
}

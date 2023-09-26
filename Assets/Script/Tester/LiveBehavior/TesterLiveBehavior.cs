using UniRx;
using UnityEngine;

public class TesterLiveBehavior : MonoBehaviour, ISkill, IDamageable, IHealthProvider
{
    private const string DEAD_ANIMATION_KEY = "DeadTrigger";
    public ReactiveProperty<bool> IsDead { get; private set; }
    public ReactiveProperty<int> Health => _healthPoint;

    private ReactiveProperty<int> _healthPoint;
    private CompositeDisposable _disposable;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private void Awake()
    {
        IsDead = new ReactiveProperty<bool>(false);
        _healthPoint = new ReactiveProperty<int>();
        _disposable = new CompositeDisposable();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Subcribe();
    }
    private void Subcribe()
    {
        _healthPoint
            .Subscribe(hp =>
            {
                Gameplay.Instance.GameSceneUI.UpdateHealthPointTester(hp);
                if (hp <= 0)
                    Dead();

            }).AddTo(_disposable);
    }
    private void Dead()
    {
        IsDead.Value = true;
        _rigidbody.isKinematic = IsDead.Value;
        _animator.SetTrigger(DEAD_ANIMATION_KEY);
        Debug.Log("Tester Is Dead");
    }
    public void InitData(int healthPoint)
    {
        _healthPoint.Value = healthPoint;
    }
    public void TakeDamage(int damageAmount)
    {
        _healthPoint.Value -= damageAmount;
    }
    private void OnDestroy()
    {
        _disposable.Clear();
    }
}

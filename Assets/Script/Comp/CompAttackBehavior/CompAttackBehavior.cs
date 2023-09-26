using System.Collections;
using UniRx;
using UnityEngine;

public class CompAttackBehavior : MonoBehaviour, ISkill
{
    private const string ATTACK_ANIMATION_KEY = "AttackTrigger";
    private const string TESTER_TAG = "Tester";

    private int _damage;
    private float _countDownAttack;
    private CompositeDisposable _disposables;
    private Animator _animator;
    private IEnumerator _countDownAttackCoroutine;
    private GameObject _attackEffect;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _disposables = new CompositeDisposable();
    }
    private void Start()
    {
        Subcribe();
    }
    public void InitData(int Damage, float timeAttack, GameObject attackEffect)
    {
        _damage = Damage;
        _countDownAttack = timeAttack;
        _attackEffect = attackEffect;
    }

    private void Subcribe()
    {
        Gameplay.Instance.CurrentState.Where(state => state == GamePlayState.Lost)
            .Subscribe(state =>
        {
            if (_countDownAttackCoroutine != null)
                StopCoroutine(_countDownAttackCoroutine);
            _disposables.Clear();
        }).AddTo(_disposables);
    }

    private IEnumerator AttackEffect()
    {
        yield return new WaitForSeconds(0.25f);
        Transform handComp = GetComponent<BodyComp>().Hand;
        GameObject attackEffect = Instantiate(_attackEffect, Vector3.zero, Quaternion.identity);
        attackEffect.transform.position = handComp.transform.position;
        attackEffect.transform.eulerAngles = transform.eulerAngles;
    }
    private void Attack(Collider target)
    {
        transform.LookAt(target.transform.position);
        _animator.SetTrigger(ATTACK_ANIMATION_KEY);
        target.GetComponent<IDamageable>().TakeDamage(_damage);

        StartCoroutine(AttackEffect());
        Debug.Log("Attack: "+ target.name);
    }
    private IEnumerator CountDownAttack(Collider target)
    {
        float timeDelay = _countDownAttack;
        while (true) 
        {
            timeDelay -= Time.deltaTime;
            if (timeDelay <= 0)
            {
                Attack(target);
                timeDelay = _countDownAttack;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TESTER_TAG)
        {
            if (_countDownAttackCoroutine == null)
                _countDownAttackCoroutine = CountDownAttack(other);
            StartCoroutine(_countDownAttackCoroutine);
            Debug.Log("Start Count Down Attack");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == TESTER_TAG)
        {
            if (_countDownAttackCoroutine != null)
            {
                StopCoroutine(_countDownAttackCoroutine);
                Debug.Log("Out Zone Attack");
            }
        }
    }
    private void OnDestroy()
    {
        _disposables.Clear();
    }
}

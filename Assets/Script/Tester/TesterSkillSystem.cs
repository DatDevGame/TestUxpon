using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class TesterSkillSystem : MonoBehaviour
{
    private List<ISkill> _skills;
    private BoxCollider _hitBoxCollider;
    private TesterMoveBehavior _moveBehavior;
    private TesterLiveBehavior _liveBehavior;
    private CompositeDisposable _disposables;
    private void Awake()
    {
        _skills = new List<ISkill>();
        _disposables = new CompositeDisposable();
        _hitBoxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        InitSkill();
    }
    private void InitSkill()
    {
        Resource resource = GameManager.Instance.Resource;

        _moveBehavior = gameObject.GetOrAddComponent<TesterMoveBehavior>();
        _moveBehavior.InitData(resource.TesterSetting.MoveBehaviorSetting.Run);
        _skills.Add(_moveBehavior);

        _liveBehavior = gameObject.GetOrAddComponent<TesterLiveBehavior>();
        _liveBehavior.InitData(resource.TesterSetting.LiveBehaviorSetting.Hp);
        _skills.Add(_liveBehavior);

        InitEvent();
    }
    private void InitEvent()
    {
        _liveBehavior.IsDead.Where(IsDead => IsDead)
            .Subscribe(IsDead =>
        {
            Gameplay.Instance.Fire(GamePlayTrigger.Lose);
            RemoveAllSkill();
        }).AddTo(_disposables);
    }

    private void RemoveAllSkill()
    {
        Destroy(_hitBoxCollider);
        foreach (ISkill skill in _skills)
            Destroy((MonoBehaviour)skill);

        _skills.Clear();
    }

    private void OnDestroy()
    {
        _disposables.Clear();
    }
}

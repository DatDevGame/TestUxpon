using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class CompSkillSystem : MonoBehaviour
{
    private List<ISkill> _skills;
    private CompMoveBehavior _moveBehavior;
    private CompLiveBehavior _liveBehavior;
    private CompAttackBehavior _attackBehavior;
    private CompositeDisposable _disposables;
    private void Awake()
    {
        _skills = new List<ISkill>();
        _disposables = new CompositeDisposable();
    }
    private void Start() => InitSkill();
    private void InitSkill()
    {
        Resource resource = GameManager.Instance.Resource;
        CompSetting dataComp = resource.CompSetting;

        _moveBehavior = gameObject.GetOrAddComponent<CompMoveBehavior>();
        _moveBehavior.InitData(Gameplay.Instance.Tester.transform,
            dataComp.MoveBehaviorSetting.Run);
        _skills.Add(_moveBehavior);

        _liveBehavior = gameObject.GetOrAddComponent<CompLiveBehavior>();
        _liveBehavior.InitData(dataComp.LiveBehaviorSetting.Hp);
        _skills.Add(_liveBehavior);

        _attackBehavior = gameObject.GetOrAddComponent<CompAttackBehavior>();
        _attackBehavior.InitData(dataComp.AttackBehaviorSetting.Damage,
            dataComp.AttackBehaviorSetting.CountDownTimeAttack,
            dataComp.AttackBehaviorSetting.AttackEffect);
        _skills.Add(_attackBehavior);
    }
    private void RemoveAllSkill()
    {
        foreach (ISkill skill in _skills)
            Destroy((MonoBehaviour)skill);

        _skills.Clear();
    }

    private void OnDestroy()
    {
        _disposables.Clear();
    }
}

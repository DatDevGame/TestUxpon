using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class CompLiveBehavior : MonoBehaviour, ISkill
{
    private int _healthPoint;
    public void InitData(int hp)
    {
        _healthPoint = hp;
    }
}

using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BehaviorData", menuName = "Comp/BehaviorData/Attack")]
public class CompAttackBehaviorSetting : ScriptableObject
{
    public int Damage;
    public float CountDownTimeAttack;
    public GameObject AttackEffect;
}

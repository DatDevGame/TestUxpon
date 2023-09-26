using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Comp/CompScriptableObject", order = 1), Serializable]
public class CompSetting : ScriptableObject
{
    public CompMoveBehaviorSetting MoveBehaviorSetting;
    public CompLiveBehaviorSetting LiveBehaviorSetting;
    public CompAttackBehaviorSetting AttackBehaviorSetting;
}

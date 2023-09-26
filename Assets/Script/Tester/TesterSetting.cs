using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Tester/TesterScriptableObject", order = 1), Serializable]
public class TesterSetting : ScriptableObject
{
    public string NameObject => "Tester";
    public TesterLiveBehaviorSetting LiveBehaviorSetting;
    public TesterMoveBehaviorSetting MoveBehaviorSetting;
}

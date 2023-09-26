using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "BehaviorData", menuName = "Tester/BehaviorData/Live")]
public class TesterLiveBehaviorSetting : ScriptableObject
{
    public int Hp;
}

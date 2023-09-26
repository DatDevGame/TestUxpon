using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class Resource : MonoBehaviour
{
    [TabGroup("Data Setting")] public TesterSetting TesterSetting;
    [TabGroup("Data Setting")] public CompSetting CompSetting;
    [TabGroup("Prefab")] public GameObject TesterPrefab;
    [TabGroup("Prefab")] public GameObject CompPrefab;
}

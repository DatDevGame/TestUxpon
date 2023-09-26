using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameSingleton<GameManager>
{
    public Resource Resource { get; private set; }

    protected override void OnAwake()
    {
        Resource = gameObject.GetComponent<Resource>();
    }
}

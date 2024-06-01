using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PickableObject : MonoBehaviour, IPickable
{
    [SerializeField] private SO_CorpseStats corpseStats;
    public SO_CorpseStats GetCorpseStats() {
        return corpseStats;
    }

    public bool GetIsBlockingJump() {
        return true;
    }
}

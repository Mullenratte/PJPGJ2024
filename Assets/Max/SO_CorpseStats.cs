using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_CorpseStats_ENTITY", menuName = "Scriptable Objects/SO_CorpseStats")]
public class SO_CorpseStats : ScriptableObject
{
    [Range(0, 1f)] public float walkSpeedPenalty;
    [Range(0, 1f)] public float jumpHeightPenalty;
    public bool isBlockingJump;
}

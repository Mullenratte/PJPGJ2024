using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    public bool GetIsBlockingJump();
    public SO_CorpseStats GetCorpseStats();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    void EnterState();
    void UpdateState();
    void ExitState();
}

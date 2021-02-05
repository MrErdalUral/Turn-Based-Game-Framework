using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    public abstract TurnAction SelectAction(TurnAction[] actions);
    public abstract List<Character> SelectTargets(TurnAction selectedAction);
}

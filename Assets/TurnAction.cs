using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class TurnAction : ScriptableObject
{
    public string ActionName;
    public Sprite ActionIcon;
    public bool SelfTarget;
    public bool OnlySelfTarget;
    public abstract IEnumerator InvokeAction(Character character,params Character[] targets);
};
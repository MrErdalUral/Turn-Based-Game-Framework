using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class TurnAction : ScriptableObject
{
    public string ActionName;
    public string AnimationName;
    public Sprite ActionIcon;
    public bool SelfTarget;
    public bool OnlySelfTarget;

    public int MagicCost;


    public abstract IEnumerator InvokeAction(Character character,params Character[] targets);
};
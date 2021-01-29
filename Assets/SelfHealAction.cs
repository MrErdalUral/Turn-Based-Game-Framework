using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class SelfHealAction : TurnAction
{
    public override IEnumerator InvokeAction(Character character, params Character[] targets)
    {
        Debug.Log($"{character.gameObject.name} {ActionName} self");
        character.HealDamage(Random.Range(1, 9) + character.GetHealBonus());
        yield return null;
    }
}
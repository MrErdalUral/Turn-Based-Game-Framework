using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class MeleeAttackAction : TurnAction
{
    public override IEnumerator InvokeAction(Character character, params Character[] targets)
    {
        foreach (var target in targets)
        {
            Debug.Log($"{character.gameObject.name} {ActionName} {target.name}");
            target.TakeDamage(Random.Range(1, 9) + character.GetMeleeAttackBonus());
        }
        yield return null;
    }
}
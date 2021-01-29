using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class MeleeAttackAction : TurnAction
{
    public override IEnumerator InvokeAction(Character Character, params Character[] targets)
    {
        foreach (var character in targets)
        {
            Debug.Log($"{Character.gameObject.name} {ActionName} {character.name}");
            Character.TakeDamage(Random.Range(1,9) + Character.GetMeleeAttackBonus());
        }
        yield return null;
    }
}
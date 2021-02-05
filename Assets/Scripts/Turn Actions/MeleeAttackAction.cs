using System.Collections;
using DG.Tweening;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class MeleeAttackAction : TurnAction
{
    public override IEnumerator InvokeAction(Character character, params Character[] targets)
    {
        var pos = character.transform.position;
        foreach (var target in targets)
        {
            //Walk To Character
            var dir = (character.transform.position - target.transform.position).normalized;
            character.transform.DOMove(target.transform.position + dir, 0.5f);
            yield return new WaitForSeconds(0.5f);

            //Trigger Animation
            yield return character.GetComponentInChildren<CharacterAnimationHandler>().PlayAnimation(AnimationName);

            //Deal Damage
            Debug.Log($"{character.gameObject.name} {ActionName} {target.name}");
            yield return target.TakeDamage(character.GetMeleeAttack());

        }
        //Return To Original Position
        character.transform.DOMove(pos, 0.5f);
        yield return new WaitForSeconds(0.5f);

        yield return null;
    }
}
using System.Collections;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu]
public class MovementAction : TurnAction
{
    public int cells = 3;
    public Vector3? TargetPos;
    public override IEnumerator InvokeAction(Character character, params Character[] targets)
    {
        TargetPos = null;
        PathNode[] path = null;
        do
        {
            yield return new WaitUntil(() => TargetPos != null);
            path = GridSystem.Instance.FindPathTo(character.transform.position, TargetPos.GetValueOrDefault());
            TargetPos = null;
        } while (path == null);

        foreach (var node in path)
        {
            character.transform.DOMove(node.x, 0.1f);
            character.transform.DOScaleX(Mathf.Sign(node.x.x - character.transform.position.x),0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
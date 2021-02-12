using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JRPGTurnManager : AbstractTurnManager
{
    public new static JRPGTurnManager Instance => (JRPGTurnManager)AbstractTurnManager.Instance;
    

    public void HighlightTargets(bool value)
    {
        var characters = FindObjectsOfType<Character>();
        foreach (var character in characters)
        {
            if (character == CurrentCharacter && SelectedAction != null && !SelectedAction.SelfTarget) continue;
            character.SetTargetIndicator(value);
        }
    }

    protected override IEnumerator PlayTurn(Character character)
    {
        SelectedAction = null;

        if (SelectedTargets == null)
            SelectedTargets = new List<Character>();
        else
            SelectedTargets.Clear();

        if (character.CharacterBehaviour == null)
        {
            ActionsPanel.Instance.Actions = character.Actions;
            yield return new WaitUntil(() =>
            {
                HighlightTargets(false);
                CurrentCharacter.SetTurnIndicator(true);
                if (SelectedAction == null) return false;
                CurrentCharacter.SetTurnIndicator(false);
                if (SelectedAction.OnlySelfTarget)
                    SelectedTargets.Add(CurrentCharacter);
                else
                    HighlightTargets(true);
                if (SelectedTargets.Count < 1) return false;
                HighlightTargets(false);

                return true;
            });
        }
        else
        {
            SelectedAction = character.CharacterBehaviour.SelectAction(character.Actions);
            SelectedTargets = character.CharacterBehaviour.SelectTargets(SelectedAction);
        }

        yield return SelectedAction.InvokeAction(character, SelectedTargets.ToArray());
    }

    

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            var target = hit.transform.GetComponent<Character>();
            if (!(target == null || target == CurrentCharacter && !SelectedAction.SelfTarget))
                AddTarget(target);
        }
    }

    private void AddTarget(Character target)
    {
        if (SelectedAction == null) return;
        if (SelectedAction.SelfTarget && target == CurrentCharacter) return;
        SelectedTargets.Add(target);
    }
}
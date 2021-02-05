using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public Character[] Characters;
    public int turnIndex;
    public static Character CurrentCharacter => Instance.Characters[Instance.turnIndex];

    private TurnAction _selectedAction;
    private List<Character> _selectedTargets;

    public TurnAction SelectedAction
    {
        get { return _selectedAction; }
        set { _selectedAction = value; }
    }

    public List<Character> SelectedTargets
    {
        get { return _selectedTargets; }
        set { _selectedTargets = value; }
    }

    public void HighlightTargets(bool value)
    {
        var characters = FindObjectsOfType<Character>();
        foreach (var character in characters)
        {
            if (character == CurrentCharacter && SelectedAction != null && !SelectedAction.SelfTarget) continue;
            character.SetTargetIndicator(value);
        }
    }

    public IEnumerator PlayTurn(Character character)
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

    IEnumerator Start()
    {
        Instance = this;
        while (Characters.Length > 1)
        {
            if (!CurrentCharacter.IsDead)
            {
                yield return PlayTurn(CurrentCharacter);
                yield return new WaitForSeconds(1);
                if (Characters.Count(m => !m.IsDead) < 2)
                    break;
            }
            turnIndex = (turnIndex + 1) % Characters.Length;
        }
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

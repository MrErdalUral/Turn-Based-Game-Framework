using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public Character[] Characters;
    public int turnIndex;
    public Character CurrentCharacter => Characters[turnIndex];

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

    public void HighlightTargets(Color color)
    {
        var characters = FindObjectsOfType<Character>();
        foreach (var character in characters)
        {
            if (character == CurrentCharacter && SelectedAction != null && !SelectedAction.SelfTarget) continue;
            character.SetColor(color);
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
                HighlightTargets(Color.white);
                character.SetColor(Color.green);
                if (SelectedAction == null) return false;
                HighlightTargets(Color.yellow);

                if (SelectedTargets.Count < 1) return false;
                HighlightTargets(Color.white);
                character.SetColor(Color.white);

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
        while (Characters.Length > 0)
        {
            turnIndex = (turnIndex + 1) % Characters.Length;
            yield return PlayTurn(CurrentCharacter);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out var hit))
        {
            var target = hit.transform.GetComponent<Character>();
            if (target != null)
                AddTarget(target);
        }
    }

    private void AddTarget(Character target)
    {
        if(SelectedAction == null) return;
        if(SelectedAction.SelfTarget && target == CurrentCharacter) return;
        SelectedTargets.Add(target);
    }
}

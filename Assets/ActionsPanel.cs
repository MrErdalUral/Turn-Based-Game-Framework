using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsPanel : MonoBehaviour
{
    public static ActionsPanel Instance;
    [SerializeField] private ActionSelection _actionSelectionPrefab;
    private List<ActionSelection> _selectionInstances;
    private TurnAction[] _actions;

    void Awake()
    {
        Instance = this;
        _selectionInstances = new List<ActionSelection>();
    }
    public TurnAction[] Actions
    {
        get => _actions;
        set => SetActions(value);
    }

    private void SetActions(TurnAction[] actions)
    {
        DisablePreviousActions();
        foreach (var turnAction in actions)
        {
            var selection = Instantiate(_actionSelectionPrefab, Vector3.zero, Quaternion.identity, transform);
            selection.Action = turnAction;
            _selectionInstances.Add(selection);
        }
        _actions = actions;
    }

    private void DisablePreviousActions()
    {
        foreach (var selection in _selectionInstances)
        {
            Destroy(selection.gameObject);
        }
        _selectionInstances.Clear();
    }
}

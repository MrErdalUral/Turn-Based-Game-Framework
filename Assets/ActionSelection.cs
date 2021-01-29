using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelection : MonoBehaviour
{
    private TurnAction _action;

    public TurnAction Action
    {
        get => _action;
        set
        {
            _action = value;
            GetComponent<Image>().sprite = _action.ActionIcon;
        }
    }

    public void OnActionSelected()
    {
        foreach (var button in transform.parent.GetComponentsInChildren<ActionSelection>())
        {
            button.GetComponent<Image>().color = Color.white;
        }
        //Deselect
        if (TurnManager.Instance.SelectedAction == Action)
        {
            TurnManager.Instance.SelectedAction = null;
            return;
        }
        TurnManager.Instance.SelectedAction = Action;
        GetComponent<Image>().color = Color.green;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelection : MonoBehaviour
{
    private TurnAction _action;
    private bool _notEnoughMagic;

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
        if(_notEnoughMagic) return;
        foreach (var button in transform.parent.GetComponentsInChildren<ActionSelection>())
        {
            button.GetComponent<Image>().color = Color.white;
        }
        //Deselect
        if (AbstractTurnManager.Instance.SelectedAction == Action)
        {
            AbstractTurnManager.Instance.SelectedAction = null;
            return;
        }
        AbstractTurnManager.Instance.SelectedAction = Action;
        GetComponent<Image>().color = Color.green;
    }

    void Update()
    {
        _notEnoughMagic = AbstractTurnManager.CurrentCharacter.CurrentMagic < _action.MagicCost;
        if (_notEnoughMagic) GetComponent<Image>().color = Color.red;
    }
}

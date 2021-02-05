using System;
using TMPro;
using UnityEngine;

public class CharacterStatusText : MonoBehaviour
{
    public StatusDisplayType Type;
    // Update is called once per frame
    void Update()
    {

        var textMesh = GetComponent<TextMeshProUGUI>();
        switch (Type)
        {
            case StatusDisplayType.Health:
                textMesh.text =
                    $"Health: {TurnManager.CurrentCharacter.CurrentHealth} / {TurnManager.CurrentCharacter.MaxHealth}";
                break;
            case StatusDisplayType.Magic:
                textMesh.text =
                    $"Magic: {TurnManager.CurrentCharacter.CurrentMagic} / {TurnManager.CurrentCharacter.MaxMagic}";
                break;
            case StatusDisplayType.Stamina:
                textMesh.text =
                    $"Stamina: {TurnManager.CurrentCharacter.CurrentStamina} / {TurnManager.CurrentCharacter.MaxStamina}";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum StatusDisplayType
{
    Health,Magic,Stamina
}
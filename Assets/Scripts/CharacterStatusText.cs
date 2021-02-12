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
                    $"Health: {JRPGTurnManager.CurrentCharacter.CurrentHealth} / {JRPGTurnManager.CurrentCharacter.MaxHealth}";
                break;
            case StatusDisplayType.Magic:
                textMesh.text =
                    $"Magic: {JRPGTurnManager.CurrentCharacter.CurrentMagic} / {JRPGTurnManager.CurrentCharacter.MaxMagic}";
                break;
            case StatusDisplayType.Stamina:
                textMesh.text =
                    $"Stamina: {JRPGTurnManager.CurrentCharacter.CurrentStamina} / {JRPGTurnManager.CurrentCharacter.MaxStamina}";
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
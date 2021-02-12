using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractTurnManager : MonoBehaviour
{
    public static AbstractTurnManager Instance;
    public Character[] Characters;
    public int turnIndex;
    public static Character CurrentCharacter => Instance.Characters[Instance.turnIndex];
    protected TurnAction _selectedAction;
    protected List<Character> _selectedTargets;

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
    IEnumerator Start()
    {
        Instance = this;
        var cameraFollow = Camera.main.GetComponent<CameraFollow>();
        while (Characters.Length > 0)
        {
            if (!CurrentCharacter.IsDead)
            {
                if (cameraFollow)
                {
                    cameraFollow.FollowTarget = CurrentCharacter.transform;
                }
                yield return PlayTurn(CurrentCharacter);
                yield return new WaitForSeconds(1);
                if (Characters.Count(m => !m.IsDead) < 1)
                    break;
            }
            turnIndex = (turnIndex + 1) % Characters.Length;
        }
    }

    protected abstract IEnumerator PlayTurn(Character character);

}
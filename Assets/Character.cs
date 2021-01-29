using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TurnAction[] _actions;
    [SerializeField] public CharacterBehaviour _characterBehaviour;

    [SerializeField]
    private CharacterStats _stats = new CharacterStats
    {
        MaxHealth = 10,
        MaxMagic = 10,
        MaxStamina = 10,
        Body = 1,
        Mind = 1,
        Senses = 1
    };

    public CharacterBehaviour CharacterBehaviour
    {
        get { return _characterBehaviour; }
        set { _characterBehaviour = value; }
    }

    public TurnAction[] Actions
    {
        get { return _actions; }
        set { _actions = value; }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($"{gameObject.name} took {amount} damage.");
    }

    public int GetMeleeAttackBonus()
    {
        return _stats.Body;
    }


    void Awake()
    {
        CharacterBehaviour = GetComponent<CharacterBehaviour>();
    }

    public void SetColor(Color color)
    {
        _sprite.color = color;
    }

    public int GetHealBonus()
    {
        return _stats.Mind;
    }

    public void HealDamage(int amount)
    {
        Debug.Log($"{gameObject.name} healed {amount} for damage.");
    }
}


[System.Serializable]
public class CharacterStats
{
    public string Name;

    public float MaxHealth;
    public float MaxStamina;
    public float MaxMagic;

    //HARD Attributes
    public int Body;
    public int Mind;
    public int Senses;

    //SOFT Attributes [Body]
    public int Agility;
    public int Might;
    public int Vitality;

    //SOFT Attributes [MIND]
    public int Will;
    public int Intellect;
    public int Intuition;

    //SOFT Attributes [Senses]
    public int Empathy;
    public int Alertness;
    public int Kinesthesia;




}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _targetIndicator;
    [SerializeField] private GameObject _turnIndicator;
    [SerializeField] public CharacterBehaviour _characterBehaviour;
    [Header("Character Actions")]
    [SerializeField] private TurnAction[] _actions;

    [Header("Character Stats")]
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
    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentMagic;
    [HideInInspector]
    public float CurrentStamina;
    public bool IsDead => CurrentHealth <= 0;

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

    public float MaxHealth => _stats.MaxHealth;
    public float MaxMagic => _stats.MaxMagic;
    public float MaxStamina => _stats.MaxStamina;

    public IEnumerator TakeDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} took {amount} damage.");
        CurrentHealth -= amount;
        

        //Trigger floating text
        FloatingText.Instance.TriggerFloatingText(amount.ToString(),transform.position,Color.red, 1,true);

        //Todo trigger take damage animation

        //Color flash
        SetColor(Color.red);
        yield return new WaitForSeconds(0.05f);
        SetColor(Color.white);

        if (CurrentHealth <= 0)
        {
            yield return CharacterDeath();
        }
    }

    private IEnumerator CharacterDeath()
    {
        transform.DOMoveY(0.5f, 0.2f).SetRelative().OnComplete(() => transform.DOMoveY(-0.5f, 0.2f).SetRelative());
        transform.DORotate(new Vector3(0, 0, 90), 0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void HealDamage(int amount)
    {
        //Debug.Log($"{gameObject.name} healed {amount} for damage.");
        CurrentHealth = Mathf.Min(CurrentHealth+amount,_stats.MaxHealth);

        FloatingText.Instance.TriggerFloatingText(amount.ToString(), transform.position, Color.green, 1, true);

    }

    public int GetHealBonus()
    {
        return _stats.Mind;
    }

    public int GetMeleeAttackBonus()
    {
        return _stats.Body;
    }

    public int GetMeleeAttack()
    {
        //todo read weapon base damage from an equipment system 
        var weaponDamageBase = Random.Range(1, 9);
        return weaponDamageBase + GetMeleeAttackBonus();
    }

    private void SetColor(Color color)
    {
        _sprite.color = color;
    }

    public void SetTargetIndicator(bool value)
    {
        _targetIndicator.SetActive(value);
    }
    public void SetTurnIndicator(bool value)
    {
        _turnIndicator.SetActive(value);
    }

    void Awake()
    {
        CharacterBehaviour = GetComponent<CharacterBehaviour>();
        CurrentHealth = _stats.MaxHealth;
        CurrentMagic = _stats.MaxMagic;
        CurrentStamina = _stats.MaxStamina;
    }

    public void SpendMagic(float amount)
    {
        CurrentMagic -= amount;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour, IDamageble, IHealble
{
    [SerializeField] private Humanoid humanoid;
    [SerializeField] private float _health, _maxHealth, _armor, _beOtherColorFor;
    [SerializeField] private Color _original, _gainHealth, _looseHealth;
    public UnityEvent OnDeathEvent;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _maxHealth = humanoid.MaxHp;
        _health = _maxHealth;
        _armor = humanoid.Armor;
        _original = spriteRenderer.color;
    }

    public void ChangeCurentHealth(float damage)
    {
        if(damage < _health)
        {
            _health -= damage;
        }
        else
        {
            _health -= damage;
            Death();
        }
    }

    public void Heal(float hp)
    {
        _health += hp;
        StartCoroutine(ChangeColor(_gainHealth, _beOtherColorFor));
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

    public void Damage(float hp)
    {
        _health -= hp;
        if(_health <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(ChangeColor(_looseHealth, _beOtherColorFor));
        }
    }

    IEnumerator ChangeColor(Color color, float time)
    {
        float timePassed = 0;
        while(timePassed < time)
        {
            float number = timePassed / time;
            spriteRenderer.color = Color.Lerp(_original, color, number);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        float timePassedSec = 0;
        while (timePassed < time)
        {
            float number = timePassed / time;
            spriteRenderer.color = Color.Lerp(color, _original, number);
            timePassedSec += Time.deltaTime;
            yield return null;
        }
    }

    public void Death(float deathTime = 0)
    {
        OnDeathEvent.Invoke();
        Destroy(gameObject, deathTime);
    }
}

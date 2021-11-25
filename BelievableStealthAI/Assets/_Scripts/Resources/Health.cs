
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    public float _maxHealth;
    public float _currentHealth;

    [SerializeField] protected bool _canBeHarmed = true;

    public bool CanBeHarmed {  get { return _canBeHarmed; } set { _canBeHarmed = value; } }

    public float CurrentHealth { get { return _currentHealth; } }
    public float HealthRatio { get { return _currentHealth / _maxHealth; } }

    protected bool _isDead = false;
    public bool IsDead { get { return _isDead; } }

    public UnityEvent _OnDie;
    public UnityEvent _OnDamage;
    public void Heal(float amount) {
        Debug.Log("Healing by: " + amount);
        //Stops the health from going above maximum.
        _currentHealth = Mathf.Min(_currentHealth += amount, _maxHealth);
        OnHeal();
    }



    void Start()
    {
        _currentHealth = _maxHealth;
        _OnDie.AddListener(OnDeath);
        
        OnStart();
    }


    public virtual void TakeDamage(float amount)
    {
        if (_isDead) return;
        if (!_canBeHarmed) return;

        

        //Take away the left over damage and get the minimum from damage or 0 and set health to this
        _currentHealth = Mathf.Max(_currentHealth -= amount, 0f);

        if (_currentHealth == 0.0f)
        {
            _isDead = true;
            _OnDie.Invoke();
            return;
        }

        _OnDamage.Invoke();
        OnDamage();
    }

    protected virtual void OnHeal() {}
    protected virtual void OnStart() {}
    protected virtual void OnDeath() {}
    protected virtual void OnDamage() {}

}

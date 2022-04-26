
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

    public UnityEvent _OnDeath;
    public UnityEvent _OnDamage;


    //Auto Kills the character this is attached to
    public void Kill()
    {
        TakeDamage(100000.0f);
    }

    private void Update()
    {
        //Stop updating if we are dead
        if (_isDead) return;

        //If out health is less than 0
        if(_currentHealth <= 0.0f)
        {
            //character is dead
            _isDead = true;
            //Invoke the on death event.
            _OnDeath.Invoke();
            //Activates the ragdoll for this character
            GetComponent<Ragdoll>().ActivateRagdoll();

            AILocomotion locomotion = GetComponent<AILocomotion>();
            if (locomotion)
            {
                locomotion.CanMove(false);
            }

            //Sets the tag for this object. This allows AI to see a corpse on the ground
            gameObject.tag = "DeadBody";
        }
    }

    void Start()
    {
        _currentHealth = _maxHealth;
        _OnDeath.AddListener(OnDeath);
        _OnDamage.AddListener(OnDamage);
        
        OnStart();
    }


    public virtual void TakeDamage(float amount)
    {
        if (_isDead) return;
        if (!_canBeHarmed) return;

        //Take away the left over damage and get the minimum from damage or 0 and set health to this
        _currentHealth = Mathf.Max(_currentHealth -= amount, 0f);

        if (_currentHealth == 0.0f) return;

        //Invoke the on damage event
        _OnDamage.Invoke();
    }

    protected virtual void OnHeal() {}
    protected virtual void OnStart() {}
    protected virtual void OnDeath() {}
    protected virtual void OnDamage() {}

}

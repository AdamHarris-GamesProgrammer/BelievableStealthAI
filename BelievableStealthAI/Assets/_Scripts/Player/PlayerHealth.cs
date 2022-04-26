using UnityEngine;

public class PlayerHealth : Health
{
    CharacterAiming _aiming;
    Animator _animator;

    protected override void OnStart()
    {
        _aiming = GetComponent<CharacterAiming>();
        _animator = GetComponent<Animator>();
    }

    public override void TakeDamage(float amount)
    {
        //Stops the Character from taking damage if they don't need to.
        if (_isDead) return;
        if (!_canBeHarmed) return;

        base.TakeDamage(amount);
    }

    protected override void OnDeath()
    {
        //Stops the camera from moving
        _aiming.enabled = false;

        //Sets the dying animation trigger
        _animator.SetTrigger("isDead");
    }

}
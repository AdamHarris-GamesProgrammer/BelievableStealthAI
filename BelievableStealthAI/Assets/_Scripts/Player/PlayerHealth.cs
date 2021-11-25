using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TGP.Control;

public class PlayerHealth : Health
{
    CharacterAiming _aiming;
    Animator _animator;
    PlayerController _player;

    [SerializeField] GameObject _CRTCamera;
    [SerializeField] GameObject _deathScreenUI;


    protected override void OnStart()
    {
        _aiming = GetComponent<CharacterAiming>();
        _animator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
    }

    public override void TakeDamage(float amount)
    {
        //Stops the Character from taking damage if they don't need to.
        if (_isDead) return;
        if (!_canBeHarmed) return;
        //Calculates the amount of damage we take after our armors resistance

        base.TakeDamage(amount);
    }

   

    protected override void OnDeath()
    {
        //_audioSource.PlayOneShot(_deathAudioClip);
        //Drops our wepaon
        _aiming.enabled = false;

        //Disables our cameras
        GetComponent<PlayerController>().AimCam.SetActive(false);
        GetComponent<PlayerController>().FollowCam.SetActive(false);

        //Sets the dying animation trigger
        _animator.SetTrigger("isDead");
        //Enables the CRT camera and death screen
        _CRTCamera.SetActive(true);
        _deathScreenUI.SetActive(true);
    }

    protected override void OnDamage()
    {
        float healthPercentage = _currentHealth / _maxHealth;
    }

    protected override void OnHeal()
    {
        float healthPercentage = _currentHealth / _maxHealth;
    }
}

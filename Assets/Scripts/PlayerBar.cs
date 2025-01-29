using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Movement _playerMovement;

    [SerializeField] private Image _healthBar, _manaBar;

    float _playerMaxHealth;
    float _playerCurHealth;

    float _playerMaxMana;
    float _playerCurMana;

    float _playerManaOld;
    float _playerHealthOld;


    private void Update()
    {
        GetInfo();
    }

    void GetInfo()
    {
        if (_playerCurHealth != _playerHealth.GetCurHealth)
        {
            StartCoroutine(ChangeHealhBar());
        }
        else
        {
            _playerMaxHealth = _playerHealth.GetMaxHealth;
            _playerCurHealth = _playerHealth.GetCurHealth;
        }
        if (_playerCurMana != _playerMovement.GetCurDashes)
        {
            StartCoroutine(ChangeManaBar());
        }
        else
        {
            _playerMaxMana = _playerMovement.GetMaxDashes;
            _playerCurMana = _playerMovement.GetCurDashes;
        }
    }


    IEnumerator ChangeManaBar()
    {
        _playerMaxMana = _playerMovement.GetMaxDashes;
        _playerCurMana = _playerMovement.GetCurDashes;

        float timeToChange = 0;
        float timeForChanging = 0.5f;
        while(timeToChange < timeForChanging)
        {
            float number = timeToChange / timeForChanging;
            _manaBar.fillAmount = Mathf.Lerp(_manaBar.fillAmount == float.NaN ? _manaBar.fillAmount : _playerManaOld, _playerCurMana / _playerMaxMana, number);
            timeToChange += Time.deltaTime;
            yield return null;
        }
        _playerManaOld = _playerCurMana / _playerMaxMana;
    }

    IEnumerator ChangeHealhBar()
    {
        _playerMaxHealth = _playerHealth.GetMaxHealth;
        _playerCurHealth = _playerHealth.GetCurHealth;

        float timeToChange = 0;
        float timeForChanging = 1;
        while (timeToChange < timeForChanging)
        {
            float number = timeToChange / timeForChanging;
            _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount == float.NaN ? _healthBar.fillAmount : _playerHealthOld, _playerCurHealth / _playerMaxHealth, number);
            timeToChange += Time.deltaTime;
            yield return null;
        }
        _playerHealthOld = _playerCurHealth / _playerMaxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void Damage(float damage);
}

public interface IHealble
{
    void Heal(float health);
}

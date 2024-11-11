using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Health", menuName ="Humanoid")]
public class Humanoid : ScriptableObject
{
    [SerializeField] private int _maxHp, _armor;

    public int MaxHp { get => _maxHp; }
    public int Armor { get => _armor; }
}

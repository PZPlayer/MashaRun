using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Pistols")]
public class Weapon : ScriptableObject
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private string _name;
    [SerializeField] private float _reload;
    [SerializeField] private int _poolSize;

    public GameObject Bullet {  get { return _bullet; } }
    public string Name { get { return _name; } }
    public float Reload {  get { return _reload; } }

    public int PoolSize { get { return _poolSize; } }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WeaponUse : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _head, _hand;
    [SerializeField] private bool weaponActive;
    List<GameObject> bullets = new List<GameObject>();
    public UnityEvent shootEvent;
    float reload, reloadTimer;
    [SerializeField] private GameObject rotateWeaponObj, weaponHold;

    private void Start()
    {
        reload = _weapon.Reload;
        RestartPool();
        rotateWeaponObj = _weapon.WeaponPref;
        GameObject weapon = Instantiate(_weapon.WeaponPref, Vector3.zero, Quaternion.identity); 
        weapon.transform.parent = _hand.transform;
        weapon.transform.localPosition = Vector3.zero; 
        weapon.transform.localRotation = Quaternion.identity; 
        weaponHold = weapon;
        _shootPoint = weaponHold.transform.GetChild(0).gameObject.transform;
        weaponHold.SetActive(weaponActive);
    }

    void RestartPool()
    {
        bullets.Clear();
        for (int i = 0; i < _weapon.PoolSize; i++)
        {
            GameObject bullet = Instantiate(_weapon.Bullet);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }

    GameObject GetBulletFromPull()
    {
        for(int i = 0; i < bullets.Count ; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return bullets[i];
        }
        return null;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && weaponActive)
        {
            Shoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            weaponActive = !weaponActive;
            weaponHold.SetActive(weaponActive);
        }
    }

    void Shoot()
    {
        shootEvent.Invoke();
        GameObject curBullet = GetBulletFromPull();
        curBullet.SetActive(true);
        curBullet.transform.position = new Vector3(_shootPoint.position.x, _shootPoint.position.y , 0);
        curBullet.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y == 0 ? 90 : -90, 0);
        StartCoroutine(KillBulletAfterDelay(curBullet, 3));
    }

    IEnumerator KillBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

}

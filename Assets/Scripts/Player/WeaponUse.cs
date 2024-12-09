using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponUse : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _shootPoint, _canvas;
    [SerializeField] private GameObject _head, _hand, _weaponEquippedUI;
    [SerializeField] private bool weaponActive;
    [SerializeField] private int _magazine;
    List<GameObject> bullets = new List<GameObject>();
    public UnityEvent shootEvent;
    float reload, reloadTimer, waitBeforeShoot;
    [SerializeField] private GameObject rotateWeaponObj, weaponHold, _magazineChange;
    public bool ifWeaponActive { get => weaponActive; }

    private void Start()
    {
        reload = _weapon.Reload;
        _magazine = _weapon.PoolSize;
        GameObject magazineUi = _weapon.MagzinePref;
        _weaponEquippedUI.GetComponent<Image>().sprite = _weapon.Image;
        _magazineChange = Instantiate(_weapon.MagzinePref, _canvas);
        RestartPool();
        rotateWeaponObj = _weapon.WeaponPref;
        SpawnWeapon();
    }

    void SpawnWeapon()
    {
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

    void BulletsHide()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].gameObject.SetActive(false);
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
        waitBeforeShoot += Time.deltaTime;

        if (weaponActive)
        {
            _weaponEquippedUI.SetActive(true);
            _magazineChange.SetActive(true);
        }
        else
        {
            _weaponEquippedUI.SetActive(false);
            _magazineChange.SetActive(false);
        }

        if(_magazine == 0 && Input.GetKey(KeyCode.R))
        {
            StartCoroutine(Reload(_weapon.Reload));
            _magazine = -1;
        }
        if(Input.GetKey(KeyCode.R))
        {
            StopAllCoroutines();
            StartCoroutine(Reload(_weapon.Reload));
            _magazine = -1;
        }
        if (Input.GetMouseButtonDown(0) && weaponActive && waitBeforeShoot > 0.3f && _magazine != -1)
        {
            if(_magazine >  0)
            {
                waitBeforeShoot = 0;
                Shoot();
                _magazine--;
            }
            else
            {
                StartCoroutine(Reload(_weapon.Reload));
                _magazine = -1;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            weaponActive = !weaponActive;
            weaponHold.SetActive(weaponActive);
        }
    }

    IEnumerator Reload(float reloadTime)
    {
        //StartReloadAnim
        yield return new WaitForSeconds(reloadTime);
        _magazine = _weapon.PoolSize;
        _magazineChange.GetComponent<IMagazineChange>().ChangeToValue(_magazine);
        BulletsHide();
    }

    void Shoot()
    {
        shootEvent.Invoke();
        _magazineChange.GetComponent<IMagazineChange>().ChangeToValue(_magazine - 1);
        weaponHold.transform.GetChild(0).Find("Partcls").GetComponent<ParticleSystem>().Play();
        GameObject curBullet = GetBulletFromPull();
        curBullet.SetActive(true);
        curBullet.transform.position = new Vector3(_shootPoint.position.x, _shootPoint.position.y , 0);
        curBullet.transform.localScale = new Vector3(transform.localScale.x > 0 ? Mathf.Abs(curBullet.transform.localScale.x) : -Mathf.Abs(curBullet.transform.localScale.x), curBullet.transform.localScale.y, curBullet.transform.localScale.z);
    }

    IEnumerator KillBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

}

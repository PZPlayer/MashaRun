using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WeaponUse : MonoBehaviour
{
    [SerializeField]private Weapon _weapon;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _head;
    List<GameObject> bullets = new List<GameObject>();
    public UnityEvent shootEvent;
    float reload, reloadTimer;

    private void Start()
    {
        reload = _weapon.Reload;
        RestartPool();
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
        Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        viewportPosition.z = 0;
        _head.transform.Rotate(viewportPosition + transform.position);
        print(" " + Camera.main.ScreenToWorldPoint(Input.mousePosition) + " " + Input.mousePosition + " " + viewportPosition + " " + Camera.main.ViewportToWorldPoint(viewportPosition));

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        shootEvent.Invoke();
        GameObject curBullet = GetBulletFromPull();
        curBullet.SetActive(true);
        curBullet.transform.position = _shootPoint.position;
        curBullet.transform.rotation = _shootPoint.rotation;
        StartCoroutine(KillBulletAfterDelay(curBullet, 3));
    }

    IEnumerator KillBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

}

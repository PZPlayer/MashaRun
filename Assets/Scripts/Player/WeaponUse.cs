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
        Vector3 mousePos = Input.mousePosition;
        Vector2 viewportPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
        _head.transform.LookAt(viewportPosition);

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
        curBullet.transform.position = new Vector3(_shootPoint.position.x, _shootPoint.position.y , 0);
        curBullet.transform.rotation = _head.transform.rotation;
        StartCoroutine(KillBulletAfterDelay(curBullet, 3));
    }

    IEnumerator KillBulletAfterDelay(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

}

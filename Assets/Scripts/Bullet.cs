using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private float speed;
    [SerializeField] private ParticleSystem _hitWall, _hitEnemy, _basePartcls;
    [SerializeField] private float _damage;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageble damageble = other.gameObject.GetComponent<IDamageble>();
        if (damageble != null)
        {
            damageble.Damage(_damage);
            if (transform.GetChild(0).GetComponent<SpriteRenderer>().enabled == true)
            {
                _hitWall.Play();
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                _basePartcls.Stop();
                StartCoroutine(Disapier(0.3f));
            }
        }
    }

    IEnumerator Disapier(float time)
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        _basePartcls.Play();
        gameObject.SetActive(false);
    }
}

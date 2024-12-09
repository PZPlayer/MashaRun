using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField][Range(0, 100)] private float speed;
    [SerializeField] private ParticleSystem _hitWall, _hitEnemy, _basePartcls;
    [SerializeField] private float _damage;


    private void Awake()
    {
        TurnOn();
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.localScale.x > 0 ? Vector3.right : Vector3.left * speed * Time.deltaTime);
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
                _hitEnemy.Play();
                TurnOff();
                StartCoroutine(Disapier(1));
            }
        }
        else
        {
            _hitWall.Play();
            TurnOff();
            StartCoroutine(Disapier(1));
        }
    }

    private void TurnOn()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void TurnOff()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator Disapier(float time)
    {
        yield return new WaitForSeconds(time);
        TurnOn();
        gameObject.SetActive(false);
    }
}

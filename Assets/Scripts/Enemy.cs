using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Health _health;

    [SerializeField] private Humanoid _humanoid;

    private bool facingRight = false;

    private float agroFallTimer = 1;

    private Rigidbody2D _rb;

    private Vector2 trajecroty = Vector2.left;

    private GameObject Player;

    float timer;

    private void Start()
    {

    }

    virtual public void Calm()
    {

    }

    virtual public void Update()
    {
        if (Player != null)
        {
            Move();
        }
        if (ifCanSeePlayer())
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > agroFallTimer)
            {
                Player = null;
                timer = 0;
            }
        }
    }

    public virtual bool ifCanSeePlayer()
    {
        RaycastHit2D ray = Physics2D.BoxCast(transform.position, new Vector2(30, 50), 0, trajecroty);
        Debug.DrawRay(transform.position, new Vector2(trajecroty.x * 5, trajecroty.y * 20));
        if(ray.collider.GetComponent<Health>() && ray.collider.GetComponent<Movement>())
        {
            Player = ray.collider.gameObject;
            return true;
        }
        return false;
    }

    public virtual void Move()
    {
        if (facingRight)
        {
            trajecroty = Vector2.right;
        }
        else
        {
            trajecroty = Vector2.left;
        }
        transform.position += new Vector3(trajecroty.x, trajecroty.y, 0) * _speed;
    }
}

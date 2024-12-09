using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _damageKnockback;
    [SerializeField] private float _visionSize;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackWait;
    [SerializeField] private float _waitAfterAttack;
    [SerializeField] private float _stopDistance;

    [SerializeField] private ParticleSystem _biteParticle;

    [SerializeField] private LayerMask _layers;

    [SerializeField] private Health _health;

    [SerializeField] private Team _teamAnger;

    [SerializeField] private Humanoid _humanoid;

    private bool facingRight = false;

    private float agroFallTimer = 1;

    private Rigidbody2D _rb;

    private Animator animator;

    private Vector2 trajecroty = Vector2.left;

    [SerializeField] private GameObject Player;

    float timer, atckTimer, afterAttackWait = -1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    virtual public void Calm()
    {

    }

    virtual public void Update()
    {
        if (Player != null)
        {
            atckTimer += Time.deltaTime;
            if(Vector2.Distance(transform.position, Player.transform.position) <= _attackRange && atckTimer >= _attackWait)
            {
                Attack();
            }
            else if (Vector2.Distance(transform.position, Player.transform.position) <= _stopDistance)
            {
                animator.SetBool("Walking", false);
            }
            else
            {
                Move();
            }
            
            if(Player.transform.position.x > transform.position.x)
            {
                facingRight = true;
            }
            else
            {
                facingRight = false;
            }
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
                animator.SetBool("Walking", false);
                Player = null;
                timer = 0;
            }
        }
    }

    public virtual void Attack()
    {
        if(afterAttackWait == -1)
        {
            animator.SetTrigger("Attack");
            _biteParticle.Play();
            afterAttackWait = 0;
        }
        afterAttackWait += Time.deltaTime;
        if(afterAttackWait >= _waitAfterAttack)
        {
            atckTimer = 0;
            afterAttackWait = -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageble health = other.GetComponent<IDamageble>();
        if (health != null)
        {
            health.Damage(_damage);
            other.GetComponent<Rigidbody2D>().AddForce(trajecroty * _damageKnockback);
        }
    }

    public virtual bool ifCanSeePlayer()
    {
        Collider2D[] hitted = Physics2D.OverlapCircleAll(transform.position, _visionSize, _layers);
        for (int i = 0; i < hitted.Length; i++)
        {
            Health health = hitted[i].gameObject.GetComponent<Health>();
            if (health && health.team == _teamAnger)
            {
                RaycastHit2D[] objects = Physics2D.LinecastAll(transform.position, hitted[i].transform.position, _layers);
                for (int j = 0; j < objects.Length; j++)
                {
                    if (objects[j].collider.gameObject != transform.gameObject)
                    {
                        if (objects[j].collider.gameObject.GetComponent<Health>() && j == 1)
                        {
                            Player = objects[j].collider.gameObject;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public virtual void Move()
    {
        animator.SetBool("Walking", true);
        if (facingRight)
        {
            trajecroty = Vector2.right;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else
        {
            trajecroty = Vector2.left;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        transform.position += new Vector3(trajecroty.x, trajecroty.y, 0) * _speed;
    }
}

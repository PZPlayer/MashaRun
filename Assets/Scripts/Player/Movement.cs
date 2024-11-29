using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashReload;
    [SerializeField] private float _dashesAvaible;
    [SerializeField] private float _dashesReload;
    [SerializeField] private float _slowOnWeaponEquip = 0.5f;
    [SerializeField] private float _originalFriction;
    [SerializeField] private float _frictionMultiplier;
    [SerializeField] private float _coyoteTimer;
    [SerializeField] [Range(0f, 1f)] private float _frictionDuration;

    [SerializeField] private GameObject _feet;

    [SerializeField] private ParticleSystem _jumpPatcls, _dashPatcls;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Animator _anmtr;

    private Vector3 trajectory;
    private Rigidbody2D rb;
    private SpriteRenderer render;
    private float gravity;
    private float dashTimer;
    private float dashAddTimer;
    private float jumpcoyoteTimer;
    private float maxDashes;
    private bool weaponUp;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = rb.GetComponent<SpriteRenderer>();
        gravity = rb.gravityScale;
        _originalFriction = rb.drag;
        maxDashes = _dashesAvaible;
    }

    private void FixedUpdate()
    {
        trajectory.x = Input.GetAxis("Horizontal");
        trajectory *= Time.deltaTime * _speed;
        weaponUp = GetComponent<WeaponUse>().ifWeaponActive;
        if (trajectory.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (trajectory.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        _anmtr.SetBool("Run", trajectory.x != 0 ? true : false);

        transform.position += trajectory * (weaponUp == true ? _slowOnWeaponEquip : 1);
    }

    private void Update()
    {
        bool ifGrounded = Physics2D.OverlapBox(_feet.transform.position, new Vector2(3, 0.3f), 0, _layerMask);
        _anmtr.SetBool("Falling", !ifGrounded);

        dashTimer += Time.deltaTime;
        dashAddTimer += Time.deltaTime;

        if (dashAddTimer >= _dashesReload && _dashesAvaible < maxDashes)
        {
            dashAddTimer = 0;
            _dashesAvaible++;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer >= _dashReload && _dashesAvaible > 0)
        {
            dashTimer = 0;
            _dashesAvaible--;
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.rotation.eulerAngles.y == 180 ? Vector2.left * _dashForce  : Vector2.right * _dashForce, ForceMode2D.Impulse);
            _dashPatcls.Play();
            _anmtr.SetTrigger("Dash");
            StartCoroutine(ReduceFriction());
        }

        if (Input.GetKeyDown(KeyCode.Space) && (ifGrounded || jumpcoyoteTimer > 0))
        {
            Jump();
            _anmtr.SetBool("Jump", true);
            jumpcoyoteTimer = 0;
        }

        if (!ifGrounded)
        {
            jumpcoyoteTimer -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                rb.gravityScale = gravity * 0.7f;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                rb.gravityScale = gravity * 2.5f;
            }

            if(rb.velocity.y >= 0)
            {
                _anmtr.SetBool("Jump", true);
            }
            else
            {
                _anmtr.SetBool("Jump", false);
            }
        }
        else
        {
            jumpcoyoteTimer = _coyoteTimer;
            rb.gravityScale = gravity * 1f;
            _anmtr.SetBool("Jump", false);
        }
    }

    IEnumerator ReduceFriction()
    {
        yield return new WaitForSeconds(_frictionDuration);
        rb.drag = _originalFriction * _frictionMultiplier;
        yield return new WaitForSeconds(_frictionDuration);
        rb.drag = _originalFriction;
    }

    void Jump()
    {
        _jumpPatcls.Play();
        rb.AddForce(Vector2.up * _jumpForce);
    }
}

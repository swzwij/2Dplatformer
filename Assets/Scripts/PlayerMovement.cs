using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;    

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;    

    [Header("Movement Variables")]
    [SerializeField] private float _movementAcceleration;    
    [SerializeField] private float _maxMoveSpeed;           
    [SerializeField] private float _groundLinearDrag;       
    private bool _facingRight = true;                       
    private float _horizontalDirection;                     
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f); 

    [Header("Jump Variables")]
    [SerializeField] private float _jumpForce = 12f;                
    [SerializeField] private float _airLinearDrag = 2.5f;           
    [SerializeField] private float _fallMultiplier = 8f;            
    [SerializeField] private float _lowJumpFallMultiplier = 5f;     
    [SerializeField] private float _hangTime;                       
    [SerializeField] private float _hangCounter;                    
    private bool _canJump => Input.GetButtonDown("Jump") && _hangCounter > 0.1f;

    [Header("Dash Variables")]
    [SerializeField] private float _horizontalDashSpeed;
    [SerializeField] private float _verticalDashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _startDashTime;
    [SerializeField] private int _dashDirction;

    [Header("Ground Collision Variables")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private bool _onGround;
    [SerializeField] private Vector3 _groundRaycastOffset;

    [Header("Particle Variables")]
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private bool _spawnDust;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dashTime = _startDashTime;
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x; 

        if (_onGround)
        {
            _hangCounter = _hangTime; 

            if (_spawnDust)
            {
                _dustEffect.Play(); 
                _spawnDust = false; 
            }
        }
        else
        {
            _hangCounter -= Time.deltaTime; 
            _spawnDust = true; 
        }

        if (_canJump)
        {
            Jump(); 
        }
        
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0)
        {
            
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * .5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && _facingRight)
        {
            Flip();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !_facingRight)
        {
            Flip();
        }

        if(_dashDirction == 0)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _dashDirction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _dashDirction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _dashDirction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _dashDirction = 4;
            }
        }
        else
        {
            if(_dashTime <= 0)
            {
                _dashDirction = 0;
                _dashTime = _startDashTime;
                _rb.velocity = Vector2.zero;
            }
            else
            {
                _dashTime -= Time.deltaTime;

                if(_dashDirction == 1)
                {
                    _rb.velocity = Vector2.left * _horizontalDashSpeed;
                }
                else if (_dashDirction == 2)
                {
                    _rb.velocity = Vector2.right * _horizontalDashSpeed;
                }
                else if (_dashDirction == 3)
                {
                    _rb.velocity = Vector2.up * _verticalDashSpeed;
                }
                else if (_dashDirction == 4)
                {
                    _rb.velocity = Vector2.down * _verticalDashSpeed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGroundCollision();
        MoveCharacter();

        if (_onGround)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
        }

    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAcceleration);

        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _groundLinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButtonDown("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(Vector3.up * 180);
    }

    private void CheckGroundCollision()
    {
        _onGround = Physics2D.Raycast(transform.position + _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer) ||
                    Physics2D.Raycast(transform.position - _groundRaycastOffset, Vector2.down, _groundRaycastLength, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundRaycastLength);
    }
}

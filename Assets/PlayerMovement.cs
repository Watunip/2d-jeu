using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Exposed
    [Header("Valeur pour le déplacement et le jump du personnage")]
    [SerializeField]
    float _moveSpeed;
    [SerializeField]
    float _JumpForce;
    [SerializeField]
    int _maxJump;
    [SerializeField]
    int _fallGravity;
    [SerializeField]
    float _jumpGravity;


    [Header("Animator")]
    [SerializeField]
    Animator _animator;

    [Header("GroundChecker")]
    [SerializeField]
    [Range(0, 0.6f)]
    float _radius;
    [SerializeField]
    [Range(-5, 5)]
    float _offsetGroundChecherk;
    [SerializeField]
    LayerMask _layer;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
        //Récupération des Bouttons pour le déplacement
        _direction.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;

        _animator.SetFloat("velocity x", Mathf.Abs(_direction.x));
        _animator.SetFloat("velocity y", _direction.y);

        //Récupération des Bouttons pour le saut
        if (Input.GetButtonDown("Jump") && _numberJump< _maxJump)
        {
            _isJumping = true;
        }



        GroundCheck();
       
        

        
    }

    private void FixedUpdate()
    {
        //Appliquer une gravité permanante
        _direction.y = _rb2D.velocity.y;

        //Application de la force pour le déplacement
        _rb2D.velocity = _direction;

        //Application de la force pour le saut
        if (_isJumping && _numberJump<= _maxJump )
        {
            _numberJump++;
            _isJumping = false;


            /*Vector2 jumpVector = new Vector2(_direction.x, _direction.y = _JumpForce);
            _rb2D.AddForce(jumpVector);*/

            _direction.y = _JumpForce;
            _rb2D.velocity = _direction * Time.fixedDeltaTime;
        }

        //Gère la vitesse de chute du player
        if (_rb2D.velocity.y < 0)
        {
            _rb2D.gravityScale = 3;
        }
        else if (_rb2D.velocity.y > 0.1f)
        {
            _rb2D.gravityScale = _jumpGravity;
        }
        else
        {
            _rb2D.gravityScale = 1;
        }

        //Tourner le personnage dans la bonne direction : 2 Methodes (avec le scale et avec le transforme.right)
        if (_direction .x < 0f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(_direction.x > 0f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        /*if (_direction.x < 0f || _direction.x > 0f)
        {
            transform.right = new Vector2(_director.x, 0);
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 positionTransformeOffset = new Vector3(transform.position.x, transform.position.y - _offsetGroundChecherk, transform.position.z);
        Gizmos.DrawWireSphere(positionTransformeOffset, _radius);
    }


    #endregion

    #region Methods

    void GroundCheck()
    {
        //Groundchecker
        Vector3 positionTransformeOffset = new Vector3(transform.position.x, transform.position.y - _offsetGroundChecherk, transform.position.z);
        Collider2D floorCollider = Physics2D.OverlapCircle(positionTransformeOffset, _radius, _layer);

        if (floorCollider != null)
        {

            _animator.SetTrigger("Grounded");
            _numberJump = 0;

            if (floorCollider.CompareTag("Platfom"))
            {
                transform.SetParent(floorCollider.transform);
            }
        }
        else
        {
            transform.SetParent(null);
        }
    }

    #endregion

    #region Private & Protected

    private Rigidbody2D _rb2D;
    private Vector2 _direction;
    private bool _isJumping;
    private int _numberJump;

    #endregion
}

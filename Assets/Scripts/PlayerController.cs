using System;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //PLAYER COMPONENTS
    private Rigidbody2D _rigidbody2D;
    private GatherInput _gatherInput;
    private Transform _transform;
    private Animator _animator;

    [Header("Move and Jump settings-------")]
    [SerializeField]private float speed;
    private int direction = 1; //La direccion donde mira el player    
    [SerializeField] private float jumpForce; //Fuerza del salto
    private int IdSpeed;
    //Doble salto
    [SerializeField] private int extraJumps=1;
    [SerializeField] private int counterExtraJumps;

    [Header("Ground settings-------")]
    //Controla el salto en el aire. Es necesario crear 2 layers Player(para el jugador) y Ground(para el suelo) asi como dos
    //GameObject en los pies del player. Configurar tambien desde el Proyect setting las capas de phisics2d
    [SerializeField] private Transform lFoot;
    [SerializeField] private Transform rFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLenght;
    [SerializeField] private LayerMask groundLayer; //La layer con la que choca    
    private int idIsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gatherInput = GetComponent<GatherInput>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        //Se usa en el animator - SetAnimatorValues
        IdSpeed = Animator.StringToHash("speed");
        idIsGrounded = Animator.StringToHash("isGrounded");

        lFoot = GameObject.Find("Lfoot").GetComponent<Transform>();
        rFoot = GameObject.Find("Rfoot").GetComponent<Transform>();

        counterExtraJumps =extraJumps;
    }

    private void Update()
    {
        SetAnimatorValues();        
    }

    /// <summary>
    /// Actualiza los parametros del animator para que transicionen los estamos
    /// </summary>
    private void SetAnimatorValues()
    {
        //Actualizar el parametro speed del animator 
        _animator.SetFloat(IdSpeed, math.abs(_rigidbody2D.linearVelocityX)); //abd->pasa el valor a positivo
        _animator.SetBool(idIsGrounded, isGrounded);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Para moverse
        Move();
        //Saltar
        Jump();
        //Controlar el salto
        CheckGround();
    }

    private void CheckGround()
    {
        //Variable donde almacena cuando choca el rayo
        RaycastHit2D lFootRay = Physics2D.Raycast(lFoot.position,Vector2.down,rayLenght,groundLayer);
        RaycastHit2D rFootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLenght, groundLayer);

        if (lFootRay || rFootRay)
        {
            isGrounded = true;
            counterExtraJumps = extraJumps; //Cuando toca el suelo se restablecen los saltos extras
        }
        else
        {
            isGrounded = false;
        }

    }

    /// <summary>
    /// Mover al personaje
    /// </summary>
    private void Move()
    {
        Flip();
        _rigidbody2D.linearVelocity = new Vector2(speed * _gatherInput.ValueX, _rigidbody2D.linearVelocityY);
    }

    /// <summary>
    /// Gira o no al personaje
    /// </summary>
    private void Flip()
    {
        if (_gatherInput.ValueX * direction < 0)
        {
            _transform.localScale = new Vector3(-_transform.localScale.x,1,1);
            direction *= -1;
        }
    }

    /// <summary>
    /// Hace saltar al personaje
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Jump()
    {
        if (_gatherInput.IsJumping) //Si esta saltando
        {
            if (isGrounded) 
              _rigidbody2D.linearVelocity = new Vector2(_gatherInput.ValueX,jumpForce);

            //Doble salto
            if (counterExtraJumps > 0)
            {
                _rigidbody2D.linearVelocity = new Vector2(_gatherInput.ValueX, jumpForce);
                counterExtraJumps--;
            }
        }

        _gatherInput.IsJumping = false; //Ya saltó
    }
}
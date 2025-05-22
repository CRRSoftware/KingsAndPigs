using System;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private GatherInput _gatherInput;
    private Transform _transform;
    [SerializeField]private float speed;
    private int direction = 1; //La direccion donde mira el player


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gatherInput = GetComponent<GatherInput>();
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Para moverse
        Move();
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
}

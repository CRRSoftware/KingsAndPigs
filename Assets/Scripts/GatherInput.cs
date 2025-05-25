using UnityEngine;
using UnityEngine.InputSystem; //Añadimos la libreria InputSystem
public class GatherInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField]private float _valueX; //ALT+ENTER para autogenerar el encapsulamiento
    [SerializeField] private bool _isJumping;
    
    private void Awake()
    {
        controls = new Controls(); 
    }

    /// <summary>
    /// Activamos todo
    /// </summary>
    private void OnEnable()
    {
        //Enlazamos a los metodos con el PlayerInput definido
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Jump.performed += StartJump;
        controls.Player.Jump.canceled += StopJump;

        controls.Player.Enable(); //Activamos los controles 'Player' del controls
    }

    /// <summary>
    /// Desactivamos todo
    /// </summary>
    private void OnDisable()
    {
        //Desenlazamos a los metodos
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Jump.performed -= StartJump;
        controls.Player.Jump.canceled -= StopJump;

        controls.Player.Disable(); //Desactivos los controles 'Player' del controls
    }

    /// <summary>
    /// Empieza el movimiento al pulsar izquierda o derecha
    /// </summary>
    /// <param name="context"></param>
    private void StartMove(InputAction.CallbackContext context)
    {
        _valueX = context.ReadValue<float>();
    }

    /// <summary>
    /// Cuando no hay movimiento
    /// </summary>
    /// <param name="context"></param>
    private void StopMove(InputAction.CallbackContext context)
    {
        _valueX = 0;
    }

    private void StartJump(InputAction.CallbackContext context)
    {
        _isJumping = true;
    }

    private void StopJump(InputAction.CallbackContext context)
    {
        _isJumping = false;
    }


    //Acceso de campos privados
    public float ValueX { get => _valueX; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
}

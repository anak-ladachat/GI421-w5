using UnityEngine;

namespace BU.Workshop
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed = 5f;

        [SerializeField]
        private float _lookSpeed = 2f;

        [SerializeField]
        private float _jumpForce = 5f;

        [SerializeField]
        private float _gravity = -9.81f;

        [SerializeField]
        private float _groundDrag = 5f;

        [SerializeField]
        private LayerMask _groundLayer;

        private CharacterController _characterController;
        private Camera _mainCamera;
        private float _xRotation = 0f;
        private Vector3 _velocity;
        private bool _isGrounded;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            HandleMouseLook();
            HandleMovement();
        }

        private void HandleMouseLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * _lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * _lookSpeed;

            // Rotate body left/right
            transform.Rotate(Vector3.up * mouseX);

            // Rotate camera up/down
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            _mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void HandleMovement()
        {
            _isGrounded = _characterController.isGrounded;

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }

            // Get input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate movement direction
            Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            moveDirection = moveDirection.normalized;

            // Apply movement
            _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
            }

            // Apply gravity
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}

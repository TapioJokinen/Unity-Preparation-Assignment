using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public CinemachineCamera playerCamera;
        public InputAction keyboardMovement;
        public InputAction mouseLeftClick;
        public InputAction mouseRightClick;
        public InputAction jump;

        private CharacterController _controller;
        private const float MovementSpeed = 5f;
        private Vector2 _movementInput;
        private Vector3 _playerVelocity;
        private Vector3 _jumpMovement;
        private const float Gravity = -9.81f;
        private const float JumpHeight = 3f;

        private void Awake()
        {
            InitializeInput();
            _controller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            keyboardMovement.Enable();
            mouseLeftClick.Enable();
            mouseRightClick.Enable();
            jump.Enable();
        }

        private void OnDisable()
        {
            keyboardMovement.Disable();
            mouseLeftClick.Disable();
            mouseRightClick.Disable();
            jump.Disable();
        }

        private void Update()
        {
            // ApplyPlayerRotation();
            ApplyMovement();
            ApplyJumpGravity();
            ApplyPlayerRotation();

        }

        /// <summary>
        /// Initialize the player's input actions
        /// </summary>
        private void InitializeInput()
        {
            keyboardMovement = new InputAction("Move", binding: "<Keyboard>/2DVector");
            keyboardMovement.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            jump = new InputAction("Jump", binding: "<Keyboard>/space");
            jump.performed += _ => TryJump();
            
            mouseLeftClick = new InputAction("MouseLeftClick", binding: "<Mouse>/leftButton");
            mouseRightClick = new InputAction("MouseRightClick", binding: "<Mouse>/rightButton");
        }

        /// <summary>
        /// Apply movement vector to the character controller
        /// </summary>
        private void ApplyMovement()
        {
            Cursor.lockState = _isMouseDoublePressed() ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !_isMouseDoublePressed();
            
            var move = GetCurrentMovement();
            _controller.Move(move * (MovementSpeed * Time.deltaTime));
        }
        
        /// <summary>
        /// Try to make the player jump. Fails if already in the air to prevent double jumping
        /// </summary>
        private void TryJump()
        {
            if (!_controller.isGrounded) return;
            
            // Store the jump movement to apply it to the player's velocity when they are in the air
            _jumpMovement = GetCurrentMovement();
            
            // Math taken from https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
            _playerVelocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        /// <summary>
        /// Apply gravity to the player's velocity and move the player
        /// </summary>
        private void ApplyJumpGravity()
        {
            if(_controller.isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }
            else
            {
                _playerVelocity.y += Gravity * Time.deltaTime;
            }
            
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Checks if the player is pressing both the left and right mouse buttons
        /// </summary>
        /// <returns>True, if both mouse buttons are held down</returns>
        private bool _isMouseDoublePressed()
        {
            return mouseLeftClick.ReadValue<float>() > 0 && mouseRightClick.ReadValue<float>() > 0;
        }
        
        /// <summary>
        /// Get mouse movement which is the direction the player is looking
        /// </summary>
        /// <returns></returns>
        private Vector3 GetMouseMovement()
        {
            return playerCamera.transform.forward;
        }
        
        /// <summary>
        /// Get keyboard movement which is the direction the player is moving
        /// </summary>
        /// <returns></returns>
        private Vector3 GetKeyboardMovement()
        {
            _movementInput = keyboardMovement.ReadValue<Vector2>();
    
            // Convert local input direction to world space
            var tf = transform;
            var moveDirection = tf.right * _movementInput.x + tf.forward * _movementInput.y;
            moveDirection.y = 0;

            return moveDirection.normalized;
        }


        /// <summary>
        /// Get the current movement based on the player's input
        /// </summary>
        /// <returns></returns>
        private Vector3 GetCurrentMovement()
        {
            if(!_controller.isGrounded) return _jumpMovement;
            
            return _isMouseDoublePressed() ? GetMouseMovement() : GetKeyboardMovement();
        }

        private void ApplyPlayerRotation()
        {
            if (!mouseRightClick.IsPressed()) return;
            
            transform.rotation = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0);
        }
    }
}

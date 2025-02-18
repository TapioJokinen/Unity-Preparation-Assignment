using System;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Camera
{
    public class FreeLookInputController : InputAxisControllerBase<FreeLookInputController.MouseReader>
    {
        private void Update()
        {
            if (GameManagerController.Instance.ActiveState == GameManagerController.GameState.InGame)
                UpdateControllers();
        }

        [Serializable]
        public class MouseReader : IInputAxisReader
        {
            private const float CameraSpeed = 5f;
            private const float Invert = -1;

            public float GetValue(Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
            {
                // Allow camera rotation only when right mouse button is pressed
                if (!Mouse.current.rightButton.isPressed) return 0;
                
                return hint switch
                {
                    IInputAxisOwner.AxisDescriptor.Hints.X => Mouse.current.delta.x.ReadValue() * CameraSpeed,
                    IInputAxisOwner.AxisDescriptor.Hints.Y => Mouse.current.delta.y.ReadValue() * CameraSpeed * Invert,
                    _ => 0
                };
            }
        }
    }
}
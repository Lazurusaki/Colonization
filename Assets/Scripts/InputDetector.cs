using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDetector : MonoBehaviour
{
    public event Action SelectButtonPressed;

    private InputMap _inputMap;

    private void Awake()
    {
        _inputMap = new InputMap();
    }

    private void OnEnable()
    {
        _inputMap.Enable();
        _inputMap.Player.Select.started += OnSelect;
    }

    private void OnDisable()
    {
        _inputMap.Disable();
        _inputMap.Player.Select.started -= OnSelect;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        SelectButtonPressed?.Invoke();
    }
}

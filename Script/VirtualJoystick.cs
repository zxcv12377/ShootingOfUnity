using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField, Range(10, 150)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    public PlayerController controller;

    //public enum JoystickType { Move, Roate};
    //public JoystickType joystickType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControllerJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControllerJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        controller.MoveTo(Vector2.zero);
    }

    private void ControllerJoystickLever(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;
        inputDirection = inputVector / leverRange;
    }

    private void InputControlVector()
    {
        controller.MoveTo(inputDirection);
    }
    void Update()
    {
        if (isInput)
        {
            InputControlVector();
        }
    }
}

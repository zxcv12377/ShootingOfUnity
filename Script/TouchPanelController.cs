using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanelController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;

    public PlayerController controller;

    [SerializeField, Range(500,1000)]
    private float range = 500f;

    private Vector2 inputDirection;
    private bool isInput;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlTouchPanel(eventData);
        isInput = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlTouchPanel(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isInput = false;
    }

    private void ControlTouchPanel(PointerEventData eventData)
    {
        var inputPos = eventData.position - rectTransform.anchoredPosition;
        var inputVector = inputPos.magnitude < range ? inputPos : inputPos.normalized * range;
        inputDirection = inputVector / range;
    }
    // Update is called once per frame
    void Update()
    {
        if (isInput)
        {
            controller.LookAround(inputDirection);
        }
    }
}

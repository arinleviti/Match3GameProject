using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PIANamespace;


// This class contains the logic that allows candies to be moved, interacting with the Input System.
public class MovementViewer : MonoBehaviour
{
    public InputAction moveAction;
    public float moveSpeed;
    private PlayerInputActions playerInputActions; // Reference to the input actions
    private GridManagerViewer gridManager;
    public GameSettings gameSettings;
    public List<GameObject> matchesVer;
    public List<GameObject> matchesHor;
    public MatchHandlerViewer matchHandler;
    private CandyPool candyPool;
    private MovementModelController movementModelController;
    public event Action OnMovePerformedComplete;
    private AudioManager _audioManager;

    private void Awake()
    {
        // Initialize PlayerInputActions
        playerInputActions = new PlayerInputActions();
        // Reference the Move action
        moveAction = playerInputActions.Player.Move;  // Player is the action map and Move is the action
        gridManager = FindObjectOfType<GridManagerViewer>();
        matchHandler = FindObjectOfType<MatchHandlerViewer>();
        moveAction.Enable();
        EventDispatcher.DisableInputEvent += DisableInputSystem;
        EventDispatcher.EnableInputEvent += EnableInputSystem;
    }
    public void Initialize(CandyPool candyPoolScript, AudioManager audioManager )
    {
        _audioManager = audioManager;
        candyPool = candyPoolScript;
        movementModelController = new MovementModelController(gridManager, gameSettings, candyPool, matchHandler, this, _audioManager);
        //OnMoveCallback is a method that will be called whenever the moveAction.performed event occurs.
        moveAction.performed += movementModelController.OnMoveCallback;
    }
    

    private void OnDisable()
    {
        moveAction.performed -= movementModelController.OnMoveCallback; // Unsubscribe from the event to avoid memory leaks
        moveAction.Disable();
        EventDispatcher.DisableInputEvent -= DisableInputSystem;
        EventDispatcher.EnableInputEvent -= EnableInputSystem;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SelectCandyWithMouse();           
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            movementModelController.SelectedCandy = null; // Reset selectedCandy when mouse button is released
            movementModelController.IsMoving = false;           
        }
    }

    private void SelectCandyWithMouse()
    {
        // Creates the ray.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Checks if the ray intersects with a 2D collider. Result stored in hit.
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            CandyViewer candy = hit.collider.GetComponent<CandyViewer>();
            if (candy != null)
            {
                SelectCandy(candy);
            }
        }
    }
    public void InvokeEvent()
    {      
        OnMovePerformedComplete?.Invoke();
    }
    private void DisableInputSystem()
    {
        moveAction.Disable();
        Debug.Log($"Player input disabled at {System.DateTime.Now:HH:mm:ss.fff}.");
    }
    private void EnableInputSystem()
    {
        moveAction.Enable();
        Debug.Log($"Player input enabled at{System.DateTime.Now:HH:mm:ss.fff}");
    }
    public void SelectCandy(CandyViewer candy)
    {
        movementModelController.SelectedCandy = candy;
    }
}





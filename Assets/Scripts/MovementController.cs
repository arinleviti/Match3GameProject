using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This class contains the logic that allows candies to be moved, interacting with the Input System.
public class MovementController : MonoBehaviour
{
    public InputAction moveAction;
    public float moveSpeed;
    private Candy selectedCandy;
    //private Vector2 initialTouchPos;
    private PlayerInputActions playerInputActions; // Reference to the input actions
    private GridManager gridManager;
    public GameSettings gameSettings;
    public List<GameObject> matchesVer;
    public List<GameObject> matchesHor;
    public MatchHandler matchHandler;
    private CandyPool candyPool;

    public event Action OnMovePerformedComplete;

    private bool isMoving = false;

    private bool isActive1= false;

    private void Awake()
    {
        // Initialize PlayerInputActions
        playerInputActions = new PlayerInputActions();

        // Reference the Move action
        moveAction = playerInputActions.Player.Move;  // Player is the action map and Move is the action

        gridManager = FindObjectOfType<GridManager>();

        moveAction.Enable();
    }
    private void Start()
    {
        candyPool = FindObjectOfType<CandyPool>();
    }
    private void OnEnable()
    {
        //It gets called as soon as the input changes and keeps firing while the input is continuously changing
        moveAction.performed += OnMovePerformed;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed; // Unsubscribe from the event to avoid memory leaks
        moveAction.Disable();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SelectCandyWithMouse();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            selectedCandy = null; // Reset selectedCandy when mouse button is released
            isMoving= false;
        }
    }

    private void SelectCandyWithMouse()
    {
        //Creates the ray.
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Checks if the ray intersects with a 2D collider. Result stored in hit.
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            Candy candy = hit.collider.GetComponent<Candy>();
            if (candy != null)
            {
                SelectCandy(candy);
            }

        }

    }
   
    //The InputAction.CallbackContext object provides the context for the event, including the current value of the action.
    //When an input action is performed, started, or canceled, Unity generates a CallbackContext struct that contains details about that event.
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Get the swipe direction from the Move action.
        //returns a Vector2 value that represents how much the mouse has moved on the X and Y axes since the last frame.
        //values received represent the movement of the mouse relative to its previous position, not the actual world position of the mouse.
        Vector2 moveInput = context.ReadValue<Vector2>();


        if (selectedCandy != null && moveInput.magnitude > gameSettings.deltaMovementThreshold)
        {
            if (!isMoving)
            {
                candyPool = FindObjectOfType<CandyPool>();
                CandySwapper.Instance.Initialize(selectedCandy, gridManager, gameSettings, candyPool);            
                //SwapCandiesWrapper(moveInput);
                CandySwapper.Instance.SwapCandies(moveInput);
                /*SwapCandies(moveInput);*/  // Move the candy based on the input direction
                Debug.Log("Move Input Magnitude: " + moveInput.magnitude);
                matchHandler = FindObjectOfType<MatchHandler>();
                
                if (isActive1==false)
                {
                    PostMatchDrop.Instance.Initialize(matchHandler, candyPool, gameSettings, gridManager, this.gameObject);
                    isActive1 = true;
                }              
                Debug.Log("OnMovePerformed triggered");
                //PostMatchDrop.Instance.PostMovementMatchCheck();
                OnMovePerformedComplete?.Invoke();
                isMoving = true;
            }

        }
        else if (selectedCandy != null)
        {
            Debug.Log("Move Input Magnitude Insufficient: " + moveInput.magnitude);
        }

    }

    public void SelectCandy(Candy candy)
    {
        selectedCandy = candy;
    }

}

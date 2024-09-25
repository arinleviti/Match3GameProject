using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public InputAction moveAction;
    public float moveSpeed;
    private Candy selectedCandy;
    //private Vector2 initialTouchPos;
    private PlayerInputActions playerInputActions; // Reference to the input actions
    private GridManager gridManager;
    public GameSettings gameSettings;

    private void Awake()
    {
        // Initialize PlayerInputActions
        playerInputActions = new PlayerInputActions();

        // Reference the Move action
        moveAction = playerInputActions.Player.Move;  // Player is the action map and Move is the action
        
        gridManager = FindObjectOfType<GridManager>();
        moveAction.Enable();
    }

    //performed, started, and canceled are specific events that belong to InputAction in Unity's Input System.
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
            SwapCandies(moveInput);  // Move the candy based on the input direction
            Debug.Log("Move Input Magnitude: " + moveInput.magnitude);
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

    private void SwapCandies(Vector2 direction)
    {
        if (selectedCandy != null)
        {

            // Get the current grid coordinates (I, J) of the selected candy
            int currentI = selectedCandy.PosInArrayI;
            int currentJ = selectedCandy.PosInArrayJ;

            // Determine new coordinates based on direction
            int newI = currentI;
            int newJ = currentJ;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Move horizontally (left or right)
                if (direction.x > 0)
                {
                    newJ++; // Move right               
                }
                else if (direction.x < 0)
                {
                    newJ--; // Move left
                }
            }
            else
            {
                // Move vertically (up or down)
                if (direction.y > 0)
                {
                    newI--; // Move up
                }
                else if (direction.y < 0)
                {
                    newI++; // Move down
                }
            }

            // Ensure the new position is within grid bounds
            if (newI >= 0 && newI < gridManager.gameSettings.tilesNumberI && newJ >= 0 && newJ < gridManager.gameSettings.tilesNumberJ)
            {
                // Get the second candy to be swapped with the selected candy
                Candy secondCandy = gridManager.candiesArray[newI, newJ].GetComponent<Candy>();

                // Swap the candies in the array
                gridManager.candiesArray[currentI, currentJ] = secondCandy.gameObject;
                gridManager.candiesArray[newI, newJ] = selectedCandy.gameObject;

                // Update their properties
                secondCandy.PosInArrayI = currentI;
                secondCandy.PosInArrayJ = currentJ;

                selectedCandy.PosInArrayI = newI;
                selectedCandy.PosInArrayJ = newJ;

                // Swap their world positions
                Vector3 selectedCandyTargetPos = gridManager.gridCellsArray[newI, newJ].transform.position;
                Vector3 secondCandyTargetPos = gridManager.gridCellsArray[currentI, currentJ].transform.position;

                selectedCandy.transform.position = selectedCandyTargetPos;
                secondCandy.transform.position = secondCandyTargetPos;

                Vector3 selectedCandyRaisedPos = new Vector3(selectedCandy.transform.position.x, selectedCandy.transform.position.y, -1f);
                Vector3 secondCandyRaisedPos = new Vector3(secondCandy.transform.position.x, secondCandy.transform.position.y, -1f);
                selectedCandy.transform.position = selectedCandyRaisedPos;
                secondCandy.transform.position = secondCandyRaisedPos;
            }

            // Deselect the candy after moving
            selectedCandy = null;
        }
    }

    
}

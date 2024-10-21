using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementModelController
{

    private CandyPool _candyPool;
    private GridManagerViewer _gridManager;
    private GameSettings _gameSettings;
    private MatchHandlerViewer _matchHandler;
    public CandyViewer SelectedCandy { get; set; }
    private MovementViewer _movementViewer;
    public bool IsMoving { get; set; } = false;
    private bool isActive1 = false;
 

    public MovementModelController(GridManagerViewer gridManager, GameSettings gameSettings, CandyPool candyPool, MatchHandlerViewer matchHandler, MovementViewer movementViewer)
    {
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        _matchHandler = matchHandler;
        _movementViewer = movementViewer;
    }

    public void OnMovePerformed(InputAction.CallbackContext context/*, CandyViewer selectedCandy*/)
    {
        // Get the swipe direction from the Move action.
        // returns a Vector2 value that represents how much the mouse has moved on the X and Y axes since the last frame.
        // values received represent the movement of the mouse relative to its previous position, not the actual world position of the mouse.
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (SelectedCandy != null && moveInput.magnitude > _gameSettings.deltaMovementThreshold)
        {
            if (!IsMoving)
            {
                
                CandySwapperViewer.Instance.Initialize(SelectedCandy, _gridManager, _gameSettings, _candyPool);
                // SwapCandiesWrapper(moveInput);
                CandySwapperViewer.Instance.SwapperModel.SwapCandies(moveInput);
                /*SwapCandies(moveInput);*/  // Move the candy based on the input direction
                Debug.Log("Move Input Magnitude: " + moveInput.magnitude);
                //matchHandler = FindObjectOfType<MatchHandlerViewer>();

                if (isActive1 == false)
                {
                    PostMatchDrop.Instance.Initialize(_matchHandler, _candyPool, _gameSettings, _gridManager, _movementViewer.gameObject);
                    isActive1 = true;
                }
                Debug.Log("OnMovePerformed triggered");
                // PostMatchDrop.Instance.PostMovementMatchCheck();
                IsMoving = true;
                _movementViewer.InvokeEvent();
            }
        }
        else if (SelectedCandy != null)
        {
            Debug.Log("Move Input Magnitude Insufficient: " + moveInput.magnitude);
        }
    }

}

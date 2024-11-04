using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static MovementModelController;

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
    private CandySwapperViewer _candySwapperViewer;
    private PostMatchDrop _postMatchDrop;

    public MovementModelController(GridManagerViewer gridManager, GameSettings gameSettings, CandyPool candyPool, MatchHandlerViewer matchHandler, MovementViewer movementViewer)
    {
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        _matchHandler = matchHandler;
        _movementViewer = movementViewer;
        _candySwapperViewer = CandySwapperViewer.Instance;
    }
    public void OnMoveCallback(InputAction.CallbackContext context)
    {
        // Convert the context to Vector2 and pass it to OnMovePerformed
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMovePerformed(moveInput);
       
    }
    private void InitializePMDIfNeeded()
    {
        if (_postMatchDrop == null)
        {
            _postMatchDrop = PostMatchDrop.Instance;
            _postMatchDrop.Initialize(_matchHandler, _candyPool, _gameSettings, _gridManager, _movementViewer.gameObject);
     
        }
        
    }
    public void OnMovePerformed(Vector2 moveInput)
    {
       
        // Get the swipe direction from the Move action.
        // returns a Vector2 value that represents how much the mouse has moved on the X and Y axes since the last frame.
        // values received represent the movement of the mouse relative to its previous position, not the actual world position of the mouse.
        //Vector2 moveInput = context.ReadValue<Vector2>();

        if (SelectedCandy != null && moveInput.magnitude > _gameSettings.deltaMovementThreshold)
        {
            if (!IsMoving)
            {

                _candySwapperViewer.Initialize(SelectedCandy, _gridManager, _gameSettings, _candyPool);
                _candySwapperViewer.SwapperModel.SwapCandies(moveInput);

                if (isActive1 == false)
                {
                    InitializePMDIfNeeded();
                    //_postMatchDrop.Initialize(_matchHandler, _candyPool, _gameSettings, _gridManager, _movementViewer.gameObject);
                    isActive1 = true;
                }

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

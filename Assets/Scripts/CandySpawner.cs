using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawner : MonoBehaviour
{
    private static CandySpawner instance;
    private GridManager _gridManager;
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    List<GameObject> newCandiesList = new List<GameObject>();
    public static CandySpawner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CandySpawner");
                instance = go.AddComponent<CandySpawner>();
            }
            return instance;
        }

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Initialize(GameSettings gameSettings, GridManager gridManager, CandyPool candyPool)
    {
        _gameSettings = gameSettings;
        _gridManager = gridManager;
        _candyPool = candyPool;
    }
    //This method checks for empties and places the GameObjects in the array but doesn't physically move them.
    public List<GameObject> CheckAndReplaceEmpties()
    {
        //newCandiesList.Clear();
        for (int i = 0; i < _gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
            {
                if (_gridManager.candiesArray[i, j] == null)
                {
                    int randomIndex = Random.Range(0, _gameSettings.candyTypes.Count);
                    GameObject newCandy = _candyPool.GetCandy((CandyType)randomIndex);
                    _gridManager.candiesArray[i, j] = newCandy;
                    Candy newCandyScript = newCandy.GetComponent<Candy>();
                    newCandyScript.PosInArrayI = i;
                    newCandyScript.PosInArrayJ = j;
                    newCandiesList.Add(newCandy);
                }
            }
        }
     
            return newCandiesList;
        

    }
    public IEnumerator SpawnObjects()
    {

            newCandiesList.Clear();

        
        newCandiesList = CheckAndReplaceEmpties();
        if (newCandiesList != null)
        {
            foreach (GameObject candy in newCandiesList)
            {

                Candy candyScript = candy.GetComponent<Candy>();

                GameObject gridCell = _gridManager.gridCellsArray[candyScript.PosInArrayI, candyScript.PosInArrayJ];
                Vector3 endPos = new Vector3(gridCell.transform.position.x, gridCell.transform.position.y, gridCell.transform.position.z -2) ;
                Vector3 startPos = new Vector3(endPos.x, (_gameSettings.tilesNumberI / 2) + 1, endPos.z - 2);

                candyScript.PosX = gridCell.GetComponent<GridCell>().PosX;
                candyScript.PosY = gridCell.GetComponent<GridCell>().PosY;
                StartCoroutine(CandyAnimationsController.Instance.MoveCandy(candy, startPos, endPos, _gameSettings.dropSpeed));
            }
        }
        yield return null;
    }
}

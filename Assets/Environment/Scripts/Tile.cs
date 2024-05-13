using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;
    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    Vector2Int coordinates = new Vector2Int();

    PathFinding pathFinder;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();   
        pathFinder = FindObjectOfType<PathFinding>();
    }

    void Start()
    {
        if(gridManager != null)
        {
            // Converting position to coordinates
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if(!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }



    public bool IsPlaceable
    {
        get
        {
            return isPlaceable;
        }
    }

    void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            // If there is not enough gold to place tower, tile should still be placeable.
            bool isSuccessful = towerPrefab.CreateTower(towerPrefab, gameObject.transform.position);

            if(isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                // If player places tower at enemy path, enemy path needs to be recalculated
                pathFinder.NotifyReceiver();
            }
        }

    }
}

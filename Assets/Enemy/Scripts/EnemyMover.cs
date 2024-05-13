using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


// Following code line is to ensure code: decreaseGold = GetComponent<Enemy>();
[RequireComponent (typeof (Enemy))]

public class EnemyMover : MonoBehaviour
{
    // Movement
    Vector3 startPosition, endPosition;
    float travelPercent;
    [SerializeField] [Range(0, 5f)] float moveSpeed = 1f;

    // Bank
    Enemy decreaseGold;

    // Path
    List<Node> path = new List<Node>();
    GridManager gridManager;
    PathFinding pathFinder;

    void Awake()
    {
        decreaseGold = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinding>();
    }

    void OnEnable()
    {
        ReturnToStart();
        // After calculating the dynamic path, we send true because when the object is activated again,
        // we want to recalculate the path
        RecalculatePath(true);
    }

    void ReturnToStart()
    {
        gameObject.transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinates);
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(gameObject.transform.position);
        }

        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        // The reason for placing it here is that we want to stop the coroutine before starting on the new path
        // because we don't want them to move before the route is calculated.
        // So, we stop it just above and start it here.
        StartCoroutine(FollowPath());
    }

    void FinishPath()
    {
        // Deactive enemies when they arrived end of path
        gameObject.SetActive(false);
        decreaseGold.StealGold();
    }

    IEnumerator FollowPath()
    {
        // The reason for starting from 1 below is that on the new path,
        // they should start moving from the next node rather than the first node.
        // If it's 0, they stop as soon as the path changes and then continue along the path.
        // If it's 1, they always continue from the next (or we can say the second) node of the current path.
        // They should continue moving from the next node anyway since they've already moved to the current node.
        // This will also solve the issue of enemies standing still at the door at the beginning.
        for (int i = 1; i < path.Count; i++)
        {
            startPosition = gameObject.transform.position;
            endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            travelPercent = 0;
            transform.LookAt(endPosition);
            while (travelPercent < 1f)
            {
                // Velocity is calculated using Time.deltatime
                travelPercent += moveSpeed * Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                // wait untill end of frame 
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}

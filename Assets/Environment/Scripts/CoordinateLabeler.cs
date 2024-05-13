using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Following code line provieds that this script works both game mode and editor mode
[ExecuteAlways]

[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    TextMeshPro label;
    // Thanks to below variable, 2d vectors can be represented in ints
    Vector2Int coordinates = new Vector2Int();

    // Coloring coordinates
    GridManager gridManager;
    [SerializeField] Color defaultColor = Color.blue;
    [SerializeField] Color blockedColor;

    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathdColor = new Color(1f, 0.5f, 0f);

    void Awake()
    {
        // Node informations. 
        gridManager = FindObjectOfType<GridManager>();

        label = GetComponent<TextMeshPro>();
        DisplayCoordinates();
        UpdateObjectName();
    }

    void Update()
    {
        // If game mode is off
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
        SetLabelColor();
        ToggleLables();
    }

    void DisplayCoordinates()
    {
        if(gridManager == null)
            return;

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = coordinates.x + ", " + coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColor()
    {
        if (gridManager == null)
            return;
        Node node = gridManager.GetNode(coordinates);

        if (node == null) return;

        // The order of the lines below is important.
        // For example, "isPath" should come before "isExplored" because the path must be discovered.
        if (!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if(node.isPath)
        {
            label.color = pathdColor;
        }
        else if(node.isExplored)
        {
            label .color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }

    }

    void ToggleLables()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }
}

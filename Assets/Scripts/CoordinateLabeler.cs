using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake()
    {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }

        SetLabelColor();
        ToggleLabels();
    }

    void SetLabelColor()
    {
        if (gridManager == null) return;

        Node node = gridManager.GetNode(coordinates);

        if (node == null) return;

        if (!node.isWalkable) label.color = blockedColor;
        else if (node.isPath) label.color = pathColor;
        else if (node.isExplored) label.color = exploredColor;
        else label.color = defaultColor;
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.L)) label.enabled = !label.IsActive();
    }

    void DisplayCoordinates()
    {
        if (gridManager == null) return;
        coordinates.x = (int) (transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = (int) (transform.parent.position.z / gridManager.UnityGridSize);
        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}

using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    [field: SerializeField] public GlobalConfig GlobalConfig { get; private set; }
    [SerializeField] private GameObject _cellVisualPrefab;
    private Cell[,] _grid;
    private Vector2Int _gridSize;
    public Action OnGridCreated;

    public Vector3 AddedLocalPosition
    {
        get
        {
            var gridTranslation = transform.position - (Vector3.right * _gridSize.x / 2f) -
                                  (Vector3.up * _gridSize.y / 2f);
            var radiusTranslation = Vector3.right * 0.5f + Vector3.up * 0.5f;
            return radiusTranslation + gridTranslation;
        }
    }

    private void Awake()
    {
        if (Instance) return;
        Instance = this;

        CreateGrid();
    }

    public void CreateGrid()
    {
        _gridSize = GlobalConfig.GridSize;
        _grid = new Cell[_gridSize.x, _gridSize.y];
        for (var x = 0; x < _gridSize.x; x++)
        {
            for (var y = 0; y < _gridSize.y; y++)
            {
                var cell = new Cell(x, y, this);
                var worldPosition = new Vector3(x, y) + AddedLocalPosition;
                var cellVisual = Instantiate(_cellVisualPrefab, worldPosition, Quaternion.identity, transform)
                    .GetComponent<CellVisual>();
                cellVisual.Initialize(cell);
                _grid[x, y] = cell;
            }
        }

        OnGridCreated?.Invoke();
    }

    public void DestroyGridObjects()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public bool TryGetCell(int x, int y, out Cell cell)
    {
        var validX = x >= 0 && x < _gridSize.x;
        var validY = y >= 0 && y < _gridSize.y;
        if (validX && validY)
        {
            cell = _grid[x, y];
            return true;
        }

        cell = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridSize.x, _gridSize.y, 1));
    }
}
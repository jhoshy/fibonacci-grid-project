using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GlobalConfig _globalConfig;
    private Cell _selectedCell;
    private Camera _camera;
    private GridController _grid;
    private void Start()
    {
        _grid = GridController.Instance;
        _camera = Camera.main;
    }

    private void Update()
    {
        var mouseToGridPos = _camera.ScreenToWorldPoint(Input.mousePosition) - _grid.AddedLocalPosition;
        var x = Mathf.RoundToInt(mouseToGridPos.x);
        var y = Mathf.RoundToInt(mouseToGridPos.y);
        if (_grid.TryGetCell(x, y, out var currentCell))
        {
            if (currentCell != _selectedCell)
            {
                // Unselects first
                _selectedCell?.Hover(false);
                // Selects the new one
                _selectedCell = currentCell;
                currentCell.Hover(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentCell.IncrementCross(1);
            }
            if (_globalConfig.EnableDecrement && Input.GetMouseButtonDown(1))
            {
                currentCell.IncrementCross(-1);
            }
        }
        else if (_selectedCell != null)
        {
            _selectedCell.Hover(false);
            _selectedCell = null;
        }
    }
}

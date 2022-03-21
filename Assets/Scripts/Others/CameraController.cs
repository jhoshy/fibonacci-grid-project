using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GlobalConfig _globalConfig;
    private GridController _grid;

    private void Start()
    {
        _grid = GridController.Instance;
        UpdateCameraZoom();
        _grid.OnGridCreated += UpdateCameraZoom;
    }

    private void UpdateCameraZoom()
    {
        if (_globalConfig.GridSize.y >= _globalConfig.GridSize.x)
        {
            _camera.orthographicSize =  Mathf.Lerp(7.0f, 28f, (_globalConfig.GridSize.y-10f)/40f);
        }
        else
        {
            _camera.orthographicSize =  Mathf.Lerp(14.7f, 28f, (_globalConfig.GridSize.y-10f)/40f);
        }
    }
}
using TMPro;
using UnityEngine;

public class CellVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _colorLerpDuration = 1f;

    private Cell _cell;
    private Material _material;
    private Color _currentColor;
    private Color _targetColor;
    private float _currentTimeStep = float.MaxValue;
    public AnimationCurve _lerpCurve;

    private void Start()
    {
        _material = _renderer.material;
    }

    public void Initialize(Cell cell)
    {
        _cell = cell;
        cell.OnValueIncremented += HandleValueIncremented;
        cell.OnHover += HandleSelected;
        cell.OnCleared += HandleCleared;
    }

    private void HandleCleared()
    {
        _textMesh.text = _cell.Value.ToString();
        _material.color = Color.green;
        SetColor(Color.white, _colorLerpDuration);
    }

    private void HandleValueIncremented(int newValue)
    {
        _textMesh.text = newValue.ToString();
        _material.color = Color.yellow;
        SetColor(Color.white, _colorLerpDuration);
    }

    private void HandleSelected(bool value)
    {
        _material.color = value ? Color.gray : Color.white;
    }

    private void SetColor(Color color, float duration)
    {
        //_material.DOColor(color, duration);
        _targetColor = color;
        _currentColor = _material.color;
        _currentTimeStep = 0f;
    }

    private void Update()
    {
        if (_currentTimeStep <= _colorLerpDuration)
        {
            _currentTimeStep += Time.deltaTime;
            var t = Mathf.Clamp01(_currentTimeStep / _colorLerpDuration);
            _material.color = Color.Lerp(_currentColor, _targetColor, _lerpCurve.Evaluate(t));
        }
    }
}
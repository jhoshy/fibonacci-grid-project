using TMPro;
using UnityEngine;

public class CellVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _colorLerpDuration = 1f;
    [SerializeField] private AnimationCurve _lerpCurve;
    [SerializeField] private GlobalConfig _globalConfig;
    
    private Cell _cell;
    private Material _material;
    private Color _currentColor;
    private Color _targetColor;
    private float _currentTimeStep = float.MaxValue;

    private void Awake()
    {
        _material = _renderer.material;
        _globalConfig.OnShowNumbers += UpdateTextValue;
    }

    private void OnEnable()
    {
        if (_cell != null)
        {
            UpdateTextValue();
        }
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
        UpdateTextValue();
        _material.color = Color.green;
        SetColor(Color.white);
    }

    private void HandleValueIncremented(int newValue)
    {
        UpdateTextValue();
        //_textMesh.text = newValue.ToString();
        _material.color = Color.yellow;
        SetColor(Color.white);
    }

    private void UpdateTextValue()
    {
        if (_globalConfig.ShowTextWithNumbers)
        {
            _textMesh.text = _cell.Value.ToString();
            if (!_textMesh.gameObject.activeSelf) _textMesh.gameObject.SetActive(true);
        }
        else
        {
            if (_textMesh.gameObject.activeSelf) _textMesh.gameObject.SetActive(false);
        }
    }
    
    private void HandleSelected(bool value)
    {
        _material.color = value ? Color.gray : Color.white;
    }

    private void SetColor(Color color)
    {
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
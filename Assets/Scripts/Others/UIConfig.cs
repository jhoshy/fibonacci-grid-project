using UnityEngine;
using UnityEngine.UI;

public class UIConfig : MonoBehaviour
{
    [SerializeField] private GlobalConfig _globalConfig;
    [SerializeField] private Toggle _toggleOnlyClassicFib, _toggleDecrement;
    [SerializeField] private InputField _inputGridX, _inputGridY, _inputMinSequence;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        _toggleOnlyClassicFib.isOn = _globalConfig.FilterByClassicFibonacciOnly;
        _toggleDecrement.isOn = _globalConfig.EnableDecrement;
        _inputGridX.text = _globalConfig.GridSize.x.ToString();
        _inputGridY.text = _globalConfig.GridSize.y.ToString();
        _inputMinSequence.text = _globalConfig.FibonacciSequenceSize.ToString();
    }

    public void ToggleDecrement(bool value)
    {
        _globalConfig.EnableDecrement = value;
    }

    public void ToggleOnlyClassicFibonacci(bool value)
    {
        _globalConfig.FilterByClassicFibonacciOnly = value;
    }
    
    public void SetMinSequenceSize(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        var value = Mathf.Clamp(int.Parse(input), 5, 10);
        _globalConfig.FibonacciSequenceSize = value;
    }

    public void SetGridSizeX(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        var value = Mathf.Clamp(int.Parse(input), 10, 50);
        _globalConfig.GridSize.x = value;
    }

    public void SetGridSizeY(string input)
    {
        if (string.IsNullOrEmpty(input)) return;
        var value = Mathf.Clamp(int.Parse(input), 10, 50);
        _globalConfig.GridSize.y = value;
    }

    public void CreateGridWithConfig()
    {
        GridController.Instance.DestroyGridObjects();
        GridController.Instance.CreateGrid();
        Refresh();
    }
}
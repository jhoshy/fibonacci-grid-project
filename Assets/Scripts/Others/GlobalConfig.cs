using System;
using UnityEngine;

public class GlobalConfig : ScriptableObject
{
    // This is a class for tests/tweaks only, these fields are being changed in the test UI
    public Vector2Int GridSize = new Vector2Int(50, 50);
    [Range(5, 10)] public int FibonacciSequenceSize = 5;
    public bool EnableDecrement = false;
    public bool FilterByClassicFibonacciOnly = true;
    public bool ShowTextWithNumbers = true;
    public Action OnShowNumbers;
}
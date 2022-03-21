using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int Value { get; private set; }

    public Action<int> OnValueIncremented;
    public Action<bool> OnHover;
    public Action OnCleared;

    private int _x;
    private int _y;
    private GridController _grid;
    private GlobalConfig _globalConfig;


    public Cell(int x, int y, GridController grid)
    {
        _x = x;
        _y = y;
        Value = 1;
        _grid = grid;
        _globalConfig = grid.GlobalConfig;
    }

    public void Hover(bool value)
    {
        OnHover?.Invoke(value);
    }

    public void IncrementCross(int incrementAmount)
    {
        Increment(incrementAmount);
        StartIncrementingColumn(incrementAmount);
        StartIncrementingLine(incrementAmount);
    }

    private void StartIncrementingColumn(int amount, int startIndex = 0)
    {
        // Starts checking the whole column recursively
        if (_grid.TryGetCell(_x, startIndex, out var cell))
        {
            if (cell != this)
            {
                cell.Increment(amount);
                cell.CheckFibonacciCloseLine();
            }

            StartIncrementingColumn(amount, ++startIndex);
        }
    }

    private void StartIncrementingLine(int amount, int startIndex = 0)
    {
        // Starts checking the whole line recursively
        if (_grid.TryGetCell(startIndex, _y, out var cell))
        {
            if (cell != this)
            {
                cell.Increment(amount);
                cell.CheckFibonacciCloseColumn();
            }

            StartIncrementingLine(amount, ++startIndex);
        }
    }

    private void Increment(int incrementAmount)
    {
        Value = Mathf.Max(Value + incrementAmount, 1);
        OnValueIncremented?.Invoke(Value);
    }

    private void Clear()
    {
        Value = 1;
        OnCleared?.Invoke();
    }

    private void CheckFibonacciCloseLine()
    {
        CheckFibonacci(1,0);
    }
    private void CheckFibonacciCloseColumn()
    {
        CheckFibonacci(0,1);
    }
    
    private void CheckFibonacci(int xDir, int yDir)
    {
        var sequenceSize = _globalConfig.FibonacciSequenceSize;
        var onlyClassicFibonacci = _globalConfig.FilterByClassicFibonacciOnly;
        var start = sequenceSize - 3;
        var cells = new List<Cell>();

        for (var i = -start; i < sequenceSize; i++)
        {
            if (_grid.TryGetCell(_x + (i - 2) * xDir, _y + (i - 2) * yDir, out var cellMinus2) &&
                _grid.TryGetCell(_x + (i - 1) * xDir, _y + (i - 1) * yDir, out var cellMinus1) &&
                _grid.TryGetCell(_x + i * xDir, _y + i * yDir, out var cell))
            {
                var sumFromPreviousCells = cellMinus2.Value + cellMinus1.Value;

                // Sums like fibonacci
                var isCorrect = cell.Value == sumFromPreviousCells;
                if (onlyClassicFibonacci && !IsClassicFibonacci(cell.Value))
                {
                    isCorrect = false;
                }

                // Sums like fibonacci
                if (isCorrect)
                {
                    if (cells.Count == 0)
                    {
                        // Starting a new sequence found
                        cells.Add(cellMinus2);
                        cells.Add(cellMinus1);
                    }

                    // Amount increases if this is a fibonacci sequence
                    cells.Add(cell);
                }
                else
                {
                    // We search through the whole possible sequence from left to right
                    if (cells.Count >= sequenceSize)
                    {
                        break;
                    }

                    // Clears the count again
                    cells.Clear();
                }
            }
        }

        var success = cells.Count >= sequenceSize;
        if (success)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                var thisCell = cells[i];
                thisCell.Clear();
            }
        }
    }

    private static bool IsClassicFibonacci(int n)
    {
        // n is Fibonacci if one of 5*n*n + 4 or 5*n*n - 4 or both are a perfect square
        return IsPerfectSquare(5 * n * n + 4) ||
               IsPerfectSquare(5 * n * n - 4);
    }

    private static bool IsPerfectSquare(int x)
    {
        var s = (int) Mathf.Sqrt(x);
        return (s * s == x);
    }
}
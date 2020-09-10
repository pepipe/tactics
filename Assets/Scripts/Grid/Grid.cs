using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Grid
{
  public class Grid<T>
  {
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
      public int X;
      public int Y;
    }

    public int GridWidth { get => m_GridWidth; }
    private int m_GridWidth;
    public int GridHeight { get => m_GridHeight; }
    private int m_GridHeight;
    public float GridCellSize { get => m_GridCellSize; }
    private float m_GridCellSize;
    public Transform GridOriginPosition { get => m_GridOriginPosition; }
    private Transform m_GridOriginPosition;

    private T[,] m_GridArray;

    public Grid(int width, int height,
              float cellSize, Transform gridPosition,
              Func<Grid<T>, int, int, bool, T> createGridObject)
    {
      m_GridWidth = width;
      m_GridHeight = height;
      m_GridCellSize = cellSize;
      m_GridOriginPosition = gridPosition;
      m_GridArray = new T[width, height];

      //init grid array
      for (int x = 0; x < m_GridArray.GetLength(0); ++x)
        for (int y = 0; y < m_GridArray.GetLength(1); ++y)
          m_GridArray[x, y] = createGridObject(this, x, y, false);
    }

    public bool IsValidGridPosition(Vector3 worldPosition)
    {
      var (x,y) = GetXY(worldPosition);
      return x >= 0 && y >= 0 && x < m_GridWidth && y < m_GridHeight;
    }

    public bool IsValidGridPosition(int x, int y)
    {
      return x >= 0 && y >= 0 && x < m_GridWidth && y < m_GridHeight;
    }

    public T GetGridObject(Vector3 worldPosition)
    {
      var pos = GetXY(worldPosition);
      return GetGridObject(pos.x, pos.y);
    }

    public List<T> ToList()
    {
      var gridObjects = new List<T>();
      for (int x = 0; x < m_GridArray.GetLength(0); ++x)
        for (int y = 0; y < m_GridArray.GetLength(1); ++y)
          gridObjects.Add(m_GridArray[x, y]);

      return gridObjects;
    }

    /// <summary>
    /// Return grid object when given a grid position
    /// </summary>
    /// <param name="x">x grid pos</param>
    /// <param name="y">y grid pos</param>
    /// <returns>Returns grid object in [x,y], if x,y is invalid it will return default grid object value</returns>
    public T GetGridObject(int x, int y)
    {
      return IsValidGridPosition(x, y) ? m_GridArray[x, y] : default(T);
    }

    public void SetGridObject(Vector3 worldPosition, T value)
    {
      var (X, Y) = GetXY(worldPosition);
      SetGridObject(X, Y, value);
    }

    /// <summary>
    /// Change x,y grid value and invoke OnGridObjectChange event.
    /// </summary>
    /// <param name="x">x Grid pos</param>
    /// <param name="y">y Grid pos</param>
    /// <param name="value">value to set</param>
    public void SetGridObject(int x, int y, T value)
    {
      if (x >= 0 && y >= 0 &&
          x < m_GridWidth && y < m_GridHeight)
      {
        m_GridArray[x, y] = value;
        TriggerGridObjectChanged(x, y);
      }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
      return new Vector3(x, y) * m_GridCellSize + m_GridOriginPosition.position;
    }

    public (int x, int y) GetXY(Vector3 worldPosition)
    {
      return (x: Mathf.FloorToInt((worldPosition - m_GridOriginPosition.position).x / m_GridCellSize), 
              y: Mathf.FloorToInt((worldPosition - m_GridOriginPosition.position).y / m_GridCellSize));
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
      OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { X = x, Y = y });
    }    
  }
}
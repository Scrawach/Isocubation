using System;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class Map
    {
        private const char Empty = '\0';
        private readonly char[,] _grid;
        private Vector2Int _startPosition;
        
        public Map(Vector2Int size)
        {
            _grid = new char[size.x, size.y];
            Borders = new RectInt(Vector2Int.zero, size);
            _startPosition = Vector2Int.zero;
        }

        public RectInt Borders { get; }

        public Vector2Int StartPosition => _startPosition;
        public int Width => Borders.xMax;
        public int Height => Borders.yMax;

        public void ApplyStartPosition(Vector2Int index)
        {
          if (InBorder(index))
          {
            _startPosition = index;
          }
          else
          {
            throw new ArgumentException("Invalid index for map symbol (out of board)");
          }
        }
        
        public void ApplySymbol(Vector2Int index, char symbol)
        {
            if (InBorder(index))
            {
                _grid[index.x, index.y] = symbol;
            }
            else
            {
                throw new ArgumentException("Invalid index for map symbol (out of board)");
            }
        }

        public void ApplySymbolAround(Vector2Int index, char symbol)
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) 
                        continue;
                
                    var point = new Vector2Int(i, j) + index;
                    
                    if (InBorder(point) == false || Exist(point))
                        continue;
                    
                    ApplySymbol(point, symbol);
                }
            }
        }

        public bool Exist(Vector2Int index)
        {
          return GetSymbolAt(index) != Empty;
        }

        public char GetSymbolAt(Vector2Int index)
        {
            if (InBorder(index))
            {
                return _grid[index.x, index.y];
            }

            return Empty;
        }

        private bool InBorder(Vector2Int index)
        {
            return index.x < Width && index.y < Height && index.x >= 0 && index.y >= 0;
        }
    }
}
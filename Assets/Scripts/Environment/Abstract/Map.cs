using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public abstract class Map<T>
    {
        private T[,] _grid;
        protected abstract T Empty { get; }

        public Map(Vector2Int size)
        {
            _grid = new T[size.x, size.y];
            Borders = new RectInt(Vector2Int.zero, size);
        }
        
        public event Action<Vector2Int> Updated;
        
        public RectInt Borders { get; private set; }
        public int Width => Borders.xMax;
        public int Height => Borders.yMax;
        public int FillPercent { get; protected set; }

        public void Extend(Map<T> map)
        {
            var resultX = Width + map.Width;
            var resultY = Height > map.Height ? Height : map.Height;
            var extendedMap = new T[resultX, resultY];

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    extendedMap[x, y] = _grid[x, y];
                }
            }

            for (var x = Width; x < resultX; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    extendedMap[x, y] = map._grid[x - Width, y];
                }
            }
            
            Borders = new RectInt(Vector2Int.zero, new Vector2Int(resultX, resultY));
            _grid = extendedMap;
        }

        public void Apply(Vector2Int index, T obj)
        {
            if (InBorder(index))
            {
                FillPercent++;
                _grid[index.x, index.y] = obj;
                Updated?.Invoke(index);
            }
        }

        public void ApplyAround(Vector2Int index, T obj)
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
                    
                    Apply(point, obj);
                }
            }
        }

        public bool Exist(Vector2Int index)
        {
            return InBorder(index) && EqualWithEmpty(_grid[index.x, index.y]);
        }

        public T Get(Vector2Int index)
        {
            return InBorder(index) ? _grid[index.x, index.y] : Empty;
        }

        public List<T> GetAround(Vector2Int index)
        {
            var result = new List<T>(9);
            
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    
                    var position = new Vector2Int(index.x + i, index.y + j);
                    if (Exist(position))
                    {
                        result.Add(Get(position));
                    }
                }
            }

            return result;
        }

        public bool TryGet(Vector2Int index, out T target)
        {
            if (Exist(index) == false)
            {
                target = Empty;
                return false;
            }

            target = Get(index);
            return true;
        }

        protected abstract bool EqualWithEmpty(T source);

        protected bool InBorder(Vector2Int index)
        {
            if (index.x < Width && index.y < Height && index.x >= 0 && index.y >= 0)
            {
                return true;
            }

            return false;
        }
    }
}
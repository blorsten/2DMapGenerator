using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapGeneration.Utils
{
    /// <summary>
    /// A Unity serializable 2D array that holds chunkholders.
    /// </summary>
    [Serializable]
    public class Float2DArray : IEnumerable<float>
    {
        [SerializeField, HideInInspector] private int _rowsLength;
        [SerializeField, HideInInspector] private int _columnsLength;

        [Serializable]
        public class Columns
        {
            [SerializeField, HideInInspector] private float[] _columnsArray;

            public Columns(int columnsLength)
            {
                ColumnsArray = new float[columnsLength];
            }

            public float[] ColumnsArray
            {
                get { return _columnsArray; }
                set { _columnsArray = value; }
            }
        }

        [SerializeField, HideInInspector] private Columns[] _rows;
        
        public Float2DArray(int rowsLength, int columnsLength)
        {
            _rows = new Columns[rowsLength];
            for (var i = 0; i < _rows.Length; i++)
            {
                _rows[i] = new Columns(columnsLength); 
            }

            _rowsLength = rowsLength;
            _columnsLength = columnsLength;
        }

        public float this[int rowIndex, int colIndex]
        {
            get { return _rows[rowIndex].ColumnsArray[colIndex]; }
            set { _rows[rowIndex].ColumnsArray[colIndex] = value; }
        }

        /// <summary>
        /// Gets the number of either rows or columns.
        /// </summary>
        /// <param name="p0">If 0 then rows length, If 1 then columns length.</param>
        /// <returns></returns>
        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _rowsLength;

            if (p0 == 1)
                return _columnsLength;
                
            return 0;
        }

        public IEnumerator<float> GetEnumerator()
        {
            return _rows.SelectMany(columns => columns.ColumnsArray.Select(arg1 => arg1)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
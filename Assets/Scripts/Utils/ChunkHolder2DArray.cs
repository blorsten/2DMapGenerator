using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapGeneration.Utils
{
    [Serializable]
    public class ChunkHolder2DArray : IEnumerable<ChunkHolder>
    {
        [SerializeField, HideInInspector] private int _rowsLength;
        [SerializeField, HideInInspector] private int _columnsLength;

        [Serializable]
        public class Columns
        {
            [SerializeField, HideInInspector] private ChunkHolder[] _columnsArray;

            public Columns(int columnsLength)
            {
                ColumnsArray = new ChunkHolder[columnsLength];
            }

            public ChunkHolder[] ColumnsArray
            {
                get { return _columnsArray; }
                set { _columnsArray = value; }
            }
        }

        [SerializeField, HideInInspector] private Columns[] _rows;
        
        public ChunkHolder2DArray(int rowsLength, int columnsLength)
        {
            _rows = new Columns[rowsLength];
            for (var i = 0; i < _rows.Length; i++)
            {
                _rows[i] = new Columns(columnsLength); 
            }

            _rowsLength = rowsLength;
            _columnsLength = columnsLength;
        }

        public ChunkHolder this[int rowIndex, int colIndex]
        {
            get { return _rows[rowIndex].ColumnsArray[colIndex]; }
            set { _rows[rowIndex].ColumnsArray[colIndex] = value; }
        }

        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _rowsLength;

            if (p0 == 1)
                return _columnsLength;
                
            return 0;
        }

        public IEnumerator<ChunkHolder> GetEnumerator()
        {
            return _rows.SelectMany(columns => columns.ColumnsArray.Select(arg1 => arg1)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
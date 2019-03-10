using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapGeneration.ChunkSystem;
using UnityEngine;

namespace MapGeneration.Utils
{
    /// <summary>
    /// A Unity serializable 2D array that holds chunkholders.
    /// </summary>
    [Serializable]
    public class ChunkHolder2DArray : IEnumerable<ChunkHolder>
    {
        [SerializeField, HideInInspector] private int _rowsLength;
        [SerializeField, HideInInspector] private int _columnsLength;

        /// <summary>
        /// Class used by <see cref="ChunkHolder2DArray"/> for containing columns of <see cref="ChunkHolder"/>(s).
        /// </summary>
        [Serializable]
        public class Columns
        {
            [SerializeField, HideInInspector] private ChunkHolder[] _columnsArray;

            /// <summary>
            /// Constructs a column of chunk holders.
            /// </summary>
            /// <param name="columnsLength">Desired length of the column.</param>
            public Columns(int columnsLength)
            {
                ColumnsArray = new ChunkHolder[columnsLength];
            }

            /// <summary>
            /// Property for containing the column.
            /// </summary>
            public ChunkHolder[] ColumnsArray
            {
                get { return _columnsArray; }
                set { _columnsArray = value; }
            }
        }

        [SerializeField, HideInInspector] private Columns[] _rows;

        /// <summary>
        /// Constructs a 2D Array for <see cref="ChunkHolder"/>(s).
        /// </summary>
        /// <param name="rowsLength">Desired amount of rows in the array.</param>
        /// <param name="columnsLength">Desired length of the columns.</param>
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

        /// <summary>
        /// Array accessor for the 2D Array, reflects the original way to access a array on.
        /// </summary>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="colIndex">Column index.</param>
        /// <returns>Returns the found chunk holder on this array position.</returns>
        public ChunkHolder this [int rowIndex, int colIndex]
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

        /// <summary>
        /// Selects all columns and calculates a enumerator for them all.
        /// </summary>
        /// <returns>Returns an enumerator for the 2D array.</returns>
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
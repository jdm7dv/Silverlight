﻿// Copyright © Microsoft Corporation. 
// This source is subject to the Microsoft Source License for Silverlight Controls (March 2008 Release).
// Please see http://go.microsoft.com/fwlink/?LinkID=111693 for details.
// All other rights reserved. 

using System.Collections;
using System.Collections.Generic; 
using System.Diagnostics; 

namespace System.Windows.Controlsb1
{
    internal class IndexToValueTable<T> : IEnumerable<Range<T>>
    { 
        private List<Range<T>> _list;

        public IndexToValueTable() 
        { 
            _list = new List<Range<T>>();
        } 

        #region Public Properties
 
        /// <summary>
        /// Total number of indicies represented in the table
        /// </summary> 
        public int IndexCount 
        {
            get 
            {
                int indexCount = 0;
                foreach (Range<T> range in _list) 
                {
                    indexCount += range.Count;
                } 
                return indexCount; 
            }
        } 

        /// <summary>
        /// Returns true if the table is empty 
        /// </summary>
        public bool IsEmpty
        { 
            get 
            {
                return _list.Count == 0; 
            }
        }
 
        /// <summary>
        /// Returns the number of index ranges in the table
        /// </summary> 
        public int RangeCount 
        {
            get 
            {
                return _list.Count;
            } 
        }

        /// <summary> 
        /// Returns the index of the first value in the table 
        /// </summary>
        public int SingleIndex 
        {
            get
            { 
                Debug.Assert(this.RangeCount == 1);
                Debug.Assert(this.IndexCount == 1);
                Debug.Assert(_list[0].LowerBound == _list[0].UpperBound); 
                return _list[0].LowerBound; 
            }
        } 

        #endregion
 
        #region Public methods

        /// <summary> 
        /// Add a value with an associated index to the table 
        /// </summary>
        /// <param name="index">Index where the value is to be added or updated</param> 
        /// <param name="value">Value to add</param>
        public void AddValue(int index, T value)
        { 
            AddValues(index, 1, value);
        }
 
        /// <summary> 
        /// Add multiples values with an associated start index to the table
        /// </summary> 
        /// <param name="startIndex">index where first value is added</param>
        /// <param name="count">Total number of values to add (must be greater than 0)</param>
        /// <param name="value">Value to add</param> 
        public void AddValues(int startIndex, int count, T value)
        {
            Debug.Assert(count > 0); 
            AddValuesPrivate(startIndex, count, value, null); 
        }
 
        /// <summary>
        /// Clears the index table
        /// </summary> 
        public void Clear()
        {
            _list.Clear(); 
        } 

        /// <summary> 
        /// Returns true if the given index is contained in the table
        /// </summary>
        /// <param name="index">index to search for</param> 
        /// <returns>True if the index is contained in the table</returns>
        public bool Contains(int index)
        { 
            return IsCorrectRangeIndex(this.FindRangeIndex(index), index); 
        }
 
        /// <summary>
        /// Returns true if the entire given index range is contained in the table
        /// </summary> 
        /// <param name="startIndex">beginning of the range</param>
        /// <param name="endIndex">end of the range</param>
        /// <returns>True if the entire index range is present in the table</returns> 
        public bool ContainsAll(int startIndex, int endIndex) 
        {
            int start = -1; 
            int end = -1;

            foreach (Range<T> range in _list) 
            {
                if (start == -1 && range.UpperBound >= startIndex)
                { 
                    if (startIndex < range.LowerBound) 
                    {
                        return false; 
                    }
                    start = startIndex;
                    end = range.UpperBound; 
                    if (end >= endIndex)
                    {
                        return true; 
                    } 
                }
                else if (start != -1) 
                {
                    if (range.LowerBound > end + 1)
                    { 
                        return false;
                    }
                    end = range.UpperBound; 
                    if (end >= endIndex) 
                    {
                        return true; 
                    }
                }
            } 
            return false;
        }
 
        // 

 


 


 
 

 


 


 
 

 


 


 
        /// <summary> 
        /// Returns true if the given index is contained in the table with the the given value
        /// </summary> 
        /// <param name="index">index to search for</param>
        /// <param name="value">value expected</param>
        /// <returns>true if the given index is contained in the table with the the given value</returns> 
        public bool ContainsIndexAndValue(int index, T value)
        {
            int lowerRangeIndex = this.FindRangeIndex(index); 
            return ((IsCorrectRangeIndex(lowerRangeIndex, index)) && (_list[lowerRangeIndex].Equals(value))); 
        }
 
        /// <summary>
        /// Returns an enumerator that goes through the indexes present in the table
        /// </summary> 
        /// <returns>an enumerator that enumerates the indexes present in the table</returns>
        public IEnumerator GetIndexesEnumerator()
        { 
            // 

            return new IndexesEnumerator<T>(_list); 
        }

        /// <summary> 
        /// Returns the value at a given index or the default value if the index is not in the table
        /// </summary>
        /// <param name="index">index to search for</param> 
        /// <returns>the value at the given index or the default value if index is not in the table</returns> 
        public T GetValueAt(int index)
        { 
            bool found;
            return GetValueAt(index, out found);
        } 

        /// <summary>
        /// Returns the value at a given index or the default value if the index is not in the table 
        /// </summary> 
        /// <param name="index">index to search for</param>
        /// <param name="found">set to true by the method if the index was found; otherwise, false</param> 
        /// <returns>the value at the given index or the default value if index is not in the table</returns>
        public T GetValueAt(int index, out bool found)
        { 
            int rangeIndex = this.FindRangeIndex(index);
            if (this.IsCorrectRangeIndex(rangeIndex, index))
            { 
                found = true; 
                return _list[rangeIndex].Value;
            } 
            else
            {
                found = false; 
                return default(T);
            }
        } 
 
        /// <summary>
        /// Return the index of the Nth element in the table. 
        /// </summary>
        /// <param name="entryIndex">entryIndex parameter represents N</param>
        public int IndexOf(int entryIndex) 
        {
            Debug.Assert(entryIndex >= 0 && entryIndex < this.IndexCount);
            int cumulatedEntries = 0; 
            foreach (Range<T> range in _list) 
            {
                if (cumulatedEntries + range.Count > entryIndex) 
                {
                    return range.LowerBound + entryIndex - cumulatedEntries;
                } 
                else
                {
                    cumulatedEntries += range.Count; 
                } 
            }
            return -1; 
        }

        /// <summary> 
        /// Inserts an index at the given location.  This does not alter values in the table
        /// </summary>
        /// <param name="index">index location to insert an index</param> 
        public void InsertIndex(int index) 
        {
            InsertIndexes(index, 1); 
        }

        /// <summary> 
        /// Inserts an index into the table with the given value
        /// </summary>
        /// <param name="index">index to insert</param> 
        /// <param name="value">value for the index</param> 
        public void InsertIndexAndValue(int index, T value)
        { 
            InsertIndexesAndValues(index, 1, value);
        }
 
        /// <summary>
        /// Inserts multiple indexes into the table.  This does not alter Values in the table
        /// </summary> 
        /// <param name="startIndex">first index to insert</param> 
        /// <param name="count">total number of indexes to insert</param>
        public void InsertIndexes(int startIndex, int count) 
        {
            Debug.Assert(count > 0);
            InsertIndexesPrivate(startIndex, count, this.FindRangeIndex(startIndex)); 
        }

        /// <summary> 
        /// Inserts multiple indexes into the table with the given value 
        /// </summary>
        /// <param name="startIndex">Index to insert first value</param> 
        /// <param name="count">Total number of values to insert (must be greater than 0)</param>
        /// <param name="value">Value to insert</param>
        public void InsertIndexesAndValues(int startIndex, int count, T value) 
        {
            Debug.Assert(count > 0);
            int lowerRangeIndex = this.FindRangeIndex(startIndex); 
            InsertIndexesPrivate(startIndex, count, lowerRangeIndex); 
            AddValuesPrivate(startIndex, count, value, lowerRangeIndex);
        } 

        /// <summary>
        /// Removes an index from the table.  This does not alter Values in the table 
        /// </summary>
        /// <param name="index">index to remove</param>
        public void RemoveIndex(int index) 
        { 
            RemoveIndexes(index, 1);
        } 

        /// <summary>
        /// Removes a value and its index from the table 
        /// </summary>
        /// <param name="index">index to remove</param>
        public void RemoveIndexAndValue(int index) 
        { 
            RemoveIndexesAndValues(index, 1);
        } 

        /// <summary>
        /// Removes multiple indexes from the table.  This does not alter Values in the table 
        /// </summary>
        /// <param name="startIndex">first index to remove</param>
        /// <param name="count">total number of indexes to remove</param> 
        public void RemoveIndexes(int startIndex, int count) 
        {
            int lowerRangeIndex = this.FindRangeIndex(startIndex); 
            if (lowerRangeIndex < 0)
            {
                lowerRangeIndex = 0; 
            }
            int i = lowerRangeIndex;
            while (i < _list.Count) 
            { 
                Range<T> range = _list[i];
                if (range.UpperBound >= startIndex) 
                {
                    if (range.LowerBound >= startIndex + count)
                    { 
                        // Both bounds will remain after the removal
                        range.LowerBound -= count;
                        range.UpperBound -= count; 
                    } 
                    else
                    { 
                        int currentIndex = i;
                        if (range.LowerBound <= startIndex)
                        { 
                            // Range gets split up
                            if (range.UpperBound >= startIndex + count)
                            { 
                                i++; 
                                _list.Insert(i, new Range<T>(startIndex, range.UpperBound - count, range.Value));
                            } 
                            range.UpperBound = startIndex - 1;
                        }
                        else 
                        {
                            range.LowerBound = startIndex;
                            range.UpperBound -= count; 
                        } 
                        if (RemoveRangeIfInvalid(range, currentIndex))
                        { 
                            i--;
                        }
                    } 
                }
                i++;
            } 
            if (!this.Merge(lowerRangeIndex)) 
            {
                this.Merge(lowerRangeIndex + 1); 
            }
        }
 
        /// <summary>
        /// Removes multiple values and their indexes from the table
        /// </summary> 
        /// <param name="startIndex">first index to remove</param> 
        /// <param name="count">total number of indexes to remove</param>
        public void RemoveIndexesAndValues(int startIndex, int count) 
        {
            RemoveValues(startIndex, count);
            RemoveIndexes(startIndex, count); 
        }

        /// <summary> 
        /// Removes a value from the table at the given index.  This does not alter other indexes in the table. 
        /// </summary>
        /// <param name="index">index where value should be removed</param> 
        public void RemoveValue(int index)
        {
            RemoveValues(index, 1); 
        }

        /// <summary> 
        /// Removes multiple values from the table.  This does not alter other indexes in the table. 
        /// </summary>
        /// <param name="startIndex">first index where values should be removed </param> 
        /// <param name="count">total number of values to remove</param>
        public void RemoveValues(int startIndex, int count)
        { 
            Debug.Assert(count > 0);

            int lowerRangeIndex = this.FindRangeIndex(startIndex); 
            if (lowerRangeIndex < 0) 
            {
                lowerRangeIndex = 0; 
            }
            while ((lowerRangeIndex < _list.Count) && (_list[lowerRangeIndex].UpperBound < startIndex))
            { 
                lowerRangeIndex++;
            }
            if (lowerRangeIndex >= _list.Count) 
            { 
                return;
            } 
            if (_list[lowerRangeIndex].LowerBound < startIndex)
            {
                // Need to split this up 
                _list.Insert(lowerRangeIndex, new Range<T>(_list[lowerRangeIndex].LowerBound, startIndex - 1, _list[lowerRangeIndex].Value));
                lowerRangeIndex ++;
            } 
            _list[lowerRangeIndex].LowerBound = startIndex + count; 
            if (!RemoveRangeIfInvalid(_list[lowerRangeIndex], lowerRangeIndex))
            { 
                lowerRangeIndex++;
            }
            while ((lowerRangeIndex < _list.Count) && (_list[lowerRangeIndex].UpperBound < startIndex + count)) 
            {
                _list.RemoveAt(lowerRangeIndex);
            } 
            if ((lowerRangeIndex < _list.Count) && (_list[lowerRangeIndex].UpperBound >= startIndex + count) && 
                (_list[lowerRangeIndex].LowerBound < startIndex + count))
            { 
                // Chop off the start of the remaining Range if it contains values that we're removing
                _list[lowerRangeIndex].LowerBound = startIndex + count;
                RemoveRangeIfInvalid(_list[lowerRangeIndex], lowerRangeIndex); 
            }
        }
 
        #endregion 

        #region Internal Methods 
        #endregion

        #region Private methods 

        private void AddValuesPrivate(int startIndex, int count, T value, int? startRangeIndex)
        { 
            Debug.Assert(count > 0); 

            int endIndex = startIndex + count - 1; 
            Range<T> newRange = new Range<T>(startIndex, endIndex, value);
            if (_list.Count == 0)
            { 
                _list.Add(newRange);
            }
            else 
            { 
                int lowerRangeIndex = (startRangeIndex.HasValue) ? startRangeIndex.Value : this.FindRangeIndex(startIndex);
                Range<T> lowerRange = (lowerRangeIndex < 0) ? null : _list[lowerRangeIndex]; 
                if (lowerRange == null)
                {
                    if (lowerRangeIndex < 0) 
                    {
                        lowerRangeIndex = 0;
                    } 
                    _list.Insert(lowerRangeIndex, newRange); 
                }
                else 
                {
                    if (!lowerRange.Value.Equals(value) && (lowerRange.UpperBound >= startIndex))
                    { 
                        // Split up the range
                        if (lowerRange.UpperBound > endIndex)
                        { 
                            _list.Insert(lowerRangeIndex + 1, new Range<T>(endIndex + 1, lowerRange.UpperBound, lowerRange.Value)); 
                        }
                        lowerRange.UpperBound = startIndex - 1; 
                        if (!RemoveRangeIfInvalid(lowerRange, lowerRangeIndex))
                        {
                            lowerRangeIndex++; 
                        }
                        _list.Insert(lowerRangeIndex, newRange);
                    } 
                    else 
                    {
                        _list.Insert(lowerRangeIndex + 1, newRange); 
                        if (!Merge(lowerRangeIndex))
                        {
                            lowerRangeIndex++; 
                        }
                    }
                } 
 
                // At this point the newRange has been inserted in the correct place, now we need to remove
                // any subsequent ranges that no longer make sense and possibly update the one at newRange.UpperBound 
                int upperRangeIndex = lowerRangeIndex + 1;
                while ((upperRangeIndex < _list.Count) && (_list[upperRangeIndex].UpperBound < endIndex))
                { 
                    _list.RemoveAt(upperRangeIndex);
                }
                if (upperRangeIndex < _list.Count) 
                { 
                    Range<T> upperRange = _list[upperRangeIndex];
                    if (upperRange.LowerBound <= endIndex) 
                    {
                        // Update the range
                        upperRange.LowerBound = endIndex + 1; 
                        RemoveRangeIfInvalid(upperRange, upperRangeIndex);
                    }
                    Merge(lowerRangeIndex); 
                } 
            }
        } 

        // Returns the index of the range that contains the input or the range before if the input is not found
        private int FindRangeIndex(int index) 
        {
            if (_list.Count == 0)
            { 
                return -1; 
            }
 
            // Do a binary search for the index
            int front = 0;
            int end = _list.Count - 1; 
            Range<T> range = null;
            while (end > front)
            { 
                int median = (front + end) / 2; 
                range = _list[median];
                if (range.UpperBound < index) 
                {
                    front = median + 1;
                } 
                else if (range.LowerBound > index)
                {
                    end = median - 1; 
                } 
                else
                { 
                    // we found it
                    return median;
                } 
            }

            if (front == end) 
            { 
                range = _list[front];
                if (range.ContainsIndex(index) || (range.UpperBound < index)) 
                {
                    // we found it or the index isn't there and we're one range before
                    return front; 
                }
                else
                { 
                    // not found and we're one range after 
                    return front - 1;
                } 
            }
            else
            { 
                // end is one index before front in this case so it's the range before
                return end;
            } 
        } 

        private bool Merge(int lowerRangeIndex) 
        {
            int upperRangeIndex = lowerRangeIndex + 1;
            if ((lowerRangeIndex >= 0) && (upperRangeIndex < _list.Count)) 
            {
                Range<T> lowerRange = _list[lowerRangeIndex];
                Range<T> upperRange = _list[upperRangeIndex]; 
                if ((lowerRange.UpperBound + 1 >= upperRange.LowerBound) && (lowerRange.Value.Equals(upperRange.Value))) 
                {
                    lowerRange.UpperBound = Math.Max(lowerRange.UpperBound, upperRange.UpperBound); 
                    _list.RemoveAt(upperRangeIndex);
                    return true;
                } 
            }
            return false;
        } 
 
        private void InsertIndexesPrivate(int startIndex, int count, int lowerRangeIndex)
        { 
            Debug.Assert(count > 0);

            // Same as AddRange after we fix the indicies affected by the insertion 
            int startRangeIndex = (lowerRangeIndex >= 0) ? lowerRangeIndex : 0;
            for (int i = startRangeIndex; i < _list.Count; i++)
            { 
                Range<T> range = _list[i]; 
                if (range.LowerBound >= startIndex)
                { 
                    range.LowerBound += count;
                }
                else 
                {
                    if (range.UpperBound >= startIndex)
                    { 
                        // Split up this range 
                        i++;
                        _list.Insert(i, new Range<T>(startIndex, range.UpperBound + count, range.Value)); 
                        range.UpperBound = startIndex - 1;
                        continue;
                    } 
                }

                if (range.UpperBound >= startIndex) 
                { 
                    range.UpperBound += count;
                } 
            }
        }
 
        private bool IsCorrectRangeIndex(int rangeIndex, int index)
        {
            return (-1 != rangeIndex) && (_list[rangeIndex].ContainsIndex(index)); 
        } 

        private bool RemoveRangeIfInvalid(Range<T> range, int rangeIndex) 
        {
            if (range.UpperBound < range.LowerBound)
            { 
                _list.RemoveAt(rangeIndex);
                return true;
            } 
            return false; 
        }
 
        #endregion

        #region IEnumerable<Range<T>> Members 

        public IEnumerator<Range<T>> GetEnumerator()
        { 
            return _list.GetEnumerator(); 
        }
 
        #endregion

        #region IEnumerable Members 

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { 
            return _list.GetEnumerator(); 
        }
 
        #endregion
    }
 
    //
    internal class IndexesEnumerator<T> : IEnumerator
    { 
        private List<Range<T>> _rangeList; 
        private int _currentRangeIndex, _currentOffsetInRange;
 
        public IndexesEnumerator(List<Range<T>> rangeList)
        {
            Debug.Assert(rangeList != null); 
            this._rangeList = rangeList;
            Reset();
        } 
 
        public object Current
        { 
            get
            {
                if (this._currentRangeIndex >= 0 && this._currentRangeIndex < this._rangeList.Count && 
                    this._currentOffsetInRange >= 0 && this._currentOffsetInRange < this._rangeList[this._currentRangeIndex].Count)
                {
                    return this._rangeList[this._currentRangeIndex].LowerBound + this._currentOffsetInRange; 
                } 
                return null;
            } 
        }

        public bool MoveNext() 
        {
            if (this._currentRangeIndex == -1 ||
                (this._currentRangeIndex < this._rangeList.Count && 
                 this._currentOffsetInRange >= this._rangeList[this._currentRangeIndex].Count - 1)) 
            {
                this._currentRangeIndex++; 
                this._currentOffsetInRange = -1;
            }
            if (this._currentRangeIndex < this._rangeList.Count) 
            {
                this._currentOffsetInRange++;
                return true; 
            } 
            return false;
        } 

        public void Reset()
        { 
            this._currentRangeIndex = -1;
            this._currentOffsetInRange = -1;
        } 
    } 
}

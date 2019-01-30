using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Artibition.ORM.SQLBuilder
{
    public partial class SQL
    {
        internal class Table
        {
            private string name;
            private string alias;

            public Table(string tableName)
            {
                name = tableName;

            }

        }

        internal class TableCollection : ICollection<Table>
        {
            public List<Table> _table = new List<Table>();
            public int Count => _table.Count;

            public bool IsReadOnly => false;

            public void Add(Table item)
            {
                _table.Add(item);
            }

            public void Clear()
            {
                _table.Clear();
            }

            public bool Contains(Table item)
            {
                return _table.Contains(item);
            }

            public void CopyTo(Table[] array, int arrayIndex)
            {
                _table.CopyTo(array, arrayIndex);
            }

            public IEnumerator<Table> GetEnumerator()
            {
                foreach (var t in _table)
                    yield return t;
            }

            public bool Remove(Table item)
            {
                return _table.Remove(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (var t in _table)
                    yield return t;
            }
        }
    }
}

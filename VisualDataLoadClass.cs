using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winFormsDataGrid
{
    public class VisualDataLoadClass
    {
        private readonly DataGridView _dataGridView;
        public List<string> TermValues;
        public List<string> TermNames;
        public List<string> ExpertNames;
        public List<List<decimal>> Values;
        public DataTable DataTable;

        public VisualDataLoadClass(DataGridView dataGridView,
            List<string> termValues,
            List<string> termNames,
            List<string> expertNames)
        {
            DataTable = new DataTable {TableName = "Таблица1"};
            _dataGridView = dataGridView;
            _dataGridView.Disposed += OnDispose;
            _dataGridView.Parent.Resize += OnFormResize;
            TermValues = termValues;
            TermNames = termNames;
            ExpertNames = expertNames;
            InitDataGridView();
        }

        public VisualDataLoadClass(DataGridView dataGridView, List<string> termValues, List<string> termNames, List<string> expertNames, List<List<decimal>> values)
        {
            DataTable = new DataTable {TableName = "Таблица1"};
            _dataGridView = dataGridView;
            _dataGridView.Disposed += OnDispose;
            _dataGridView.Parent.Resize += OnFormResize;
            TermValues = termValues;
            TermNames = termNames;
            ExpertNames = expertNames;
            Values = values;
            InitDataGridView();
        }

        public VisualDataLoadClass(DataGridView dataGridView, string path)
        {
            DataTable = new DataTable { TableName = "Таблица1" };
            _dataGridView = dataGridView;
            _dataGridView.Disposed += OnDispose;
            _dataGridView.Parent.Resize += OnFormResize;
            LoadTerm(path);
            InitDataGridView();
        }

        public VisualDataLoadClass(DataGridView dataGridView, string path, List<List<decimal>> values)
        {
            DataTable = new DataTable { TableName = "Таблица1" };
            _dataGridView = dataGridView;
            _dataGridView.Disposed += OnDispose;
            _dataGridView.Parent.Resize += OnFormResize;
            LoadTerm(path);
            Values = values;
            InitDataGridView();
        }

        private void OnDispose(Object sender, EventArgs e)
        {
            DataTable.WriteXml($"{DataTable.TableName}.xml", XmlWriteMode.WriteSchema);
        }

        private void OnFormResize(Object sender, EventArgs e)
        {
            //var tempWidth = _dataGridView.Columns[0].Width * _dataGridView.ColumnCount+_dataGridView.Location.X+10;
            //_dataGridView.Width = tempWidth > _dataGridView.Parent.Width - _dataGridView.Location.X - 20
            //    ? _dataGridView.Parent.Width - _dataGridView.Location.X - 20
            //    : tempWidth;
            //var tempHeight = _dataGridView.Rows[0].Height * _dataGridView.RowCount-30;
            //_dataGridView.Height = tempHeight > _dataGridView.Parent.Height - 20
            //    ? _dataGridView.Parent.Height - 20
            //    : tempHeight;
        }

        private void LoadTerm(string path)
        {
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {
                var expertCount = Convert.ToInt32(streamReader.ReadLine());
                var termNamesLine = streamReader.ReadLine().Split().ToList();
                var termValuesLine = streamReader.ReadLine().Split().ToList();
                ExpertNames = Enumerable.Range(1,expertCount).Select(c => $"Экс{c}").ToList();
                TermNames = termNamesLine;
                TermValues = termValuesLine;
            }
        }

        private void InitDataGridView()
        {
            _dataGridView.RowHeadersVisible = false;
            _dataGridView.MultiSelect = false;
            if (Values == null)
            {
                DataTable.ReadXmlSchema($"{DataTable.TableName}.xml");
                DataTable.ReadXml($"{DataTable.TableName}.xml");
                Values = new List<List<decimal>>();
                foreach (DataRow row in DataTable.Rows)
                {
                    var tempList = (from cell in row.ItemArray
                        where (cell != row.ItemArray[0] && cell != row.ItemArray[1])
                        select Convert.ToDecimal(cell)).ToList();
                    Values.Add(tempList);
                }
            }
            else
            {
                var expertDataColumn = new DataColumn()
                {
                    ColumnName = "Эксперты",
                    DataType = typeof(string),
                    ReadOnly = true,
                };
                var termNameColumn = new DataColumn()
                {
                    ColumnName = "Термы",
                    DataType = typeof(string),
                    ReadOnly = true,
                };
                DataTable.Columns.Add(expertDataColumn);
                DataTable.Columns.Add(termNameColumn);
                foreach (var columnName in TermValues)
                {
                    var dataColumn = new DataColumn()
                    {
                        ColumnName = columnName,
                        DataType = typeof(decimal)
                    };
                    DataTable.Columns.Add(dataColumn);
                }

                for (int i = 0; i < ExpertNames.Count;i++)
                {
                    for (var j = 0; j < TermNames.Count; j++)
                    {
                        var row = DataTable.NewRow();
                        row[0] = ExpertNames[i];
                        row[1] = TermNames[j];
                        DataTable.Rows.Add(row);
                    }
                }

                var lastI = DataTable.Columns.Count > Values.Count ? Values.Count : DataTable.Columns.Count;
                for (var i = 0; i < lastI; i++)
                {
                    var lastJ = DataTable.Rows.Count - 1 > Values[i].Count ? Values[i].Count : DataTable.Rows.Count - 1;
                    for (var j = 0; j < lastJ; j++)
                    {
                        DataTable.Rows[i][j + 2] = Values[i][j];
                    }
                }
            }
            _dataGridView.DataSource = DataTable;
            foreach (DataGridViewColumn column in _dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            OnFormResize(null, null);
        }

        public void SetNullCells()
        {
            foreach (DataRow row in DataTable.Rows)
            {
                for (var i = 0; i < row.ItemArray.Length; i++)
                {
                    if (row.IsNull(i))
                    {
                        row[i] = 0;
                    }
                }
            }
        }
    }
}

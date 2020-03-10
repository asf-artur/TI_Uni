using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winFormsDataGrid
{
    public class CalculationClass
    {
        private VisualDataLoadClass _visualDataLoadClass;
        public List<List<decimal>> Values => _visualDataLoadClass.Values;
        public DataTable DataTable => _visualDataLoadClass.DataTable;

        public CalculationClass(VisualDataLoadClass visualDataLoadClass)
        {
            _visualDataLoadClass = visualDataLoadClass;
        }

        public void Calculate()
        {
            DataClass dataClass = new DataClass(
                _visualDataLoadClass.ExpertNames,
                _visualDataLoadClass.TermNames,
                _visualDataLoadClass.TermValues);

            DataClass result = new DataClass(
                new List<string>(){"1", "2"}, 
                _visualDataLoadClass.TermNames,
                _visualDataLoadClass.TermValues);

            var a = dataClass.Experts.GroupBy(c => c.Terms.ToList().Select(d => d.Name));

            //foreach (var termValue in _visualDataLoadClass.TermValues)
            //{
            //    foreach (var termName in _visualDataLoadClass.TermNames)
            //    {

            //    }
            //}
        }

        public List<List<decimal>> Calculate1()
        {
            var result = new List<List<decimal>>();
            for (int i = 0; i < _visualDataLoadClass.TermNames.Count*2; i++)
            {
                var tempList = new List<decimal>();
                for (int j = 0; j < _visualDataLoadClass.TermValues.Count; j++)
                {
                    tempList.Add(0);
                }
                result.Add(tempList);
            }


            return result;
        }


    }
}

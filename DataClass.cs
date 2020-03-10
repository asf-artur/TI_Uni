using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace winFormsDataGrid
{
    public class DataClass
    {
        public List<Expert> Experts = new List<Expert>();

        public DataClass(List<string> expertNames, List<string> termNames, List<string> termValues)
        {
            for (int i = 0; i < expertNames.Count; i++)
            {
                var tempTerm = termNames.Select(c =>
                {
                    var tempTermValue = termValues.Select(d =>
                    {
                        return new TermValue(d);
                    }).ToList();
                    return new Term(c, tempTermValue);
                }).ToList();
                Experts.Add(new Expert(expertNames[i], tempTerm));
            }
        }
    }

    public class Expert
    {
        public string Name;
        public List<Term> Terms;

        public Expert(string name, List<Term> terms)
        {
            Name = name;
            Terms = terms;
        }
    }

    public class Term
    {
        public string Name;
        public List<TermValue> TermValues;

        public Term(string name, List<TermValue> termValues)
        {
            Name = name;
            TermValues = termValues;
        }
    }
    public class TermValue
    {
        public string Name;
        public int Value;

        public TermValue(string name)
        {
            Name = name;
            Value = 0;
        }
    }
}

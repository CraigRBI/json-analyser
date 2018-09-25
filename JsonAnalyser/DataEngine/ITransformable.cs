using System;
using System.Data;

namespace JsonAnalyser.DataEngine
{
    public interface ITransformable
    {
        DataTable CsvFormat { get; }
    }
}

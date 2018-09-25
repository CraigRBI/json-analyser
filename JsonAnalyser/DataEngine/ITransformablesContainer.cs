using System;
using System.Collections.Generic;

namespace JsonAnalyser.DataEngine
{
    public interface ITransformablesContainer
    {
        List<ITransformable> List { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

public class TypeListProvider : TestProvider<List<Type>>
{
    public override bool Compare(List<Type> before, List<Type> after)
    {
        return before.Except(after).Count<Type>() == 0 && after.Except(before).Count<Type>() == 0;
    }

    public override IEnumerable<List<Type>> GetValues()
    {
        yield return new List<Type>
        {
            typeof(int),
            typeof(int),
            typeof(float),
            typeof(int)
        };
        yield break;
    }
}
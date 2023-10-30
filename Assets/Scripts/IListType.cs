using System.Collections;
using System.Collections.Generic;

public interface IListType : IList<int>, ICollection<int>, IEnumerable<int>, IEnumerable
{
}

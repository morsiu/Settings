using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSettings.Infrastructure
{
    public interface ITreeWalker<TElement>
    {
        ITreeWalker<TElement> ClimbToRoot();

        ITreeWalker<TElement> ClimbUp(int ladderCount);

        ITreeWalker<TElement> ClimbDown(params int[] ladderIndexes);

        IEnumerable<TElement> GetDepthFirstUpwards(TElement element);

        IEnumerable<TElement> GetDepthFirstDownards(TElement element);

        TElement Current { get; }

        T CurrentAs<T>() where T : class, TElement;
    }
}

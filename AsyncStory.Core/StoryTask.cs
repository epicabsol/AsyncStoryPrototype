using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStory.Core
{
    /// <summary>
    /// A task that can be completed at some other time.
    /// Story hosts create subclasses of this type that can invoke its continuation.
    /// </summary>
    /// <typeparam name="TResult">The type of result that this task returns.</typeparam>
    public abstract class StoryTask<TResult> : INotifyCompletion
    {
        public StoryTask()
        {

        }

        public StoryTask<TResult> GetAwaiter()
        {
            return this;
        }

        #region Awaiter (INotifyCompletion)
        public bool IsCompleted => this.Result != null;

        protected Action Continuation { get; set; }
        protected TResult Result { get; set; }

        public virtual void OnCompleted(Action continuation)
        {
            this.Continuation = continuation;
        }

        public TResult GetResult()
        {
            return Result;
        }
        #endregion
    }
}

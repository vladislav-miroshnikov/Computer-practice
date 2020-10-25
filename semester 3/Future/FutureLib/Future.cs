using System;
using System.Threading.Tasks;

namespace FutureLib
{
    class Future<TResult>
    {
        private Task<TResult> task;

        public Future(Func<TResult> func)
        {
            task = new Task<TResult>(func);
            task.Start();
        }

        public TResult GetResult()
        {
            task.Wait();
            return task.Result;
        }

        public void Dispose()
        {
            task.Dispose();
        }
      
    }
}

using System.Threading;

namespace ExamSystemLib.LockFree
{
    public class LockFreeList : ILockFreeList
    {
        private volatile LockFreeNode head;
        //insert into the head, no need to maintain sorting
        public void Add(LockFreeNode item)
        {
            if (item == null)
            {
                return;
            }
            if(item.IsRemovedLogically)
            {
                item.IsRemovedLogically = false; //re-init
            }
            while(true)
            {
                if(Contains(item))  //if multiple threads want to insert the same element
                {
                    return;
                }
                item.SetNextValue(head);
                if(Interlocked.CompareExchange(ref head, item, item.GetNextValue()) != head)
                {
                    return;
                }

            }
        }

        public bool Contains(LockFreeNode item)
        {
            var pair = Find(item);
            if (pair == (null, null))
            {
                return false;
            }
            return (!pair.Item2.IsRemovedLogically);
        }

        public void Remove(LockFreeNode item)
        {
            if (item == null)
            {
                return;
            }

            while(true)
            {
                var pair = Find(item);
                if (pair == (null, null))
                {
                    return;
                }
                pair.Item2.IsRemovedLogically = true;
                if(pair.Item1 == null) //removing head
                {
                    if(Interlocked.CompareExchange(ref head, pair.Item2.GetNextValue(), pair.Item2) == pair.Item2)
                    {
                        return;
                    }
                }
                else //removing not head
                {
                    if(pair.Item1.IsRemovedLogically)
                    {
                        continue; //if another thread removed pred
                    }
                    if(Interlocked.CompareExchange(ref pair.Item1.GetNextRef(), pair.Item2.GetNextValue(), pair.Item2) == pair.Item2)
                    {
                        return;
                    }
                }

            }
        }

        private (LockFreeNode, LockFreeNode) Find(LockFreeNode item)
        {
            if(head == null)
            {
                return (null, null);
            }
            LockFreeNode curr = head;
            LockFreeNode pred = null;
            while (!curr.Equals(item))
            {
                if (curr.GetNextValue() == null)
                {
                    return (null, null);
                }
                pred = curr;
                curr = curr.GetNextValue();
            }
            return (pred, curr);

        }

       
    }
}

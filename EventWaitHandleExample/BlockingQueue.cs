namespace EventWaitHandleExample
{
    public class BlockingQueue<T>
    {
        private readonly List<T> _queue = [];
        private readonly EventWaitHandle _ewh = new(false, EventResetMode.AutoReset);

        public void Enqueue(T item)
        {
            _queue.Add(item);

            _ewh.Set();
        }

        public T Dequeue() { 
            _ewh.WaitOne();

            var item = _queue.First();
            _queue.Remove(item);


            return item;
        }
    }
}

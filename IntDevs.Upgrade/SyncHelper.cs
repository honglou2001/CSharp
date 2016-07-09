using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Threading;

namespace IntDevs.Upgrade
{
    //线程同步类
    public class SynchronizedDictionary<TKey, TValue>
    {
        private object _syncRoot = new object();
        private Dictionary<TKey, TValue> _dictionaryBase;

        public SynchronizedDictionary()
        {
            _dictionaryBase = new Dictionary<TKey, TValue>();
        }

        internal Dictionary<TKey, TValue> DictionaryBase
        {
            get
            {
                return _dictionaryBase;
            }
        }

        public void Add(TKey key, TValue val)
        {
            this[key] = val;
        }

        public bool Remove(TKey key)
        {
            lock (_syncRoot)
            {
                return _dictionaryBase.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _dictionaryBase.Clear();
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionaryBase.ContainsKey(key);
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dictionaryBase.Keys;
                }
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dictionaryBase.Values;
                }
            }
        }

        //public TKey[] AllKeys
        //{
        //    get
        //    {
        //        lock (_syncRoot)
        //        {
        //            return _dictionaryBase.Keys.ToArray();
        //        }
        //    }
        //}


        public TValue this[TKey key]
        {
            get
            {
                TValue v = default(TValue);
                try
                {
                    _dictionaryBase.TryGetValue(key, out v);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                    lock (_syncRoot)
                    {
                        _dictionaryBase.TryGetValue(key, out v);
                    }
                }
                return v;
            }
            set
            {
                lock (_syncRoot)
                {
                    _dictionaryBase[key] = value;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _dictionaryBase.Count;
                }
            }
        }

        public object SyncRoot
        {
            get
            {
                return _syncRoot;
            }
        }
    }

    public sealed class SynchronizeQueue<T>
    {
        #region private Fields
        private int isTaked = 0;
        private Queue<T> queue = new Queue<T>();
        private int MaxCount = 1000 * 1000;
        #endregion


        public int Count
        {
            get { return queue.Count; }
        }

        public void Enqueue(T t)
        {
            try
            {
                while (Interlocked.Exchange(ref isTaked, 1) != 0)
                {
                }
                this.queue.Enqueue(t);
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public T Dequeue()
        {
            try
            {
                while (Interlocked.Exchange(ref isTaked, 1) != 0)
                {
                }
                T t = this.queue.Dequeue();
                return t;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public bool TryEnqueue(T t)
        {
            try
            {
                for (int i = 0; i < MaxCount; i++)
                {
                    if (Interlocked.Exchange(ref isTaked, 1) == 0)
                    {
                        this.queue.Enqueue(t);
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public bool TryDequeue(out T t)
        {
            try
            {
                for (int i = 0; i < MaxCount; i++)
                {
                    if (Interlocked.Exchange(ref isTaked, 1) == 0)
                    {
                        t = this.queue.Dequeue();
                        return true;
                    }
                }
                t = default(T);
                return false;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }
    }
}

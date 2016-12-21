using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeferredObjectMaterialization
{
    public class Profiler : IDisposable
    {
        private static Dictionary<string, DateTime> entries;
        private DateTime startTime;

        static Profiler()
        {
            entries = new Dictionary<string, DateTime>(StringComparer.InvariantCultureIgnoreCase);
        }

        public string MethodName { get; set; }

        public Profiler()
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            var type = methodInfo.ReflectedType.Name;

            MethodName = string.Format("{0}.{1}", type, methodInfo.Name);
            startTime = DateTime.Now;
        }

        public void Dispose()
        {
            var duration = DateTime.Now - startTime;

            var s = string.Format("{0} completed in {1} milliseconds.",
                MethodName, duration.TotalMilliseconds.ToString());

            Debug.Print(s);
        }

        public static bool Add(string key, DateTime value)
        {
            try
            {
                if (entries.ContainsKey(key))
                {
                    entries[key] = value;
                }
                else
                {
                    entries.Add(key, value);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime? Get(string key)
        {
            return entries.ContainsKey(key) ? (Nullable<DateTime>)entries[key] : null;
        }

        public static string Start(string key)
        {
            var b = Add(key, DateTime.Now);

            return b ? key : null;
        }

        public static string Start()
        {
            var key = new Guid().ToString();

            var b = Add(key, DateTime.Now);

            return b ? key : null;
        }

        public static string Stop(string key)
        {
            try
            {
                var startTime = Get(key).Value;

                var duration = DateTime.Now - startTime;

                var s = string.Format("The operation completed in {0} milliseconds. Key: {1}",
                    duration.TotalMilliseconds.ToString(), key);

                Debug.Print(s);

                return s;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.ToString());

                return null;
            }
        }
    }
}
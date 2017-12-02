using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using ARMNotify;
using System.Threading.Tasks;

namespace HttpTriggerCoreDebug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var log = new ConsoleTraceWriter(TraceLevel.Info);
            await HttpTrigger.Run(null, log);
            Console.ReadLine();
        }

        class ConsoleTraceWriter : TraceWriter
        {
            internal ConsoleTraceWriter(TraceLevel level) : base(level)
            {
            }

            public override void Trace(TraceEvent traceEvent)
            {
                Console.WriteLine(traceEvent.Message);
            }
        }
    }
}

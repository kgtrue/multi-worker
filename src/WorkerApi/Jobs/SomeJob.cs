using TickerQ.Utilities.Base;

namespace WorkerApi.Jobs;

public class SomeJob
{
    [TickerFunction("ReportJob", cronExpression:"0 0 0 * * *")]
    public Task ReportJob(TickerFunctionContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{nameof(SomeJob)} started");
        return Task.CompletedTask;
    }
    
    [TickerFunction("ReportJob2")]
    public Task ReportJob2(TickerFunctionContext context, CancellationToken cancellationToken)
    {
        Console.WriteLine($"{nameof(SomeJob)} started");
        return Task.CompletedTask;
    }
}
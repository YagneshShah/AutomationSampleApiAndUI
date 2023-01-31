using AutomationTestSample.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AutomationTestSample.Controllers.Abstract;

public abstract class TestControllerBase : ControllerBase
{
    protected readonly ITestDbContext _dbContext;
    protected readonly ILogger _logger;

    private const int MinimumDelayInMilliseconds = 2 * 1000;
    private const int MaximumDelayInMilliseconds = 5 * 1000;

    private static readonly Random Random = new Random();

    public TestControllerBase(ITestDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task RandomDelay()
    {
        var secondsDelay = Random.Next(MinimumDelayInMilliseconds, MaximumDelayInMilliseconds);
        await Task.Delay(secondsDelay);
    }
}

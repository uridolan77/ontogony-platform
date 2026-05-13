using Xunit;

namespace Ontogony.Http.Tests;

/// <summary>
/// Tests for <see cref="RetryBudgetTracker"/>.
/// </summary>
public class RetryBudgetTrackerTests
{
    [Fact]
    public void CanRetry_WithinBudget_ReturnsTrue()
    {
        var tracker = new RetryBudgetTracker(maxRetries: 3);
        
        Assert.True(tracker.CanRetry());
        Assert.True(tracker.CanRetry());
        Assert.True(tracker.CanRetry());
    }

    [Fact]
    public void CanRetry_ExhaustedBudget_ReturnsFalse()
    {
        var tracker = new RetryBudgetTracker(maxRetries: 1);
        
        Assert.True(tracker.CanRetry());
        Assert.False(tracker.CanRetry());
    }

    [Fact]
    public void CanRetry_ZeroBudget_ReturnsFalse()
    {
        var tracker = new RetryBudgetTracker(maxRetries: 0);
        
        Assert.False(tracker.CanRetry());
    }

    [Fact]
    public void Reset_ReleasesPreviouslyExhaustedBudget()
    {
        var tracker = new RetryBudgetTracker(maxRetries: 1);
        
        Assert.True(tracker.CanRetry());
        Assert.False(tracker.CanRetry());
        
        tracker.Reset();
        
        Assert.True(tracker.CanRetry());
    }

    [Fact]
    public void AttemptCount_TracksRetries()
    {
        var tracker = new RetryBudgetTracker(maxRetries: 5);
        
        Assert.Equal(0, tracker.AttemptCount);
        
        tracker.CanRetry();
        Assert.Equal(1, tracker.AttemptCount);
        
        tracker.CanRetry();
        Assert.Equal(2, tracker.AttemptCount);
    }
}

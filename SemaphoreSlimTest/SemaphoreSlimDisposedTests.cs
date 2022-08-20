namespace semaphoreSlimTest;

public class SemaphoreSlimDisposedTests
{
    [Fact(Timeout = 2000)]
    public async Task ShouldThrowOperationCanceledException_WhenSemaphoreDisposedAfterCancellingTheTokenWithoutDelay()
    {
        var semaphore = new SemaphoreSlim(0);
        using var cancelSource = new CancellationTokenSource();
        var t = semaphore.WaitAsync(cancelSource.Token);
        cancelSource.Cancel();
        semaphore.Dispose();
        await Assert.ThrowsAsync<OperationCanceledException>(() => t);
    }

    [Fact(Timeout = 2000)]
    public async Task ShouldThrowOperationCanceledException_WhenSemaphoreDisposedAfterCancellingTheTokenWithDelay()
    {
        var semaphore = new SemaphoreSlim(0);
        using var cancelSource = new CancellationTokenSource();
        var t = semaphore.WaitAsync(cancelSource.Token);
        cancelSource.Cancel();
        await Task.Delay(200);
        semaphore.Dispose();
        await Assert.ThrowsAsync<OperationCanceledException>(() => t);
    }

    [Fact(Timeout = 2000)]
    public async Task ShouldThrowObjectDisposedException_WhenWhenCallingWaitAsyncAfterSemaphoreDisposed()
    {
        var semaphore = new SemaphoreSlim(0);
        semaphore.Dispose();
        using var cancelSource = new CancellationTokenSource();
        await Assert.ThrowsAsync<ObjectDisposedException>(() => semaphore.WaitAsync(cancelSource.Token));
    }

    [Fact(Timeout = 2000)]
    public async Task ShouldThrowObjectDisposedException_WhenWhenCallingDisposedAfterCallingWaitAsyncOnSemaphore()
    {
        var semaphore = new SemaphoreSlim(0);
        using var cancelSource = new CancellationTokenSource();
        var t = semaphore.WaitAsync(cancelSource.Token);
        semaphore.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(() => t);
    }
}
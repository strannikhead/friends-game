using System.Diagnostics;

public static class TimeSystem
{
    private static readonly Stopwatch timer = new();

    public static float ElapsedTime => timer.ElapsedMilliseconds / 1000f;
    // Start is called before the first frame update
    public static void Start()
    {
        timer.Start();
    }

    public static void Restart()
    {
        timer.Restart();
    }

    public static void Stop()
    {
        timer.Stop();
    }

    public static void Reset()
    {
        timer.Reset();
    }
}

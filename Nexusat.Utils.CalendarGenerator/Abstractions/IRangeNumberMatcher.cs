namespace Nexusat.Utils.CalendarGenerator
{
    public interface IRangeNumberMatcher : INumberMatcher
    {
        int? Left { get; }
        int? Right { get; }
        bool IsLeftOpenRange { get; }
        bool IsRightOpenRange { get; }
        bool IsOpenRange { get; }
        bool IsLeftRightOpenRange { get; }
        bool IsClosedRange { get; }
        bool IsOneValue { get; }
    }
}
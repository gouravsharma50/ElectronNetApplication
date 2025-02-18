using System;
namespace DesktopApplication.Utilities
{
    public static class DateTimeFormatter
    {
        public static string DateTimeStrMMM(DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("dd MMM yyyy") : string.Empty;
        }
    }
}

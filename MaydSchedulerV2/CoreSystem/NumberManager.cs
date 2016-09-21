namespace MaydSchedulerApp
{
    public static class NumberManager
    {
        public static string Validate(string time)
        {
            if (time != "")
            {
                if (int.Parse(time) < 0)
                    return 0.ToString();
                else if (int.Parse(time) > 24)
                    return 24.ToString();
                else
                    return time;
            }
            return time;
        }

        public static bool CheckValidShifts(string start, string end)
        {
            if (int.Parse(end) - int.Parse(start) < SystemSettings.minShift || int.Parse(end) - int.Parse(start) > SystemSettings.maxShift)
                return false;
            else
                return true;
        }

        public static bool CheckValid(string time)
        {
            if (int.Parse(time) > 24 || int.Parse(time) < 0)
                return false;
            else
                return true;
        }
    }
}
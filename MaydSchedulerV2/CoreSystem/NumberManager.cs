namespace MaydSchedulerApp
{
    public static class NumberManager
    {
        public static string Validate(string time)
        {
            if (int.Parse(time) < 0)
                return 0.ToString();
            else if (int.Parse(time) > 23)
                return 23.ToString();
            else
                return time;
        }

        public static bool CheckValidShifts(int start, int end)
        {
            if (end - start < SystemSettings.minShift || end - start > SystemSettings.maxShift)
                return false;
            else
                return true;
        }

        public static bool Validate(int time)
        {
            if (time > 23 || time < 0)
                return false;
            else return false;
        }
    }
}
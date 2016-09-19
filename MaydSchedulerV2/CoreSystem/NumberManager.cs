namespace MaydSchedulerApp
{
    public static class NumberManager
    {
        public static int Convert(int time)
        {
            if (time < 0)
                return 0;
            else if (time > 23)
                return 23;
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
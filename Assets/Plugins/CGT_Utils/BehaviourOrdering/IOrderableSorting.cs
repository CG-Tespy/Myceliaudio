namespace CGT
{
    public static class IOrderableSorting
    {
        public static int ByPriorityDescending(IOrderableBehaviour first, IOrderableBehaviour second)
        {
            if (first.Priority > second.Priority)
            {
                return -1;
            }
            else if (second.Priority > first.Priority)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
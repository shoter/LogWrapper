namespace LogWrapper.Data
{
    public class GrouppedLogs
    {
        public string GroupName { get; }

        public string Logs { get; }

        public GrouppedLogs(string groupName, string logs)
        {
            this.GroupName = groupName;
            this.Logs = logs;
        }
    }
}

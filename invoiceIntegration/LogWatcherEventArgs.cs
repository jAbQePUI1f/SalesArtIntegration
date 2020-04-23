namespace invoiceIntegration
{
    internal class LogWatcherEventArgs
    {
        private string contents;

        public LogWatcherEventArgs(string contents)
        {
            this.contents = contents;
        }
    }
}
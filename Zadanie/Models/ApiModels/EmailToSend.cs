namespace Models.ApiModels
{
    public class EmailToSend
    {
        public string EmailReceiver { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
    }
}
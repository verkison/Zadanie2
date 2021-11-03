namespace Models.ApiModels
{
    public class EmailToSend
    {
        public string EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
    }
}
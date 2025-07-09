namespace MenMaxBackEnd.ModelsDTO
{
    public class Mail
    {
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailCc { get; set; }
        public string MailBcc { get; set; }
        public string MailSubject { get; set; }
        public string MailContent { get; set; }
        public string ContentType { get; set; } = "text/plain";
        public List<object> Attachments { get; set; }

        public DateTime GetMailSendDate()
        {
            return DateTime.Now; // Sửa để trả về thời gian hiện tại, thay vì new DateTime()
        }
    }
}

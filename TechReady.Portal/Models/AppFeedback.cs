namespace TechReady.Portal.Models
{
    public class AppFeedback
    {
        public int AppFeedbackID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FeedbackType { get; set; }

        public string FeedbackText { get; set; }

        public int AppUserID { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}

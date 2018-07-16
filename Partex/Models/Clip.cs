using System;

namespace PersianEditor.Models
{
    public enum ClipboardOperation
    {
        Cut,
        Copy
    }

    public class Clip
    {
        public string Content { get; set; }
        public DateTime When { get; set; }
        public ClipboardOperation Operation { get; set; }

        public  Clip(string content)
        {
            Content = content;
        }

        public Clip(string content, ClipboardOperation operation, DateTime when)
        {
            Content = content;
            Operation = operation;
            When = when;
        }
    }
}

using Farcin.Editor.Core.Date;
using System;

namespace Farcin.Editor.Core.Models {
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
        public string OperationDisplay => Operation == ClipboardOperation.Copy
            ? "کپی"
            : "برش";
        public string WhenPersian => new PersianDateHelper(When)
            .ToString(PersianDateFormat.Day_MonthName_Year_Time);

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

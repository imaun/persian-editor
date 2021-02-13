using System;
using System.Globalization;
using System.Collections.Generic;

namespace Farcin.Editor.Core.Date {

    public enum PersianDateFormat {
        ShortDate = 0,
        LongDate = 1,
        WeekName_LongDate = 2,
        WeekName_Day_MonthName_Year = 3,
        Day_MonthName_Year = 4,
        WeekName_Day_MonthName_Year_Time = 5,
        Day_MonthName_Year_Time = 6
    }

    public class PersianDateHelper {

        public PersianDateHelper(DateTime value) {
            DateValue = value;
        }

        private Dictionary<int, string> _months = new Dictionary<int, string>() {
            { 1, "فروردین"},
            { 2, "اردیبهشت"},
            { 3, "خرداد"},
            { 4, "تیر"},
            { 5, "مرداد"},
            { 6, "شهریور"},
            { 7, "مهر"},
            { 8, "آبان"},
            { 9, "آذر"},
            { 10, "دی"},
            { 11, "بهمن"},
            { 12, "اسفند"}
        };

        private Dictionary<DayOfWeek, string> _weekDayNames = new Dictionary<DayOfWeek, string> {
            { DayOfWeek.Friday, "جمعه"},
            { DayOfWeek.Monday, "دوشنبه"},
            { DayOfWeek.Saturday, "شنبه"},
            { DayOfWeek.Sunday, "یکشنبه"},
            { DayOfWeek.Thursday, "پنجشنبه"},
            { DayOfWeek.Tuesday, "سه شنبه"},
            { DayOfWeek.Wednesday, "چهارشنبه"}
        };

        public DateTime DateValue { get; set; }

        public string ToString(PersianDateFormat format = PersianDateFormat.ShortDate) {
            return Convert(format);
        }

        private string Convert(PersianDateFormat format = PersianDateFormat.ShortDate) {
            var pc = new PersianCalendar();
            var dayNum = pc.GetDayOfMonth(DateValue);
            var monthNum = pc.GetMonth(DateValue);
            var monthName = _months[monthNum];
            var year = pc.GetYear(DateValue);
            var time = $"{DateValue.Date.Hour}:{DateValue.Date.Minute}";
            var dayName = _weekDayNames[DateValue.DayOfWeek];

            switch (format) {
                case PersianDateFormat.Day_MonthName_Year:
                    return $"{dayNum} {monthName} {year}";
                case PersianDateFormat.Day_MonthName_Year_Time:
                    return $"{dayNum} {monthName} {year} {time}";
                case PersianDateFormat.LongDate:
                    return $"{dayNum}/{monthNum}/{year} {time}";
                case PersianDateFormat.ShortDate:
                    return $"{dayNum}/{monthNum}/{year}";
                case PersianDateFormat.WeekName_LongDate:
                    return $"{dayName} {dayNum}/{monthNum}/{year} {time}";
                case PersianDateFormat.WeekName_Day_MonthName_Year:
                    return $"{dayName} {dayNum} {monthName} {year}";
                case PersianDateFormat.WeekName_Day_MonthName_Year_Time:
                    return $"{dayName} {dayNum} {monthName} {year} {time}";
                default:
                    return $"{dayNum}/{monthNum}/{year}";
            }
        }
    }
}

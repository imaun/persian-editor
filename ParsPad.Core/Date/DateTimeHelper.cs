using System;
using System.Collections.Generic;
using System.Globalization;
using Farcin.Editor.Core.Models.Types;

namespace Farcin.Editor.Core.Helpers {
    public static class DateTimeHelper {

        private static Dictionary<int, string> monthNames = new Dictionary<int, string> {
            { 0, "January" },
            { 1, "February" },
            { 2, "March" },
            { 3, "April" },
            { 4, "May" },
            { 5, "June" },
            { 6, "July" },
            { 7, "August" },
            { 8, "September" },
            { 9, "October" },
            { 10, "November" },
            { 11, "December" }
        };

        private static Dictionary<int, string> persianMonthNames = new Dictionary<int, string> {
            { 1, "فروردین" },
            { 2, "اردیبهشت" },
            { 3, "خرداد" },
            { 4, "تیر" },
            { 5, "مرداد" },
            { 6, "شهریور" },
            { 7, "مهر" },
            { 8, "آبان" },
            { 9, "آذر" },
            { 10, "دی" },
            { 11, "بهمن" },
            { 12, "اسفند" }
        };

        private static Dictionary<DayOfWeek, string> weekDayNames = new Dictionary<DayOfWeek, string> {
            { DayOfWeek.Sunday, "Sunday" },
            { DayOfWeek.Monday, "Monday" },
            { DayOfWeek.Tuesday, "Tuesday" },
            { DayOfWeek.Wednesday, "Wednesday" },
            { DayOfWeek.Thursday, "Thursday" },
            { DayOfWeek.Friday, "Friday" },
            { DayOfWeek.Saturday, "Saturday" }
        };

        private static Dictionary<DayOfWeek, string> persianWeekDays = new Dictionary<DayOfWeek, string> {
            { DayOfWeek.Saturday, "شنبه" },
            { DayOfWeek.Sunday, "یکشنبه" },
            { DayOfWeek.Monday, "دوشنبه" },
            { DayOfWeek.Tuesday, "سه شنبه" },
            { DayOfWeek.Wednesday, "چهارشنبه" },
            { DayOfWeek.Thursday, "پنجشنبه" },
            { DayOfWeek.Friday, "جمعه" }
        };

        private static PersianCalendar persianDate = new PersianCalendar();

        public static DateTime Now() {
            return DateTime.Now;
        }

        public static string GetSystemDateStr(
            DateTime date, InsertDateFormat dateFormat = InsertDateFormat.DateNumbers) {
            
            switch(dateFormat) {
                case InsertDateFormat.WeekDayDateNumbers:
                    return $"{weekDayNames[date.DayOfWeek]} {date.ToShortDateString()}";
                case InsertDateFormat.WeekDayMonthNameDayYear:
                    return $"{weekDayNames[date.DayOfWeek]}, {monthNames[date.Month]} {date.Day}, {date.Year}";
                case InsertDateFormat.DayMonthNameYear:
                    return $"{date.Day} {monthNames[date.Month]} {date.Month}";
                case InsertDateFormat.WeekDayDayMonthNameYear:
                    return $"{weekDayNames[date.DayOfWeek]} {date.Day} {monthNames[date.Month]} {date.Year}";
            }
            
            return date.ToShortDateString();
        }

        public static string GetSystemTimeStr(DateTime date) {
            return $"{date.Hour}:{date.Minute}:{date.Second}";
        }

        public static string GetCurrentSystemTimeStr() {
            return GetSystemTimeStr(Now());
        }

        public static string GetCurrentSystemDateLongStr() {
            return Now().ToLongDateString();
        }

        public static string GetPersianDateStr(
            DateTime date, 
            InsertDateFormat dateFormat = InsertDateFormat.DateNumbers,
            bool includeTime = false) {

            var year = persianDate.GetYear(date);
            var month = persianDate.GetMonth(date);
            var day = persianDate.GetDayOfMonth(date);
            var weekDay = persianDate.GetDayOfWeek(date);
            var dateStr = $"{day}/{month}/{year}";
            var time = $"{date.Hour}:{date.Minute}:{date.Second}";

            string result = string.Empty;
            switch(dateFormat) {
                case InsertDateFormat.DayMonthNameYear:
                    result = $"{day} {persianMonthNames[month]} {year}";
                    break;
                case InsertDateFormat.WeekDayDateNumbers:
                    result = $"{persianWeekDays[weekDay]} {dateStr}";
                    break;
                case InsertDateFormat.WeekDayDayMonthNameYear:
                    result = $"{persianWeekDays[weekDay]} {day} {persianMonthNames[month]} {year}";
                    break;
                case InsertDateFormat.WeekDayMonthNameDayYear:
                    result = $"{persianWeekDays[weekDay]}, {persianMonthNames[month]} {day}, {year}";
                    break;
            }

            if (includeTime)
                result += $" {time}";

            return result;
        }

        public static string GetCurrentPersianDateStr(InsertDateFormat dateFormat = InsertDateFormat.DateNumbers) {
            return GetPersianDateStr(Now(), dateFormat);
        }

    }
}

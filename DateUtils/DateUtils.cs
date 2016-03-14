using System;
using System.Collections.Generic;

namespace DateUtils
{
    public class DateUtils
    {
        private static DateTime GetInstanceofWeekdayinMonth(DateTime dt, DayOfWeek weekday, int instance)
        {

            if (instance <= 0)
            {
                throw new ArgumentException("Instance count must be greater than zero", "instance");
            }

            DateTime dtRet = new DateTime();
            DateTime dtFirstDay = GetFirstInstancedDayOfMonth(dt, weekday);
            int instancesInMonth = GetInstanceCountofWeekdayInMonth(dt, weekday);

            if (instance <= instancesInMonth)
            {
                int padDays = 7 * (instance - 1);
                dtRet = new DateTime(dt.Year, dt.Month, dtFirstDay.Day + padDays);
            }
            else
            {
                throw new ArgumentException("Instance range exceeded, _max: " + instancesInMonth.ToString(), "instance");
            }

            return dtRet;

        }

        private static int GetInstanceCountofWeekdayInMonth(DateTime dt, DayOfWeek weekday)
        {

            return ((GetLastInstacedDayOfMonth(dt, weekday).Day - GetFirstInstancedDayOfMonth(dt, weekday).Day) / 7) + 1;

        }

        private static DateTime GetFirstInstancedDayOfMonth(DateTime dt, DayOfWeek weekday)
        {
            DateTime dtFirstDay = new DateTime(dt.Year, dt.Month, 1);

            if (weekday < dtFirstDay.DayOfWeek)
            {
                dtFirstDay = dtFirstDay.AddDays(weekday - dtFirstDay.DayOfWeek + 7);
            }
            else
            {
                dtFirstDay = dtFirstDay.AddDays(weekday - dtFirstDay.DayOfWeek);
            }

            return dtFirstDay;
        }

        private static DateTime GetLastInstacedDayOfMonth(DateTime dt, DayOfWeek weekday)
        {

            int Days = DateTime.DaysInMonth(dt.Year, dt.Month);

            

            DateTime dtLastDay = new DateTime(dt.Year, dt.Month, Days);

            if (weekday > dtLastDay.DayOfWeek)
            {
                dtLastDay = dtLastDay.AddDays(weekday - dtLastDay.DayOfWeek - 7);
            }
            else
            {
                dtLastDay = dtLastDay.AddDays(weekday - dtLastDay.DayOfWeek);
            }

            return dtLastDay;

        }
        /// <summary>
        /// Calculates a valid business date for shipping. Allows you to retrieve a valid date X number of days out.
        /// > Shipdate(DateTime.Today, 2) should return a date that is two business days from today
        /// 
        /// </summary>
        /// <param name="startdate">The starting date</param>
        /// <param name="days">The number of business days to skip</param>
        /// <returns></returns>
        public static DateTime ShipDate(DateTime startdate, int days)
        {
            DateTime result = startdate;
            List<DateTime> daystoskip = BlackListedDays();
            
            if (DateTime.Now.TimeOfDay >= new TimeSpan(15, 0, 0))
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    days = days + 1;
                }
            }

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                days = days + 1;
            }

            if (days == 0)
            {
                return startdate;
            }

            do
            {
                if (days >= 0)
                {
                    result = result.AddDays(1);

                    if (daystoskip.Contains(result.Date) == false && result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
                    {
                        days = days - 1;
                    }
                }
                else
                {
                    result = result.AddDays(-1);

                    if (daystoskip.Contains(result) == false && result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
                    {
                        days = days + 1;
                    }
                }

            } while (days != 0);

            return result;
        }

        /// <summary>
        /// Container object for listing Holiday Calculations
        /// </summary>
        public class Holiday
        {
            // New Years Day is 1/1/XXXX
            public static DateTime NewYears = Convert.ToDateTime("1/1/" + DateTime.Now.Year).AddYears(1);


            // Memorial Day - Last Monday in May
            public static DateTime MemorialDay = GetLastInstacedDayOfMonth(new DateTime(DateTime.Now.Year, 5, 1), DayOfWeek.Monday);

            // Labour Day = First Monday in September
            public static DateTime LabourDay = GetInstanceofWeekdayinMonth(new DateTime(DateTime.Now.Year, 9, 1), DayOfWeek.Monday, 1);

            // Thanksgiving - Last Thursday in November
            public static DateTime Thanksgiving = GetInstanceofWeekdayinMonth(new DateTime(DateTime.Now.Year, 11, 1), DayOfWeek.Thursday, 4);

            // Christmas - 25th December
            public static DateTime Christmas = Convert.ToDateTime("12/25/" + DateTime.Now.Year);
        }

        /// <summary>
        /// List of Holidays or scheduled downtimes
        /// </summary>
        /// <returns>Returns list of Holidays in DateTime</returns>
        public static List<DateTime> BlackListedDays()
        {
            List<DateTime> result = new List<DateTime>();

            result.Add(Holiday.Christmas);
            result.Add(Holiday.LabourDay);
            result.Add(Holiday.NewYears);
            result.Add(Holiday.Thanksgiving);
            result.Add(Holiday.MemorialDay);

            return result;
        }

    }
}

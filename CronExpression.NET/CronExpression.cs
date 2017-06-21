using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CronExpression.NET.Extensions;

namespace CronExpression.NET
{
    public class CronExpression
    {
        #region Enums
        public enum MinutesModes
        {
            Undefined,
            EveryMinute,
            EvenMinutes,
            OddMinutes,
            EveryXMinutes,
            Minutes
        }

        public enum HoursModes
        {
            Undefined,
            EveryHour,
            EvenHours,
            OddHours,
            EveryXHours,
            Hours
        }

        public enum DaysModes
        {
            Undefined,
            EveryDay,
            EvenDays,
            OddDays,
            EveryXDays,
            Days
        }

        public enum MonthsModes
        {
            Undefined,
            EveryMonth,
            EvenMonths,
            OddMonths,
            EveryXMonths,
            Months
        }

        public enum WeekdaysModes
        {
            Undefined,
            EveryWeekday,
            WorkingWeekdays,
            WeekendWeekdays,
            Weekdays
        }

        public enum DaysOfWeek
        {
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }
        #endregion

        #region Private members
        private MinutesModes minutesMode;
        private HoursModes hoursMode;
        private DaysModes daysMode;
        private MonthsModes monthsMode;
        private WeekdaysModes weekdaysMode;

        private List<int> minutes;
        private List<int> hours;
        private List<int> days;
        private List<int> months;
        private List<int> weekdays;

		private List<int> validMinutes;
		private List<int> validHours;
		private List<int> validDays;
		private List<int> validMonths;
		private List<int> validWeekdays;
        #endregion

        #region Public properties
        public MinutesModes MinutesMode { get { return this.minutesMode; } }
        public HoursModes HoursMode { get { return this.hoursMode; } }
        public DaysModes DaysMode { get { return this.daysMode; } }
        public MonthsModes MonthsMode { get { return this.monthsMode; } }
        public WeekdaysModes WeekdaysMode { get { return this.weekdaysMode; } }

        public List<int> Minutes { get { return this.minutes; } }
		public List<int> Hours { get { return this.hours; } }
		public List<int> Days { get { return this.days; } }
		public List<int> Months { get { return this.months; } }
		public List<int> Weekdays { get { return this.weekdays; } }
        #endregion

        #region Constructor(s)
        public CronExpression()
        {
            this.minutesMode = MinutesModes.Undefined;
            this.hoursMode = HoursModes.Undefined;
            this.daysMode = DaysModes.Undefined;
            this.monthsMode = MonthsModes.Undefined;
            this.weekdaysMode = WeekdaysModes.Undefined;

            this.minutes = null;
            this.hours = null;
            this.days = null;
            this.months = null;
            this.weekdays = null;
        }
        #endregion

        #region Public methods
        public void SetMinutes(MinutesModes mode, params int[] values) {
            this.minutesMode = mode;
            this.minutes = new List<int>();
            foreach (int tempValue in values) {
                if (!this.minutes.Contains(tempValue)) this.minutes.Add(tempValue);
            }
        }

        public void SetHours(HoursModes mode, params int[] values)
		{
            this.hoursMode = mode;
			this.hours = new List<int>();
			foreach (int tempValue in values)
			{
				if (!this.hours.Contains(tempValue)) this.hours.Add(tempValue);
			}
		}

        public void SetDays(DaysModes mode, params int[] values)
		{
			this.daysMode = mode;
			this.days = new List<int>();
			foreach (int tempValue in values)
			{
				if (!this.days.Contains(tempValue)) this.days.Add(tempValue);
			}
		}

        public void SetMonths(MonthsModes mode, params int[] values)
		{
            this.monthsMode = mode;
			this.months = new List<int>();
			foreach (int tempValue in values)
			{
				if (!this.months.Contains(tempValue)) this.months.Add(tempValue);
			}
		}

        public string GenerateExpression() {
            return String.Format("{0} {1} {2} {3} {4}",
                                 this.GenerateMinutes(), 
                                 this.GenerateHours(),
                                 this.GenerateDays(),
                                 this.GenerateMonths(),
                                 this.GenerateWeekdays());
        }

        public List<DateTime> GenerateDates(DateTime startDate, DateTime endDate, int limit = 0) {
            List<DateTime> output = new List<DateTime>();
            DateTime tempDate = startDate;
            int minutesOffset = 60;
            int hoursOffset = 0;
            int daysOffset = 0;

            if (this.minutes != null && this.minutesMode.Equals(MinutesModes.EveryXMinutes))
                minutesOffset = this.minutes[0] * 60;

            if (this.hours != null && this.hoursMode.Equals(HoursModes.EveryXHours))
                hoursOffset = this.hours[0] * 60 * 60;

            if (this.days != null && this.daysMode.Equals(DaysModes.EveryXDays))
				daysOffset = this.hours[0] * 60 * 60 * 24;

            int offset = minutesOffset + hoursOffset + daysOffset;

            while (tempDate.CompareTo(endDate) <= 0) {

                if (validMinutes.Contains(tempDate.Minute) &&
                    validHours.Contains(tempDate.Hour) &&
                    validMonths.Contains(tempDate.Month) &&
                    validDays.Contains(tempDate.Day) &&
                    validWeekdays.Contains((int)tempDate.DayOfWeek))
                {
                    output.Add(tempDate);
                    if (limit > 0 && output.Count() > limit) break;
                }
                tempDate.AddSeconds(offset);
            }

            return output;
        }
		#endregion

		#region Private methods
        private bool IsValidMinute(int minutes) {
            return true;
        }

		private bool IsValidHours(int hours)
		{
			return true;
		}

		private bool IsValidDays(int days)
		{
			return true;
		}

		private bool IsValidMonth(int month)
		{
			return true;
		}

		private bool IsValidWeekday(int weekday)
		{
			return true;
		}

        private string GenerateMinutes()
		{
            StringBuilder sb = new StringBuilder();
            switch (this.minutesMode) {
                case MinutesModes.EveryMinute :
                    sb.Append("*");
                    break;
				case MinutesModes.EvenMinutes:
					sb.Append("*/2");
					break;
                case MinutesModes.OddMinutes:
					sb.Append("1-59/2");
					break;
                case MinutesModes.EveryXMinutes :
                    if (this.minutes == null || this.minutes.Count != 1) {
                        throw new Exception("Invalid values for minutes");
                    }
                    sb.Append("*/");
                    sb.Append(this.minutes[0]);
                    break;
                case MinutesModes.Minutes :
                    if (this.minutes == null || this.minutes.Count <= 0)
						throw new Exception("No specified values for minutes");
                    if (this.minutes.Count == 1)
                        return this.minutes[0].ToString();
                    this.minutes.Sort();
                    sb.Append(String.Join(", ", this.minutes.ToRanges()));
                    break;
            }
            return sb.ToString();
		}

		private string GenerateHours()
		{
			StringBuilder sb = new StringBuilder();
            switch (this.hoursMode)
			{
                case HoursModes.EveryHour:
					sb.Append("*");
					break;
				case HoursModes.EvenHours:
					sb.Append("*/2");
					break;
                case HoursModes.OddHours:
					sb.Append("1-23/2");
					break;
				case HoursModes.EveryXHours:
					if (this.hours == null || this.hours.Count != 1)
					{
						throw new Exception("Invalid values for hours");
					}
					sb.Append("*/");
					sb.Append(this.hours[0]);
					break;
				case HoursModes.Hours:
					if (this.hours == null || this.hours.Count <= 0)
						throw new Exception("No specified values for hours");
					if (this.hours.Count == 1)
						return this.hours[0].ToString();
                    this.hours.Sort();
					sb.Append(String.Join(", ", this.hours.ToRanges()));
					break;
			}
			return sb.ToString();
		}

		private string GenerateDays()
		{
			StringBuilder sb = new StringBuilder();
            switch (this.daysMode)
			{
				case DaysModes.EveryDay:
					sb.Append("*");
					break;
				case DaysModes.EvenDays:
					sb.Append("*/2");
					break;
                case DaysModes.OddDays:
					sb.Append("1-31/2");
					break;
				case DaysModes.EveryXDays:
					if (this.days == null || this.days.Count != 1)
					{
						throw new Exception("Invalid values for days");
					}
					sb.Append("*/");
					sb.Append(this.days[0]);
					break;
				case DaysModes.Days:
					if (this.days == null || this.days.Count <= 0)
						throw new Exception("No specified values for hours");
					if (this.days.Count == 1)
						return this.days[0].ToString();
					this.days.Sort();
					sb.Append(String.Join(", ", this.days.ToRanges()));
					break;
			}
			return sb.ToString();
		}

		private string GenerateMonths()
		{
			StringBuilder sb = new StringBuilder();
            switch (this.monthsMode)
			{
                case MonthsModes.EveryMonth:
					sb.Append("*");
					break;
                case MonthsModes.EvenMonths:
					sb.Append("*/2");
					break;
                case MonthsModes.OddMonths:
					sb.Append("1-11/2");
					break;
                case MonthsModes.EveryXMonths:
                    if (this.months == null || this.months.Count != 1)
					{
						throw new Exception("Invalid values for months");
					}
					sb.Append("*/");
					sb.Append(this.months[0]);
					break;
                case MonthsModes.Months:
					if (this.months == null || this.months.Count <= 0)
						throw new Exception("No specified values for months");
					if (this.months.Count == 1)
						return this.months[0].ToString();
					this.months.Sort();
					sb.Append(String.Join(", ", this.months.ToRanges()));
					break;
			}
			return sb.ToString();
		}

		private string GenerateWeekdays()
		{
			StringBuilder sb = new StringBuilder();
			switch (this.weekdaysMode)
			{
				case WeekdaysModes.EveryWeekday:
					sb.Append("*");
					break;
                case WeekdaysModes.WorkingWeekdays:
					sb.Append("1-5");
					break;
                case WeekdaysModes.WeekendWeekdays:
					sb.Append("0,6");
					break;
				case WeekdaysModes.Weekdays:
					if (this.weekdays == null || this.weekdays.Count <= 0)
						throw new Exception("No specified values for months");
					if (this.weekdays.Count == 1)
						return this.weekdays[0].ToString();
					this.weekdays.Sort();
					sb.Append(String.Join(", ", this.weekdays.ToRanges()));
					break;
			}
			return sb.ToString();
		}
        #endregion
    }
}

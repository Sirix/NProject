using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NProject.Models.Infrastructure
{
    /// <summary>
    /// Checks that LaterDate value has greater DateTime value than EarlyDate
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DataComparisonAttribute : ValidationAttribute
    {
        public string EarlyDatePropertyName { get; set; }
        public string LaterDatePropertyName { get; set; }

        private const string _defaultErrorMessage = "{0} must be later than {1}";
        private readonly object _typeId = new object();

        public DataComparisonAttribute(string earlyDate, string laterDate)
            :base(_defaultErrorMessage)
        {
            EarlyDatePropertyName = earlyDate;
            LaterDatePropertyName = laterDate;
        }
        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                EarlyDatePropertyName, LaterDatePropertyName);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            var earlyDate =
                (properties.Find(EarlyDatePropertyName, true /* ignoreCase */).GetValue(value));
            var laterDate =
               (properties.Find(LaterDatePropertyName, true /* ignoreCase */).GetValue(value));

            return earlyDate is DateTime && laterDate is DateTime && (DateTime)laterDate > (DateTime)earlyDate;
        }
    }
}
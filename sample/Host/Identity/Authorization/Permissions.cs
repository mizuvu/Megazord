using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Host.Identity.Authorization;

public class Permissions
{
    [DisplayName("System permissions")]
    public static class System
    {
        [Display(Name = "App identity", Description = "use this to access identity")]
        public const string Identity = $"{nameof(System)}.{nameof(Identity)}";

        [Display(Name = "Push notifications", Description = "use this to push notifications")]
        public const string Notification = $"{nameof(System)}.{nameof(Notification)}";

        [Display(Name = "Job Queue", Description = "Hangfire jobs management")]
        public const string JobQueue = $"{nameof(System)}.{nameof(JobQueue)}";
    }

    [DisplayName("Users")]
    [Description("Users permissions")]
    public static class Users
    {
        private const string _user = nameof(Users);

        [Display(Name = "View users list")]
        public const string View = $"{_user}.{nameof(View)}";
        public const string Create = $"{_user}.{nameof(Create)}";
        public const string Update = $"{_user}.{nameof(Update)}";
        public const string Delete = $"{_user}.{nameof(Delete)}";
    }

    [Description("Roles permissions")]
    public static class Roles
    {
        private const string _role = nameof(Roles);

        [Display(Description = "use this to view roles list")]
        public const string View = $"{_role}.{nameof(View)}";
        public const string Create = $"{_role}.{nameof(Create)}";
        public const string Update = $"{_role}.{nameof(Update)}";
        public const string Delete = $"{_role}.{nameof(Delete)}";
    }

    [Description("Organization permissions")]
    public static class Organization
    {
        public const string Base = nameof(Organization);

        public const string Company = $"{Base}.Company";
    }

    [Description("HR permissions")]
    public static class Hr
    {
        public const string Base = nameof(Hr);

        public const string ViewEmployee = $"{Base}.Employee.View";

        public const string UpdateEmployee = $"{Base}.Employee.Update";

        public const string Banker = $"{Base}.Employee.Banker";

        [Display(Description = "view / update balance for employee")]
        public const string EmployeeBalance = $"{Base}.Employee.Balance";

        [Display(Description = "reserved a booking for employee")]
        public const string BookEmployeeFoodcourt = $"{Base}.Employee.{nameof(BookEmployeeFoodcourt)}";
    }

    [Description("Retail permissions")]
    public static class Retail
    {
        public const string View = $"{nameof(Retail)}.{nameof(View)}";

        public const string MasterData = $"{nameof(Retail)}.{nameof(MasterData)}";

        public const string Foodcourt = $"{nameof(Retail)}.{nameof(Foodcourt)}";

        public const string ExportFoodcourtBooking = $"{nameof(Retail)}.{nameof(ExportFoodcourtBooking)}";
    }
}
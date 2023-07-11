using System.Collections.Generic;

namespace Zord.Core.ValueObjects
{
    public class Status : ValueObject
    {
        public Status() { }

        public Status(Enums.ActiveStatus status)
        {
            Value = status;
        }

        public Enums.ActiveStatus Value { get; set; } = Enums.ActiveStatus.active;

        public bool IsUnactive => Value == Enums.ActiveStatus.unactive;
        public bool IsActive => Value == Enums.ActiveStatus.active;
        public bool IsLocked => Value == Enums.ActiveStatus.locked;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Value;
            yield return IsUnactive;
            yield return IsActive;
            yield return IsLocked;
        }

        public void Update(Enums.ActiveStatus status)
        {
            // only update 2 status
            if (status == Enums.ActiveStatus.active || status == Enums.ActiveStatus.locked)
                Value = status;
        }
    }
}

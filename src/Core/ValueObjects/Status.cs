using System.Collections.Generic;
using Zord.Enums;

namespace Zord.ValueObjects
{
    public class Status : ValueObject
    {
        public Status() { }

        public Status(ActiveStatus status)
        {
            Value = status;
        }

        public ActiveStatus Value { get; set; } = ActiveStatus.active;

        public bool IsUnactive => Value == ActiveStatus.unactive;
        public bool IsActive => Value == ActiveStatus.active;
        public bool IsLocked => Value == ActiveStatus.locked;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Value;
            yield return IsUnactive;
            yield return IsActive;
            yield return IsLocked;
        }

        public void Update(ActiveStatus status)
        {
            // only update 2 status
            if (status == ActiveStatus.active || status == ActiveStatus.locked)
                Value = status;
        }
    }
}

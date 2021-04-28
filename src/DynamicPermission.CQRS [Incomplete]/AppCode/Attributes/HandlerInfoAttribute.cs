using System;

namespace DynamicPermission.CQRS.AppCode
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlerInfoAttribute : Attribute
    {
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string TabName { get; set; }

        public HandlerInfoAttribute(string name, string groupName = null, string tabName = null)
        {
            Name = name;
            GroupName = groupName;
            TabName = tabName;
        }
    }
}

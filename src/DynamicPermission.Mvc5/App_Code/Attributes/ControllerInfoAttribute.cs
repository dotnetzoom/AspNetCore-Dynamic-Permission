using System;

namespace DynamicPermission.Mvc5.App_Code
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ControllerInfoAttribute : Attribute
    {
        public string Name { get; }
        public string GroupName { get; }

        public ControllerInfoAttribute(string name)
        {
            GroupName = Name = name;
        }

        public ControllerInfoAttribute(string name, string groupName)
        {
            Name = name;
            GroupName = groupName;
        }
    }
}

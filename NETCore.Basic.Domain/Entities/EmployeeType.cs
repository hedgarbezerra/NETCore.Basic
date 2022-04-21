using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Entities
{
    public class Employee
    {
        private EmployeeType _type;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public EmployeeType Type { get => _type; set => _type = Enumeration.FromValue<EmployeeType>(value.Value); }
    }
    public class EmployeeType : Enumeration
    {
        public static readonly EmployeeType Manager = new ManagerType();
        public static readonly EmployeeType Common = new CommonType();
        public static readonly EmployeeType Director = new DirectorType();

        public EmployeeType() { }
        protected EmployeeType(int value, string displayName) : base(value, displayName) { }

        //Definir prop de acordo com a regra de negócio que deverá ser aplicada à cada tipo
        public virtual decimal BonusSize { get; }

        private class ManagerType : EmployeeType
        {
            public ManagerType() : base(0, "Manager") { }

            public override decimal BonusSize
            {
                get { return 1000m; }
            }
        }
        private class CommonType : EmployeeType
        {
            public CommonType() : base(1, "Common") { }

            public override decimal BonusSize
            {
                get { return 100m; }
            }
        }
        private class DirectorType : EmployeeType
        {
            public DirectorType() : base(2, "Director") { }

            public override decimal BonusSize
            {
                get { return 1500m; }
            }
        }
    }
}

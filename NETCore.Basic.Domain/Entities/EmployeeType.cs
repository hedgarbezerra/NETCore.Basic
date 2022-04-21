using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public EmployeeType Type { get; set; }
    }
    public class EmployeeType : Enumeration
    {
        public static readonly EmployeeType Manager
            = new ManagerType();

        protected EmployeeType() { }
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
            public CommonType() : base(0, "Common") { }

            public override decimal BonusSize
            {
                get { return 100m; }
            }
        }
        private class DirectorType : EmployeeType
        {
            public DirectorType() : base(0, "Director") { }

            public override decimal BonusSize
            {
                get { return 1500m; }
            }
        }
    }
}

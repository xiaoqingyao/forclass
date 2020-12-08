using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CoursePlatform.infrastructure.Validators
{
    public class IsNullVisitor : ExpressionVisitor
    {
        public bool IsNull { get; private set; }
        public object CurrentObject { get; set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            base.VisitMember(node);
            if (CheckNull())
            {
                return node;
            }

            var member = (PropertyInfo)node.Member;
            CurrentObject = member.GetValue(CurrentObject, null);
            CheckNull();
            return node;
        }

        private bool CheckNull()
        {
            if (CurrentObject == null)
            {
                IsNull = true;
            }
            return IsNull;
        }
    }

}

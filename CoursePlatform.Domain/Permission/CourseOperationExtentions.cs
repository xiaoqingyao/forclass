using AutoMapper;
using CoursePlatform.Domain.Core.CourseAggregate;
using CoursePlatform.infrastructure.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Permission
{
    public static class CourseOperationExtentions
    {
        public static async Task<T> SetOperationValAsync<T>(this T op, ValidateParameter param, IPermission pm, IMapper mapper) where T : CourseOperation
        {

            pm.SetValidateParameter(param);

            var scop = await pm.SchoolEnableOperationAsync();
            var items = new List<OperationVO>();

            if (scop.NoData() is false)
            {

                op.SchoolOperation = mapper.Map<IList<OperationVO>>(scop);
                items.AddRange(op.SchoolOperation);
            }

            var reop = await pm.RegionEnableOperationAsync();

            if (reop.NoData() is false)
            {

                op.RegionOperation = mapper.Map<IList<OperationVO>>(reop);
                items.AddRange(op.RegionOperation);
            }

            var peop = pm.PersonalEnableOperation();
            if (peop.NoData() is false)
            {
                op.PersonalOperation = mapper.Map<IList<OperationVO>>(peop);
                items.AddRange(op.PersonalOperation);

            }

            op.AllOperations = items.Count == 0 ? null : items;

            op.EnableEdit = pm.EnableEdit;

            op.IsCreator = pm.IsOwner;

            op.IsCollabrator = pm.IsCollabrator;
            

            return op;
        }
    }
}

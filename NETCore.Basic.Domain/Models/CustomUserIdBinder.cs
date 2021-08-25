using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Basic.Domain.Models
{
    public class CustomUserIdBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var data = bindingContext.HttpContext.Request.Query;
            var result = data.TryGetValue("ids", out StringValues ids);

            if (result)
            {
                var idList = ids.ToString().Split("|");
                bindingContext.Result = ModelBindingResult.Success(idList);
            }
            else bindingContext.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }
    }
}

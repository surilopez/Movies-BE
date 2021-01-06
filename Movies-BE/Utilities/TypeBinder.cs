using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Movies_BE.Utilities
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(propertyName);
            if (value == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var unserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(unserializedValue);
            }
            catch 
            {

                bindingContext.ModelState.TryAddModelError(propertyName, "Wrong Type of Value");
            }

            return Task.CompletedTask;
        }
    }
}

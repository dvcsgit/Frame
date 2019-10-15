using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFG.QisQualityControl.Web.Utils
{
    public static class ModelStateUtils
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary state)
        {
            return state.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
    }
}
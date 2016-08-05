using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eSignLive.Lottery.Configuration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RangeBallNumber : RangeAttribute
    {
        public RangeBallNumber(int from) : base(from, ConfigurationParameters.MinNumber) { }

        #region IClientValidatable Members

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rules = new ModelClientValidationRangeRule(this.ErrorMessage, this.Minimum, this.Maximum);
        //    yield return rules;
        //}

        #endregion
    }

    //        private double _MinValue, _MaxValue; // no readonly keyword

    //        public RangeBallNumber(double min, double max)        {
    //            _MinValue = min;
    //            _MaxValue = max;
    //        }

    //        public RangeBallNumber(double min, double max, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
    //{
    //            _MinValue = min;
    //            _MaxValue = max;
    //        }
}

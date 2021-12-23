using ExpenseCalculator.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseCalculator.Repos
{
    public static class ServerJobs
    {
        public static object JobTesting1()
        {
            var data = (dynamic)null;
            try
            {
                data = DataLayer.dataLayer.ExcuteNonQuery("[ExecuteJob1]", null);
            }
            catch (Exception ex)
            {
                data = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
            }
            return data;
        }
    }
}

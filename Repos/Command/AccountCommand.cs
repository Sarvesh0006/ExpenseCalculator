using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseCalculator.Repos.Command
{
    public class AccountCommand
    {
    }
    public class AccountLogin:IRequest<object>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class InsertExpence : IRequest<object>
    {
        public string Title { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
    }
}

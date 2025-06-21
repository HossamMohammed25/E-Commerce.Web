using Domain.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
     class OrderWithPaymentIntentIdSpecifications: BaseSpecification<Order,Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string PaymentIntentId):base(O=>O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}

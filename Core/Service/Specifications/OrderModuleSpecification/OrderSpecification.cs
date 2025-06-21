using Domain.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications.OrderModuleSpecification
{
    internal class OrderSpecification:BaseSpecification<Order,Guid>
    {
        //Get All Orders
        public OrderSpecification(string email):base(o=>o.buyerEmail == email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
            AddOrderByDes(O => O.OrderDate);
        }
        //Get Order by Id 
        public OrderSpecification(Guid id):base(o=>o.Id == id)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }
        
    }
}

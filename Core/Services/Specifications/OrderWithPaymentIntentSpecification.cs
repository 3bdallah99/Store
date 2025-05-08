using Domain.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
	public class OrderWithPaymentIntentSpecification :BaseSpecification<Order,Guid>
	{
        public OrderWithPaymentIntentSpecification(string paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)  
        {
            
        }
    }
}

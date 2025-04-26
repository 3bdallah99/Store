using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IServiceManger
    {
        public IProductService productService { get; }
        public IBasketService basketService { get;  }
        public ICashService cashService { get;  }
        public IAuthenticationService authenticationService { get;}


    }
}

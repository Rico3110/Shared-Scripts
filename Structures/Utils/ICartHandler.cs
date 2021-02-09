using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Structures
{
    public interface ICartHandler
    {
        List<Cart> Carts { get; set; }

        void HandleCarts();
    }
}

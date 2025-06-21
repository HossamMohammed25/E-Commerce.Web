using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class ProductNotFoundException(int id) : Exception($"Product With id = {id} is Not Found")
    {
    }
}

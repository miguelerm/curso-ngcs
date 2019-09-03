using System;
using System.Collections.Generic;
using System.Text;

namespace Abs.Messages.BooksCatalog.Queries
{
    public interface IGetBookByIdRequest
    {
        int Id { get; }
    }
}

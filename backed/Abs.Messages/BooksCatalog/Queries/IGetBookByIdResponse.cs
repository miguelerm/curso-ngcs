using System;
using System.Collections.Generic;
using System.Text;

namespace Abs.Messages.BooksCatalog.Queries
{
    public interface IGetBookByIdResponse
    {
        string Title { get; }
    }
}

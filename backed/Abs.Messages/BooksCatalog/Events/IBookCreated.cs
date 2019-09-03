using System;
using System.Collections.Generic;

namespace Abs.Messages.BooksCatalog.Events
{
    public interface IBookCreated
    {
        int Id { get; }
        IEnumerable<IBookCreatedAuthor> Authors { get; }
        IEnumerable<IBookCreatedCover> Covers { get; }
    }

    public interface IBookCreatedAuthor
    {
        int Id { get; }
    }

    public interface IBookCreatedCover
    {
        string Code { get; }
    }
}

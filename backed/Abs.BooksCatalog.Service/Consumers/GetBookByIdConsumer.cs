using Abs.BooksCatalog.Service.Data;
using Abs.Messages.BooksCatalog.Queries;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abs.BooksCatalog.Service.Consumers
{
    public class GetBookByIdConsumer : IConsumer<IGetBookByIdRequest>
    {
        private readonly BooksCatalogContext db;

        public GetBookByIdConsumer(BooksCatalogContext db)
        {
            this.db = db;
        }
        public async Task Consume(ConsumeContext<IGetBookByIdRequest> context)
        {
            var book = await db.Books.FirstOrDefaultAsync(x => x.Id == context.Message.Id, context.CancellationToken);
            await context.RespondAsync<IGetBookByIdResponse>(book);
        }
    }
}

# Commands

## Backend

```sh
dotnet new sln --name AwesomeBookstore
dotnet new webapi --name Abs.BooksCatalog.Service
dotnet sln .\AwesomeBookStore.sln add .\Abs.BooksCatalog.Service\Abs.BooksCatalog.Service.csproj
cd .\Abs.BooksCatalog.Service\
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet tool install --global dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet aspnet-codegenerator controller -api -async -name BooksController -m Book -dc BooksCatalogContext -namespace Abs.BooksCatalog.Service.Controllers --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Frontend

```sh
tsc --init
```

```sh
ng new awesome-bookstore --directory . --routing --prefix abs --style css
```

## Request

```http
POST https://localhost:5001/api/books

{
    "title": "The Time Machine",
    "description": "So begins the Time Traveller’s astonishing firsthand account of his journey 800,000 years beyond his own era—and the story that launched H.G. Wells’s successful career and earned him his reputation as the father of science fiction. With a speculative leap that still fires the imagination, Wells sends his brave explorer to face a future burdened with our greatest hopes...and our darkest fears. A pull of the Time Machine’s lever propels him to the age of a slowly dying Earth.  There he discovers two bizarre races—the ethereal Eloi and the subterranean Morlocks—who not only symbolize the duality of human nature, but offer a terrifying portrait of the men of tomorrow as well.  Published in 1895, this masterpiece of invention captivated readers on the threshold of a new century. Thanks to Wells’s expert storytelling and provocative insight, The Time Machine will continue to enthrall readers for generations to come.",
    "publishedOn": "1895-05-07",
    "authors": [
        {
            "name": "H.G. Wells"
        }
    ]
}
```

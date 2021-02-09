using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace library_test_task.Data.Migrations.Custom
{
    /// <summary>
    /// Создать check constraint
    /// </summary>
    [DbContext(typeof(ApplicationContext))]
    [Migration("BookStorageCheckConstraint")]
    public class BookStorageCheckConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // У Entity Framework в SQLite есть некоторые ограничения, в частности нельзя создавать codefirst Constraint'ы
            // Также в SQLite нельзя модифицировать колонку, поэтому приходится создавать новую таблицу, а перед этим удалять старую
            migrationBuilder.Sql(@"
DROP TABLE ""BookStorage"";
CREATE TABLE ""BookStorage"" (
    ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    ""BookId""	INTEGER NOT NULL,
    ""Total""	INTEGER NOT NULL,
    ""Free""	INTEGER NOT NULL CONSTRAINT ""FreeBookCheckConstraint"" CHECK (""Free"" >= 0 AND ""Free"" <= ""Total""),
    CONSTRAINT ""FK_BookStorage_Books_BookId"" FOREIGN KEY(""BookId"") REFERENCES ""Books""(""Id"") ON DELETE CASCADE);");
        }
    }
}
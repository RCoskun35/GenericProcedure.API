using GenericProcedure.DATA;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer("YourConnectionString"));

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    if (dbContext.Products.FromSqlRaw($"SELECT ISNULL(OBJECT_ID('dbo.sp_resultProcedure'),0) Id,'asd' Name").FirstOrDefault().Id < 1)
    {
        dbContext.Database.ExecuteSqlRaw(@"create PROCEDURE [dbo].[sp_resultProcedure]
	-- Add the parameters for the stored procedure here
	(
		@response nvarchar(max) out,
		@Id INT,
		@Name NVARCHAR(MAX)
	)
AS
BEGIN
	DECLARE @count INT = 0

	WHILE(@count<5)
	BEGIN
	INSERT INTO Products(Name)
	SELECT @Name + CONVERT(NVARCHAR(MAX),@count)
	
	SET @count = @count+1
	END


	SET @response=(SELECT Id,Name,DATEADD(MINUTE,Id,GETDATE()) Date,Name + CONVERT(NVARCHAR(MAX),Id) Description FROM Products WHERE Id>@Id FOR JSON PATH)
END");
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

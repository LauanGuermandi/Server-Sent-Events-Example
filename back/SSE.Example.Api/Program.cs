var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option => option.AllowAnyOrigin());
app.UseHttpsRedirection();

app.MapGet("/events", async (HttpResponse response) =>
{
    response.Headers["Content-Type"] = "text/event-stream";

    for (var i = 0; i < 10; i++)
    {
        await response.WriteAsync($"data: Message {i + 1}\n\n");
        await response.Body.FlushAsync();
        await Task.Delay(1000);
    }
})
.WithName("SSE")
.WithOpenApi();

app.Run();

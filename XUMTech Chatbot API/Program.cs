
using Chatbot.Tools.Classes;
using Chatbot.Tools.Interfaces;
using ChatBot.Controller.Classes;
using ChatBot.Controller.Interfaces;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200") 
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IMyHttpClient, MyHttpClient>();
builder.Services.AddSingleton<IMyHttpClient, MyHttpClient>();

ConfigureDependencyInjection(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureDependencyInjection(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<IQuestionLogic>(provider => new QuestionLogic(provider.GetRequiredService<IConfiguration>(), provider.GetRequiredService<IMyHttpClient>()));
}
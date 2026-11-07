using BusinessLogic.Mapper;
using eCommerce.ProductsMicroservice.Api.MiddleWare;
using eCommerce.ProductsMicroservice.BusinessLogic;
using eCommerce.ProductsMicroservice.DAL;
using FluentValidation.AspNetCore;
using ProductsMicroservice.Api.ProductApi;


var builder = WebApplication.CreateBuilder(args);

//API
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

//builder.Services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();


//controllers
builder.Services.AddControllers();
//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//AutoMapper

builder.Services.AddAutoMapper(cfg => { },
    typeof(ProductAddRequestToProductProfile).Assembly);


//FluentValidation
builder.Services.AddFluentValidationAutoValidation();
//Middleware
var app = builder.Build();

app.UseExceptionHandlerMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapProductApiEndPoints();
app.MapControllers();

app.Run();

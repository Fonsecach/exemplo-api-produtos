using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using produtos.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppContextData>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/categorias", async ([FromServices] AppContextData contextCategorias) =>
{
    var categorias = await contextCategorias.Categorias.ToListAsync();

    if (categorias.Any()){
        return Results.Ok(categorias);
    }
    return Results.NotFound("Nenhuma categoria encontrada");
})
.WithName("GetCategorias")
.WithOpenApi();

app.MapGet("/produtos", async ([FromServices] AppContextData contextProdutos) =>
{
    var produtos = await contextProdutos.Produtos.ToListAsync();

    if (produtos.Any()){
        return Results.Ok(produtos);
    }
    return Results.NotFound("Nenhuma produto encontrada");
})
.WithName("GetProdutos")
.WithOpenApi();

app.MapPost("/categorias/cadastrar", async ([FromBody] Categoria categoria, [FromServices] AppContextData contextCategorias) =>
{
    if (categoria == null){
        return Results.BadRequest("Categoria não informada");
    }
    contextCategorias.Categorias.Add(categoria);
    await contextCategorias.SaveChangesAsync();
    return Results.Ok(categoria);

}).WithName("CadastrarCategoria")
.WithOpenApi();

app.MapPost("/produtos/cadastrar", async ([FromBody] Produto produto, [FromServices] AppContextData contextProdutos) => {
    if (produto == null){
        return Results.BadRequest("Produto não informado");
    }
    contextProdutos.Produtos.Add(produto);
    await contextProdutos.SaveChangesAsync();
    return Results.Ok(produto);
}).WithName("CadastrarProduto")
.WithOpenApi();

app.MapPut("/produtos/{id}", async ([FromRoute] int id, [FromBody] Produto produto, [FromServices] AppContextData contextProdutos) => {
    if (produto == null){
        return Results.BadRequest("Produto não informado");
    }
    var produtoAtual = await contextProdutos.Produtos.FindAsync(id);
    if (produtoAtual == null){
        return Results.NotFound("Produto não encontrado");
    }
    produtoAtual.Nome = produto.Nome;
    produtoAtual.Preco = produto.Preco;
    produtoAtual.CategoriaId = produto.CategoriaId;

    await contextProdutos.SaveChangesAsync();
    return Results.Ok(produtoAtual);
}).WithName("AtualizarProduto").WithOpenApi();

app.MapDelete("/produtos/{id}", async ([FromRoute] int id, [FromServices] AppContextData contextProdutos) => {
    var produtoAtual = await contextProdutos.Produtos.FindAsync(id);
    if (produtoAtual == null){
        return Results.NotFound("Produto não encontrado");
    }
    contextProdutos.Produtos.Remove(produtoAtual);
    await contextProdutos.SaveChangesAsync();
    return Results.Ok();
}).WithName("DeletarProduto").WithOpenApi();

app.Run();


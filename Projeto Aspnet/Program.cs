using System.Text.Json;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<Carro> carros = new List<Carro>(); 

Carro carro1 = new Carro("BYD HOLY", "BYD", true, 1, false);
Carro carro2 = new Carro("BYD HOLY", "BYD", true, 2, true);

carros.Add(carro1);
carros.Add(carro2);


app.MapGet("/", CadastroCarro);
app.MapGet("/carros", GetCarros);
app.MapGet("/carro/{bancos}", GetCarro);
app.MapPost("/carro", inserirCarro);
app.MapDelete ("/carro/{bancos}", DeletarCarro);
app.MapPut ("/carro", AtualizarCarro);


app.MapGet("/clientes", ListarClientes);

void ListarClientes(){
    try{
    string stringConexao = "server=localhost;uid=root;pwd=1234;database=Mysql";
    MySqlConnection conexao = new MySqlConnection(stringConexao);  
    conexao.Open(); 
    
    MySqlCommand comando = new MySqlCommand();
    comando.Connection = conexao;
    comando.CommandText = "SELECT * FROM sakila.clientes";
    MySqlDataReader resultado = comando.ExecuteReader();
    while(resultado.Read()){
        int id = resultado.GetInt32("id");
        string nome = resultado.GetString("nome");
        string senha = resultado.GetString("senha");
        string email = resultado.GetString("email");
        string usuario = resultado.GetString("usuario");

        Console.WriteLine($"{id} - {nome} - {senha} - {email} - {usuario}");   
    }
    } catch (MySqlException ex){
        Console.WriteLine(ex);

    }
}



IResult AtualizarCarro(Carro carro){
for (int i = 0; i < carros.Count; i++)
{
    if (carros[i].Id == carros[i].Id){
        carros[i] = carro;
        return TypedResults.NoContent();
    }  
}
return TypedResults.NotFound();
}


IResult GetCarros()
{
    return TypedResults.Ok(carros);
}

IResult GetCarro(int Id){
    foreach (Carro carro in carros)
    {
        if(carro.Id == Id){
        return TypedResults.Ok(carro);
        }
    }
    return TypedResults.NotFound();
}

IResult DeletarCarro(int bancos){
for (int i = 0; i < carros.Count; i++)
{
    if(carros[i].Id == bancos){
        carros.RemoveAt(i);
        return TypedResults.NoContent();
    }
}
    return TypedResults.NoContent();
}

IResult inserirCarro(Carro carro){
    carros.Add(carro);
    return TypedResults.Created("/carro" , carro);     
}


void CadastroCarro(){
    JsonSerializerOptions options = new JsonSerializerOptions();
    options.WriteIndented = true;   
    string json = JsonSerializer.Serialize(carros);
    string caminho = $"{Environment.CurrentDirectory}\\json\\carros.json";
    File.WriteAllText(caminho, json);    
}



app.Run();

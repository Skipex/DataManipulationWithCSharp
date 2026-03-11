
using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);




void OperacoesDeAgrupamento(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
                       .GroupBy(m => m.Artista);

    foreach (var artista in artistas.Take(5))
    {
        System.Console.WriteLine($"\nMúsicas de {artista.Key}:\n");
        foreach (var musica in artista)
        {
            System.Console.WriteLine($"\t - {musica.Titulo} ({musica.Duracao} segundos)");
        }
    }
}

void EstatisticasDeMusicas(StreamReader stream) // Operações de agregação
{
    var musicas = ObterMusicas(stream).ToList();

    System.Console.WriteLine($"Existem {musicas.Count()} músicas na coleção");
    System.Console.WriteLine($"Existes {musicas.Count(m => m.Duracao >= 600)} músicas com mais de 10 minutos na coleção");
    System.Console.WriteLine($"A música com menor duração é de {musicas.Min(m => m.Duracao)} segundos");
    System.Console.WriteLine($"A música com maior duração é de {musicas.Max(m => m.Duracao)} segundos");
    System.Console.WriteLine($"A duração média das músicas é: {musicas.Average(m => m.Duracao)} segundos");
    System.Console.WriteLine($"Você vai levar {musicas.Sum(m => m.Duracao) / (3600 * 24)} dias para ouvir toda a coleção");
}

void OperacoesDeProjecao2(StreamReader stream)
{
    var generos = ObterMusicas(stream)
                    .SelectMany(m => m.Generos) // projeção e 'achata' a coleção de gêneros
                    .Distinct() 
                    .OrderBy(g => g);

    foreach (var genero in generos)
    {
        System.Console.WriteLine(genero);
    }
}


void OperacoesDeProjecao(StreamReader stream)
{
    var artistas = ObterMusicas(stream)
        .Select(m => m.Artista) // projeção/transformação
        .Distinct() // filtragem
        .OrderBy(a => a); // ordenação

    foreach (var artista in artistas)
    {
        System.Console.WriteLine(artista);
    }
}

void OperacoesDeFiltroEOrdenacao(StreamReader stream)
{
    var musicas = 
        ObterMusicas(stream) 
        .Where( m => m.Artista == "Metallica")
        .OrderBy( m => m.Titulo)
        //.OrderByDescending( m => m.Titulo)
        .ThenBy( m => m.Duracao)
        //.ThenByDescending( m => m.Duracao)
        .Skip(5 * 3) // pula os primeiros 5 resultados
        .Take(5) // pega os primeiros 5 resultados
        ;


    ExibirMusicas(musicas);
}


void ExibirMusicas(IEnumerable<Musica> musicas)
{
    System.Console.WriteLine("Exibindo a lista de músicas:");

    var contador = 0;
    foreach (var musica in musicas)
    {
        contador++;
        if (contador > 10) break;

        System.Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao} segundos");
        
    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    while (linha is not null)
    {
        var partes = linha.Split(';');
        var musica = new Musica
        {
            Titulo = partes[0],
            Artista = partes[1],
            Duracao = int.Parse(partes[2]),
            Generos = partes[3].Split(',').Select(g => g.Trim())
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
}
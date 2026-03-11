/*
    Seja um arquivo com músicas em formato CSV (Comma Separated Values). 

    Implemente as funções abaixo:
    //     [x] Leia-o como uma coleção de músicas
    //     [x] Filtre a coleção por artista (por ex. Coldplay, Metallica, AC/DC)
    //     [x] Filtre a coleção por duração (por ex. maiores que 5 minutos)
    //     [x] Ordene a coleção por artista
    //     [x] Ordene a coleção por artista e em seguida por músicas com duração crescente
    //     [x] Recupere as 10 músicas mais longas
    //     [x] Crie uma coleção de artistas
    //     [x] Crie uma coleção de gêneros
    //     [x] Informe a duração média das músicas da coleção
    //     [x] Informe a duração total das músicas da coleção
    //     [x] Crie uma coleção de artistas e suas músicas
    //     [x] Informe qual artista tem mais músicas na coleção
    //     [x] Filtre a coleção por gênero (por ex. rock)
 
*/

/*
 
  Fluxo Padrão: Estágio 1 (Origem Dados) > Estágio 2 > ... > Estágio N

  LINQ - Categorias de operações para manipulação de coleções 
  ========================================================================================
  | Filtro (+)      | coleção c/ tam menor/igual atendendo condição | Where, Distinct    |
  | Projeção (+)    | coleção transformada, do mesmo tipo ou não    | Select, SelectMany |
  | Ordenação (*)   | coleção ordenada pela expressão lambda        | OrderBy, ThenBy    |
  | Agregação (*)   | valor único a partir de operação de acúmulo   | Sum, Min, Max      |
  | Agrupamento (+) | coleção de grupos onde a chave é o argumento  | GroupBy            |
  | Elementos (*)   | elemento único T a partir do argumento        | First, Last, MinBy |
  | Existência (*)  | booleano a partir da operação e argumento     | All, Any, Contains |
  | Conversão (*)   | coleção em outra estrutura                    | ToList, ToArray    |
  ========================================================================================

    + operações avaliadas sob demanda (yield)
    * operações avaliadas imediatamente
*/



using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);



void OperacoesDeVerificacaoDeExistencia(StreamReader stream)
{
    var musicas = ObterMusicas(stream).ToList();

    var artistas = musicas
                    .GroupBy(m => m.Artista)
                    .Where(g => g.Any(m => m.Duracao >= 480));

    foreach (var artista in artistas.Take(1))
    {
        System.Console.WriteLine($"O artista {artista.Key} tem pelo menos uma música com 480 segundos");
        int i = 0;
        foreach (var musica in artista.OrderBy(m => m.Duracao))
            System.Console.WriteLine($"\t {++i} - {musica.Titulo} ({musica.Duracao} segundos)");
    
    }

    var reggae = musicas
                    .GroupBy(m => m.Artista)
                    .Where(g => g.Any(m => m.Generos.Contains("Reggae")));
    if(reggae is not null)
    {
        System.Console.WriteLine("\nArtistas que possuem pelo menos uma música do gênero Reggae:");
        foreach (var artista in reggae)
        {
            System.Console.WriteLine($"\t - {artista.Key}");
        }    
    }
    
}


void ArtistaComMaiorQuantidadeDeMusicas(StreamReader stream)
{
    var artistaComMaiorQuantidadeDeMusicas = ObterMusicas(stream)
                                                .GroupBy(m => m.Artista)
                                                .Select(g => new { Artista = g.Key, TotalMusicas = g.Count(), Musicas = g })
                                                .MaxBy(g => g.TotalMusicas);
    if(artistaComMaiorQuantidadeDeMusicas is not null)
    {
        System.Console.WriteLine($"O artista com maior quantidade de músicas é: {artistaComMaiorQuantidadeDeMusicas.Artista} com {artistaComMaiorQuantidadeDeMusicas.TotalMusicas} músicas");

        if(artistaComMaiorQuantidadeDeMusicas.Musicas.Count() > 0)
        {
            System.Console.WriteLine($"As músicas do {artistaComMaiorQuantidadeDeMusicas.Artista} são:");
            int i = 0;
            foreach (var musica in artistaComMaiorQuantidadeDeMusicas.Musicas.OrderBy(m => m.Titulo))
                System.Console.WriteLine($"\t {++i} - {musica.Titulo}");
        }
    }
}    

void OperacoesDeObtencaoDeElementos(StreamReader stream)
{
    var musica = ObterMusicas(stream).ToList();

    var primeiraMusica = musica.First();
    System.Console.WriteLine($"A primeira música é: {primeiraMusica.Titulo}");

    var maiorDuracao = musica.MaxBy(m => m.Duracao);
    System.Console.WriteLine($"A música com maior duração é: {maiorDuracao.Titulo} ({maiorDuracao.Duracao} segundos)");
}


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
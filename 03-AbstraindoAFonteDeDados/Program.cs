
using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);


var musicasColdplay = 
        ObterMusicas(stream) // 1. Obter os dados da fonte (arquivo)
        //.FiltrarPor(FiltrarPorMetalica) // 2. Filtrar os dados (artista)
        //.FiltrarPor( m => m.Artista == "Metallica")
        .Where( m => m.Artista == "Metallica")
        //.FiltrarPor(FiltrarPorDuracao) // 3. Filtrar os dados (duração)
        //.FiltrarPor( m => m.Duracao >= 400);
        .Where( m => m.Duracao >= 400);


ExibirMusicas(musicasColdplay);


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
            Duracao = int.Parse(partes[2])
        };
        yield return musica;
        linha = stream.ReadLine();
    }
}

// delegate = tipos que representam métodos com a mesma assinatura
// Func<Musica, bool> condicao = FiltrarPorArtista;

static class MusicasExtensions
{
    public static IEnumerable<T> FiltrarPor<T>(this IEnumerable<T> colecao, Func<T, bool> condicao)
    {
        foreach (var elemento in colecao)
            if (condicao(elemento)) yield return elemento;
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
}
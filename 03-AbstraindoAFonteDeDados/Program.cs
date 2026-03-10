
using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);


var musicasColdplay = 
        ObterMusicas(stream).
        FiltrarMusicasPor("Adele");

ExibirMusicas(musicasColdplay);


void ExibirMusicas(IEnumerable<Musica> musicas)
{
    System.Console.WriteLine("Exibindo a lista de músicas:");

    var contador = 0;
    foreach (var musica in musicas)
    {
        contador++;
        if (contador > 10) break;

        System.Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista})");
        
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

static class MusicasExtensions
{
    public static IEnumerable<Musica> FiltrarMusicasPor(this IEnumerable<Musica> musicas, string artista)
    {
        foreach (var musica in musicas)
            if (musica.Artista == artista) yield return musica;
    }
}


class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
}
using System.Text.RegularExpressions;
using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicas = ObterMusicas(stream)
                .Take(20);
ExibirMusicasEmTabela(musicas);

void ExibirMusicasEmTabela(IEnumerable<Musica> musicas)
{
    System.Console.WriteLine("Exibindo a lista de músicas:");

    var colunaTitulo = "Título".PadRight(40);
    var colunaArtista = "Artista".PadRight(35);
    var colunaDuracao = "Duração".PadRight(10);
    var colunaLancamento = "Lançada em".PadRight(12);

    System.Console.WriteLine($"{colunaTitulo} | {colunaArtista} | {colunaDuracao} | {colunaLancamento}");
    System.Console.WriteLine("".PadLeft(100, '='));


    var contador = 0;
    foreach (var musica in musicas)
    {
        var duracao = string.Format("{0, -11:F3}", musica.Duracao / 60.0);
        System.Console.WriteLine($"{musica.Titulo,-41} {musica.Artista,-36}  {duracao} {musica.Lancamento,-16: dd/MM/yyyy}");

    }
}

void ExibirMusicas(IEnumerable<Musica> musicas)
{
    System.Console.WriteLine("Exibindo a lista de músicas:");

    var contador = 0;
    foreach (var musica in musicas)
    {
        contador++;
        if (contador > 10) break;

        System.Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao}s [{musica.Lancamento:dd/MM/yyyy}]");

    }
}

IEnumerable<Musica> ObterMusicas(StreamReader stream)
{
    var linha = stream.ReadLine();
    int duracao = 0;

    while (linha is not null)
    {
        var match = Regex.Match(linha, @"(\d?\d):(\d\d)");
        if (match.Success)
        {
            var minutos = int.Parse(match.Groups[1].Value);
            var segundos = int.Parse(match.Groups[2].Value);
            duracao = (minutos * 60) + segundos;
        }

        var partes = linha.Split(';');

        if (partes.Length == 5)
        {
            var musica = new Musica
            {
                Titulo = string.IsNullOrWhiteSpace(partes[0]) ? "Título não definido" : partes[0],
                Artista = string.IsNullOrWhiteSpace(partes[1]) ? "Artista não definido" : partes[1],
                Duracao = duracao,
                Generos = partes[3].Split(',', StringSplitOptions.TrimEntries),
                Lancamento = DateTime.TryParse(partes[4], out var data) ? data : DateTime.Today
            };
            yield return musica;
        }
        linha = stream.ReadLine();
    }
}

class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }
    public IEnumerable<string> Generos { get; set; }
    public DateTime Lancamento { get; set; }

    public override string ToString()
    {
        return $"{Titulo} ({Artista}) - {Duracao}s [{Lancamento:dd/MM/yyyy}]";
    }
}
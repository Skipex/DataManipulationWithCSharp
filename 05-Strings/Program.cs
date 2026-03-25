using var arquivo = new FileStream("musicas.csv", FileMode.Open, FileAccess.Read);
using var stream = new StreamReader(arquivo);

var musicas = ObterMusicas(stream)
                .Take(20);
ExibirMusicasEmTabela(musicas);

void ValidacaoDeStrings()
{
    var artista1 = "Coldplay"; // interning - somente quando usa string literal
    var artista2 = "Coldplay";
    var artista3 = new string("Coldplay"); // não faz interning pois não é uma string literal
    var artista4 = "COLDPLAY"; // é uma string literal mas é diferente da 1 e 2
    var artista5 = artista1.ToUpper(); // Vai gravar no HEAP criando uma nova referência
    var artista6 = string.Intern(artista1.ToUpper()); // leva a referência da HEAP para a string pool

    System.Console.WriteLine(artista1 == artista2); // True pois sobrescreve o método equals
    System.Console.WriteLine(ReferenceEquals(artista1, artista2)); // True pois pega a mesma referência do pool de string
    System.Console.WriteLine(ReferenceEquals(artista1, artista3)); // False - pois o artista3 não é uma string literal
    System.Console.WriteLine(ReferenceEquals(artista4, artista6));
}

void ComparandoTitulo(StreamReader stream)
{
    var musicas = ObterMusicas(stream)
                    .Where(m => m.Artista.Equals("CoLdPlAy", StringComparison.OrdinalIgnoreCase))
                    .Take(20);

    ExibirMusicasEmTabela(musicas);
    
}

void AlterandoTituloMusica()
{
    var musica = ObterMusicas(stream)
                    .Where(m => m.Titulo.StartsWith('T'))
                    .FirstOrDefault();

    if(musica is not null)
    {
        System.Console.WriteLine($"A música é: {musica.Titulo} - {musica.Artista}");
        musica.Titulo = musica.Titulo.Replace("The ", "");
        System.Console.WriteLine($"A música é: {musica.Titulo} - {musica.Artista}");
    }
}

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
        var duracao = string.Format("{0, -11:F3}", musica.Duracao/60.0);
        System.Console.WriteLine($"{musica.Titulo, -41} {musica.Artista, -36}  {duracao} {musica.Lancamento, -16: dd/MM/yyyy}");
        
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
    while (linha is not null)
    {
        var partes = linha.Split(';');

        if(partes.Length == 5)
        {
            var musica = new Musica
            {
                Titulo = string.IsNullOrWhiteSpace(partes[0]) ? "Título não definido" : partes[0],
                Artista = string.IsNullOrWhiteSpace(partes[1]) ? "Artista não definido" : partes[1],
                Duracao = int.TryParse(partes[2], out int duracao) ? duracao : 0,
                Generos = partes[3].Split(',', StringSplitOptions.TrimEntries),
                Lancamento = DateTime.TryParse(partes[4], out var data) ? data : DateTime.Today
            };
            yield return musica;
        }
        linha = stream.ReadLine();
    }
}

void ValidaSenha(string senha = "123456!sA")
{
    var totalCaracteres = senha.Length;
    var totalMaiusculas = senha.Count(c => char.IsUpper(c));
    var totalMinusculas = senha.Count(c => char.IsLower(c));
    var totalDigitos = senha.Count(c => char.IsDigit(c));
    var totalSimbolos = senha.Count(c => !char.IsLetterOrDigit(c));

    if ( totalCaracteres < 8 ||
            totalMaiusculas == 0 ||
            totalMinusculas == 0 ||
            totalDigitos == 0 ||
            totalSimbolos == 0 )
    {
        System.Console.WriteLine("A senha é fraca.");
    } else
    {
        System.Console.WriteLine("A senha é forte.");
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
/*
   Seja um aplicativo de gerenciamento de músicas onde os usuários podem organizar suas faixas favoritas em playlists personalizadas. Para cada playlist, é essencial que o usuário tenha controle total sobre a sequência de reprodução das músicas, permitindo reordená-las livremente a qualquer momento. Além disso, o aplicativo precisa oferecer a funcionalidade de reprodução aleatória para uma playlist específica, proporcionando uma experiência de audição dinâmica e variada, sem, contudo, alterar a ordem original que o usuário definiu. O desafio é criar uma estrutura robusta que suporte a adição e remoção eficiente de músicas, a reordenação flexível dentro das playlists e a seleção de faixas tanto em modo sequencial quanto aleatório.

   Funções que vamos implementar:
   //     [x] Criar as classes para musicas e playlist
   //     [x] Listar músicas da playlist
   //     [x] Adicionar música à playlist
   //     [x] Obter uma música específica da playlist
   //     [x] Remover música da playlist
   //     [x] Tocar uma música aleatória da playlist
   //     [x] Reordenar músicas segundo alguma lógica específica (ex. duração)
   //     [x] Uma playlist não pode ter músicas repetidas
   //     [x] Exibir as 3 músicas mais tocadas em todas as playlists (ranking)
   //     [ ] Player de música com:
   //     [ ] - Fila de reprodução (para músicas avulsas e/ou playlists)
   //     [ ] - Histórico de reprodução

*/

using System.Collections;

var musica1 = new Musica { Titulo = "Que País é Esse?", Artista = "Legião Urbana", Duracao = 350 };
var musica2 = new Musica { Titulo = "Tempo Perdido", Artista = "Legião Urbana", Duracao = 455 };
var musica3 = new Musica { Titulo = "Pro Dia Nascer Feliz", Artista = "Barão Vermelho", Duracao = 345 };
var musica4 = new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 530 };
var musica5 = new Musica { Titulo = "Geração Coca-Cola", Artista = "Legião Urbana", Duracao = 380 };

var rockNacional = new Playlist { Nome = "Rock Nacional" };
rockNacional.Add(musica2);
rockNacional.Add(musica1);
rockNacional.Add(musica3);
rockNacional.Add(musica4);
rockNacional.Add(musica5);
rockNacional.Add(new Musica { Titulo = "Eduardo e Mônica", Artista = "Legião Urbana", Duracao = 530 });

//ExibirPlaylist(rockNacional);

var legiaoUrbana = new Playlist() { Nome = "Mais populares da Legião" };
legiaoUrbana.Add(musica1);
legiaoUrbana.Add(musica2);
legiaoUrbana.Add(musica4);
legiaoUrbana.Add(musica5);

//ExibirPlaylist(legiaoUrbana);

var player = new PlayerDeMusica();
player.AdicionarNaFila(musica1);
player.AdicionarNaFila(rockNacional);

ExibirFila(player);
ExibirHistorico(player);

var proxima = player.ProximaMusicaDaFila();
if (proxima is not null)
    Console.WriteLine($"\nTocando a música {proxima.Titulo}...");
else Console.WriteLine("\nFila de reprodução vazia!");

ExibirFila(player);
ExibirHistorico(player);

var anterior = player.MusicaAnterior();
if (anterior is not null)
    Console.WriteLine($"\nTocando a música {anterior.Titulo}...");
else Console.WriteLine("\nHistórico de reprodução vazio!");

ExibirFila(player);
ExibirHistorico(player);

void ExibirHistorico(PlayerDeMusica player)
{
    Console.WriteLine($"\nExibindo o histórico:");
    foreach(var musica in player.Historico())
    {
        Console.WriteLine($"\t - {musica.Titulo}");
    }
}

void ExibirFila(PlayerDeMusica player)
{
    Console.WriteLine($"\nExibindo a fila de reprodução:");
    foreach (var musica in player.Fila())
    {
        Console.WriteLine($"\t - {musica.Titulo}");
    }
}


void ExibirMaisTocadas(Playlist playlist1, Playlist playlist2)
{
    // Musica (chave/key), Contagem (valor/value)
    Dictionary<Musica, int> ranking = [];

    foreach(var musica in playlist1)
    {
        ranking.Add(musica, 1);
    }

    foreach(var musica in playlist2)
    {
        if (ranking.TryGetValue(musica, out int contagem))
        {
            contagem++;
            ranking[musica] = contagem;
        } else
        {
            ranking[musica] = 1;
        }
    }

    List<KeyValuePair<Musica, int>> top = new(ranking); //[..ranking];
    top.Sort(new PorContagem());

    Console.WriteLine("\nTop 3 músicas mais incluídas nas playlists:");
    int contador = 1;
    foreach(var par in top)
    {
        Console.WriteLine($"\t - {par.Key.Titulo}");
        contador++;
        if (contador > 3) break;
    }
}


void ExibirPlaylist(Playlist playlist)
{
    Console.WriteLine($"\n Tocando as músicas de {playlist.Nome}");
    foreach (var musica in playlist)
    {
        Console.WriteLine($"\t - {musica.Titulo} ({musica.Artista}) - {musica.Duracao} segundos");
    }
}

void RemoverMusicaPeloTitulo(Playlist playlist, string titulo)
{
    var musicaEncontrada = playlist.ObterPeloTitulo(titulo);
    if (musicaEncontrada is not null)
    {
        Console.WriteLine("\nRemovendo música...");
        rockNacional.Remove(musicaEncontrada);
    }
    else
    {
        Console.WriteLine("\nMúsica não encontrada!");
    }

    ExibirPlaylist(rockNacional);
}

void ExibirMusicaAleatoria(Playlist playlist)
{
    var musicaAleatoria = playlist.ObterAleatoria();
    if (musicaAleatoria is not null)
    {
        Console.WriteLine($"\nA música aleatória é {musicaAleatoria.Titulo}");
    }
    else
    {
        Console.WriteLine("Playlist vazia!");
    }
}

class PorContagem : IComparer<KeyValuePair<Musica, int>>
{
    public int Compare(KeyValuePair<Musica, int> x, KeyValuePair<Musica, int> y)
    {
        return y.Value.CompareTo(x.Value);
    }
}

class PorArtista : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Artista.CompareTo(y.Artista);
    }
}

class PorTitulo : IComparer<Musica>
{
    public int Compare(Musica? x, Musica? y)
    {
        if (x is null || y is null) return 0;
        if (x is null) return 1;
        if (y is null) return -1;
        return x.Titulo.CompareTo(y.Titulo);
    }
}

class Musica : IComparable
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public int Duracao { get; set; }

    public int CompareTo(object? other) // iguais: 0; menor: -1; maior: 1
    {
        if (other is null) return -1;
        if (other is Musica outraMusica) return this.Duracao.CompareTo(outraMusica.Duracao);
        return -1;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj is Musica outraMusica) 
            return this.Titulo.Equals(outraMusica.Titulo) && 
                this.Artista.Equals(outraMusica.Artista);
        return false;
    }

    public override int GetHashCode()
    {
        return this.Titulo.GetHashCode() ^ this.Artista.GetHashCode();
    }
}

class Playlist : ICollection<Musica>
{
    private HashSet<Musica> set = [];
    private List<Musica> lista = [];
    public string Nome { get; set; }

    public int Count => lista.Count;

    public bool IsReadOnly => false;

    public void Add(Musica musica)
    {
        if (set.Add(musica))
        {
            lista.Add(musica);
        }
    }

    public void Clear()
    {
        lista.Clear();
    }

    public bool Contains(Musica item)
    {
        return lista.Contains(item);
    }

    public Musica? ObterPeloTitulo(string titulo)
    {
        foreach(var musica in lista)
        {
            if (musica.Titulo == titulo) return musica;
        }
        return null;
    }

    public Musica? ObterAleatoria()
    {
        if (lista.Count == 0) return null;
        var random = new Random();
        var indiceAleatorio = random.Next(0, lista.Count - 1);
        return lista[indiceAleatorio];
    }

    public void OrdenarPorDuracao()
    {
        lista.Sort(); // duração? IComparable
    }

    public void OrdenarPorArtista()
    {
        lista.Sort(new PorArtista());
    }

    public void CopyTo(Musica[] array, int arrayIndex)
    {
        lista.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Musica> GetEnumerator()
    {
        return lista.GetEnumerator();
    }

    public bool Remove(Musica item)
    {
        return lista.Remove(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class PlayerDeMusica
{
    private Queue<Musica> fila = [];   // primeiro a entrar, primeiro a sair (FIFO)
    private Stack<Musica> pilha = [];  // último a entrar, primeiro a sair   (LIFO)
    public void AdicionarNaFila(Musica musica)
    {
        fila.Enqueue(musica);
    }

    public void AdicionarNaFila(Playlist playlist)
    {
        foreach (var musica in playlist)
            AdicionarNaFila(musica);
    }

    public Musica? ProximaMusicaDaFila()
    {
        if (fila.Count == 0) return null;
        var musica = fila.Dequeue();
        pilha.Push(musica);
        return musica;
    }

    public Musica? MusicaAnterior()
    {
        if (pilha.Count == 0) return null;
        return pilha.Pop();
    }

    public IEnumerable<Musica> Fila()
    {
        foreach (var musica in fila)
            yield return musica;
    }

    public IEnumerable<Musica> Historico()
    {
        foreach (var musica in pilha)
            yield return musica;
    }
}
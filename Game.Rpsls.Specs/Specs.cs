using FluentAssertions;
using Xunit;

namespace Game.Rpsls.Specs;

public class Specs
{
    [Theory]
    [MemberData(nameof(WinningCombos))]
    public void Winning_rules(RpslsPick player1Pick, RpslsPick player2Pick)
    {
        _player1.PreparePick(player1Pick);
        _player2.PreparePick(player2Pick);
        _game.Play();

        _game.Winner.Should().NotBeNull();
        _game.Winner.Name.Should().Be(_player1.Name);
    }

    [Theory]
    [MemberData(nameof(NoWinnerCombos))]
    public void No_winner(RpslsPick player1Pick, RpslsPick player2Pick)
    {
        _player1.PreparePick(player1Pick);
        _player2.PreparePick(player2Pick);
        _game.Play();

        _game.Winner.Should().BeNull();
    }

    #region Specs setup

    private readonly Game _game;
    private readonly Player _player1;
    private readonly Player _player2;

    public Specs()
    {
        _player1 = new Player("Mary");
        _player2 = new Player("John");

        _game = new Game(_player1, _player2);
    }

    public static IEnumerable<object[]> WinningCombos()
    {
        foreach (var (player1Pick, player2Pick) in Game.Rules) yield return [player1Pick, player2Pick];
    }

    public static IEnumerable<object[]> NoWinnerCombos()
    {
        yield return [RpslsPick.Rock, RpslsPick.Rock];
        yield return [RpslsPick.Scissors, RpslsPick.Scissors];
    }

    #endregion
}

public class Game(Player player1, Player player2)
{
    public static readonly List<(RpslsPick, RpslsPick)> Rules =
    [
        (RpslsPick.Scissors, RpslsPick.Paper), // Scissors cuts Paper
        (RpslsPick.Paper, RpslsPick.Rock), // Paper covers Rock
        (RpslsPick.Rock, RpslsPick.Lizard), // Rock crushes Lizard
        (RpslsPick.Lizard, RpslsPick.Spock), // Lizard poisons Spock
        (RpslsPick.Spock, RpslsPick.Scissors), // Spock smashes Scissors
        (RpslsPick.Scissors, RpslsPick.Lizard), // Scissors decapitates Lizard
        (RpslsPick.Lizard, RpslsPick.Paper), // Lizard eats Paper
        (RpslsPick.Paper, RpslsPick.Spock), // Paper disproves Spock
        (RpslsPick.Spock, RpslsPick.Rock), // Spock vaporizes Rock
        (RpslsPick.Rock, RpslsPick.Scissors) // Rock crushes Scissors
    ];

    public Player Winner { get; private set; }

    public void Play()
    {
        if (Rules.Any(rule => rule.Item1 == player1.Pick && rule.Item2 == player2.Pick))
            Winner = player1;
        if (Rules.Any(rule => rule.Item1 == player2.Pick && rule.Item2 == player1.Pick))
            Winner = player2;
    }
}

public class Player(string name)
{
    public RpslsPick Pick { get; private set; }

    public string Name => name;

    public void PreparePick(RpslsPick pick)
    {
        Pick = pick;
    }
}

public enum RpslsPick
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}
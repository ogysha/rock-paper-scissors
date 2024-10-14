using FluentAssertions;
using Xunit;

namespace Game.Rpsls.Specs;

public class Specs
{
    [Theory]
    [MemberData(nameof(WinningCombos))]
    public void Winning_rules(HandGesture player1Gesture, HandGesture player2Gesture)
    {
        _player1.PrepareGesture(player1Gesture);
        _player2.PrepareGesture(player2Gesture);
        _game.Play();

        _game.Winner.Should().NotBeNull();
        _game.Winner.Name.Should().Be(_player1.Name);
    }

    [Theory]
    [MemberData(nameof(NoWinnerCombos))]
    public void No_winner(HandGesture player1Gesture, HandGesture player2Gesture)
    {
        _player1.PrepareGesture(player1Gesture);
        _player2.PrepareGesture(player2Gesture);
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
        foreach (var (player1Gesture, player2Gesture) in Game.Rules) yield return [player1Gesture, player2Gesture];
    }

    public static IEnumerable<object[]> NoWinnerCombos()
    {
        yield return [HandGesture.Rock, HandGesture.Rock];
        yield return [HandGesture.Scissors, HandGesture.Scissors];
    }

    #endregion
}

public class Game(Player player1, Player player2)
{
    public static readonly IEnumerable<(HandGesture, HandGesture)> Rules =
    [
        (HandGesture.Scissors, HandGesture.Paper), // Scissors cuts Paper
        (HandGesture.Paper, HandGesture.Rock), // Paper covers Rock
        (HandGesture.Rock, HandGesture.Lizard), // Rock crushes Lizard
        (HandGesture.Lizard, HandGesture.Spock), // Lizard poisons Spock
        (HandGesture.Spock, HandGesture.Scissors), // Spock smashes Scissors
        (HandGesture.Scissors, HandGesture.Lizard), // Scissors decapitates Lizard
        (HandGesture.Lizard, HandGesture.Paper), // Lizard eats Paper
        (HandGesture.Paper, HandGesture.Spock), // Paper disproves Spock
        (HandGesture.Spock, HandGesture.Rock), // Spock vaporizes Rock
        (HandGesture.Rock, HandGesture.Scissors) // Rock crushes Scissors
    ];

    public Player Winner { get; private set; }

    public void Play()
    {
        if (Rules.Any(rule => rule.Item1 == player1.ChosenGesture && rule.Item2 == player2.ChosenGesture))
            Winner = player1;
        if (Rules.Any(rule => rule.Item1 == player2.ChosenGesture && rule.Item2 == player1.ChosenGesture))
            Winner = player2;
    }
}

public class Player(string name)
{
    public HandGesture ChosenGesture { get; private set; }

    public string Name => name;

    public void PrepareGesture(HandGesture gesture)
    {
        ChosenGesture = gesture;
    }
}

public enum HandGesture
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}
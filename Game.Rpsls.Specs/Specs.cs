using FluentAssertions;
using Xunit;
using static Game.Rpsls.Specs.HandGesture;

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

        _game.WinnerName.Should().Be(_player1.Name);
    }

    [Theory]
    [MemberData(nameof(NoWinnerCombos))]
    public void No_winner_rules(HandGesture player1Gesture, HandGesture player2Gesture)
    {
        _player1.PrepareGesture(player1Gesture);
        _player2.PrepareGesture(player2Gesture);
        _game.Play();

        _game.WinnerName.Should().BeNull();
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
        foreach (var (player1Gesture, player2Gesture) in Game.Rules)
        {
            yield return [player1Gesture, player2Gesture];
        }
    }

    public static IEnumerable<object[]> NoWinnerCombos()
    {
        foreach (var handGesture in Enum.GetValues(typeof(HandGesture)))
        {
            yield return [handGesture, handGesture];
        }
    }

    #endregion
}

public class Game(Player player1, Player player2)
{
    public static readonly IEnumerable<(HandGesture, HandGesture)> Rules =
    [
        (Scissors, Paper), // Scissors cuts Paper
        (Paper, Rock), // Paper covers Rock
        (Rock, Lizard), // Rock crushes Lizard
        (Lizard, Spock), // Lizard poisons Spock
        (Spock, Scissors), // Spock smashes Scissors
        (Scissors, Lizard), // Scissors decapitates Lizard
        (Lizard, Paper), // Lizard eats Paper
        (Paper, Spock), // Paper disproves Spock
        (Spock, Rock), // Spock vaporizes Rock
        (Rock, Scissors) // Rock crushes Scissors
    ];

    private Player Winner { get; set; }

    public string WinnerName
    {
        get => Winner?.Name;
    }

    public void Play()
    {
        if (Rules.Any(rule => rule.Item1 == player1.ChosenGesture && rule.Item2 == player2.ChosenGesture))
        {
            Winner = player1;
        }

        if (Rules.Any(rule => rule.Item1 == player2.ChosenGesture && rule.Item2 == player1.ChosenGesture))
        {
            Winner = player2;
        }
    }
}

public record Player(string Name)
{
    public HandGesture ChosenGesture { get; private set; }

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

// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();
//
// app.MapGet("/", () => "Hello World!");
//
// app.Run();

using DnDDamageCalculator.Models;
using DnDDamageCalculator.Models.Character;
using DnDDamageCalculator.Models.Combat;
using DnDDamageCalculator.Utils;

Dice[] _2D6 = [DiceDistributions.D6, DiceDistributions.D6];

var simpleAttack = new Attack(5, _2D6, 3, 20);

Attack[] attacksLevel1 =
[
    simpleAttack
];

Attack[] attacksLevel5 =
[
    new Attack(4, _2D6, 7, 20),
    new Attack(4, _2D6, 7, 20)
];

var lvl1 = new CharacterLevel(1,
    attacksLevel1);
var lvl5 = new CharacterLevel(5,
    attacksLevel5);

var character = new Character([
    // lvl1,
    lvl5
]);

var scenariosByLevel = character.GenerateResults([new CombatConfiguration(13), new CombatConfiguration(14)]);

foreach (var (level, scenarios) in scenariosByLevel)
{
    Console.WriteLine($"---------- level {level} ----------");
    Console.WriteLine(scenarios.Select(s => s.Probability).Sum());
    foreach (var scenario in scenarios)
    {
        var hits = string.Join(',', scenario.HitHistory.Select(hit => hit.ToString()));
        Console.WriteLine(
            $"hit: {hits}, dealing {scenario.DamageDices.DiesString()}+{scenario.DamageModifier} at {scenario.Probability}% probability");
    }
}
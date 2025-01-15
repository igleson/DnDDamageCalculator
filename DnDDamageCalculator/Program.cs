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


Attack[] attacksLevel1 =
[
    new Attack(5, [DiceDistributions.D8], 3 + 2, 20, 1)
];

Attack[] attacksLevel2 =
[
    new Attack(5, [DiceDistributions.D8], 3 + 2, 20, 1, new Topple(0.60)), //attack
    new Attack(5, [DiceDistributions.D8], 3 + 2, 20, 0.125, new Topple(0.60)) //action surge attack
];

Attack[] attacksLevel3 =
[
    new Attack(5, [DiceDistributions.D8], 3 + 2, 19, 1, new Topple(0.60)), //attack
    new Attack(5, [DiceDistributions.D8], 3 + 2, 19, 0.125, new Topple(0.60)) //action surge attack
];

Attack[] attacksLevel4 =
[
    new Attack(6, [DiceDistributions.D8], 4 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(6, [DiceDistributions.D4], 4 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(6, [DiceDistributions.D8], 4 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(6, [DiceDistributions.D8], 4 + 2, 19, 0.25, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel5 =
[
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.25, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel6 =
[
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(6, [DiceDistributions.D10], 4 + 2, 19, 0.5, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel8 =
[
    new Attack(7, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(7, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(7, [DiceDistributions.D10], 5 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(7, [DiceDistributions.D10], 5 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(7, [DiceDistributions.D10], 5 + 2, 19, 0.5, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel9 =
[
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.5, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel10 =
[
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.5, new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel11 =
[
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 1, new Topple(0.60)), //attack action
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.75, new Topple(0.60)), //ba attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.125, new Topple(0.60)), //action surge attack
    new Attack(9, [DiceDistributions.D10], 5 + 2, 19, 0.5, new Topple(0.60)), //reaction attack
];

var lvl1 = new CharacterLevel(1, attacksLevel1, []);
var lvl2 = new CharacterLevel(2, attacksLevel2, []);
var lvl3 = new CharacterLevel(3, attacksLevel3, []);
var lvl4 = new CharacterLevel(4, attacksLevel4, []);
var lvl5 = new CharacterLevel(5, attacksLevel5, []);
var lvl6 = new CharacterLevel(6, attacksLevel6, []);
var lvl7 = new CharacterLevel(7, attacksLevel6, []);
var lvl8 = new CharacterLevel(8, attacksLevel8, [new ShieldMasterFeat(0.5)]);
var lvl9 = new CharacterLevel(9, attacksLevel9, [new ShieldMasterFeat(0.5)]);
var lvl10 = new CharacterLevel(10, attacksLevel10, [new HeroicWarriorFeature(), new ShieldMasterFeat(0.5)]);
var lvl11 = new CharacterLevel(11, attacksLevel11, [new HeroicWarriorFeature(), new ShieldMasterFeat(0.5)]);

var character = new Character([
    // lvl1,
    // lvl2,
    // lvl3,
    // lvl4,
    // lvl5,
    // lvl6,
    // lvl7,
    // lvl8,
    // lvl9,
    lvl10,
    lvl11,
]);

var resultsByLevel = character.GenerateResults(
[
    // new CombatConfiguration(14, new AttackEffects()),
    // new CombatConfiguration(14, new AttackEffects()),
    // new CombatConfiguration(14, new AttackEffects()),
    // new CombatConfiguration(15, new AttackEffects()),
    // new CombatConfiguration(15, new AttackEffects()),
    // new CombatConfiguration(15, new AttackEffects()),
    // new CombatConfiguration(15, new AttackEffects()),
    // new CombatConfiguration(16, new AttackEffects()),
    // new CombatConfiguration(18, new AttackEffects()),
    new CombatConfiguration(18, new AttackEffects()),
    new CombatConfiguration(18, new AttackEffects()),
]);

resultsByLevel.ToList().ForEach(pair =>
{
    var totalProb = pair.results.Select(result => result.Probability).Sum();
    Console.WriteLine($"level: {pair.level}, totalProb: {totalProb}, damage: {pair.results.Select(result => result.AverageDamage()).Sum()}");
});
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

Attack[] attacksLevel1 =
[
    new Attack(5, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 3 + 2, 20, 1)
];

Attack[] attacksLevel2 =
[
    new Attack(5, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 3 + 2, 20, 1, new Topple(0.60)), //attack
    new Attack(5, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 3 + 2, 20, 0.125,
        new Topple(0.60)) //action surge attack
];

Attack[] attacksLevel3 =
[
    new Attack(5, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 3 + 2, 19, 1, new Topple(0.60)), //attack
    new Attack(5, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 3 + 2, 19, 0.125,
        new Topple(0.60)) //action surge attack
];

Attack[] attacksLevel4 =
[
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 4 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 4 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 4 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 8, 1 } }), 4 + 2, 19, 0.25,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel5 =
[
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 4 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 0.25,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel6 =
[
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 4 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(6, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 4 + 2, 19, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel8 =
[
    new Attack(7, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(7, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(7, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(7, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(7, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel9 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel10 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 10, 1 } }), 5 + 2, 19, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel11 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 19, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 19, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 19, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 19, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel15 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 18, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 18, 0.125,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 12, 1 } }), 5 + 2, 18, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel17 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 18, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 0.25,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 0.5,
        new Topple(0.60)), //reaction attack
];

Attack[] attacksLevel20 =
[
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 1,
        new Topple(0.60)), //attack action
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 4, 1 } }), 5 + 2, 18, 0.75,
        new Topple(0.60)), //ba attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 0.25,
        new Topple(0.60)), //action surge attack
    new Attack(9, new DamageDices(new Dictionary<int, int> { { 6, 2 } }), 5 + 2, 18, 0.5,
        new Topple(0.60)), //reaction attack
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
var lvl13 = new CharacterLevel(13, attacksLevel11,
    [new HeroicWarriorFeature(), new StudiedAttacks(), new ShieldMasterFeat(0.5)]);
var lvl15 = new CharacterLevel(15, attacksLevel15,
    [new HeroicWarriorFeature(), new StudiedAttacks(), new ShieldMasterFeat(0.5)]);
var lvl17 = new CharacterLevel(17, attacksLevel17,
    [new HeroicWarriorFeature(), new StudiedAttacks(), new ShieldMasterFeat(0.5)]);
var lvl19 = new CharacterLevel(19, attacksLevel17,
    [new HeroicWarriorFeature(), new BoonOfCombatProwess(), new StudiedAttacks(), new ShieldMasterFeat(0.5)]);
var lvl20 = new CharacterLevel(20, attacksLevel20,
    [new HeroicWarriorFeature(), new BoonOfCombatProwess(), new StudiedAttacks(), new ShieldMasterFeat(0.5)]);

var combat1To3 = new CombatConfiguration(14, new AttackEffects());
var combat4To7 = new CombatConfiguration(15, new AttackEffects());
var combat8 = new CombatConfiguration(16, new AttackEffects());
var combat9To20 = new CombatConfiguration(16, new AttackEffects());

var shilelaghChampion = new CharacterCombat([
    (lvl1, combat1To3),
    (lvl2, combat1To3),
    (lvl3, combat1To3),
    (lvl4, combat4To7),
    (lvl5, combat4To7),
    (lvl6, combat4To7),
    (lvl7, combat4To7),
    (lvl8, combat8),
    (lvl9, combat9To20),
    (lvl10, combat9To20),
    (lvl11, combat9To20),
    (lvl13, combat9To20),
    (lvl15, combat9To20),
    (lvl17, combat9To20),
    (lvl19, combat9To20),
    (lvl20, combat9To20)
]);

var resultsByLevel = shilelaghChampion.GenerateResults();

foreach (var (level, results) in resultsByLevel)
{
    var res = results.AggregateSimilarDamage().ToList();
    Console.WriteLine($"---level {level}, total prob {res.Select(r => r.Probability).Sum()}---");
    foreach (var result in res)
    {
        Console.WriteLine($"{result}");
    }

    Console.WriteLine("--------------------------------------------");
}


// HEX WARLOCK
//
// var hexEldritchBlastTier1 = new Attack(3 + 2, [new DamageDices(new Dictionary<int, int>{{10, 1}}), new DamageDices(new Dictionary<int, int>{{6, 1}})], 0, 20);
// var hexEldritchBlastTier1AgonizingBlast = hexEldritchBlastTier1 with { DamageMod = 3 };
//
// var hexEldritchBlastLvl4 = hexEldritchBlastTier1AgonizingBlast with
// {
//     AttackMod = 4 + 2,
//     DamageMod = 4
// };
//
// var hexEldritchBlastLvl5 = hexEldritchBlastLvl4 with
// {
//     AttackMod = 4 + 3,
//     DamageMod = 4
// };
//
// var hexEldritchBlastLvl8 = hexEldritchBlastLvl5 with
// {
//     AttackMod = 5 + 3,
//     DamageMod = 5
// };
//
// var hexEldritchBlastLvl9 = hexEldritchBlastLvl8 with
// {
//     AttackMod = 5 + 4,
//     DamageMod = 5
// };
//
// var hexEldritchBlastLvl13 = hexEldritchBlastLvl9 with
// {
//     AttackMod = 5 + 5,
//     DamageMod = 5
// };
//
// var hexEldritchBlastLvl17 = hexEldritchBlastLvl13 with
// {
//     AttackMod = 5 + 6,
//     DamageMod = 5
// };
//
//
// var warlockBaseLine = new Character(
// [
//     // new CharacterLevel(1, [hexEldritchBlastTier1], []),
//     // new CharacterLevel(2, [hexEldritchBlastTier1AgonizingBlast], []),
//     // new CharacterLevel(3, [hexEldritchBlastTier1AgonizingBlast], []),
//     // new CharacterLevel(4, [hexEldritchBlastLvl4], []),
//     // new CharacterLevel(5, [hexEldritchBlastLvl5, hexEldritchBlastLvl5], []),
//     // new CharacterLevel(6, [hexEldritchBlastLvl5, hexEldritchBlastLvl5], []),
//     // new CharacterLevel(7, [hexEldritchBlastLvl5, hexEldritchBlastLvl5], []),
//     // new CharacterLevel(8, [hexEldritchBlastLvl8, hexEldritchBlastLvl8], []),
//     // new CharacterLevel(9, [hexEldritchBlastLvl9, hexEldritchBlastLvl9], []),
//     // new CharacterLevel(10, [hexEldritchBlastLvl9, hexEldritchBlastLvl9], []),
//     // new CharacterLevel(11, [hexEldritchBlastLvl9, hexEldritchBlastLvl9, hexEldritchBlastLvl9], []),
//     // new CharacterLevel(12, [hexEldritchBlastLvl9, hexEldritchBlastLvl9, hexEldritchBlastLvl9], []),
//     // new CharacterLevel(13, [hexEldritchBlastLvl13, hexEldritchBlastLvl13, hexEldritchBlastLvl13], []),
//     // new CharacterLevel(14, [hexEldritchBlastLvl13, hexEldritchBlastLvl13, hexEldritchBlastLvl13], []),
//     // new CharacterLevel(15, [hexEldritchBlastLvl13, hexEldritchBlastLvl13, hexEldritchBlastLvl13], []),
//     // new CharacterLevel(16, [hexEldritchBlastLvl13, hexEldritchBlastLvl13, hexEldritchBlastLvl13], []),
//     // new CharacterLevel(17, [hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17],
//     //     []),
//     // new CharacterLevel(18, [hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17],
//     //     []),
//     // new CharacterLevel(19, [hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17],
//     //     []),
//     new CharacterLevel(20, [hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17, hexEldritchBlastLvl17],
//         []),
// ]);
//
// var results = warlockBaseLine.GenerateResults(combats);
//
// Console.WriteLine("Warlock baseline");
// foreach (var (_, res) in results)
// {
//     foreach (var r in res)
//     {
//        Console.WriteLine($"{r.Probability} - {r.DamageRepresentation()}");
//     }
// }
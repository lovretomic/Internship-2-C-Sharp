
using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

// Inicijalna tablica igrača
var players = new Dictionary<string, (string Position, int Rating)>()
{
    {"Luka Modrić", ("MF", 88) },
    {"Marcelo Brozović", ("MF", 86) },
    {"Mateo Kovačić", ("MF", 84) },
    {"Ivan Perišić", ("MF", 84) },
    {"Andrej Kramarić", ("FW", 82) },
    {"Ivan Rakitić", ("MF", 82) },
    {"Joško Gvardiol", ("DF", 81) },
    {"Mario Pašalić", ("MF", 81) },
    {"Lovro Majer", ("MF", 80) },
    {"Dominik Livaković", ("GK", 80) },
    {"Ante Rebić", ("FW", 80) }, 
    {"Josip Brekalo", ("MF", 79) },
    {"Borna Sosa", ("DF", 78) }, 
    {"Nikola Vlašić", ("MF", 78) },
    {"Duje Ćaleta-Car", ("DF", 78) },
    {"Dejan Lovren", ("DF", 78) },
    {"Mislav Oršić", ("MF", 77) }, 
    {"Marko Livaja", ("FW", 77) }, 
    {"Domagoj Vida", ("DF", 76) }, 
    {"Ante Budimir", ("FW", 76) } 
};

// Postavljanje random objekta i tablice za praćenje kola i utakmica
Random rnd = new Random();
var roundNumber = 0; // Prati broj kola
Dictionary<int, List<(string, string)>> matches = new Dictionary<int, List<(string, string)>>()
{
    {1, new List<(string Country1, string Country2)>() {("Maroko", "Hrvatska"), ("Belgija", "Kanada")}},
    {2, new List<(string Country1, string Country2)>() {("Belgija", "Maroko"), ("Hrvatska", "Kanada")}},
    {3, new List<(string Country1, string Country2)>() {("Hrvatska", "Belgija"), ("Kanada", "Maroko")}}
};
var input = -1;

// Postavljanje tablice za praćenje rezultata utakmica
Dictionary<string, List<string>> teamResults = new Dictionary<string, List<string>>
{
    {"Hrvatska", new List<string>() },
    {"Maroko", new List<string>() },
    {"Belgija", new List<string>() },
    {"Kanada", new List<string>() }
};
// Postavljanje tablice za praćenje bodova nacionalnih timova
Dictionary<string, int> tableScores = new Dictionary<string, int>
{
    {"Hrvatska", 0},
    {"Maroko", 0},
    {"Belgija", 0},
    {"Kanada", 0}
};

// Postavljanje tablice za gol-razliku
Dictionary<string, int> goalDifference = new Dictionary<string, int>
{
    {"Hrvatska", 0},
    {"Maroko", 0},
    {"Belgija", 0},
    {"Kanada", 0}
};
List<int> goalDifferenceList = new List<int>(goalDifference.Values);

// Inicijalno razdvajanje tablice na 2 niza radi sortiranja
List<string> tableKeys = new List<string>(tableScores.Keys);
List<int> tableValues = new List<int>(tableScores.Values);

// Postavljanje tablice za praćenje strijelaca i broja golova
Dictionary<string, int> scorers = new Dictionary<string, int>();
foreach(var player in players)
{
    if (player.Value.Position == "FW")
        scorers.Add(player.Key, 0);
}

// Funkcija za povratak na glavni izbornik
void ConfirmMainMenu()
{
    Console.WriteLine("Unesi bilo koji znak za povratak na glavni izbornik.");
    Console.ReadLine();
    MainMenu();
}

// Funkcija za povratak na izbornik statistika
void ConfirmStatsMenu()
{
    Console.WriteLine("Unesi bilo koji znak za povratak na statistiku.");
    Console.ReadLine();
    MenuStats();
}
void MainMenu ()
{
    Console.Clear();
    Console.WriteLine("- GLAVNI IZBORNIK -");
    Console.WriteLine("1 - Odradi trening");
    Console.WriteLine("2 - Odigraj utakmicu");
    Console.WriteLine("3 - Statistika");
    Console.WriteLine("4 - Kontrola igrača");
    Console.WriteLine("0 - Izlaz iz aplikacije");

    input = int.Parse(Console.ReadLine());
    switch(input)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("- Odradi trening -");
            TrainPlayers();
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("- Odigraj utakmicu -");
            PlayMatch();
            break;
        case 3:
            MenuStats();
            break;
        case 4:
            Console.Clear();
            Console.WriteLine("- Kontrola igrača -");
            Console.WriteLine("1 - Unos novog igrača");
            Console.WriteLine("2 - Brisanje igrača");
            Console.WriteLine("3 - Uređivanje igrača");
            Console.WriteLine("0 - Povratak na glavni izbornik");

            input = int.Parse(Console.ReadLine());
            switch(input)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("- Kontrola igrača > Unos novog igrača -");
                    AddPlayer();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("- Kontrola igrača > Brisanje igrača -");
                    RemovePlayer();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("- Kontrola igrača > Uređivanje igrača -");
                    Console.WriteLine("1 - Uredi ime i prezime igrača");
                    Console.WriteLine("2 - Uredi poziciju igrača");
                    Console.WriteLine("3 - Uredi rating igrača");

                    input = int.Parse(Console.ReadLine());
                    switch(input)
                    {
                        case 1:
                            EditPlayerName();
                            break;
                        case 2:
                            EditPlayerPosition();
                            break;
                        case 3:
                            EditPlayerRating();
                            break;
                    }
                    break;
                case 0:
                    MainMenu();
                    break;
            }
            break;
        case 0:
            Console.WriteLine("Izlazim iz aplikacije...");
            break;
        default:
            Console.WriteLine("Pogrešan unos!");
           ConfirmMainMenu();
            break;
    }
}

void MenuStats()
{
    Console.Clear();
    Console.WriteLine("- Statistika -");
    Console.WriteLine("1 - Ispis svih igrača");
    Console.WriteLine("0 - Povratak na glavni izbornik");

    input = int.Parse(Console.ReadLine());
    switch (input)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("- Statistika > Ispis svih igrača -");
            Console.WriteLine("1 - Ispis onako kako su spremljeni");
            Console.WriteLine("2 - Ispis po ratingu uzlazno");
            Console.WriteLine("3 - Ispis po ratingu silazno");
            Console.WriteLine("4 - Ispis igrača po imenu i prezimenu (ispis pozicije i ratinga)");
            Console.WriteLine("5 - Ispis igrača po ratingu (ako ih je više ispisati sve)");
            Console.WriteLine("6 - Ispis igrača po poziciji (ako ih je više ispisati sve)");
            Console.WriteLine("7 - Ispis trenutnih prvih 11 igrača (na pozicijama odabrati igrače s najboljim ratingom)");
            Console.WriteLine("8 - Ispis strijelaca i koliko golova imaju");
            Console.WriteLine("9 - Ispis svih rezultata ekipe");
            Console.WriteLine("10 - Ispis rezultata svih ekipa");
            Console.WriteLine("11 - Ispis tablice grupe (mjesto na tablici, ime ekipe, broj bodova, gol razlika)");
            Console.WriteLine("0 - Povratak na statistiku");

            input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    PrintPlayers();
                    break;
                case 2:
                    PrintPlayersSorted(0);
                    break;
                case 3:
                    PrintPlayersSorted(1);
                    break;
                case 4:
                    PrintPlayersSorted(2);
                    break;
                case 5:
                    PrintPlayersByRating();
                    break;
                case 6:
                    PrintPlayersByPosition();
                    break;
                case 7:
                    Print11Players();
                    break;
                case 8:
                    Console.Clear();
                    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis svih strijelaca -");
                    var counter = 1;
                    foreach (var scorer in scorers)
                        Console.WriteLine($"{counter++}. {scorer.Key} (Broj golova: {scorer.Value})");
                    ConfirmStatsMenu();
                    break;
                case 9:
                    Console.Clear();
                    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis svih rezultata ekipe -");
                    if (teamResults["Hrvatska"].Count() == 0)
                        Console.WriteLine("Ekipa nije odigrala niti jednu utakmicu.");
                    else
                        foreach (var result in teamResults["Hrvatska"])
                            Console.WriteLine(result);
                    ConfirmStatsMenu();
                    break;
                case 10:
                    Console.Clear();
                    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis rezultata svih ekipa -");
                    List<string> teams = new List<string>(teamResults.Keys);
                    for (var i = 0; i < teamResults.Count(); i++)
                    {
                        Console.WriteLine(teams[i]);
                        if(teamResults[teams[i]].Count == 0)
                            Console.WriteLine("Ekipa nije odigrala niti jednu utakmicu.\n");
                        else
                            for (var j = 0; j < teamResults[teams[i]].Count; j++)
                                Console.WriteLine(teamResults[teams[i]][j]);
                    }
                    ConfirmStatsMenu();
                    break;
                case 11:
                    Console.Clear();
                    counter = 1;
                    Console.WriteLine("Ekipa (bodovi na ljestvici) [gol-razlika]");
                    Console.WriteLine("--- GRUPA F ---");
                    for (var i = 0; i < 4; i++)
                        Console.WriteLine($"{counter++}. {tableKeys[i]} ({tableValues[i]}) [{goalDifferenceList[i]}]");
                    ConfirmStatsMenu();
                    break;
                case 0:
                    Console.Clear();
                    MenuStats();
                    break;
                default:
                    Console.WriteLine("Pogrešan unos!");
                    ConfirmMainMenu();
                    break;
            }

            break;
        case 0:
            Console.Clear();
            MainMenu();
            break;
        default:
            Console.WriteLine("Pogrešan unos!");
            ConfirmMainMenu();
            break;
    }
}
void TrainPlayers()
{
    List<string> playerNames = new List<string>(players.Keys);
    for (var i = 0; i < playerNames.Count(); i++)
    {
        var randomNum = rnd.Next(0, 6);
        var randomOpr = rnd.Next();

        var tempScore = players[playerNames[i]].Rating;
        int newScore;
        
        if (randomOpr % 2 == 0) newScore = (int)(tempScore * ((100 + randomNum) / 100.0));
        else                    newScore = (int)(tempScore * ((100 - randomNum) / 100.0));
        
        if (newScore > 100) newScore = 100;
        else if (newScore < 1) newScore = 1;

        players[playerNames[i]] = (players[playerNames[i]].Position, newScore);
        Console.WriteLine($"{playerNames[i]} ({tempScore} --> {newScore})");
    }
    ConfirmMainMenu();
}

void PlayMatch()
{   
    // Generiranje postave
    
    var lineup = new Dictionary<string, List<(string, int)>>()
    {
        { "GK", new List<(string, int)>() },
        { "DF", new List<(string, int)>() },
        { "MF", new List<(string, int)>() },
        { "FW", new List<(string, int)>() }
    };

    // Sortiranje postave
    List<string> playerNames = new List<string>(players.Keys);
    List<int> playerRatings = new List<int>();
    for (var i = 0; i < playerNames.Count(); i++)
        playerRatings.Add(players[playerNames[i]].Rating);

    for (var i = 0; i < playerNames.Count() - 1; i++)
        for (var j = 0; j < playerNames.Count() - i - 1; j++)
            if (playerRatings[j + 1] > playerRatings[j])
            {
                var temp1 = playerRatings[j];
                playerRatings[j] = playerRatings[j + 1];
                playerRatings[j + 1] = temp1;

                var temp2 = playerNames[j];
                playerNames[j] = playerNames[j + 1];
                playerNames[j + 1] = temp2;
            }

    // Pridruživanje pozicijama
    for (var i = 0; i < playerNames.Count(); i++)
    {
        if (players[playerNames[i]].Position == "GK" && lineup["GK"].Count() < 1)
            lineup["GK"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "DF" && lineup["DF"].Count() < 4)
            lineup["DF"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "MF" && lineup["MF"].Count() < 3)
            lineup["MF"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "FW" && lineup["FW"].Count() < 3)
            lineup["FW"].Add((playerNames[i], players[playerNames[i]].Rating));
    }

    // Ispis postave
    Console.WriteLine("Hrvatska će reprezentacija igrati u sljedećoj postavi:");
    var positions = new List<string>() { "GK", "DF", "MF", "FW" };
    for(var i = 0; i < 4; i++)
    {
        Console.WriteLine(positions[i]);
        for(var j = 0; j < lineup[positions[i]].Count(); j++)
            Console.WriteLine(lineup[positions[i]][j]);
    }

    // Provjera broja igraca po pozicijama u postavi
    if (lineup["GK"].Count() < 1 || lineup["DF"].Count < 4 || lineup["MF"].Count() < 3 || lineup["FW"].Count() < 3)
        Console.WriteLine("Nedovoljan broj igrača za utakmicu.");
    else
    {
        // Generiranje rezultata kola
        roundNumber++;
        if (!matches.ContainsKey(roundNumber))
        {
            Console.WriteLine("\nSve su utakmice u skupini odigrane.");
            ConfirmMainMenu();
        }
        
        Console.WriteLine($"\n--- Rezultati {roundNumber}. kola ---");
        for (var i = 0; i < matches[roundNumber].Count(); i++)
        {
            var goals1 = rnd.Next(0, 6);
            var goals2 = rnd.Next(0, 6);

            goalDifference[matches[roundNumber][i].Item1] += goals1 - goals2;
            goalDifference[matches[roundNumber][i].Item2] += goals2 - goals1;

            Console.WriteLine($"{matches[roundNumber][i].Item1} {goals1} : {goals2} {matches[roundNumber][i].Item2}");
            if ((matches[roundNumber][i].Item1 == "Hrvatska" && goals1 > goals2) || (matches[roundNumber][i].Item2 == "Hrvatska" && goals1 < goals2))
            {
                // Ako Hrvatska pobijedi...
                for (var j = 0; j < players.Count(); j++)
                {
                    var changeRating = 0;
                    if (players[playerNames[j]].Position != "FW") changeRating = (int)(players[playerNames[j]].Rating * 1.02);
                    else                                          changeRating = (int)(players[playerNames[j]].Rating * 1.05);

                    if (changeRating > 100) changeRating = 100;

                    var constPosition = players[playerNames[j]].Position;
                    players.Remove(playerNames[j]);
                    players.Add(playerNames[j], (constPosition, changeRating));
                }
            }

            // Zapisivanje rezultata u tablicu za rezultate
            teamResults[matches[roundNumber][i].Item1].Add($"{matches[roundNumber][i].Item1} {goals1} : {goals2} {matches[roundNumber][i].Item2}");
            teamResults[matches[roundNumber][i].Item2].Add($"{matches[roundNumber][i].Item1} {goals1} : {goals2} {matches[roundNumber][i].Item2}");

            // Osvježivanje tablice s bodovima (i sortiranje)
            if (goals1 > goals2) tableScores[matches[roundNumber][i].Item1] += 3;
            else if (goals1 == goals2)
            {
                tableScores[matches[roundNumber][i].Item1] += 1;
                tableScores[matches[roundNumber][i].Item2] += 1;
            }
            else tableScores[matches[roundNumber][i].Item2] += 3;

            tableKeys = new List<string>(tableScores.Keys);
            tableValues = new List<int>(tableScores.Values);
            goalDifferenceList = new List<int>(goalDifference.Values);
;            for (var a = 0; a < tableKeys.Count() - 1; a++)
                for (var b = 0; b < tableKeys.Count() - a - 1; b++)
                    if (tableValues[b + 1] > tableValues[b])
                    {
                        var temp1 = tableValues[b];
                        tableValues[b] = tableValues[b + 1];
                        tableValues[b + 1] = temp1;

                        var temp2 = tableKeys[b];
                        tableKeys[b] = tableKeys[b + 1];
                        tableKeys[b + 1] = temp2;

                        
                        var temp3 = goalDifferenceList[b];
                        goalDifferenceList[b] = goalDifferenceList[b + 1];
                        goalDifferenceList[b + 1] = temp3;
                    }

            // Raspodjela golova prema strijelcima
            if (String.Equals(matches[roundNumber][i].Item1, "Hrvatska") || String.Equals(matches[roundNumber][i].Item2, "Hrvatska"))
            {
                var totalGoals = 0;
                if (matches[roundNumber][i].Item1 == "Hrvatska") totalGoals = goals1;
                else totalGoals = goals2;

                List<string> scorerNames = new List<string>(scorers.Keys);
                for(var j = 0; j < scorerNames.Count(); j++)
                {
                    var constName = scorerNames[j];
                    var newGoals = scorers[scorerNames[j]];
                    var randomGoals = rnd.Next(0, totalGoals + 1);   // Izaberi nasumičan broj od postignutih golova
                    newGoals += randomGoals;                         // Dodaj j-tom strijelcu
                    totalGoals -= randomGoals;                       // Oduzmi od dostupnih golova

                    scorers.Remove(constName);
                    scorers.Add(constName, newGoals);
                }
            }
        }
        Console.WriteLine($"-------------------------\n");
    }
    ConfirmMainMenu();
}

void PrintPlayers()
{
    Console.Clear();
    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis igrača onako kako su spremljeni -");
    foreach(var player in players)
    {
        Console.WriteLine(player);
    }
    ConfirmStatsMenu();
}
void PrintPlayersSorted(int option)
{
    //option == 0; uzlazno / option == 1; silazno / option == 2; uzlazno (imena)
    List<string> playerNames = new List<string>(players.Keys);
    List<int> playerRatings = new List<int>();
    List<string> playerPositions = new List<string>();
    for (var i = 0; i < playerNames.Count(); i++)
    {
        playerRatings.Add(players[playerNames[i]].Rating);
        playerPositions.Add(players[playerNames[i]].Position);
    }
        


    if (option == 0)
    {
        Console.Clear();
        Console.WriteLine("- Statistika > Ispis svih igrača > Ispis po ratingu uzlazno -");
        for (var i = 0; i < playerNames.Count() - 1; i++)
            for (var j = 0; j < playerNames.Count() - i - 1; j++)
                if (playerRatings[j + 1] < playerRatings[j])
                {
                    var temp1 = playerRatings[j];
                    playerRatings[j] = playerRatings[j + 1];
                    playerRatings[j + 1] = temp1;

                    var temp2 = playerNames[j];
                    playerNames[j] = playerNames[j + 1];
                    playerNames[j + 1] = temp2;
                }
        for (var i = 0; i < playerNames.Count(); i++)
            Console.WriteLine($"{i + 1}. {playerNames[i]} ({playerRatings[i]})");
    }
    else if (option == 1)
    {
        Console.Clear();
        Console.WriteLine("- Statistika > Ispis svih igrača > Ispis po ratingu silazno -");
        for (var i = 0; i < playerNames.Count() - 1; i++)
            for (var j = 0; j < playerNames.Count() - i - 1; j++)
                if (playerRatings[j + 1] > playerRatings[j])
                {
                    var temp1 = playerRatings[j];
                    playerRatings[j] = playerRatings[j + 1];
                    playerRatings[j + 1] = temp1;

                    var temp2 = playerNames[j];
                    playerNames[j] = playerNames[j + 1];
                    playerNames[j + 1] = temp2;
                }
        for (var i = 0; i < playerNames.Count(); i++)
            Console.WriteLine($"{i + 1}. {playerNames[i]} ({playerRatings[i]})");
    }
    else if(option == 2)
    {
        Console.Clear();
        Console.WriteLine("- Statistika > Ispis svih igrača > Ispis po imenu i prezimenu uzlazno -");
        for (var i = 0; i < playerNames.Count() - 1; i++)
            for (var j = 0; j < playerNames.Count() - i - 1; j++)
                if (String.Compare(playerNames[j + 1], playerNames[j], StringComparison.OrdinalIgnoreCase) < 0)
                {
                    var temp1 = playerRatings[j];
                    playerRatings[j] = playerRatings[j + 1];
                    playerRatings[j + 1] = temp1;

                    var temp2 = playerNames[j];
                    playerNames[j] = playerNames[j + 1];
                    playerNames[j + 1] = temp2;
                }
        for (var i = 0; i < playerNames.Count(); i++)
            Console.WriteLine($"{i + 1}. {playerNames[i]} ({playerPositions[i]}, {playerRatings[i]})");
    }

    ConfirmStatsMenu();
}

void PrintPlayersByRating()
{
    Console.Clear();
    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis po ratingu -");
    Console.WriteLine("Unesi rating za pretraživanje.");
    var ratingInput = int.Parse(Console.ReadLine());
    var counter = 0;
    foreach(var player in players)
    {
        if(player.Value.Rating == ratingInput)
            Console.WriteLine($"{++counter}. {player.Key} ({player.Value.Rating})");
    }
    if (counter == 0) Console.WriteLine("Niti jedan igrač ne odgorava pretraživanju.");

    ConfirmStatsMenu();
}
void PrintPlayersByPosition()
{
    Console.Clear();
    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis po poziciji -");
    Console.WriteLine("Unesi poziciju za pretraživanje.");
    var positionInput = Console.ReadLine();
    var counter = 0;
    foreach (var player in players)
    {
        if (player.Value.Position == positionInput)
            Console.WriteLine($"{++counter}. {player.Key} ({player.Value.Position})");
    }
    if (counter == 0) Console.WriteLine("Niti jedan igrač ne odgorava pretraživanju.");

    ConfirmStatsMenu();
}

void Print11Players()
{
    Console.Clear();
    Console.WriteLine("- Statistika > Ispis svih igrača > Ispis prvih 11 igrača -");
    var lineup = new Dictionary<string, List<(string, int)>>()
    {
        { "GK", new List<(string, int)>() },
        { "DF", new List<(string, int)>() },
        { "MF", new List<(string, int)>() },
        { "FW", new List<(string, int)>() }
    };

    // Sortiranje postave
    List<string> playerNames = new List<string>(players.Keys);
    List<int> playerRatings = new List<int>();
    for (var i = 0; i < playerNames.Count(); i++)
        playerRatings.Add(players[playerNames[i]].Rating);

    for (var i = 0; i < playerNames.Count() - 1; i++)
        for (var j = 0; j < playerNames.Count() - i - 1; j++)
            if (playerRatings[j + 1] > playerRatings[j])
            {
                var temp1 = playerRatings[j];
                playerRatings[j] = playerRatings[j + 1];
                playerRatings[j + 1] = temp1;

                var temp2 = playerNames[j];
                playerNames[j] = playerNames[j + 1];
                playerNames[j + 1] = temp2;
            }

    for (var i = 0; i < playerNames.Count(); i++)
    {
        if (players[playerNames[i]].Position == "GK" && lineup["GK"].Count() < 1)
            lineup["GK"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "DF" && lineup["DF"].Count() < 4)
            lineup["DF"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "MF" && lineup["MF"].Count() < 3)
            lineup["MF"].Add((playerNames[i], players[playerNames[i]].Rating));
        else if (players[playerNames[i]].Position == "FW" && lineup["FW"].Count() < 3)
            lineup["FW"].Add((playerNames[i], players[playerNames[i]].Rating));
    }

    if (lineup["GK"].Count() < 1 || lineup["DF"].Count < 4 || lineup["MF"].Count() < 3 || lineup["FW"].Count() < 3)
        Console.WriteLine("Nedovoljan broj igrača za utakmicu.");
    else
    {
        Console.WriteLine("Prvih 11 igrača u postavi:");
        var positions = new List<string>() { "GK", "DF", "MF", "FW" };
        for (var i = 0; i < 4; i++)
        {
            Console.WriteLine(positions[i]);
            for (var j = 0; j < lineup[positions[i]].Count(); j++)
                Console.WriteLine(lineup[positions[i]][j]);
        }
    }

    ConfirmStatsMenu();
}
void AddPlayer()
{
    if(players.Count() > 26)
    {
        Console.WriteLine("Već je unesen maksimalan broj igrača (26). Nije moguće dodati novog.");
        ConfirmMainMenu();
    }
    var inputName = "";
    var inputPosition = "";
    var inputRating = 0;

    Console.WriteLine("Unesi ime i prezime igrača.");
    inputName = Console.ReadLine();
    if(inputName == null || inputName.Trim() == "")
    {
        Console.WriteLine("Pogrešan unos!");
        ConfirmMainMenu();
    }
    if(players.ContainsKey(inputName))
    {
        Console.WriteLine("Igrač je več unesen!");
    }
    else
    {
        Console.WriteLine("Unesi poziciju igrača (GK, DF, MF, FW).");
        inputPosition = Console.ReadLine().ToUpper();
        if(inputPosition != "GK" && inputPosition != "DF" && inputPosition != "MF" && inputPosition != "FW")
        {
            Console.WriteLine("Pogrešan unos.");
            ConfirmMainMenu();
        }
        Console.WriteLine("Unesi rating igrača.");
        inputRating = int.Parse(Console.ReadLine());
    }

    if(inputRating > 100)
    {
        Console.WriteLine("Rating ne smije biti veći od 100. Upisan je rating 100.");
        inputRating = 100;
    }
    else if(inputRating < 1)
    {
        Console.WriteLine("Rating ne smije biti manji od 1. Upisan je rating 1.");
        inputRating = 1;
    }

    Console.WriteLine($"Jeste li sigurni da želite dodati ovog igrača (upišite 'N' za novi unos ili unesite bilo koji znak)? ({inputName}, {inputPosition}, {inputRating})");
    var inputString = Console.ReadLine().ToUpper();
    if(String.Equals(inputString, "N"))
    {
        Console.WriteLine("\n### Novi unos");
        AddPlayer();
    }
        
    players.Add(inputName, (inputPosition, inputRating));
    if (inputPosition == "FW")
        scorers.Add(inputName, 0);
    Console.WriteLine($"Dodan novi igrač! ({inputName}, {inputPosition}, {inputRating})");
    ConfirmMainMenu();
}
void RemovePlayer() {
    Console.WriteLine("Dostupni igrači:");
    foreach(var player in players)
        Console.WriteLine(player.Key);
    Console.WriteLine("\nUnesi ime i prezime igrača.");
    var inputName = Console.ReadLine();
    if(players.ContainsKey(inputName))
    {
        Console.WriteLine($"Jeste li sigurni da želite izbrisati ovog igrača (upišite 'N' za novi unos ili unesite bilo koji znak)? ({inputName})");
        var inputString = Console.ReadLine().ToUpper();
        if (String.Equals(inputString, "N"))
        {
            Console.WriteLine("\n### Novi unos");
            RemovePlayer();
        }
        players.Remove(inputName);
        Console.WriteLine("Igrač uspješno izbrisan!");
    }
    else Console.WriteLine("Taj igrač ne postoji u bazi podataka.");

    ConfirmMainMenu();
}
void EditPlayerName()
{
    Console.WriteLine("Dostupni igrači:");
    foreach (var player in players)
        Console.WriteLine(player.Key);
    Console.WriteLine("\nUnesi ime i prezime igrača.");
    var inputName = Console.ReadLine();
    if(players.ContainsKey(inputName))
    {
        Console.WriteLine($"Jeste li sigurni da želite urediti ovog igrača (upišite 'N' za novi unos ili unesite bilo koji znak)? ({inputName})");
        var inputString = Console.ReadLine().ToUpper();
        if (String.Equals(inputString, "N"))
        {
            Console.WriteLine("\n### Novi unos");
            EditPlayerName();
        }
        var changePosition = players[inputName].Position;
        var changeRating = players[inputName].Rating;
        players.Remove(inputName);

        Console.WriteLine("Unesi novo ime i prezime igrača.");
        inputName = Console.ReadLine();

        players.Add(inputName, (changePosition, changeRating));
        Console.WriteLine("Uspješno promijenjeno ime.");
    }
    ConfirmMainMenu();
}

void EditPlayerPosition()
{
    Console.WriteLine("Dostupni igrači:");
    foreach (var player in players)
        Console.WriteLine($"{player.Key} ({player.Value.Position})");
    Console.WriteLine("\nUnesi ime i prezime igrača.");
    var inputName = Console.ReadLine();
    if (players.ContainsKey(inputName))
    {
        Console.WriteLine($"Jeste li sigurni da želite urediti ovog igrača (upišite 'N' za novi unos ili unesite bilo koji znak)? ({inputName})");
        var inputString = Console.ReadLine().ToUpper();
        if (String.Equals(inputString, "N"))
        {
            Console.WriteLine("\n### Novi unos");
            EditPlayerPosition();
        }
        Console.WriteLine($"Taj se igrač nalazi na poziciji {players[inputName].Position}. Unesite novu poziciju.");
        var newPosition = Console.ReadLine().ToUpper();
        if(newPosition == "GK" || newPosition == "DF" || newPosition == "MF" || newPosition == "FW")
        {
            var changeRating = players[inputName].Rating;
            players.Remove(inputName);
            players.Add(inputName, (newPosition, changeRating));
        }
        else Console.WriteLine("Pogrešan unos, nova pozicija mora biti GK, DF, MF ili FW!");
    }
    else Console.WriteLine("Igrač se ne nalazi u bazi podataka.");
    ConfirmMainMenu();
}
void EditPlayerRating()
{
    Console.WriteLine("Dostupni igrači:");
    foreach (var player in players)
        Console.WriteLine($"{player.Key} ({player.Value.Rating})");
    Console.WriteLine("\nUnesi ime i prezime igrača.");
    var inputName = Console.ReadLine();
    if (players.ContainsKey(inputName))
    {
        Console.WriteLine($"Taj igrač ima rating od {players[inputName].Rating}. Unesite novi rating.");
        var newRating = int.Parse(Console.ReadLine());
        if (newRating > 100)
        {
            Console.WriteLine("Rating ne smije biti veći od 100. Upisan je rating 100.");
            newRating = 100;
        }
        else if (newRating < 1)
        {
            Console.WriteLine("Rating ne smije biti manji od 1. Upisan je rating 1.");
            newRating = 1;
        }

        var changePosition = players[inputName].Position;
        players.Remove(inputName);
        players.Add(inputName, (changePosition, newRating));
    }


    else Console.WriteLine("Igrač se ne nalazi u bazi podataka.");
    ConfirmMainMenu();
}

// Pokretanje programa
MainMenu();

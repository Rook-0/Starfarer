using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfarer_1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random roll = new Random();
        //Console.WriteLine(args[0]);

        //Opening text and stat gathering.
        Start:
            Navigator theNavigator = new Navigator();

            Console.WriteLine("\"Welcome to Warpstrider. Press Enter to Continue.\"");
            Console.ReadLine();
            Console.Clear();

            Console.Write("Please enter the Navigator's Intelligence Score: ");
            theNavigator.Intelligence = Convert.ToInt32(Console.ReadLine());

            Console.Write("What is the Navigators Skill Bonus to Navigation (Warp)?: ");
            theNavigator.NavigationWarp = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nPlease enter the Navigators Perception Score: ");
            theNavigator.Perception = Convert.ToInt32(Console.ReadLine());

            Console.Write("What is the Navigators Skill Bonus to Psyniscience?: ");
            theNavigator.Psyniscience = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            theJourney journeyStats = new theJourney();

            //Duration_Generation:            
            bool easyWarp = false;
            string journeyStability = null;
            Console.Write("Is this route Stable, Previously Charted, or Easy to Travel? (y/n)");
            string easyRouteQuery = Console.ReadLine();
        Rando_Duration:
            if (easyRouteQuery == "y" || easyRouteQuery == "yes")
            {
                Console.Clear();
                Console.Write("How many days should this take?: ");
                journeyStats.journeyDays += Convert.ToInt32(Console.ReadLine());
                easyWarp = true;
                Console.Clear();
            }
            else
                journeyStats.journeyDays = journeyDuration();

            journeyStability = routeStability();
            if (journeyStability == "Stable Route. (+10 to chart this route)." || easyWarp == true)
                journeyStats.journeyDays += 0;
            else if (journeyStability == "Indirect Path." ||
                journeyStability == "Haunted Passage. (+10 to any Warp Hallucination Rolls)." ||
                journeyStability == "Surly Route. (-10 to Psyniscience to Divining Auguries Roll)." ||
                journeyStability == "Untraceable Trail. (Route cannot be charted)." ||
                journeyStability == "Lightless Path. (Astromican is not visible to the Navigator).")
                journeyStats.journeyDays *= 2;
            else if (journeyStability == "Byzantine Route." && easyWarp == false)
                journeyStats.journeyDays *= 3;

            //Divining_Auguries:
            int psyniscienceRoll1 = roll.Next(1, 101);
            int psyniscienceBonus = 0;
            bool astronomicanVisible = true;
            string auguriesPortent = "All is well, the Astronomican is clear for this route.";
            if (journeyStability == "Surly Route. (-10 to Psyniscience to Divining Auguries Roll).")
                psyniscienceBonus += -10;
            if (easyWarp == true)
                psyniscienceBonus += 10;
            int psyniscienceTotalDA = theNavigator.Perception + theNavigator.Psyniscience + psyniscienceBonus;

            if (psyniscienceTotalDA < psyniscienceRoll1)
            {
                auguriesPortent = divingAuguries();
                if (auguriesPortent == "The clarity of the Astronomican is difficult.")
                    journeyStats.journeyDays /= 2;
                else if (auguriesPortent == "The clarity of the Astronomican is clouded.")
                    journeyStats.journeyDays *= 2;
                else if (auguriesPortent == "The Astronomican is obscured." || auguriesPortent == "The Astronican is visible...")
                    journeyStats.journeyDays *= 3;
            }

            psyniscienceBonus = 0;
            Console.Clear();
            Console.WriteLine("The path is: {0}\nIt should take {1} days.", journeyStability, journeyStats.journeyDays);
            Console.WriteLine("");
            Console.WriteLine("The Navigators Portent:\n{0}", auguriesPortent);
            Console.ReadLine();
            Console.Clear();

            Console.Write("Would you like to travel? (y/n): ");
            string warpInput = Console.ReadLine();
            if (warpInput == "n" || warpInput == "no")
            {
                Console.Clear();
                int navWait = roll.Next(1, 6);
                Console.WriteLine("You can either decide not to travel, or wait {0} days to try again.", navWait);
                Console.ReadLine();
            Wait_Or_Quit:
                Console.Clear();
                Console.WriteLine("Would you like to \"wait\" or \"quit\"?");
                string warpInput2 = Console.ReadLine();
                if (warpInput2 == "wait")
                    goto Rando_Duration;
                else if (warpInput2 == "quit")
                    System.Environment.Exit(0);
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Invalid");
                    Console.ReadLine();
                    goto Wait_Or_Quit;
                }
            }
            Console.Clear();

            Console.Write("What is the fleet's Morale?: ");
            theFleet fleetStats = new theFleet();
            fleetStats.Morale = Convert.ToInt32(Console.ReadLine());
            Console.Write("What is the Strength of the fleet?: ");
            fleetStats.Strength = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many days of Supplies do you have?: ");
            fleetStats.Supplies = Convert.ToInt32(Console.ReadLine());
            Console.Clear();

            int moraleRoll = roll.Next(1, 101);
            bool badOmens = false;
            if (fleetStats.Morale < moraleRoll || moraleRoll >= 99)//Fail State
            {
            Omens:
                Console.Write("\"The crew mutters that there are bad omens. They seem uneasy.\"\n\"Would you like to try to calm them?\"(y/n)");
                string captainMoraleResponse = Console.ReadLine();
                if (captainMoraleResponse == "y" || captainMoraleResponse == "yes")
                {
                    Console.Clear();
                    Console.Write("What is your Captain's skill (including stat score) at Command?: ");
                    fleetStats.CaptainCommand = Convert.ToInt32(Console.ReadLine());
                    Console.Write("What is your Missionary's skill (including stat score) at Charm?: ");
                    fleetStats.MissionaryCharm = Convert.ToInt32(Console.ReadLine());
                    Console.Clear();

                    int commandRoll = roll.Next(1, 101);
                    //Pass State
                    if (fleetStats.CaptainCommand >= commandRoll || fleetStats.MissionaryCharm >= commandRoll)
                        Console.WriteLine("Their fears have been calmed.");
                    else //Fail State
                    {
                        badOmens = true;
                        Console.WriteLine("You have failed to calm the worries of the fleet.");
                        Console.Write("Would you like to use a fate point to try again? (y/n): ");
                        string omenFailResponse = Console.ReadLine();
                        if (omenFailResponse == "y" || omenFailResponse == "yes")
                        {
                            Console.Clear();
                            goto Omens;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("You have failed to calm the worries of the fleet.");
                        }
                    }
                }
                else
                {
                    badOmens = true;
                    Console.WriteLine("Their cowardice is their own. We sail.");
                }
            }
            else //Pass State
                Console.WriteLine("The fleet is unafraid.");

            Console.ReadLine();
            if (journeyStability == "Haunted Passage. (+10 to any Warp Hallucination Rolls).")
                Console.WriteLine("Your fleet enters the Warp.\nAll players and important NPC's must roll Willpower Tests.\nIncrease any Failures by 1 degree (10)");
            else
                Console.WriteLine("Your fleet enters the Warp.\nAll players and important NPC's must roll Willpower Tests.");
            Console.ReadLine();
            Console.Clear();

            //Translation and then Locating the Astronomican
            Console.WriteLine("\"A purple bruise in space yawns open. The Captain gives the order.\"\n\n\"You begin to sail the warp.\"");
            Console.ReadLine();
            Console.Clear();

        Translation:
            if (journeyStability == "Lightless Path. (Astromican is not visible to the Navigator).")
                astronomicanVisible = false;

            if (easyWarp == true)
                psyniscienceBonus += 20;

            if (astronomicanVisible == false)
                psyniscienceBonus += -20;

            int navigationWarpBonus = 0;
            int psyniscienceRoll2 = roll.Next(1, 101);
            int psyniscienceTestLA = theNavigator.Perception + theNavigator.Psyniscience + psyniscienceBonus;
            if (psyniscienceTestLA >= psyniscienceRoll2)//Pass State
            {
                astronomicanVisible = true;
                navigationWarpBonus += 10;
            }
            else//Fail State
            {
                astronomicanVisible = false;
                navigationWarpBonus += -20;
                Console.Write("The Navigator cannot locate the astronomican...\nUse a fate point to retry? (y/n): ");
                string astronomicanFail = Console.ReadLine();
                if (astronomicanFail == "y" || astronomicanFail == "yes")
                {
                    astronomicanVisible = true;
                    psyniscienceBonus = 0;
                    goto Translation;
                }
            }

            //Steering:
            int navigationWarpScore = trueVoyageDuration(theNavigator, navigationWarpBonus);
            int navigationWarpTest = roll.Next(1, 101);

            int navWarpCompare = navigationWarpTest - navigationWarpScore;

            int actualJourneyDays = trueJourneyDays(journeyStats.journeyDays, navWarpCompare);

            Console.WriteLine("Trip Total Duration: {0} days.", actualJourneyDays);
            Console.ReadLine();
            Console.Clear();

            //Warp_Travel_Encounters:

            journeyStats.warpEncounters = actualJourneyDays / 5;
            if (journeyStats.warpEncounters < 1)
                journeyStats.warpEncounters += 1;
            int daysElapsed = 0;
            bool onTarget = false;
            bool offCourse = false;
            bool severelyOC = false;

            //CORE LOOP STARTS HERE:
            for (int Count = 0; Count < journeyStats.warpEncounters; Count++)
            {
                string warpShenanigans = warpEncountersTable(easyWarp, astronomicanVisible, badOmens);
                string warpIncursions = warpIncursionTable();
                string shenanigansEffect = warpEncountersEffect(warpShenanigans, theNavigator, psyniscienceBonus, warpIncursions);
                Console.WriteLine(warpShenanigans);
                if (warpShenanigans != "All's Well. (No Encounter)")
                {
                    int checkItRoll = roll.Next(1, 101);
                    int persCheck = theNavigator.Perception + psyniscienceBonus;
                    if (persCheck >= checkItRoll)
                        shenanigansEffect = "The Navigator skillfully avoids the danger in the Warp.";
                }

                Console.WriteLine("");
                if (astronomicanVisible == true && warpShenanigans == "All's Well. (No Encounter)")
                    shenanigansEffect = "Smooth Sailing.";

                Console.WriteLine(shenanigansEffect);
                if (shenanigansEffect == "The Navigator is able to relocate the Astronomican.")
                    astronomicanVisible = true;
                else if (shenanigansEffect == "The ship becomes adrift in a warp rift.")
                {
                    int daysAdded = roll.Next(1, 6);
                    journeyStats.journeyDays += daysAdded;
                    if (daysAdded >= 5)
                        journeyStats.warpEncounters += 1;
                    daysElapsed += daysAdded;
                    fleetStats.Supplies -= daysAdded;
                    Console.WriteLine("");
                    Console.WriteLine("The fleet is adrift for {0} days.", daysAdded);
                }
                else if (shenanigansEffect == "The fleet runs afoul of a deep Warp Rift.")
                {
                    int riftDays = roll.Next(1, 11);
                    journeyStats.journeyDays += riftDays;
                    if (riftDays >= 5)
                        journeyStats.warpEncounters += 1;
                    if (riftDays >= 10)
                        journeyStats.warpEncounters += 1;
                    Console.WriteLine("");
                    Console.WriteLine("Your fleet is mired for {0} days.", riftDays);
                    for (int RD = 0; RD < riftDays; RD++)
                    {
                        string riftIncursion = warpIncursionTable();
                        Console.WriteLine("");
                        Console.WriteLine("Day {0}:\n\n{1}", RD, riftIncursion);
                        daysElapsed += 1;
                        fleetStats.Supplies -= 1;
                        fleetStats.Morale -= 1;
                        Console.ReadLine();
                    }
                }
                else if (shenanigansEffect == "The fleet falls into a Temporal Hole back to reality")
                {
                    Console.ReadLine();
                    severelyOC = true;
                    goto Severely_Off_Course;
                }
                else if (shenanigansEffect == "The fleet runs afoul of a dense Warp Rift.")
                {
                    int riftDaysMajor = roll.Next(1, 11) + 4;
                    journeyStats.journeyDays += riftDaysMajor;
                    if (riftDaysMajor >= 5)
                        journeyStats.warpEncounters += 1;
                    if (riftDaysMajor >= 10)
                        journeyStats.warpEncounters += 1;
                    if (riftDaysMajor >= 15)
                        journeyStats.warpEncounters += 1;
                    Console.WriteLine("");
                    Console.WriteLine("Your fleet is mired for {0} days.", riftDaysMajor);
                    for (int RDM = 0; RDM < riftDaysMajor; RDM++)
                    {
                        string riftIncursion2 = warpIncursionTable();
                        Console.WriteLine("");
                        Console.WriteLine("Day {0}:\n\n{1}", RDM, riftIncursion2);
                        daysElapsed += 1;
                        fleetStats.Supplies -= 1;
                        fleetStats.Morale -= 1;
                        Console.ReadLine();
                    }
                }
                else if (shenanigansEffect == "Your fleet falls into a massive churning Warp Storm around a Temporal Hole!")
                {
                    int warpStormMajor = roll.Next(3, 10);
                    Console.WriteLine("");
                    Console.WriteLine("The fleet is rocked by a huge Warp Storm. All ships take {0} critical damage and then tumble out of the Warp!", warpStormMajor);
                    Console.ReadLine();
                    severelyOC = true;
                    goto Severely_Off_Course;
                }

                if(warpIncursions == "Warp Sickness: Disease spreads through the ship like wildfire. For each day that passes roll 1d10." +
                "\nOn a roll of a 1-5 lose that many morale and strength. On a roll of 6-10 nothing happens." +
                "\nThe Explorers can attempt to cure the disease with a Medicae Test at -40.")
                {
                    int warpSickTime = 1;

                    Console.Write("What is the Intelligence of your best Physician?: ");
                    fleetStats.MedicInt = Convert.ToInt32(Console.ReadLine());
                    Console.Write("What is the Medicae Bonus of your best Physician?: ");
                    fleetStats.MedicaeBonus = Convert.ToInt32(Console.ReadLine());

                    int medicaeTest = fleetStats.MedicInt + fleetStats.MedicaeBonus;

                    for (int Ck = 0; Ck < warpSickTime; Ck++)
                    {
                        Console.WriteLine("Day: {0}", Ck + 1);

                        int sickRoll = roll.Next(1, 11);
                        if (sickRoll<= 5)
                        {
                            fleetStats.Morale -= sickRoll;
                            fleetStats.Strength -= sickRoll;
                            Console.WriteLine("The sickness wreaks havoc on your fleet. Today you lose {0}% morale and crew.", sickRoll);
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Everyone is sick, but none die this day.");
                            Console.ReadLine();
                            Console.Clear();
                        }

                        daysElapsed += 1;
                        fleetStats.Supplies -= 1;

                        int medicaeRoll = roll.Next(1, 101);
                        if (medicaeTest >= medicaeRoll)//Pass State
                        {
                            Console.WriteLine("Your Physician has worked out a cure!\nWith the crew healing, the fleet moves on.");
                            fleetStats.Morale += 2;
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            warpSickTime += 1;
                            fleetStats.Morale -= 1;
                            Console.WriteLine("The Physician is stumped. Unable to stem the disease as it runs rampant.");
                            Console.ReadLine();
                            Console.Clear();
                        }

                    }
                }

                Console.ReadLine();
                if (actualJourneyDays < 5)
                    daysElapsed += actualJourneyDays;
                else
                    daysElapsed += 5;

                Console.WriteLine("You have travelled {0} days in the warp", daysElapsed);
                fleetStats.Supplies -= 5;
                Console.WriteLine("You have {0} days of food left.", fleetStats.Supplies);
                if (fleetStats.Supplies <= 0)
                {
                    int moraleLoss = roll.Next(2, 12);
                    int crewLoss = roll.Next(1, 11) + 4;
                    fleetStats.Morale -= moraleLoss;
                    fleetStats.Strength -= crewLoss;
                    Console.WriteLine("");
                    Console.WriteLine("The fleet is starving!");
                    Console.WriteLine("");
                    Console.WriteLine("Crew Strength: {0}%\nCrew Morale: {1}%", fleetStats.Strength, fleetStats.Morale);
                }

                if (fleetStats.Morale <= 26)
                {
                    int mutinyLoss = roll.Next(1, 11);
                    Console.WriteLine("");
                    Console.WriteLine("A mutiny has broken out in the fleet!" +
                        "\n{0}% of your crew was lost to the fighting.", mutinyLoss);
                    fleetStats.Strength -= mutinyLoss;
                }

                if (fleetStats.Strength <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("All hands lost. Your fleet becomes a tomb drifting in the Warp.");
                    System.Environment.Exit(0);
                }

                Console.ReadLine();
                Console.Clear();
            }
            //END OF CORE LOOP.

            //Re-entry Generation:
            int astroLocateRoll = roll.Next(1, 101);
            int navWarpEscape = roll.Next(1, 101);

            if (daysElapsed == actualJourneyDays ||
                astronomicanVisible == true ||
                navigationWarpScore - 20 >= navWarpEscape)
                onTarget = true;
            else
            {
                int offTargetRoll = roll.Next(1, 101);
                if (psyniscienceTestLA >= offTargetRoll)
                    offCourse = true;
                else
                    severelyOC = true;
            }

            Console.WriteLine("Your journey lasted {0} days.\n", actualJourneyDays);
            Console.ReadLine();

            //On_Target:
            if (onTarget == true)
            {
                Console.WriteLine("Your fleet has arrived on the edge of your destination system.");
                Console.WriteLine("\nCurrent Morale: {0}%\nCurrent Crew Strength: {1}%",
                    fleetStats.Morale,
                    fleetStats.Strength);
                Console.ReadLine();
                Console.Clear();
            }

            //Slightly_Off_Course:
            if (offCourse == true)
            {
                string offCourseResult = slightlyOffCourseTable();
                Console.WriteLine(offCourseResult);
                Console.WriteLine("\nCurrent Morale: {0}%\nCurrent Crew Strength: {1}%",
                    fleetStats.Morale,
                    fleetStats.Strength);
                Console.ReadLine();
                Console.Clear();
            }

        Severely_Off_Course:
            if (severelyOC == true)
            {
                string offCourseResult2 = severelyOCTable();
                Console.WriteLine(offCourseResult2);
                Console.WriteLine("\nCurrent Morale: {0}%\nCurrent Crew Strength: {1}%",
                    fleetStats.Morale,
                    fleetStats.Strength);
                Console.ReadLine();
                Console.Clear();
            }

        EndText:
            Console.Write("Your journey is complete. Would you like to start \"again\", or \"quit\"?: ");
            string userInputEnd = Console.ReadLine();
            if (userInputEnd == "again")
            {
                Console.Clear();
                goto Start;
            }
            else if (userInputEnd == "quit")
                System.Environment.Exit(0);
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid Entry");
                Console.ReadLine();
                Console.Clear();
                goto EndText;
            }

        }

        public static string severelyOCTable()
        {
            Random roll = new Random();
            int offCourseChoice = roll.Next(1, 11);
            int d10DaysRoll = roll.Next(1, 11) + 2;
            int veryLongDaysRoll = roll.Next(3, 20) * 10;
            int yearsRoll = roll.Next(1, 6);

            var severelyOCLookup = new Dictionary<int, string>
            {
                {1, "You are "+d10DaysRoll+" days from your destination system." },
                {2, "You are nearby your destination. "+d10DaysRoll+" days by Warp."+
                "\nOr "+veryLongDaysRoll+" days in real space." },
                {3, "You are nearby your destination. "+d10DaysRoll+" days by Warp."+
                "\nOr "+veryLongDaysRoll+" days in real space." },
                {4, "You're in the same region as your destination. But far off course." },
                {5, "You're in the same region as your destination. But far off course." },
                {6, "You've drifted far afield. You aren't even in the same region as before." },
                {7, "You've drifted far afield. You aren't even in the same region as before." },
                {8, "This looks nothing like where you were trying to go... And the date is "+yearsRoll+
                " years before you left!"},
                {9, "This looks nothing like where you were trying to go... And the date is "+yearsRoll+
                " years after you left!"},
                {10, "The way out was a lie... You are still in the warp. Lost for all time." }
            };

            return severelyOCLookup[offCourseChoice];
        }

        public static string slightlyOffCourseTable()
        {
            Random roll = new Random();

            int offCourseChoice = roll.Next(1, 11);
            int d5DaysRoll = roll.Next(1, 6);
            int d10DaysRoll = roll.Next(1, 11);
            int longDaysRoll = roll.Next(2, 13) * 10;

            var slightlyOffCourseLookup = new Dictionary<int, string>
            {
                {1, "You are "+d5DaysRoll+" days from your destination system." },
                {2, "You are "+d5DaysRoll+" days from your destination system." },
                {3, "You are "+d5DaysRoll+" days from your destination system." },
                {4, "You are "+d10DaysRoll+" days from your destination system." },
                {5, "You are "+d10DaysRoll+" days from your destination system." },
                {6, "You are "+d10DaysRoll+" days from your destination system." },
                {7, "You are nearby your destination. "+d5DaysRoll+" days by Warp."+
                "\nOr "+longDaysRoll+" days in real space."},
                {8, "You are nearby your destination. "+d5DaysRoll+" days by Warp."+
                "\nOr "+longDaysRoll+" days in real space."},
                {9, "You're in the same region as your destination. But far off course." },
                {10, "You're in the same region as your destination. But far off course." },
            };

            return slightlyOffCourseLookup[offCourseChoice];
        }

        public static string warpIncursionTable()
        {
            Random roll = new Random();
            int madnessDays = roll.Next(1, 6);
            int daemonNumber = roll.Next(1, 6);

            int warpIncRoll = roll.Next(1, 11);

            var warpIncursions = new Dictionary<int, string>
            {
                {1, "Swarming Malice: Choose one component at random to infest on one of the ships in the fleet."+
                "\nThis component becomes inoperable until either the ship exits the Warp, or is cleansed by a holy ritual."},
                {2, "Swarming Malice: Choose one component at random to infest on one of the ships in the fleet."+
                "\nThis component becomes inoperable until either the ship exits the Warp, or is cleansed by a holy ritual." },
                {3, "Posession: Each character must make a Willpower Test. If the Gellar Field is active this test is +30,"+
                "\nif it is damaged this test is +0, if it is destroyed it is -30. The character who fails by the most degrees"+
                "\nis possessed by a Daemon. They must now embark on a secret mission to destroy the ship."},
                {4, "Posession: Each character must make a Willpower Test. If the Gellar Field is active this test is +30,"+
                "\nif it is damaged this test is +0, if it is destroyed it is -30. The character who fails by the most degrees"+
                "\nis possessed by a Daemon. They must now embark on a secret mission to destroy the ship." },
                {5, "Plague of Madness: Insanity spreads through the vessels in the fleet for "+madnessDays+" days."+
                "\nAll players must make a Willpower Test at +0. Those who fail are also affected by the madness." },
                {6, "Plague of Madness: Insanity spreads through the vessels in the fleet for "+madnessDays+" days."+
                "\nAll players must make a Willpower Test at +0. Those who fail are also affected by the madness." },
                {7, "Daemonic Incursion: "+daemonNumber+" Ebon Geist Daemon(s) materialize(s) on the ship and begin attacking crew."+
                "\nEbon Geist Profile is on pg. 378 of the Rogue Trader Core Book."},
                {8, "Daemonic Incursion: "+daemonNumber+" Ebon Geist Daemon(s) materialize(s) on the ship and begin attacking crew."+
                "\nEbon Geist Profile is on pg. 378 of the Rogue Trader Core Book."},
                {9, "Warp Sickness: Disease spreads through the ship like wildfire. For each day that passes roll 1d10."+
                "\nOn a roll of a 1-5 lose that many morale and strength. On a roll of 6-10 nothing happens."+
                "\nThe Explorers can attempt to cure the disease with a Medicae Test at -40."},
                {10, "Warp Monster: The fury of the warp manifests in a gargantuan creature of tentacles and teeth."+
                "\nIt wraps itself around the hull of one of your ships and begins to cause untold damage."+
                "\nThis creature attacks with Strength 4 weapons, 1d10+4 damage. Critical Rating of 4, Armour 14, Hull integrity 60."}
            };

            return warpIncursions[warpIncRoll];
        }

        public static string warpEncountersEffect(string encResult, Navigator methNav, int psynBonus, string warpIncurs)
        {
            Random roll = new Random();

            string effectResults = null;

            if (encResult == "All's Well. (No Encounter)")
            {
                int astroTest = roll.Next(1, 101);
                int psynScore = methNav.Perception + methNav.Psyniscience + psynBonus;

                if (psynScore >= astroTest)
                    effectResults = "The Navigator is able to relocate the Astronomican.";
                else
                    effectResults = "The journey is smooth, but the Astronomican still eludes the Navigator";
            }
            else if (encResult == "Delusion Mirage. (Psychic Encounter)")
            {
                effectResults = "All players must roll a Willpower Test.\nIf the Gellar Field is active this test is at +30, if not -30." +
                    "\nThose who fail must roll on the Warp Hallucinations Table. Degrees of failure are added to the roll total.";
            }
            else if (encResult == "Psychic Predators. (Psychic Encounter)")
                effectResults = warpIncurs;
            else if (encResult == "Stasis. (Phyisical Encounter)")
                effectResults = "The ship becomes adrift in a warp rift.";
            else if (encResult == "Spontaneous Inhuman Combustion.")
            {
                var shipComponents = new Dictionary<int, string>
                {
                    {1, "Crew Quarters" },
                    {2, "Macrocannons" },
                    {3, "Lance Battery" },
                    {4, "Main Engine" },
                    {5, "Cargo Hold" },
                    {6, "Gellar Field Generator" },
                    {7, "Void Shield Array" },
                    {8, "Augury Array" },
                    {9, "Life Sustainer Systems" }
                };

                int componentChosen = roll.Next(1, 10);

                string componentOnFire = shipComponents[componentChosen];
                effectResults = "A serious fire breaks out in the " + componentOnFire + " area! It must be put out before it does serious damage!";
            }
            else if (encResult == "Warp Storm. (Phyiscal Encounter)")
            {
                int warpStormCrit = roll.Next(1, 9);
                effectResults = "The fleet is rocked by a Warp Storm. A random ship takes " + warpStormCrit + " critical damage.";
            }
            else if (encResult == "Aethiric Reef. (Physical Encounter)")
            {
                int reefDamage = roll.Next(1, 11) + 3;
                effectResults = "The fleet runs aground on fragments of false reality. A random ship takes " + reefDamage + " damage that ignores" +
                    "\narmour. If the Gellar Field is damaged add +2 to the damage. If it is destroyed add +4";
            }
            else if (encResult == "Warp Rift. (Physical Encounter)")
                effectResults = "The fleet runs afoul of a deep Warp Rift.";
            else if (encResult == "Temporal Hole. (Physical Encounter)")
                effectResults = "The fleet falls into a Temporal Hole back to reality";
            else if (encResult == "Major Warp Storm. (Physical Encounter)")
            {
                int warpStormMajor = roll.Next(2, 10);
                effectResults = "The fleet is devastated by a major Warp Storm. All ships take " + warpStormMajor + " critical damage.";
            }
            else if (encResult == "Dense Warp Rift. (Physical Encounter)")
                effectResults = "The fleet runs afoul of a dense Warp Rift.";
            else if (encResult == "Major Warp Storm around a Temporal Hole. (Physical Encounter)")
                effectResults = "Your fleet falls into a massive churning Warp Storm around a Temporal Hole!";
            else if (encResult == "Gellar Field Failure. (Nightmare Encounter)")
            {
                effectResults = "By some nefarious turn of events, or machinations of the Ruinous Powers, your Gellar Field fails." +
                    "\nThe GM must engineer all manner of horrors to befall the crew while they desperately get the field up again.";
            }


            return effectResults;
        }

        public static string warpEncountersTable(bool ezWarp, bool astroVis, bool bOmens)
        {
            Random roll = new Random();

            int warpEncBonus = 0;
            if (ezWarp == true)
                warpEncBonus += -2;

            if (astroVis == false)
                warpEncBonus += 3;
            if (bOmens == true)
                warpEncBonus += 1;

            int warpEncRoll = roll.Next(1, 11) + warpEncBonus;

            var warpEncounters = new Dictionary<int, string>
            {
                {0, "All's Well. (No Encounter)" },
                {1, "All's Well. (No Encounter)" },
                {2, "All's Well. (No Encounter)" },
                {3, "Delusion Mirage. (Psychic Encounter)" },
                {4, "Psychic Predators. (Psychic Encounter)" },
                {5, "Stasis. (Phyisical Encounter)" },
                {6, "Spontaneous Inhuman Combustion." },
                {7, "Warp Storm. (Phyiscal Encounter)" },
                {8, "Aethiric Reef. (Physical Encounter)" },
                {9, "Warp Rift. (Physical Encounter)" },
                {10, "Temporal Hole. (Physical Encounter)" },
                {11, "Major Warp Storm. (Physical Encounter)" },
                {12, "Dense Warp Rift. (Physical Encounter)" },
                {13, "Major Warp Storm around a Temporal Hole. (Physical Encounter)" },
                {14, "Gellar Field Failure. (Nightmare Encounter)" }
            };

            if (warpEncRoll < 0)
                warpEncRoll = 0;

            return warpEncounters[warpEncRoll];

        }

        public static int trueJourneyDays(int navDays, int navWarpComp)
        {
            if (navWarpComp >= 20)
                return navDays * 4;
            else if (navWarpComp >= 10 && navWarpComp < 20)
                return navDays * 3;
            else if (navWarpComp >= 1 && navWarpComp < 10)
                return navDays * 2;
            else if (navWarpComp <= -10 && navWarpComp > -20)
                return (navDays / 4) * 3;
            else if (navWarpComp <= -20 && navWarpComp > -30)
                return navDays / 2;
            else if (navWarpComp <= -30)
                return navDays / 4;
            else
                return navDays;
        }

        public static int trueVoyageDuration(Navigator nWS, int nWBns)
        {
            return nWS.Intelligence + nWS.NavigationWarp + nWBns;
        }

        private static string routeStability()
        {
            Random roll = new Random();

            var stablLookup = new Dictionary<int, string>
            {
                {1, "Stable Route. (+10 to chart this route)." },
                {2, "Stable Route. (+10 to chart this route)." },
                {3, "Stable Route. (+10 to chart this route)." },
                {4, "Indirect Path." },//*2 to Journey Duration
                {5, "Indirect Path." },//*2 to Journey Duration
                {6, "Haunted Passage. (+10 to any Warp Hallucination Rolls)." },//*2 to Journey Duration, must advise players making these rolls, or build into tool.
                {7, "Surly Route. (-10 to Psyniscience to Divining Auguries Roll)." },//*2 to Journey Duration. Also must add effect to Psyniscience Test.
                {8, "Untraceable Trail. (Route cannot be charted)." },//*2 to Journey Duration.
                {9, "Lightless Path. (Astromican is not visible to the Navigator)." },//*2 to Journey Duration. Also must force failure on test to find Astronomican.
                {10, "Byzantine Route." }//*3 to Journey Duration.
            };

            return stablLookup[roll.Next(1, 11)];
        }

        private static int journeyDuration()
        {
            Random roll = new Random();

            var durationLookup = new Dictionary<int, int>
            {
                {1, roll.Next(1,6) },
                {2, roll.Next(1,6) },
                {3, roll.Next(1,6)+5 },
                {4, roll.Next(1,6)+5 },
                {5, roll.Next(2,20)+10 },
                {6, roll.Next(2,20)+10 },
                {7, roll.Next(3,30)+50 },
                {8, roll.Next(3,30)+50 },
                {9, roll.Next(4,40)+150 },
                {10, roll.Next(5,50)+250 }
            };

            return durationLookup[roll.Next(1, 11)];
        }

        private static string divingAuguries()
        {
            Random roll = new Random();

            var auguriesLookup = new Dictionary<int, string>
            {
                {1, "The clarity of the Astronomican is uncertain."},//x1 for journeyDays
                {2, "The clarity of the Astronomican is difficult."},//half journeyDays
                {3, "The clarity of the Astronomican is clouded."},//*2 journeyDays
                {4, "The Astronomican is obscured." },//*3 journeyDays
                {5, "The Astronican is visible..." }//*3 journeyDays
            };

            return auguriesLookup[roll.Next(1, 6)];
        }

    }

    class Navigator
    {
        public int Intelligence { get; set; }
        public int NavigationWarp { get; set; }
        public int Perception { get; set; }
        public int Psyniscience { get; set; }
    }

    class theFleet
    {
        public int Morale { get; set; }
        public int Strength { get; set; }
        public string Quality { get; set; }
        public int CaptainCommand { get; set; }
        public int MissionaryCharm { get; set; }
        public int Supplies { get; set; }
        public int MedicInt { get; set; }
        public int MedicaeBonus { get; set; }
    }

    class theJourney
    {
        public int journeyDays { get; set; }
        public int warpEncounters { get; set; }
    }
}

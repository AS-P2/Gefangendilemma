using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gefangenendilemma.Basis;

namespace Gefangenendilemma
{
    /// <summary>
    /// Diese Klasse können Sie beliebig umschreiben, jenachdem welche Tasks sie erledigen.
    /// </summary>
    class VerwaltungProgramm
    {
        /// <summary>
        /// Diese Liste(Collection) enthält alle Gefangene/Strategien
        /// </summary>
        private static List<BasisStrategie> _strategien;

        static void Main(string[] args)
        {
            //Spielstart
            Console.Title = "Gefangendilemma";

            //bekannt machen der ganzen strategien
            _strategien = new List<BasisStrategie>();
            _strategien.Add(new GrollStrategie());
            _strategien.Add(new VerrateImmerStrategie());
            _strategien.Add(new StrategieTFTT());
            _strategien.Add(new Niggos_Geheimtaktik());
            _strategien.Add(new Strategie3());

            string eingabe;
            do
            {
                // Begrüßung
                Console.WriteLine("Willkommen zum Gefangenendilemma");
                Console.WriteLine("SvS - Verhör zwischen 2 Strategien");
                Console.WriteLine("PvS - Verhör gegen eine Strategie");
                Console.WriteLine("X - Beenden");

                // Eingabe
                Console.Write("Treffen Sie ihre Option: ");
                eingabe = Console.ReadLine();

                // Auswerten der Eingabe
                switch (eingabe.ToLower())
                {
                    case "svs":
                        Gefangene2();
                        break;
                    case "pvs":
                        PlayerVsStrategie();
                        break;
                    case "X":
                        break;
                    default:
                        Console.WriteLine($"Eingabe {eingabe} nicht erkannt.");
                        break;
                }
            } while (!"x".Equals(eingabe?.ToLower()));
        }

        /// <summary>
        /// Fragt 2 Strategien, Länge und Schwere ab.
        /// </summary>
        static void Gefangene2()
        {
            //Rundenstart
            try { Console.Clear(); }
            catch (IOException e) { }

            int st1, st2;
            int spiele;

            Console.WriteLine("Willkommen zum Verhör zwischen 2 Strategien\nStrategien:");
            for (int i = 0; i < _strategien.Count; i++)
            {
                Console.WriteLine($"{i} - {_strategien[i].Name()}");
            }
            Console.WriteLine("\nWählen Sie ihre 2 Strategien:");
            st1 = VerwaltungKram.EingabeZahlMinMax("Wählen Sie die 1. Strategie", 0, _strategien.Count);
            st2 = VerwaltungKram.EingabeZahlMinMax("Wählen Sie die 2. Strategie", 0, _strategien.Count);
            spiele = VerwaltungKram.EingabeZahlMinMax("Wie viele Spiele sollen gespielt werden?", 1, 10);

            for (int i = 0; i < spiele; ++i)
                Spiel(i, st1, st2, i == spiele - 1);

        }

        /// <summary>
        /// Startet ein Spiel mit neuer Wahl für Rundenanzahl und Schwere.
        /// </summary>
        /// <param name="spiel"></param>
        /// <param name="st1"></param>
        /// <param name="st2"></param>
        /// <param name="lastGame"></param>
        static void Spiel(int spiel, int st1, int st2, bool lastGame)
        {

            int punkte1 = 0, punkte2 = 0;

            BasisStrategie strategie1 = _strategien[st1];
            BasisStrategie strategie2 = _strategien[st2];


            int runde = VerwaltungKram.EingabeZahlMinMax("Wie viele Runden sollen diese verhört werden?", 1, 101);
            int schwere = VerwaltungKram.EingabeZahlMinMax("Wie schwer sind die Verstöße? (0=leicht  | 1=mittel | 2=schwer)", 0, 3);

            Verhoer(spiel, ref strategie1, ref strategie2, runde, schwere, ref punkte1, ref punkte2);

            //ausgabe
            Console.WriteLine($"Spiel {spiel}:");
            Console.WriteLine($"{strategie1.Name()} hat {punkte1} Punkte erhalten.");
            Console.WriteLine($"{strategie2.Name()} hat {punkte2} Punkte erhalten.");

            if (lastGame)
                if (punkte1 < punkte2)
                {
                    Console.WriteLine("Somit hat {0} gewonnen.", strategie1.Name());
                }
                else if (punkte1 == punkte2)
                {
                    Console.WriteLine("Somit steht es unentschieden.");
                }
                else
                {
                    Console.WriteLine("Somit hat {0} gewonnen.", strategie2.Name());
                }
        }

        /// <summary>
        /// Startet ein Verhör zwischen der Strategie an der Position st1 und Position st2 über die Länge von runde und der Schwere schwere
        /// </summary>
        /// <param name="spiel"></param>
        /// <param name="strategie1"></param>
        /// <param name="strategie2"></param>
        /// <param name="runde"></param>
        /// <param name="schwere"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void Verhoer(int spiel, ref BasisStrategie strategie1, ref BasisStrategie strategie2, int runde, int schwere, ref int punkte1, ref int punkte2)
        {
            //holt die beiden Strategien aus der Collection.

            //setzt Startwerte
            int reaktion1 = BasisStrategie.NochNichtVerhoert;
            int reaktion2 = BasisStrategie.NochNichtVerhoert;

            //beide Strategien über den Start informieren (Also es wird die Startmethode aufgerufen)
            strategie1.Start(runde, schwere);
            strategie2.Start(runde, schwere);

            Console.WriteLine($"Verhör zwischen {strategie1.Name()} und {strategie2.Name()} für {runde} Runden.");

            //start
            for (int i = 0; i < runde; i++)
            {
                //beide verhören
                int aktReaktion1 = strategie1.Verhoer(reaktion2);
                int aktReaktion2 = strategie2.Verhoer(reaktion1);

                //punkte berechnen
                switch (schwere)
                {
                    case 0:
                        VerhoerLeichtPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                        break;
                    case 1:
                        VerhoerMittelPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                        break;
                    case 2:
                        VerhoerSchwerPunkte(aktReaktion1, aktReaktion2, ref punkte1, ref punkte2);
                        break;
                }

                //reaktion für den nächsten durchlauf merken
                reaktion1 = aktReaktion1;
                reaktion2 = aktReaktion2;
            }

        }

        /// <summary>
        /// Fragt gegnerische Strategie, Länge, Schwere und Spielername ab und startet den Verhör.
        /// </summary>
        static void PlayerVsStrategie ()
        {
            //Rundenstart
            try { Console.Clear(); }
            catch (IOException e) { }

            int st;
            int runde, schwere;
            string spieler;

            Console.WriteLine("Willkommen zum Verhör gegen eine Strategie\nStrategien:");
            for (int i = 0; i < _strategien.Count; i++)
            {
                Console.WriteLine($"{i} - {_strategien[i].Name()}");
            }
            st = VerwaltungKram.EingabeZahlMinMax("Wählen Sie die 1. Strategie", 0, _strategien.Count);
            runde = VerwaltungKram.EingabeZahlMinMax("Wie viele Runden sollen verhoert werden?", 1, 101);
            schwere = VerwaltungKram.EingabeZahlMinMax("Wie schwer sind die Verstöße? (0=leicht  | 1=mittel | 2=schwer)", 0, 3);
            Console.WriteLine("Geben sie ihren Spielernamen ein: ");
            spieler = Console.ReadLine();

            PlayerVerhoer(st, runde, schwere, spieler);

            //Rundenende
            Console.ReadKey();
            try { Console.Clear(); }
            catch (IOException e) { }
        }

        /// <summary>
        /// Startet ein Verhör zwischen der Strategie an der Position st und einem Spieler über die Länge von runde und der Schwere schwere
        /// </summary>
        /// <param name="st">Angabe der gegnerischen Strategie</param>
        /// <param name="runde">Anzahl der Runden die verhört wird</param>
        /// <param name="schwere">Schwere des Verbrechens</param>
        /// <param name="spieler"> Gibt den Spielernamen an</param>
        static void PlayerVerhoer (int st, int runde, int schwere, string spieler)
        {
            //Strategie festlegen
            BasisStrategie strategie = _strategien[st];

            //Startwerte festlegen
            int reaktionS = BasisStrategie.NochNichtVerhoert;
            int reaktionP = BasisStrategie.NochNichtVerhoert;
            int punkteS = 0, punkteP = 0;

            //Strategie wird über Start informiert (Startmethode wird aufgerufen)
            strategie.Start(runde, schwere);

            Console.WriteLine($"\nVerhör zwischen {strategie.Name()} und Spieler {spieler} für {runde} Runden.");

            //start
            for (int i = 0;i < runde;i++)
            {
                //Rundenstart
                Console.WriteLine($"\nRunde {i+1}:");

                //aktuelle Reaktionen
                int aktReaktionS = strategie.Verhoer(reaktionP);
                int aktReaktionP = BasisStrategie.NochNichtVerhoert;

                //Reaktion des Spielers
                string eingabe;
                do
                {
                    Console.WriteLine("Wollen sie kooperieren (k) oder verraten (v): ");
                    Console.Write("Ihre Reaktion: ");
                    eingabe = Console.ReadLine();
                    switch (eingabe.ToLower())
                    {
                        case "k":
                            aktReaktionP = BasisStrategie.Kooperieren;
                            break;
                        case "v":
                            aktReaktionP = BasisStrategie.Verrat;
                            break;
                        default:
                            Console.WriteLine($"Eingabe {eingabe} nicht erkannt. Bitte geben sie \"k\" für kooperieren oder \"v\" für verraten an.");
                            break;
                    }
                } while (aktReaktionP == BasisStrategie.NochNichtVerhoert);

                //punkte berechnen
                switch (schwere)
                {
                    case 0:
                        VerhoerLeichtPunkte(aktReaktionS, aktReaktionP, ref punkteS, ref punkteP);
                        break;
                    case 1:
                        VerhoerMittelPunkte(aktReaktionS, aktReaktionP, ref punkteS, ref punkteP);
                        break;
                    case 2:
                        VerhoerSchwerPunkte(aktReaktionS, aktReaktionP, ref punkteS, ref punkteP);
                        break;
                }

                //Rundenende
                switch (aktReaktionP)
                {
                    case BasisStrategie.Kooperieren:
                        Console.WriteLine("Reaktion des Gegners: k");
                        break;
                    case BasisStrategie.Verrat:
                        Console.WriteLine("Reaktion des Gegners: v");
                        break;
                }
                Console.WriteLine($"{spieler}: {punkteP}\t|\t{strategie.Name()}: {punkteS}");

                //reaktion für den nächsten durchlauf merken
                reaktionP = aktReaktionP;
                reaktionS = aktReaktionS;
            }

            if (punkteP < punkteS)
            {
                Console.WriteLine("Somit hat {0} gewonnen.", spieler);
            }
            else if (punkteP == punkteS)
            {
                Console.WriteLine("Somit steht es unentschieden.");
            }
            else
            {
                Console.WriteLine("Somit hat {0} gewonnen.", strategie.Name());
            }
        }

        /// <summary>
        /// Berechnet für schwere Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// </summary>
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerSchwerPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 4;
                punkte2 += 4;
                return;
            } 
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 0;
                punkte2 += 10;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 10;
                punkte2 += 0;
                return;
            }
            else
            {
                punkte1 += 8;
                punkte2 += 8;
                return;
            }
        }

        /// <summary>
        /// Berechnet für mittlere Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// </summary>
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerMittelPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 10;
                punkte2 += 10;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 8;
                punkte2 += 0;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 0;
                punkte2 += 8;
                return;
            }
            else
            {
                punkte1 += 4;
                punkte2 += 4;
                return;
            }
        }

        /// <summary>
        /// Berechnet für leichte Verstöße die Punkte und verwendet die 2 letzten Eingabeparameter als Rückgabe
        /// </summary>
        /// <param name="aktReaktion1"></param>
        /// <param name="aktReaktion2"></param>
        /// <param name="punkte1"></param>
        /// <param name="punkte2"></param>
        static void VerhoerLeichtPunkte(int aktReaktion1, int aktReaktion2, ref int punkte1, ref int punkte2)
        {
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 3;
                punkte2 += 3;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Verrat && aktReaktion2 == BasisStrategie.Kooperieren)
            {
                punkte1 += 0;
                punkte2 += 9;
                return;
            }
            if (aktReaktion1 == BasisStrategie.Kooperieren && aktReaktion2 == BasisStrategie.Verrat)
            {
                punkte1 += 9;
                punkte2 += 0;
                return;
            }
            else
            {
                punkte1 += 6;
                punkte2 += 6;
                return;
            }
        }
    }
}
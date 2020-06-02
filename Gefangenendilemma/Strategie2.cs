using Gefangenendilemma.Basis;

namespace Gefangenendilemma
{
    public class Strategie2 : BasisStrategie
    {
        /// <summary>
        /// Gibt den Namen der Strategie zurück, wichtig zum Anzeigen für die Auswahl
        /// </summary>
        /// <returns></returns>
        /// 
        int count = 0;
        private bool easy;
        private bool middle;
        private bool hard;
        private int rounds;
        private int dieserunde;
        private bool nurnochkooperieren;

        public override string Name()
        {
            return "Niggos Geheimtaktik";
        }

        /// <summary>
        /// Gibt den Namen des Autors der Strategie zurück, wichtig für die Turnierpart um den Sieger zu ermitteln.
        /// </summary>
        /// <returns></returns>
        public override string Autor()
        {
            return "Nicolas Marks";
        }

        /// <summary>
        /// Teilt mit, dass ein Verhoer jetzt startet
        /// </summary>
        /// <param name="runde">Anzahl der Runden, die verhört wird</param>
        /// <param name="schwere">Schwere des Verbrechen (VLeicht = 0, VMittel = 1, VSchwer = 2)</param>
        public override void Start(int runde, int schwere)
        {
            if (schwere == 0)
            {
                easy = true;
            }
            else if (schwere == 1)
            {
                middle = true;
            }
            else
            {
                hard = true;
            }
            rounds = runde;
        }

        /// <summary>
        /// Verhoert einen Gefangenen
        /// </summary>
        /// <param name="letzteReaktion">Reaktion des anderen Gefangenen, die Runde davor (NochNichtVerhoert = -1, Kooperieren = 0, Verrat = 1)</param>
        /// <returns>Gibt die eigene Reaktion für diese Runde zurück (Kooperieren = 0, Verrat = 1)</returns>
        public override int Verhoer(int letzteReaktion)
        {
            dieserunde++;
            if (nurnochkooperieren)
            {
                return Kooperieren;
            }
            if (easy && rounds == 5)                                    // gewinnt hiermit gegen Groll auf einfach
            {
                if (dieserunde == 5)                                    // && letzteReaktion == Kooperieren
                {
                    return Verrat;
                }
                if (letzteReaktion == NochNichtVerhoert)
                {
                    return Kooperieren;
                }
                else if (letzteReaktion == Kooperieren)
                {
                    return Kooperieren;
                }
                else if (letzteReaktion == Verrat)
                {
                    return Verrat;
                }
            }
            else if (easy && rounds == 25)
            {

                if (dieserunde == 25)                                   // && letzteReaktion == Kooperieren
                {
                    return Verrat;
                }
                else
                {

                    if (dieserunde == 1)
                    {
                        return Verrat;
                    }
                    else
                    {
                        return letzteReaktion;
                    }
                }
            }
            else if (easy && rounds == 100)
            {
                if (dieserunde == 1)
                {
                    return Kooperieren;
                }
                else
                {
                    if (dieserunde == 100)      //  && letzteReaktion == Kooperieren
                    {
                        return Verrat;
                    }
                    else
                    {
                        return Verrat;
                    }
                }
            }
            else if (middle && rounds == 5)
            {
                if (dieserunde == 5 && letzteReaktion == Verrat)
                {
                    return Verrat;
                }
                else
                {
                    return Kooperieren;
                }
            }
            else if (middle && rounds == 25)
            {
                if (dieserunde == 1)
                {
                    return Kooperieren;
                }
                if (dieserunde >= 2 && letzteReaktion == Verrat)
                {
                    nurnochkooperieren = true;
                }
                else
                {
                    //return NochNichtVerhoert;
                }
            }
            else if (middle && rounds == 100)
            {
                if (letzteReaktion == Kooperieren)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
                if (count == 9)
                {
                    count = 0;
                    return Verrat;
                }
                if (dieserunde == 1)
                {
                    return Kooperieren;
                }
                else
                {
                    if (letzteReaktion == Verrat)
                    {
                        nurnochkooperieren = true;
                    }
                }
            }
            else if (hard && rounds == 5)                               // gewinnt hiermit gegen Groll auf einfach
            {
                if (dieserunde == 5)                                    // && letzteReaktion == Kooperieren
                {
                    return Verrat;
                }
                if (letzteReaktion == NochNichtVerhoert)
                {
                    return Kooperieren;
                }
                else if (letzteReaktion == Kooperieren)
                {
                    return Kooperieren;
                }
                else if (letzteReaktion == Verrat)
                {

                    return Verrat;
                }
            }
            else if (hard && rounds == 25)
            {

                if (dieserunde == 25)                                   // && letzteReaktion == Kooperieren
                {
                    return Verrat;
                }
                else
                {

                    if (dieserunde == 1)
                    {
                        return Verrat;
                    }
                    else
                    {
                        return letzteReaktion;
                    }
                }
            }
            else if (hard && rounds == 100)
            {
                if (dieserunde == 1)
                {
                    return Kooperieren;
                }
                else
                {
                    if (dieserunde == 100)                              //  && letzteReaktion == Kooperieren
                    {
                        return Verrat;
                    }
                    else
                    {
                        return Verrat;
                    }
                }
            }
            return 0;
        }
    }
}
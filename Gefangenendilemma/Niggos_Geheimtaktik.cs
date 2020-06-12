using Gefangenendilemma.Basis;

namespace Gefangenendilemma
{
    public class Niggos_Geheimtaktik : BasisStrategie
    {
        /// <summary>
        /// Gibt den Namen der Strategie zurück, wichtig zum Anzeigen für die Auswahl
        /// </summary>
        /// <returns></returns>
        /// 
        int count = 0;
        private bool easy = false;
        private bool middle = false;
        private bool hard = false;
        private int rounds = 0;
        static int dieserunde = 0;
        private bool nurnochkooperieren = false;

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
            resetValues();
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
            if (easy)                                   
            {
                if (rounds == 5)                                            // gewinnt hiermit gegen Groll
                {
                    if (dieserunde == 5)
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
                else if (rounds == 25)
                {

                    if (dieserunde == 25)
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
                else if (rounds == 100)
                {
                    if (dieserunde == 1)
                    {
                        return Kooperieren;
                    }
                    else
                    {
                        if (dieserunde == 100)
                        {
                            return Verrat;
                        }
                        else
                        {
                            return Verrat;
                        }
                    }
                }
            }
            else if (middle)                
            {
                if (rounds == 5)                    //gewinnt gegen VerrateImmer
                {

                    nurnochkooperieren = true;      //damit er nicht immer alle if's durchgehen muss

                }
                else if (rounds == 25)
                {
                    nurnochkooperieren = true;
                }
                else if (rounds == 100)                     //gewinnt gegen Groll
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
            }





            else if (hard)
            {
                if (rounds == 5)
                {
                    if (dieserunde == 1)
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
                else if (rounds == 25)
                {

                    if (dieserunde == 1)
                    {
                        return Kooperieren;
                    }
                    else
                    {

                        if (dieserunde == 25)
                        {
                            return Verrat;
                        }
                        else
                        {
                            return letzteReaktion;
                        }
                    }
                }
                else if (rounds == 100)
                {
                    if (dieserunde == 1)
                    {
                        return Kooperieren;
                    }
                    else
                    {
                        if (dieserunde == 100)
                        {
                            return Verrat;
                        }
                        else
                        {
                            return letzteReaktion;
                        }
                    }
                }

            }


            return Kooperieren;
        }
        private void resetValues()
        {
            count = 0;
            easy = false;
            middle = false;
            hard = false;
            rounds = 0;
            dieserunde = 0;
            nurnochkooperieren = false;
        }
    }
}
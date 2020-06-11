using Gefangenendilemma.Basis;
using System;

namespace Gefangenendilemma
{
    /// <summary>
    /// Nur nutzen, wenn es ein 3. Gruppenmitglied gibt.
    /// Die Strategy basiert auf Ergebnissen von verschiedener Algorhitmen, die im Turniermodus getestet wurden.
    /// Bei kurzen Spielen bis zu fünf Runden wird auf Random zurückgegriffen.
    /// Ab sechs Runden aufwärst wird der Tit-for-Tat-Algorhitmus verwendet, mit der kleinen Modifikation, dass die letzte Runde immer ein Verrat ist.
    /// An die Aufgabenstellung angepasst, wird Verrate-Immer umgedreht.
    /// </summary>
    public class Strategie3 : BasisStrategie
    {
        private int schwere;
        private int runde;
        private int count;

        /// <summary>
        /// Gibt den Namen der Strategie zurück, wichtig zum Anzeigen für die Auswahl
        /// </summary>
        /// <returns></returns>
        public override string Name()
        {
            return "Hybrid";
        }

        /// <summary>
        /// Gibt den Namen des Autors der Strategie zurück, wichtig für die Turnierpart um den Sieger zu ermitteln.
        /// </summary>
        /// <returns></returns>
        public override string Autor()
        {
            return "Marvin Mahn";
        }

        /// <summary>
        /// Teilt mit, dass ein Verhoer jetzt startet
        /// </summary>
        /// <param name="runde">Anzahl der Runden, die verhört wird</param>
        /// <param name="schwere">Schwere des Verbrechen (VLeicht = 0, VMittel = 1, VSchwer = 2)</param>
        public override void Start(int runde, int schwere)
        {
            // Schwierigkeit und Rundenanzahl merken
            this.schwere = schwere;
            this.runde = runde;
            this.count = 0;
        }

        /// <summary>
        /// Verhoert einen Gefangenen
        /// </summary>
        /// <param name="letzteReaktion">Reaktion des anderen Gefangenen, die Runde davor (NochNichtVerhoert = -1, Kooperieren = 0, Verrat = 1)</param>
        /// <returns>Gibt die eigene Reaktion für diese Runde zurück (Kooperieren = 0, Verrat = 1)</returns>
        public override int Verhoer(int letzteReaktion)
        {
            ++this.count;
            // Bei kurzen Spielen Random anwenden
            if (this.runde < 6)
                return new Random().Next(0, 2);
            // Bei längeren Spielen auf gutmütiges Tit-for-Tat mit modifizierten Verhalten zurückgreifen (letzte Runde immer Verraten)
            return this.count == this.runde ? Verrat : letzteReaktion == -1 ? Kooperieren : letzteReaktion;
        }
    }
}
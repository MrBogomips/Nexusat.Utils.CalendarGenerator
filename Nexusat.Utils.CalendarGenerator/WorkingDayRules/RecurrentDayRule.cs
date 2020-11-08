using System;
using System.Runtime.Serialization;

namespace Nexusat.Utils.CalendarGenerator
{
    //TODO: regola per i giorni ricorrenti
    //      possibilita di specicare:
    //      - Ogni LUN,MAR,… DOM della settimana
    //      - Ogni 1, 2,… 31 di GEN, FEB, … DEC
    //      - Ogni 1, 2, 365, 366, ULTIMO dell' ANNO  ??
    //      - Ogni primo, secondo, terzo, quarto, ultimo LUN, MAR, … DOM del mese
    // Parser con Sintassi stile CRON [ANNO] [MESE] [GIORNO] [SETT/1=F,2,3,4,5,L]
    // Hint:
    //    Mi conviene fare una Rule WeekdayRule e gestire i giorni della settimana (compreso FIRST e LAST del mese)
    //          Parser [SETT/1=F,2,3,4,5,L]
    //          Esempi:  Monday,Tuesday,Wednasday => Ogni Lun, mar, mercol
    //                   Monday/1, Friday/L => Ogni primo lunedì e ultimo venerdì del mese
    //                   * => Ogni giorno della settimana
    //    e una Rule YearlyRule e gestire i giorni del mese eventualmente specificando l'anno
    //          Parser [ANNO] [MESE] [GIORNO]
    //          Esempi:
    //                 * * * => Ogni giorno            => Alias: @everyday
    //                 * January 1 => Ogni primo gennaio
    //                 * * 1 => Ogni primo del mese
    //                 * * L => Ogni ultimo del mese
    //                 * January,February 1 => Ogni primo gennaio e febbraio
    //                 * December 25 => Ogni natale
    //                 @easter => Ogni pasqua   (????????????)
    //                 B January 1 => Ogni anno bisestile, il primo di gennaio
    //                 b December 31 => Ogni anno non bisestile, l'ultimo dell'anno
    //                 2021,2023 January 1,L => Nel 2021 e 2023 nel primo e ultimo giorno di gennaio
    [DataContract(Namespace = "http://www.nexusat.it/schemas/calendar")]
    public class RecurrentDayRule : DayRule
    {
        public override DayInfo GetDayInfo(DateTime date)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetDayInfo(DateTime date, out DayInfo dayInfo)
        {
            throw new NotImplementedException();
        }
    }
}
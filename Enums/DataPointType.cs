using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfKThenX
{
    public enum DataPointType
    {
        Dpt1 = 1,       
        Dpt2 = 2,       
        Dpt3 = 3,       
        Dpt4 = 4,       
        Dpt5 = 5,      
        Dpt6 = 6,    
        Dpt7 = 7,      
        Dpt8 = 8,      
        Dpt9 = 9,       
        Dpt10 = 10,     
        Dpt11 = 11,     
        Dpt12 = 12,     
        Dpt13 = 13,     
        Dpt14 = 14,     
        Dpt15 = 15,     
        Dpt16 = 16,

        //https://www.promotic.eu/en/pmdoc/Subsystems/Comm/PmDrivers/KNXDTypes.htm

        //https://support.knx.org/hc/de/articles/115001133744-Datenpunkttyp
        //Die am häufigsten verwendeten Datenpunkttypen sind:

        //1.yyy = boolesch, wie Schalten, Bewegen nach oben/unten, Schritt
        //2.yyy = 2 x boolesch, z. B. Schalten + Prioritätssteuerung
        //3.yyy = boolesch + vorzeichenloser 3-Bit-Wert, z. B. Auf-/Abdimmen
        //4.yyy = Zeichen(8 - Bit)
        //5.yyy = vorzeichenloser 8-Bit-Wert, wie Dimm-Wert (0..100 %), Jalousienposition (0..100 %)
        //6.yyy = 8 - Bit - 2 - Komplement, z. B. %
        //7.yyy = 2 x vorzeichenloser 8-Bit-Wert, z. B. Impulszähler
        //8.yyy = 2 x 8-Bit-2-Komplement, z. B. %
        //9.yyy = 16 - Bit - Gleitkommazahl, z. B. Temperatur
        //10.yyy = Uhrzeit
        //11.yyy = Datum
        //12.yyy = 4 x vorzeichenloser 8-Bit-Wert, z. B. Impulszähler
        //13.yyy = 4 x 8-Bit-2-Komplement, z. B. Impulszähler
        //14.yyy = 32 - Bit - Gleitkommazahl, z. B. Temperatur
        //15.yyy = Zugangskontrolle
        //16.yyy = String-> 14 Zeichen (14 x 8-Bit)
        //17.yyy = Szenennummer
        //18.yyy = Szenensteuerung
        //19.yyy = Uhrzeit + Datum
        //20.yyy = 8 - Bit - Nummerierung, z. B. HLK-Modus („Automatik“, „Komfort“, „Stand-by“, „Sparen“, „Schutz“)
    }
}

using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Common
{
    public static class Reparti
    {
        public const string Confezionamento = "CONF";
        public const string Modelleria = "MOD";
        public const string ControlloQualitaPost1 = "CTRL.P.1";
        public const string ControlloQualitaPost2 = "CTRL.P.2";
        public const string Pressofusione = "PRES";
        public const string ControlloCollaudo = "CHK";
        public const string Slegatura = "SLEGAT";
        public const string PulimentaturaTF = "PUL TF";
        public const string Stampaggio = "STAMP";
        public const string Saldatura = "SALD";
        public const string Magazzino = "MAG";
        public const string VibraturaTF = "VIBR TF";
        public const string Piegafilo = "PEGF";
        public const string Pulimentatura = "PULI";
        public const string Tranciatura = "TRAN";
        public const string Montaggio = "MONT";
        public const string Riprese = "RIPR";
        public const string Verniciatura = "VERNI";
        public const string Vibratura = "VIBR";
        public const string GalvanicaAuto = "GALVA";
        public const string Floccatura = "FLOC";
        public const string Legatura = "LEGAT";
        public const string Tornitura = "TORN";
        public const string PVD = "PVD";
        public const string Smaltatura = "02694";

        public static List<RWListItem> CreaListaReparti()
        {
            List<RWListItem> lista = new List<RWListItem>();
            lista.Add(new RWListItem(LeggiEtichetta(Confezionamento), Confezionamento));
            lista.Add(new RWListItem(LeggiEtichetta(Modelleria), Modelleria));
            lista.Add(new RWListItem(LeggiEtichetta(ControlloQualitaPost1), ControlloQualitaPost1));
            lista.Add(new RWListItem(LeggiEtichetta(Pressofusione), Pressofusione));
            lista.Add(new RWListItem(LeggiEtichetta(Slegatura), Slegatura));
            lista.Add(new RWListItem(LeggiEtichetta(Stampaggio), Stampaggio));
            lista.Add(new RWListItem(LeggiEtichetta(Saldatura), Saldatura));
            lista.Add(new RWListItem(LeggiEtichetta(Piegafilo), Piegafilo));
            lista.Add(new RWListItem(LeggiEtichetta(Pulimentatura), Pulimentatura));
            lista.Add(new RWListItem(LeggiEtichetta(Tranciatura), Tranciatura));
            lista.Add(new RWListItem(LeggiEtichetta(Verniciatura), Verniciatura));
            lista.Add(new RWListItem(LeggiEtichetta(GalvanicaAuto), GalvanicaAuto));
            lista.Add(new RWListItem(LeggiEtichetta(Legatura), Legatura));
            lista.Add(new RWListItem(LeggiEtichetta(PVD), PVD));
            lista.Add(new RWListItem(LeggiEtichetta(Tornitura), Tornitura));
            lista.Add(new RWListItem(LeggiEtichetta(Vibratura), Vibratura));

            return lista;
        }

        public static string LeggiEtichetta(string Reparto)
        {
            switch (Reparto)
            {
                case Reparti.Confezionamento:
                    return "Confezionamento";
                case Reparti.Modelleria:
                    return "Modelleria";
                case Reparti.ControlloQualitaPost1:
                    return "Controllo Qualita P. 1";
                case Reparti.ControlloQualitaPost2:
                    return "Controllo Qualita P. 2";
                case Reparti.Pressofusione:
                    return "Pressofusione";
                case Reparti.ControlloCollaudo:
                    return "Controllo e collaudo";
                case Reparti.Slegatura:
                    return "Slegatura";
                case Reparti.PulimentaturaTF:
                    return "Pulimentatura TF";
                case Reparti.Stampaggio:
                    return "Stampaggio";
                case Reparti.Saldatura:
                    return "Saldatura";
                case Reparti.Magazzino:
                    return "Magazzino";
                case Reparti.VibraturaTF:
                    return "Vibratura TF";
                case Reparti.Piegafilo:
                    return "Piegafilo";
                case Reparti.Pulimentatura:
                    return "Pulimentatura";
                case Reparti.Tranciatura:
                    return "Tranciatura";
                case Reparti.Montaggio:
                    return "Montaggio";
                case Reparti.Riprese:
                    return "Riprese";
                case Reparti.Verniciatura:
                    return "Verniciatura";
                case Reparti.Vibratura:
                    return "Vibratura";
                case Reparti.GalvanicaAuto:
                    return "Galvanica";
                case Reparti.Floccatura:
                    return "Floccatura";
                case Reparti.Legatura:
                    return "Legatura";
                case Reparti.Tornitura:
                    return "Tornitura";
                case Reparti.PVD:
                    return "PVD";


                default:
                    return string.Empty;


            }
        }
    }
}

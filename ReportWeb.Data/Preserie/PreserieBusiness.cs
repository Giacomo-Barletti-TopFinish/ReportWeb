﻿using ReportWeb.Common.Helpers;
using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Preserie
{
    public class PreserieBusiness : ReportWebBusinessBase
    {
        public PreserieBusiness() : base() { }

        [DataContext]
        public void TrovaCommessaPerNome(string nomeCommessa, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.TrovaCommessaPerNome(nomeCommessa, ds);
        }

        [DataContext]
        public void TrovaCommessaPerModello(string Modello, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.TrovaCommessaPerModello(Modello, ds);
        }

        [DataContext]
        public void FillMAGAZZ(PreserieDS ds, List<string> IDMAGAZZ)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillUSR_PRD_LANCIOD(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_LANCIOD(IDLANCIOD, ds);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(IDLANCIOD, ds);
        }
        [DataContext]
        public void FillUSR_PRD_FASI(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI(IDLANCIOD, ds);
        }
    }
}
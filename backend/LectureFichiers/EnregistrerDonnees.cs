using LireFichiers.Models;
using ReadConfig;
using System;
using System.Collections.Generic;
using ToolBox.DataAccess.DataBase;

// lecture des listes d'importation pour créer les enregistrements dans la BD
// --------------------------------------------------------------------------

namespace LireFichiers
{
    class EnregistrerDonnees
    {
        #region \taux de tva
        // -----------------

        public static void SaveVat(List<VatImport> tvas, GetConfig config)
        {
            Connection connect = new Connection(config.ConnectionString);

            foreach (VatImport v in tvas)
            {
                // vérifier si le taux de tva existe déjà dans la BD
                // -------------------------------------------------
                Command requete = new Command($"select ID from vat where  code = '{v.Code}'");
                var tva = connect.ExecuteScalar(requete);

                if (tva == null)
                {
                    requete = new Command($@"INSERT INTO vat (code, vatRate)
                                   values ({ v.Code }, {v.VatRate})");
                    connect.ExecuteScalar(requete);
                }
                else
                {
                    //TODO mise à jour du taux de TVA
                }
            }
        }
        #endregion

        #region \catégories
        // -----------------

        public static void SaveCategory(List<CategoryImport> categories, GetConfig config)
        {
            Connection connect = new Connection(config.ConnectionString);

            foreach (CategoryImport c in categories)
            {
                // vérifier si la catégorie existe dans la BD
                // ------------------------------------------
                Command requete = new Command($"select ID from category where  code = '{c.Code}'");
                var cat = connect.ExecuteScalar(requete);

                if (cat == null)
                {
                    requete = new Command($@"INSERT INTO category (code, name) values ({ c.Code }, 'nouvelle CAT')");
                    connect.ExecuteScalar(requete);

                    // récupérer l'ID de la nouvelle catégorie
                    //requete = new Command($"select ID from category where  code = '{c.Code}'");
                    //cat = connect.ExecuteScalar(requete);


                }
                
                // vérifier si le produit existe dans la BD
                // ----------------------------------------
                requete = new Command($"select ID from product where number = '{c.ProductNumber}'");
                var prod = connect.ExecuteScalar(requete);

                if (prod != null)
                {
                    // vérifier si un enregistrement existe déjà dans categoryProduct
                    // --------------------------------------------------------------
                    requete = new Command($@"select * from  categoryProduct where productNumber = '{c.ProductNumber}' and categoryCode = '{c.Code}'");
                    var prodCat = connect.ExecuteScalar(requete);

                    // création d'un nouveau enregistrement categoryProduct
                    // ----------------------------------------------------
                    if (prodCat == null)
                    { 
                    requete = new Command($@"INSERT INTO categoryProduct (categoryCode, productNumber)
                                   values ({c.Code}, {c.ProductNumber})");
                    connect.ExecuteScalar(requete);
                    }
                }
                else
                {
                    // erreur car produit n'existe pas
                }

            }
        }
        #endregion

        #region \produits
        // --------------
        public static void SaveProduct(List<ProductImport> produits, GetConfig config)
        {
            Connection connect = new Connection(config.ConnectionString);

            foreach (ProductImport p in produits)
            {
                // vérifier si l'article existe déjà dans la BD
                // --------------------------------------------
                Command requete = new Command($"select ID from product where number = '{p.Number}'");
                var prod = connect.ExecuteScalar(requete);

                if (prod == null)
                {
                    requete = new Command($@"INSERT INTO product (number, name, vatCode, unitCode, unitPrice) values ('{ p.Number }', '{p.Name}', {p.VatCode}, {p.UnitCode} ,{p.Price})");
                    connect.ExecuteScalar(requete);
                }
                else
                {
                    //TODO mise à jour de l'article
                }
            }
        }

        #endregion

        #region \barcodes
        // --------------
        public static void SaveBarcode(List<BarcodeImport> barcodes, GetConfig config)
        {
            Connection connect = new Connection(config.ConnectionString);

            foreach (BarcodeImport b in barcodes)
            {
                //Console.WriteLine($@"{b.Code,1} {b.ProductNumber,10}");

                // vérifier si le barcode existe déjà dans la BD
                // N.B. : normalement, il faut supprimer tous les barcodes existants de l'article
                // et ensuite les recrées car il n'est pas possible de vérifier à ce stade si un barcode
                // à été créer sur deux articles
                // -------------------------------------------------------------------------------------
                Command requete = new Command($"select ID from barcode where barcode = '{b.Code}'");
                var bar = connect.ExecuteScalar(requete);

                if (bar == null)
                {
                    requete = new Command($@"INSERT INTO barcode (barcode, productNumber)
                                   values ({b.Code}, {b.ProductNumber})");
                    connect.ExecuteScalar(requete);
                }
                else
                {
                    //TODO mise à jour du barcode

                }
            }

        }
        #endregion

        #region \statistiques
        // ------------------
        public static void SaveStats(List<StatImport> stats, string statDate, GetConfig config)
        {
            Connection connect = new Connection(config.ConnectionString);

            foreach (StatImport s in stats)
            {
                Command requete = new Command($@"INSERT INTO productStatistic 
                        (date, productNumber, name, netPrice, quantity, samt, ramt, tamt, damt, sexc, 
                            rexc, exc, svat, rvat, vat, reg) 
                        values 
                        ('{statDate}','{ s.Number }', '{s.Name}', {s.Price}, {s.Quantity}, {s.Samt}, 
                            {s.Ramt}, {s.Tamt}, {s.Damt}, {s.Sexc}, {s.Rexc}, {s.Exc}, {s.Svat}, {s.Rvat}, 
                            {s.Vat}, {s.Reg})");
                connect.ExecuteScalar(requete);
            }
        }
        #endregion
    }
}

using CsvHelper;
using LireFichiers.Models;
using ReadConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace LireFichiers
{
    class Program
    {
        static void Main(string[] args)
        {

            List<LogFile> logs = new List<LogFile>(); // fichier LOG

            //charger la configuration
            // -----------------------
            GetConfig config = new GetConfig();

            if (config.ConnectionString == null)
            {
                logs.Add(new LogFile(DateTime.Now, $"ERREUR --> connectionString backend pas trouvé"));
                //TODO arreter le traitement avec une erreur
            }

            string targetDirectory = config.appConfig.ImportPath;

            logs.Add(new LogFile(DateTime.Now, "début importation"));

            // ouvrir le répertoire avec les fichiers à importé.
            Environment.CurrentDirectory = (targetDirectory);
            logs.Add(new LogFile(DateTime.Now,$"répertoire des données --> {Directory.GetCurrentDirectory()}"));

            // TODO lire la table des blocs pour l'importation des données
            //------------------------------------------------------------


            // lire tous les fichiers et les traiter un à un 
            // ---------------------------------------------
            string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (string fileName in fileEntries)
            {
                //TODO - tester stationID et Rep_NR : --> à partir du nom du fichier
                //     - si nous avons déjà traité le fichier il faut interrompre l'importation
                ImportFile(fileName, logs, config);
                //lastImportNumber++;
            }

            // TODO enregistrer le numéro du dernier fichier importé dans le fichier de configuration

            // enregistrer le fichier LOG
            // --------------------------
            logs.Add(new LogFile(DateTime.Now, "fin importation"));
            targetDirectory = config.appConfig.LogPath;
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);
            string csvFile = $@"{targetDirectory}{DateTime.Now.Month:00}{DateTime.Now.Day:00}-{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}.log";

            if (File.Exists(csvFile)) File.Delete(csvFile);

            using (var writer = new StreamWriter(csvFile))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(logs);
            }

        }

        #region \Méthodes
        // --------------

        // -----------------------
        // traitement d'un fichier
        // -----------------------

        public static void ImportFile(string fileName, List<LogFile> logs, GetConfig config)
        {
            logs.Add(new LogFile(DateTime.Now, $"lecture fichier --> {fileName}"));
            Console.WriteLine($"importation du fichier {fileName}");

            // lecture de toutes les lignes
            // ----------------------------
            string[] rapJournalier = File.ReadAllLines(fileName);
            string[] input; // table utilisé pour le split de la ligne en cours avec regex
			string typ, plu, num;
            string statDate = "";
            int index, rechIndex;

            // listes temporaires utilisées pour ajouter les données dans la BD
            // ----------------------------------------------------------------
            List<ProductImport> products = new List<ProductImport>();
			ProductImport art = new ProductImport();
            List<StatImport> stats = new List<StatImport>();
            StatImport stat = new StatImport();
            List<BarcodeImport> barcodes = new List<BarcodeImport>();
			BarcodeImport bar = new BarcodeImport();
			List<VatImport> vats = new List<VatImport>();
			VatImport vat = new VatImport();
			List<CategoryImport> categories = new List<CategoryImport>();
			CategoryImport cat = new CategoryImport();


			// vérifier que le  fichier commence par [START FILE] et se termine par [END FILE]
			// -------------------------------------------------------------------------------
			IEnumerable<string> query = rapJournalier.Where(ligne => ligne.Contains("[START_FILE]")
																   || ligne.Contains("[END_FILE]"));

            if (query.Count() == 2)
            {
                // TODO vérifier le numéro de suite du fichier
                // -------------------------------------------
                query = rapJournalier.Where(ligne => ligne.Contains("[REP_NR]")
                                                  || ligne.Contains("[ACC_REP_NR]"));

                #region \STATUS
                // lire bloc [STATUS]
                // ------------------
                index = Array.IndexOf(rapJournalier, "[STATUS]");
                index++;

                while (index > 0 && !rapJournalier[index].Contains("[END_STATUS]"))
                {
                    input = rapJournalier[index].Split('=');
                    typ = Regex.Match(input[0], @"^[a-zA-Z]*").Value;
                    plu = Regex.Match(input[0], @"[0-9]+\.?[0-9,]*").Value;

                    if (typ == "CLOS") // date de fermeture du rapport = date de la journée des statistiques
                    {
                        if (input[1].Length == 14)
                        {
                            statDate = input[1].Substring(0, 8);
                        }
                        else
                        {
                            logs.Add(new LogFile(DateTime.Now, $"ERREUR --> Date d'ouverture pas correcte --> {input[1]}"));
                            //TODO gérer l'erreur de date
                        }
                    }

                    index++;
                }
                #endregion

                #region \VAT_INFO
                // lire bloc [VAT_INFO]
                // les taux de tva sont stockés dans la liste tauxTva pour traartIndexent ultérieur
                // ----------------------------------------------------------------------------
                index = Array.IndexOf(rapJournalier, "[VAT_INFO]");
                index++;

                while (index > 0 && !rapJournalier[index].Contains("[END_VAT_INFO]"))
                {
                    input = rapJournalier[index].Split('=');
                    typ = Regex.Match(input[0], @"^[a-zA-Z]*").Value;
                    plu = Regex.Match(input[0], @"[0-9]+\.?[0-9,]*").Value;
                    if (typ == "PRC")
                    {
                        vat.Code = plu;
                        vat.VatRate = input[1];
                        vats.Add(vat);
                    }
                    index++;
                }
                // exeception stations de service Tokheim : taux de TVA 0 % (cigarettes, lotto, ...) = code 9
                // ------------------------------------------------------------------------------------------
                vat = default(VatImport);
                vat.Code = "9";
                vat.VatRate = "0.00";
                vats.Add(vat);
                #endregion

                #region \FUEL_INFO
                // TODO lire bloc [FUEL_INFO]
                // --------------------------
                index = Array.IndexOf(rapJournalier, "[FUEL_INFO]");
                #endregion

                #region \ARTICLE_SOLD_INFO
                
                // TODO lire bloc [ARTICLE_SOLD_INFO]
                // ----------------------------------
                index = Array.IndexOf(rapJournalier, "[ARTICLE_SOLD_INFO]");
				index++;
                string pluEnCours = "";
                art = new ProductImport(9, "0.00");

                while (index > 0 && !rapJournalier[index].Contains("[END_ARTICLE_SOLD_INFO]"))
				{
					input = rapJournalier[index].Split('=');
					typ = Regex.Match(input[0], @"^[a-zA-Z]*").Value;
					plu = Regex.Match(input[0], @"[0-9]+\.?[0-9]*").Value;

                    // lorsque le PLU change il faut enregistrer les données du produit en cours
                    // -------------------------------------------------------------------------
                    if(pluEnCours != plu)
                    {
                        if (pluEnCours != "")
                        {
                            products.Add(art);
                            art = new ProductImport(9, "0.00");
                            pluEnCours = plu;
                            art.Number = plu;
                        }
                        else
                        {
                            pluEnCours = plu;
                            art.Number = plu;
                        }
                    }

                    switch (typ)
                    {
						case "NAM":
                            art.Name = input[1].Trim().Replace("'", "''"); // il faut ajouter un ' pour les noms contenant un '
                            break;
						case "CRD":
							cat.Code = input[1].Trim();
							cat.ProductNumber = plu;
							categories.Add(cat);
							break;
                        case "GRP":
                            // TODO traitement des consignes;
                            break;
                        case "REP":
							//TODO traitement pour type REP
							break;
						case "VAT":
							art.VatCode = Convert.ToInt16(input[1]);
							break;
                        case "DVAT":
                            //TODO traitement pour type DVAT
                            break;
                        case "PRI":
                            art.Price = input[1];
                            break;
                        case "DVAL":
							// TODO traitement des consignes;
							break;
                        case "RECUPR":
                            // TODO traitement pour RECUPR
                            break;
                        case "RECDVAL":
                            // TODO traitement pour RECDVAL
                            break;
                        case "RECPRI":
                            // TODO traitement pour RECPRI
                            break;
                        case "QUAUNIT":
							art.UnitCode = input[1].Trim();
							break;
						case "BAR":
                            if (input[1].TrimStart(new Char[] { '0' }).Length > 0 && Regex.IsMatch(input[1], @"[0-9]*$")) // supprime les zero avant le barcode et test si tout numérique
                            {
                                Match code = Regex.Match(input[1], @"[0-9]*$");
                                if (code.Success)
                                {
                                    bar.Code = code.Value.TrimStart(new Char[] { '0' });
                                    bar.ProductNumber = plu;
                                    barcodes.Add(bar);
                                }
                            }
							break;
						default:
                            logs.Add(new LogFile(DateTime.Now, $"WARNING --> ARTICLE_SOLD Type inconnu --> {typ} {plu}"));
							break;
					}
					index++;
                }

				// il faut enregistrer le dernier article traité
				if(art.Number != null) products.Add(art);

                // création d'un fichier LOG produits
                // ----------------------------------
                string targetDirectory = config.appConfig.LogPath;
                if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

                string csvFile = $@"{targetDirectory}{fileName.Substring(fileName.LastIndexOf('\\') + 1,8)}.prod";

                if (File.Exists(csvFile)) File.Delete(csvFile);

                using (var writer = new StreamWriter(csvFile))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(products);
                }

                // création d'un fichier LOG barcodes
                // ----------------------------------
                csvFile = $@"{targetDirectory}{fileName.Substring(fileName.LastIndexOf('\\') + 1, 8)}.bar";
                if (File.Exists(csvFile)) File.Delete(csvFile);
                using (var writer = new StreamWriter(csvFile))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(barcodes);
                }
                #endregion

                #region \SHOP_STATISTICS_BY_NR_DET

                // TODO lire bloc [SHOP_STATISTICS_BY_NR_DET]
                // ------------------------------------------
                index = Array.IndexOf(rapJournalier, "[SHOP_STATISTICS_BY_NR_DET]");
				index++;
                string numEnCours = "";
                stat = new StatImport(9, "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00",
                     "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00");

                while (index > 0 && !rapJournalier[index].Contains("[END_SHOP_STATISTICS_BY_NR_DET]"))
				{
					input = rapJournalier[index].Split('=');
					typ = Regex.Match(input[0], @"^[a-zA-Z]*").Value;
                    num = Regex.Match(input[0], @"[0-9]+\.?[0-9,]*").Value;

                    // lorsque le NUM change il faut enregistrer les stats du produit en cours
                    // -------------------------------------------------------------------------

                    if (numEnCours != num)
                    {
                        if (numEnCours != "")
                        {
                            stats.Add(stat);
                            stat = new StatImport(9, "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00",
                                 "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00");
                            numEnCours = num;
                        }
                        else
                        {
                            numEnCours = num;
                        }
                    }

                    switch (typ)
					{
						case "PLU":
                            stat.Number = input[1].Trim();
                            break;
                        case "NAM":
                            stat.Name = input[1].Trim().Replace("'", "''"); ;
                            break;
                        case "PRC":
                            rechIndex = vats.FindIndex(v =>  v.VatRate == input[1].Trim());
                            if (rechIndex > -1) stat.VatCode = Convert.ToInt16(vats[rechIndex].Code);
                            break;
                        case "PRI":
                            stat.Price = input[1];
							break;
						case "QUA":
                            stat.Quantity = input[1];
							break;
						case "SAMT":
                            stat.Samt = input[1];
							break;
                        case "RAMT":
                            stat.Ramt = input[1];
                            break;
                        case "TAMT":
                            stat.Tamt = input[1];
							break;
                        case "SDAMT":
                            stat.Sdamt = input[1];
                            break;
                        case "RDAMT":
                            stat.Rdamt = input[1];
                            break;
                        case "DAMT":
                            stat.Damt = input[1];
                            break;
                        case "SEXC":
                            stat.Sexc = input[1];
                            break;
                        case "REXC":
                            stat.Rexc = input[1];
                            break;
                        case "EXC":
                            stat.Exc = input[1];
							break;
						case "SVAT":
                            stat.Svat = input[1];
							break;
                        case "RVAT":
                            stat.Rvat = input[1];
                            break;
                        case "VAT":
                            stat.Vat = input[1];
                            break;
                        case "SREG":
                            stat.Sreg = input[1];                      
							break;
                        case "RREG":
                            stat.Rreg = input[1];
                            break;
                        case "REG":
                            stat.Reg = input[1];
                            break;
                        case "TAM":
                            // TODO Tester si TAM = somme des TAMTx - somme des DAMTx
                            break;
                        case "STAM":
                            // TODO Tester si STAM = somme des SAMTx
                            break;
                        case "RTAM":
                            // TODO Tester si RTAM = somme des RAMTx
                            break;
                        case "TTAM":
                            // TODO Tester si TTAM = somme des SAMTx - somme des RAMTx
                            break;
                        case "SDTAM":
                            // TODO Tester si SDTAM = somme des SDAMTx
                            break;
                        case "RDTAM":
                            // TODO Tester si RDTAM = somme des RDAMTx
                            break;
                        case "DTAM":
                            // TODO Tester si DTAM = somme des DAMTx
                            break;

                        // champs pas encore traités
                        // -------------------------
                        case "UPR":
						case "REP":
						case "GRP":
                        case "QUAUNIT":
                        case "SQUA":
						case "RQUA":
						case "DVAL":
						case "DPRC":
                        case "GAMT":
                        case "SGAMT":
						case "RGAMT":
						case "TGAMT":
                        case "AMT":
						case "TSQUA":
						case "TRQUA":
						case "TQUA":
							// TODO traitement spécifique pour chaque  type de ligne
							break;
						default:
                            logs.Add(new LogFile(DateTime.Now, $"WARNING --> STAT Type inconnu --> {typ}"));
							break;
					}
					index++;
				}

                // il faut enregistrer le dernier article traité
                if (stat.Number != null) stats.Add(stat);

                // écriture du fichier LOG
                // -------------------------
                targetDirectory = config.appConfig.LogPath;
                if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

                csvFile = $@"{targetDirectory}{fileName.Substring(fileName.LastIndexOf('\\') + 1, 8)}.stat";

                if (File.Exists(csvFile)) File.Delete(csvFile);

                using (var writer = new StreamWriter(csvFile))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(stats);
                }

                #endregion

                //foreach (Category c in ListeCategories)
                //{
                //    Console.WriteLine($"{c.Code,10} {c.ProductImportNumber,20}");
                //}

                // enregistrer les données dans la BD
                // ---------------------------------------
                if (vats.Count() > 0) EnregistrerDonnees.SaveVat(vats, config);
                if (products.Count() > 0) EnregistrerDonnees.SaveProduct(products, config);
                if (barcodes.Count() > 0) EnregistrerDonnees.SaveBarcode(barcodes, config);
                if (categories.Count() > 0) EnregistrerDonnees.SaveCategory(categories, config);
                if (stats.Count() > 0) EnregistrerDonnees.SaveStats(stats, statDate, config);

                // transférer le fichier traité dans un répertoire 'Backup'
                // --------------------------------------------------------
                targetDirectory = config.appConfig.BackupPath;
                if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);
				File.Move(fileName, targetDirectory + fileName.Substring(fileName.LastIndexOf('\\') + 1));

            }
			else
			{
                logs.Add(new LogFile(DateTime.Now, $"ERREUR --> le fichier {fileName} n'est pas complète !"));
            }
		}
        #endregion
    }
}

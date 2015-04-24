using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace projectModel
{
    public class XML_Database
    {
        public XML_Database()
        {}

        /// <summary>
        /// Nacte z XML souboru databazi majitelu i psu.
        /// </summary>
        /// <param name="databaseM"></param>
        /// <param name="databaseP"></param>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public bool load_XML(Majitele databaseM, Psi databaseP, string sourceFile)
        {
            try
            {
              //  string databasePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "projectData/database.xml");
                string databasePath = sourceFile;

                String[] majitelArrayData;
                String[] pesArrayData;
                XmlDocument doc = new XmlDocument();

                FileStream fileStrm = new FileStream(databasePath, FileMode.Open);
                doc.Load(fileStrm);
                fileStrm.Close();

                XmlNode root = doc.DocumentElement;

                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Majitele")
                    {
                        foreach (XmlNode nodeMajitel in node.ChildNodes)
                        {
                            majitelArrayData = new String[10];
                             Int32 i = 0;
                            foreach (XmlNode nodeMajitelElemnts in nodeMajitel)
                            {
                                majitelArrayData[i] = (String)nodeMajitelElemnts.InnerText;
                                i++;
                            }
                            databaseM.Add(new Majitel(majitelArrayData));
                        }
                    }
                    else if (node.Name == "Psi")
                    {
                        foreach (XmlNode nodePes in node.ChildNodes)
                        {
                            pesArrayData = new String[43];
                             Int32 i = 0;
                            foreach (XmlNode nodePesElements in nodePes)
                            {
                                pesArrayData[i] = (String)nodePesElements.InnerText;
                                i++;
                            }
                            databaseP.Add(new Pes(pesArrayData));
                        }
                    }
                }
            }
            catch (XmlException xmlEx)
            {
                Console.WriteLine("{0}", xmlEx.Message);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return false;
            }

            return true;
        }


        public bool save_XML(Majitele databaseM, Psi databaseP, string destinationFile)
        {
            try
            {
              //  string databasePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "projectData/database.xml");

                string databasePath = destinationFile;

                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlElement root = doc.CreateElement("DBS");
                doc.AppendChild(root);


                Majitel currentMajitel;

                int[] majitelId = databaseM.GetAllIds();
                
                XmlElement parentNode;
                XmlElement[] childNodes = new XmlElement[10];


                XmlElement majitelNode = doc.CreateElement("Majitele");
              //  doc.DocumentElement.AppendChild(root);

                for (int i = 0; i < majitelId.Length; i++)
                {
                    currentMajitel = databaseM.GetMajitelById(majitelId[i]);

                    parentNode = doc.CreateElement("Majitel");
                    childNodes[0] = doc.CreateElement("Id");
                    childNodes[0].InnerText = Convert.ToString(currentMajitel.id);
                    childNodes[1] = doc.CreateElement("FirstName");
                    childNodes[1].InnerText = currentMajitel.firstName;
                    childNodes[2] = doc.CreateElement("LastName");
                    childNodes[2].InnerText = currentMajitel.lastName;
                    childNodes[3] = doc.CreateElement("Clen");
                    childNodes[3].InnerText = currentMajitel.clen;
                    childNodes[4] = doc.CreateElement("Penize");
                    childNodes[4].InnerText = Convert.ToString(currentMajitel.penize);
                    childNodes[5] = doc.CreateElement("Potvrzeni");
                    childNodes[5].InnerText = currentMajitel.potvrzeni;
                    childNodes[6] = doc.CreateElement("Telefon");
                    childNodes[6].InnerText = currentMajitel.telefon;
                    childNodes[7] = doc.CreateElement("Email");
                    childNodes[7].InnerText = currentMajitel.email;
                    childNodes[8] = doc.CreateElement("Narodnost");
                    childNodes[8].InnerText = currentMajitel.narodnost;
                    childNodes[9] = doc.CreateElement("Pocet_psu");
                    childNodes[9].InnerText = Convert.ToString(currentMajitel.pocet_psu);

                    doc.DocumentElement.AppendChild(parentNode);

                    for (int j = 0; j < 10; j++)
                    {
                        doc.DocumentElement.LastChild.AppendChild(childNodes[j]);
                    }

                    doc.DocumentElement.AppendChild(majitelNode);
                    doc.DocumentElement.LastChild.AppendChild(parentNode);

                }

                Pes currentPes;
                int[] pesId = databaseP.GetAllIds();

                XmlElement pesNode = doc.CreateElement("Psi");

                childNodes = new XmlElement[43];


                for (int i = 0; i < pesId.Length; i++)
                {
                    currentPes = databaseP.GetPesById(pesId[i]);

                    parentNode = doc.CreateElement("Pes");
                    childNodes[0] = doc.CreateElement("Id");
                    childNodes[0].InnerText = Convert.ToString(currentPes.Id);
                    childNodes[1] = doc.CreateElement("Jmeno");
                    childNodes[1].InnerText = currentPes.Jmeno;
                    childNodes[2] = doc.CreateElement("Plemeno");
                    childNodes[2].InnerText = currentPes.Plemeno;
                    childNodes[3] = doc.CreateElement("Pohlavi");
                    childNodes[3].InnerText = currentPes.Pohlavi;
                    childNodes[4] = doc.CreateElement("FCI");
                    childNodes[4].InnerText = currentPes.Fci;
                    childNodes[5] = doc.CreateElement("Datum");
                    childNodes[5].InnerText = currentPes.Datum.ToString("d. M. yyyy");
                    childNodes[6] = doc.CreateElement("Licence");
                    childNodes[6].InnerText = currentPes.Licence;
                    childNodes[7] = doc.CreateElement("Majitel");
                    childNodes[7].InnerText = Convert.ToString(currentPes.Majitel);
                    childNodes[8] = doc.CreateElement("MajitelJmeno");
                    childNodes[8].InnerText = Convert.ToString(currentPes.MajitelJmeno);
                    childNodes[9] = doc.CreateElement("Skupina");
                    childNodes[9].InnerText = Convert.ToString(currentPes.Skupina);
                    childNodes[10] = doc.CreateElement("Start_beh1");
                    childNodes[10].InnerText = Convert.ToString(currentPes.StartBeh1);
                    childNodes[11] = doc.CreateElement("Start_beh2");
                    childNodes[11].InnerText = Convert.ToString(currentPes.StartBeh2);

                    childNodes[12] = doc.CreateElement("Zavod");
                    childNodes[12].InnerText = currentPes.ZavodLicence;
                    childNodes[13] = doc.CreateElement("Platba");
                    childNodes[13].InnerText = Convert.ToString(currentPes.Platba);
                    childNodes[14] = doc.CreateElement("Doplatit");
                    childNodes[14].InnerText = Convert.ToString(currentPes.Doplatit);
                    childNodes[15] = doc.CreateElement("Pozn");
                    childNodes[15].InnerText = currentPes.Poznamka;

                    childNodes[16] = doc.CreateElement("Diskvalifikace");
                    childNodes[16].InnerText = currentPes.Diskval;

                    //body první běh -->
                    childNodes[17] = doc.CreateElement("Agility0");
                    childNodes[17].InnerText = Convert.ToString(currentPes.AgilityA0);
                    childNodes[18] = doc.CreateElement("Speed0");
                    childNodes[18].InnerText = Convert.ToString(currentPes.SpeedA0);
                    childNodes[19] = doc.CreateElement("Endurance0");
                    childNodes[19].InnerText = Convert.ToString(currentPes.EnduranceA0);
                    childNodes[20] = doc.CreateElement("Enthusiasm0");
                    childNodes[20].InnerText = Convert.ToString(currentPes.EnthusiasmA0);
                    childNodes[21] = doc.CreateElement("Intelligence0");
                    childNodes[21].InnerText = Convert.ToString(currentPes.IntelligenceA0);

                    childNodes[22] = doc.CreateElement("Agility1");
                    childNodes[22].InnerText = Convert.ToString(currentPes.AgilityA1);
                    childNodes[23] = doc.CreateElement("Speed1");
                    childNodes[23].InnerText = Convert.ToString(currentPes.SpeedA1);
                    childNodes[24] = doc.CreateElement("Endurance1");
                    childNodes[24].InnerText = Convert.ToString(currentPes.EnduranceA1);
                    childNodes[25] = doc.CreateElement("Enthusiasm1");
                    childNodes[25].InnerText = Convert.ToString(currentPes.EnthusiasmA1);
                    childNodes[26] = doc.CreateElement("Intelligence1");
                    childNodes[26].InnerText = Convert.ToString(currentPes.IntelligenceA1);
                    childNodes[27] = doc.CreateElement("Body1celekm");
                    childNodes[27].InnerText = Convert.ToString(currentPes.Body1); //<-- body1

                    //body druhý běh -->
                    childNodes[28] = doc.CreateElement("Agility0_2kolo");
                    childNodes[28].InnerText = Convert.ToString(currentPes.AgilityB0);
                    childNodes[29] = doc.CreateElement("Speed0_2kolo");
                    childNodes[29].InnerText = Convert.ToString(currentPes.SpeedB0);
                    childNodes[30] = doc.CreateElement("Endurance0_2kolo");
                    childNodes[30].InnerText = Convert.ToString(currentPes.EnduranceB0);
                    childNodes[31] = doc.CreateElement("Enthusiasm0_2kolo");
                    childNodes[31].InnerText = Convert.ToString(currentPes.EnthusiasmB0);
                    childNodes[32] = doc.CreateElement("Intelligence0_2kolo");
                    childNodes[32].InnerText = Convert.ToString(currentPes.IntelligenceB0);

                    childNodes[33] = doc.CreateElement("Agility1_2kolo");
                    childNodes[33].InnerText = Convert.ToString(currentPes.AgilityB1);
                    childNodes[34] = doc.CreateElement("Speed1_2kolo");
                    childNodes[34].InnerText = Convert.ToString(currentPes.SpeedB1);
                    childNodes[35] = doc.CreateElement("Endurance1_2kolo");
                    childNodes[35].InnerText = Convert.ToString(currentPes.EnduranceB1);
                    childNodes[36] = doc.CreateElement("Enthusiasm1_2kolo");
                    childNodes[36].InnerText = Convert.ToString(currentPes.EnthusiasmB1);
                    childNodes[37] = doc.CreateElement("Intelligence1_2kolo");
                    childNodes[37].InnerText = Convert.ToString(currentPes.IntelligenceB1);
                    childNodes[38] = doc.CreateElement("Body2celekm");
                    childNodes[38].InnerText = Convert.ToString(currentPes.Body2); //<-- body2

                    childNodes[39] = doc.CreateElement("Dvojice_kolo1");
                    childNodes[39].InnerText = Convert.ToString(currentPes.Dvojice0);
                    childNodes[40] = doc.CreateElement("Dvojice_kolo2");
                    childNodes[40].InnerText = Convert.ToString(currentPes.Dvojice1);

                    childNodes[41] = doc.CreateElement("Barva_kolo1");
                    childNodes[41].InnerText = Convert.ToString(currentPes.Barva0);
                    childNodes[42] = doc.CreateElement("Barva_kolo2");
                    childNodes[42].InnerText = Convert.ToString(currentPes.Barva1);

                    doc.DocumentElement.AppendChild(parentNode);

                    for (int j = 0; j < 43; j++)
                    {
                        doc.DocumentElement.LastChild.AppendChild(childNodes[j]);
                    }

                    doc.DocumentElement.AppendChild(pesNode);
                    doc.DocumentElement.LastChild.AppendChild(parentNode);
                }
                doc.DocumentElement.AppendChild(pesNode);

                FileStream fileStrm = new FileStream(databasePath, FileMode.Create);
                doc.Save(fileStrm);
                fileStrm.Close();

            }
            catch (XmlException xmlEx)
            {
                Console.WriteLine("{0}", xmlEx.Message);
                return false;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                return false;
            }

            return true;
        }
    }
}

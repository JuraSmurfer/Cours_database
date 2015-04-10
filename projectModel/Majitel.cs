using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
    public class Majitel
    {
        /// <summary>
        /// Implicitní konstruktor.
        /// </summary>
        public Majitel()
        {
            this.id = 0;
            this.firstName = "";
            this.lastName = "";
            this.clen = "";
            this.penize = 0;
            this.potvrzeni = "";
            this.telefon = "";
            this.email = "";
            this.narodnost = "";
            this.pocet_psu = 0;
        }
        /// <summary>
        /// Konstruktor z pole řetězců.
        /// </summary>
        /// <param name="data"></param>
        public Majitel(String[] data)
        {
            this.id =  Int32.Parse(data[0]);
            this.firstName = data[1];
            this.lastName = data[2];
            this.clen = data[3];
            this.penize =  Int32.Parse(data[4]);
            this.potvrzeni = data[5];
            this.telefon = data[6];
            this.email = data[7];
            this.narodnost = data[8];
            this.pocet_psu = Int32.Parse(data[9]);
        }
        /// <summary>
        /// Kopykonstruktor.
        /// </summary>
        /// <param name="aMajitel"></param>
        public Majitel(Majitel aMajitel)
        {
            this.id = aMajitel.id;
            this.firstName = aMajitel.firstName;
            this.lastName = aMajitel.lastName;
            this.clen = aMajitel.clen;
            this.penize = aMajitel.penize;
            this.potvrzeni = aMajitel.potvrzeni;
            this.telefon = aMajitel.telefon;
            this.email = aMajitel.email;
            this.narodnost = aMajitel.narodnost;
            this.pocet_psu = aMajitel.pocet_psu;
        }
        public  Int32 id { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String clen { get; set; }
        public  Int32 penize { get; set; }
        public String potvrzeni { get; set; }
        public String telefon { get; set; }
        public String email { get; set; }
        public String narodnost { get; set; }
        public Int32 pocet_psu { get; set; }
    }
}

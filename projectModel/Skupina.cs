using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
    public class Skupina
    {
        public Skupina(Pes iPes1, Pes iPes2, Pes iPes3)
        {
            this.id = iPes1.Id; // WTF?! proč ID prvniho psa ?!

            this.pes1name = iPes1.Jmeno;
            this.pes2name = iPes2.Jmeno;
            this.pes3name = iPes3.Jmeno;

            this.pes1plemeno = iPes1.Plemeno;
            this.pes2plemeno = iPes2.Plemeno;
            this.pes3plemeno = iPes3.Plemeno;

            this.pes1barva = "červená";
            this.pes2barva = "bílá";
            this.pes3barva = "modrá";
        }

        public Skupina(Pes iPes1, Pes iPes2)
        {
            this.id = iPes1.Id;

            this.pes1name = iPes1.Jmeno;
            this.pes2name = iPes2.Jmeno;

            this.pes1plemeno = iPes1.Plemeno;
            this.pes2plemeno = iPes2.Plemeno;

            this.pes1barva = "červená";
            this.pes2barva = "bílá";
        }

        public  Int32 id { get; set; }
        public String pes1name { get; set; }
        public String pes2name { get; set; }
        public String pes3name { get; set; }
        public String pes1plemeno { get; set; }
        public String pes2plemeno { get; set; }
        public String pes3plemeno { get; set; }
        public String pes1barva { get; set; }
        public String pes2barva { get; set; }
        public String pes3barva { get; set; }
    }
}

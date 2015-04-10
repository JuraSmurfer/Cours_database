using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
    public class Majitele
    {
        /// <summary>
        /// Pole pro ulozeni objektu tridy Majitele
        /// </summary>
        private ArrayList majitele;

        /// <summary>
        /// Konstruktor tridy Majitele
        /// </summary>
        public Majitele()
        {
            this.majitele = new ArrayList();
        }

        /// <summary>
        /// Metoda vraci nove nejvyssi ID, ktere se v DB jeste nevyskytuje.
        /// </summary>
        /// <returns></returns>
        public int GetNewId()
        {
            int highestID = 0;

            foreach(Majitel majitel in majitele)
            {
                if (majitel.id > highestID)
                    highestID = majitel.id;
            }

            return highestID + 1;
        }

        /// <summary>
        /// Vraci majitele na zaklade vstupniho ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Majitel GetMajitelById(int id)
        {
            foreach (Majitel majitel in majitele)
            {
                if (majitel.id == id)
                    return majitel;
            }

            return null;
        }

        /// <summary>
        /// Vraci vsechny ID majitelu vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public int[] GetAllIds()
        {
            int[] ids = new int[this.majitele.Count];
            int i = 0;

            foreach (Majitel majitel in majitele)
            {
                ids[i] = majitel.id;
                i++;
            }

            return ids;
        }

        /// <summary>
        /// Prida majitele do databaze
        /// </summary>
        /// <param name="newMajitel"></param>
        public void Add(Majitel newMajitel)
        {
            this.majitele.Add(newMajitel);
        }

        /// <summary>
        /// Ziska data o majiteli s danym ID, upravi je a zpet ulozi.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newData"></param>
        public void Edit(int id, Majitel newData)
        {
            Majitel oldData = this.GetMajitelById(id);

            oldData.firstName = newData.firstName;
            oldData.lastName = newData.lastName;
            oldData.clen = newData.clen;
            oldData.penize = newData.penize;
            oldData.potvrzeni = newData.potvrzeni;
            oldData.telefon = newData.telefon;
            oldData.email = newData.email;
            oldData.narodnost = newData.narodnost;
            oldData.pocet_psu = newData.pocet_psu;
        }

        /// <summary>
        /// Smaze majitele dle daneho ID na vstupu.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            
            int i = 0;

            foreach (Majitel majitel in majitele)
            {
                if (majitel.id == id)
                    break;
                i++;
            }

            this.majitele.RemoveAt(i);
            
        }
        
        /// <summary>
        /// Najde jestli se dana polozka nachazi v databazi a vrati jeho ID.
        /// </summary>
        /// <param name="jmeno"></param>
        /// <param name="prijmeni"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public int FindSame(String jmeno, String prijmeni, String email)
        {
            foreach (Majitel majitel in majitele)
            {
                if ((majitel.firstName == jmeno) && (majitel.lastName == prijmeni) && (majitel.email == email))
                    return majitel.id;
            }
            return -1;
        }

        /// <summary>
        /// Vrací počet majitelů.
        /// </summary>
        /// <returns></returns>
        public int Length()
        {
            return majitele.Count;
        }
    }
}

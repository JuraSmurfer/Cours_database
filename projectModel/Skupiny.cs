using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
    public class Skupiny
    {
        /// <summary>
        /// Pole pro ulozeni objektu tridy Skupiny
        /// </summary>
        private ArrayList skupiny;

        /// <summary>
        /// Konstruktor tridy Skupiny
        /// </summary>
        public Skupiny()
        {
            this.skupiny = new ArrayList();
        }

        /// <summary>
        /// Metoda vraci nove nejvyssi ID, ktere se v DB jeste nevyskytuje.
        /// </summary>
        /// <returns></returns>
        public  Int32 GetNewId()
        {
            Int32 highestID = 0;

            foreach (Skupina skupina in skupiny)
            {
                if (skupina.id > highestID)
                    highestID = skupina.id;
            }

            return ++highestID;
        }

        /// <summary>
        /// Vraci skupinu s hledanym ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Skupina GetSkupinaById(int id)
        {
            foreach (Skupina skupina in skupiny)
            {
                if (skupina.id == id)
                    return skupina;
            }
            return null;
        }

        /// <summary>
        /// Vraci vsechny ID obsazene v DB
        /// </summary>
        /// <returns></returns>
        public int[] GetAllIds()
        {
            int[] ids = new int[this.skupiny.Count];
            Int32 i = 0;

            foreach (Skupina skupina in skupiny)
            {
                ids[i++] = skupina.id;
            }

            return ids;
        }

        /// <summary>
        /// Prida skupinu do DB
        /// </summary>
        /// <param name="newSkupina"></param>
        public void Add(Skupina newSkupina)
        {
            this.skupiny.Add(newSkupina);
        }

        /// <summary>
        /// Ziska data o skupine s danym ID, upravi je a ulozi zpet.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newData"></param>
        public void Edit(int id, Skupina newData)
        {
            Skupina oldData = this.GetSkupinaById(id);

            oldData.pes1name = newData.pes1name;
            oldData.pes2name = newData.pes2name;
            oldData.pes3name = newData.pes3name;

            oldData.pes1plemeno = newData.pes1plemeno;
            oldData.pes2plemeno = newData.pes2plemeno;
            oldData.pes3plemeno = newData.pes3plemeno;

            oldData.pes1barva = newData.pes1barva;
            oldData.pes2barva = newData.pes2barva;
            oldData.pes3barva = newData.pes3barva;
        }

        /// <summary>
        /// Smaze skupinu s danym ID
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Int32 i = 0;

            foreach (Skupina skupina in skupiny)
            {
                if (skupina.id == id)
                    break;
                i++;
            }

            this.skupiny.RemoveAt(i);
        }
    }
}

using Biblioteca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Models
{
    /// <summary>
    /// Model used to keep both the country and it's original index
    /// </summary>
    public class CountryModel
    {
        private int _index;
        private Country _country;

        public int Index => _index;
        public Country Country => _country;

        public CountryModel(int index, Country country)
        {
            _index = index;
            _country = country;
        }

        public override string ToString()
        {
            return _country.ToString();
        }
    }
}

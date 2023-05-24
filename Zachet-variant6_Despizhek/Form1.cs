using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Zachet_variant6_Despizhek
{
    public partial class Form1 :Form
    {
        List<Country> countries = new List<Country>( );
        List<Hotel> hotels = new List<Hotel>( );
        public Form1 ()
        {
            InitializeComponent( );
            ReadFromFile( );
            ReadFromFileHotel( );
        }

        public void ReadFromFile ()
        {
            if ( File.Exists("country.txt") )
            {
                int id = 0;
                StreamReader sr = File.OpenText("country.txt");
                while ( !sr.EndOfStream )
                {
                    id++;
                    string s = sr.ReadLine( );
                    Country cnry = new Country( );
                    cnry.Id = id;
                    cnry.Name = s;
                    countries.Add(cnry);
                }
                sr.Close( );
            }
            else
            {
                MessageBox.Show("Не удалось найти файл");
            }
        }

        private void button2_Click (object sender, EventArgs e)
        {
            if ( textBox2.Text == "" )
            {
                MessageBox.Show("Нельзя вводить пустую строку");
            }
            else
            {
                foreach ( var con in countries )
                {
                    if ( con.Name == textBox2.Text )
                    {
                        MessageBox.Show("Такая страна уже есть");
                        return;
                    }
                }
                Country country = new Country( );
                country.Name = textBox2.Text;
                country.Id = countries.Count + 1;
                countries.Add(country);
                textBox2.Text = null;
                MessageBox.Show("Страна добавлена");
                listBox1.Items.Clear( );
                foreach ( Country g in countries )
                {
                    listBox1.Items.Add($"{g.Id}. {g.Name}");
                }
            }
        }

        private void tabControl1_Click (object sender, EventArgs e)
        {
            numericUpDown2.Maximum = countries.Count;
            listBox1.Items.Clear( );
            foreach ( Country g in countries )
            {
                listBox1.Items.Add($"{g.Id}. {g.Name}");
            }
            /*listBox2.Items.Clear( );
            var result = from hotel in hotels
                         join country in countries on hotel.CountryId equals country.Id
                         select new { HotelName = hotel.Name, CountryName = country.Name, HotelPrice = hotel.Price, HotelType = hotel.Type };
            foreach ( var res in result )
            {
                listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice}");
            }*/
        }
        public void ReadFromFileHotel ()
        {
            if ( File.Exists("hotel.txt") )
            {
                int id = 0;
                StreamReader sr = File.OpenText("hotel.txt");
                while ( !sr.EndOfStream )
                {
                    id++;
                    string s = sr.ReadLine( );
                    string [ ] ss = s.Split(new char [ ] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                    Hotel hl = new Hotel( );
                    hl.Name = ss [ 0 ];
                    hl.Price = int.Parse(ss [ 1 ]);
                    hl.Type = ss [ 2 ];
                    hl.CountryId = int.Parse(ss [ 3 ]);
                    hotels.Add(hl);
                }
                sr.Close( );
            }
            else
            {
                MessageBox.Show("Не удалось найти файл");
            }
        }

        private void tabPage4_Click (object sender, EventArgs e)
        {
            listBox1.Items.Clear( );
            foreach ( Country g in countries )
            {
                listBox1.Items.Add($"{g.Id}. {g.Name}");
            }
        }

        private void button1_Click (object sender, EventArgs e)
        {
            if ( textBox1.Text == "" || textBox3.Text == "" )
            {
                MessageBox.Show("Нельзя вводить пустую строку");
            }
            else
            {
                foreach ( Hotel htl in hotels )
                {
                    if ( htl.Name == textBox1.Text && htl.CountryId == numericUpDown2.Value && htl.Type == textBox3.Text )
                    {
                        MessageBox.Show("Такой отель уже есть");
                        return;
                    }
                }
                Hotel hotel = new Hotel( );
                hotel.Name = textBox1.Text;
                hotel.Type = textBox3.Text;
                hotel.Price = (int) numericUpDown1.Value;
                hotel.CountryId = (int) numericUpDown2.Value;
                hotels.Add(hotel);
                MessageBox.Show("Отель добавлен");
            }
        }

        private void Form1_FormClosed (object sender, FormClosedEventArgs e)
        {
            SaveHotel( );
            SaveCountry( );
        }
        public void SaveHotel ()
        {
            if ( File.Exists("hotel.txt") )
            {
                StreamWriter sw = File.CreateText("hotel.txt");
                foreach ( Hotel ht in hotels )
                {
                    sw.Write($"{ht.Name}~{ht.Price}~{ht.Type}~{ht.CountryId}");
                    sw.WriteLine( );
                }
                sw.Close( );
                MessageBox.Show("Все данные сохранены (Отели)");
            }
        }
        public void SaveCountry ()
        {
            if ( File.Exists("country.txt") )
            {
                StreamWriter sw = File.CreateText("country.txt");
                foreach ( Country cnt in countries )
                {
                    sw.Write($"{cnt.Name}");
                    sw.WriteLine();
                }
                sw.Close( );
                MessageBox.Show("Все данные сохранены (Страны)");
            }
        }

        private void comboBox1_SelectedIndexChanged (object sender, EventArgs e)
        {
            listBox2.Items.Clear( );
                var result = from hotel in hotels
                             join country in countries on hotel.CountryId equals country.Id
                             select new { HotelName = hotel.Name, CountryName = country.Name ,HotelPrice=hotel.Price, HotelType=hotel.Type};
            foreach ( var res in result )
            {
                listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice}");
            }
            if ( comboBox1.Text == "Сортировка по цене" )
            {
                int [ ] hotls = new int [ result.Count( ) ];
                int i = 0;
                foreach ( var res in result )
                {
                    hotls [ i ] = res.HotelPrice;
                    i++;
                }
                var orderedNumbers = from it in hotls
                                     orderby it
                                     select it;
                foreach ( int it in orderedNumbers )
                {
                    foreach ( var res in result )
                    {
                        if ( it == res.HotelPrice )
                        {
                            listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice} Тип:{res.HotelType}");
                        }
                    }
                }
                    
            }
            else if ( comboBox1.Text == "Сортировка по названию страны" )
            {
                string [ ] hotls = new string [ result.Count( ) ];
                int i = 0;
                foreach ( var res in countries )
                {
                    hotls [ i ] = res.Name;
                    i++;
                }
                Array.Sort(hotls);
                for ( int j = 0; j < hotls.Length; j++ )
                {
                    foreach ( var res in result )
                    {
                        if ( hotls [ j ] == res.CountryName )
                        {
                            listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice} Тип:{res.HotelType}");
                        }
                    }
                }
            }

        }

        private void Form1_Load (object sender, EventArgs e)
        {
            /*listBox2.Items.Clear( );
            var result = from hotel in hotels
                         join country in countries on hotel.CountryId equals country.Id
                         select new { HotelName = hotel.Name, CountryName = country.Name, HotelPrice = hotel.Price, HotelType = hotel.Type };
            foreach ( var res in result )
            {
                listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName}; Цена: {res.HotelPrice}; Тип:{res.HotelType};");
            }*/
        }

        private void comboBox1_SelectedIndexChanged_1 (object sender, EventArgs e)
        {
            listBox2.Items.Clear( );
            var result = from hotel in hotels
                         join country in countries on hotel.CountryId equals country.Id
                         select new { HotelName = hotel.Name, CountryName = country.Name, HotelPrice = hotel.Price, HotelType = hotel.Type };
           /* foreach ( var res in result )
            {
                listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice}");
            }*/
            if ( comboBox1.Text == "Сортировка по цене" )
            {
                int [ ] hotls = new int [ result.Count( ) ];
                int i = 0;
                foreach ( var res in result )
                {
                    hotls [ i ] = res.HotelPrice;
                    i++;
                }
                var orderedNumbers = from it in hotls
                                     orderby it
                                     select it;
                foreach ( int it in orderedNumbers )
                {
                    foreach ( var res in result )
                    {
                        if ( it == res.HotelPrice )
                        {
                            listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice} Тип:{res.HotelType}");
                        }
                    }
                }

            }
            else if ( comboBox1.Text == "Сортировка по названию страны" )
            {
                string [ ] hotls = new string [ countries.Count( ) ];
                int i = 0;
                foreach ( var res in countries )
                {
                    hotls [ i ] = res.Name;
                    i++;
                }
                Array.Sort(hotls);
                for ( int j = 0; j < hotls.Length; j++ )
                {
                    foreach ( var res in result )
                    {
                        if ( hotls [ j ] == res.CountryName )
                        {
                            listBox2.Items.Add($"Отель: {res.HotelName}; Страна: {res.CountryName} Цена: {res.HotelPrice} Тип:{res.HotelType}");
                        }
                    }
                }
            }
        }
    }
}
